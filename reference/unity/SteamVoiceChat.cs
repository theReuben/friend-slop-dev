using System.IO;
using Unity.Netcode;
using UnityEngine;

namespace Friendslop.Reference
{
    /// <summary>
    /// Proximity voice chat: Facepunch Steamworks voice capture -> compressed
    /// bytes over NGO unreliable RPCs -> decompressed on each client into a
    /// streaming AudioClip on the SPEAKER'S HEAD. 3D spatialization then gives
    /// proximity voice for free; VoiceOcclusion.cs adds the wall-muffle.
    ///
    /// ⚠ THE FIDDLIEST FILE IN reference/. Facepunch.Steamworks' voice API
    /// surface has shifted between versions (ReadVoiceData/ReadVoiceDataBytes,
    /// DecompressVoice overloads). VERIFY every SteamUser.* call against the
    /// installed package source before trusting a compile error fix. The
    /// architecture (capture->RPC->ring buffer->streaming clip) is the part
    /// to preserve; the exact capture calls are the part to re-check.
    ///
    /// Attach to the player prefab. One instance per player; each client
    /// records only on its owned instance and plays back on all remote ones.
    /// SteamClient.Init must have succeeded (Boot scene) before enabling.
    /// </summary>
    public class SteamVoiceChat : NetworkBehaviour
    {
        public enum Mode { Proximity, AlwaysAudible, PushToTalk }

        [SerializeField] private AudioSource output;             // on the head bone; spatialBlend 1, Voice mixer group
        [SerializeField] private Mode mode = Mode.Proximity;     // settings UI writes this + per-player mute
        [SerializeField] private float maxDistance = 25f;        // rolloff matched to SFX world (framework/07)

        public bool LocallyMuted { get; set; }                   // per-player mute — ship-check requires the UI for this
        public bool IsTalking { get; private set; }              // drive mouth-flap / talk icon from this

        private readonly MemoryStream captureStream = new MemoryStream();
        private float[] ringBuffer;
        private int writeHead, readHead;
        private int sampleRate;

        public override void OnNetworkSpawn()
        {
            sampleRate = (int)Steamworks.SteamUser.OptimalSampleRate;
            ringBuffer = new float[sampleRate * 2];              // 2 s of headroom

            if (!IsOwner)
            {
                // Streaming clip that pulls from the ring buffer as Unity needs samples.
                var clip = AudioClip.Create($"voice_{OwnerClientId}", sampleRate, 1, sampleRate,
                                            true, OnAudioRead);
                output.clip = clip;
                output.loop = true;
                output.spatialBlend = mode == Mode.AlwaysAudible ? 0f : 1f;
                output.maxDistance = maxDistance;
                output.Play();
            }
        }

        private void Update()
        {
            if (!IsOwner) { IsTalking = Time.time - lastPacketTime < 0.3f; return; }

            bool wantRecord = mode != Mode.PushToTalk || PushToTalkHeld();
            Steamworks.SteamUser.VoiceRecord = wantRecord;

            while (Steamworks.SteamUser.HasVoiceData)
            {
                captureStream.SetLength(0);
                int compressed = Steamworks.SteamUser.ReadVoiceData(captureStream);
                if (compressed <= 0) continue;
                lastPacketTime = Time.time;
                SendVoiceServerRpc(captureStream.ToArray());     // small packets, ~20 ms cadence
            }
            IsTalking = Time.time - lastPacketTime < 0.3f;       // decays to false when the mic goes quiet
        }

        private static bool PushToTalkHeld() =>
            UnityEngine.InputSystem.Keyboard.current?.vKey.isPressed ?? false; // wire to InputActions in-project

        [ServerRpc(Delivery = RpcDelivery.Unreliable)]
        private void SendVoiceServerRpc(byte[] compressed, ServerRpcParams _ = default)
        {
            ReceiveVoiceClientRpc(compressed);                   // host relays to everyone (incl. sender; sender ignores)
        }

        private float lastPacketTime;

        [ClientRpc(Delivery = RpcDelivery.Unreliable)]
        private void ReceiveVoiceClientRpc(byte[] compressed)
        {
            if (IsOwner || LocallyMuted) return;                 // never play your own voice back
            lastPacketTime = Time.time;

            using var pcm = new MemoryStream();
            int bytes = Steamworks.SteamUser.DecompressVoice(compressed, pcm); // 16-bit signed mono PCM
            if (bytes <= 0) return;

            byte[] raw = pcm.GetBuffer();
            for (int i = 0; i + 1 < bytes; i += 2)
            {
                short sample = (short)(raw[i] | (raw[i + 1] << 8));
                ringBuffer[writeHead] = sample / 32768f;
                writeHead = (writeHead + 1) % ringBuffer.Length;
            }
        }

        // Unity's audio thread pulls samples; feed silence when the buffer runs dry (gaps are normal on unreliable delivery).
        private void OnAudioRead(float[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (readHead != writeHead)
                {
                    data[i] = ringBuffer[readHead];
                    readHead = (readHead + 1) % ringBuffer.Length;
                }
                else data[i] = 0f;
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsOwner) Steamworks.SteamUser.VoiceRecord = false;
        }
    }
}
