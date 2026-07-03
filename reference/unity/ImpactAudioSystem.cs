using UnityEngine;

namespace Friendslop.Reference
{
    /// <summary>
    /// ALL physics collision audio flows through this one system
    /// (framework/07): strength = collision impulse -> band -> random clip +
    /// pitch variance from a pooled 3D AudioSource. Nothing ever repeats
    /// identically. Tuning lives in the ImpactAudioConfig asset, not code.
    ///
    /// Setup: one ImpactAudioSystem in the Boot scene; an ImpactAudioEmitter
    /// (bottom of file) on every prefab that should thud.
    /// </summary>
    public class ImpactAudioSystem : MonoBehaviour
    {
        public static ImpactAudioSystem Instance { get; private set; }

        [SerializeField] private ImpactAudioConfig config;
        [SerializeField] private int poolSize = 16;
        [SerializeField] private UnityEngine.Audio.AudioMixerGroup impactsGroup;

        private AudioSource[] pool;
        private int next;

        private void Awake()
        {
            Instance = this;
            pool = new AudioSource[poolSize];
            for (int i = 0; i < poolSize; i++)
            {
                var go = new GameObject($"ImpactVoice{i}");
                go.transform.SetParent(transform);
                var src = go.AddComponent<AudioSource>();
                src.playOnAwake = false;
                src.spatialBlend = 1f;                        // 3D — rolloff matched to voice chat (framework/07)
                src.rolloffMode = AudioRolloffMode.Linear;
                src.maxDistance = 30f;
                src.outputAudioMixerGroup = impactsGroup;
                pool[i] = src;
            }
        }

        public void Report(Vector3 position, float impulse, ImpactSurface surface)
        {
            var band = config.FindBand(surface, impulse);
            if (band == null) return;

            var src = pool[next]; next = (next + 1) % pool.Length;
            src.transform.position = position;
            src.pitch = 1f + Random.Range(-config.pitchVariance, config.pitchVariance);
            float t = Mathf.InverseLerp(band.minImpulse, band.maxImpulse, impulse);
            src.PlayOneShot(band.clips[Random.Range(0, band.clips.Length)],
                            Mathf.Lerp(band.minVolume, 1f, t));
        }
    }

    public enum ImpactSurface { Generic, Body, Wood, Metal, Squish }
}
