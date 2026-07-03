using Unity.Netcode;
using UnityEngine;

namespace Friendslop.Reference
{
    /// <summary>
    /// Everything a player wants to do this tick, as data. This struct is THE
    /// netcode boundary: gather it from input in Update, apply it to physics
    /// in FixedUpdate, and in multiplayer send it owner->server as an RPC
    /// payload. Gameplay code below this line never touches the Input System,
    /// which is what makes the Phase 1 -> Phase 2 conversion mechanical.
    ///
    /// WHY: friendslop is host-authoritative with no client prediction
    /// (framework/04-netcode.md). Clients only ever produce intents; the host
    /// produces truth. Keep this struct small — it's sent ~30 times/second.
    /// </summary>
    public struct PlayerIntent : INetworkSerializable
    {
        public Vector2 Move;      // normalized stick/WASD, camera-relative conversion happens host-side
        public float LookYaw;     // degrees; motor uses it to orient movement
        public bool Jump;         // edge-triggered: true only on the tick it was pressed
        public bool GrabHeld;     // level-triggered: held grabs are the comedy default
        public bool Emote;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Move);
            serializer.SerializeValue(ref LookYaw);
            serializer.SerializeValue(ref Jump);
            serializer.SerializeValue(ref GrabHeld);
            serializer.SerializeValue(ref Emote);
        }
    }
}
