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

    [CreateAssetMenu(menuName = "Friendslop/Impact Audio Config")]
    public class ImpactAudioConfig : ScriptableObject
    {
        [System.Serializable]
        public class Band
        {
            public ImpactSurface surface;
            public float minImpulse = 50f;    // N*s — below this, silence (don't machine-gun tiny contacts)
            public float maxImpulse = 800f;   // at/above this, full volume (the big splat)
            public float minVolume = 0.3f;
            public AudioClip[] clips;         // 3+ variants per band, always
        }

        [Range(0f, 0.3f)] public float pitchVariance = 0.1f;
        public Band[] bands;

        public Band FindBand(ImpactSurface surface, float impulse)
        {
            Band best = null;
            foreach (var b in bands)
                if (b.surface == surface && impulse >= b.minImpulse)
                    best = b;                 // list bands per surface in ascending minImpulse order
            return best;
        }
    }

    /// <summary>Put on every thud-worthy prefab. Cooldown prevents contact-jitter spam.</summary>
    [RequireComponent(typeof(Rigidbody))]
    public class ImpactAudioEmitter : MonoBehaviour
    {
        [SerializeField] private ImpactSurface surface = ImpactSurface.Generic;
        [SerializeField] private float cooldown = 0.08f;
        private float lastPlay;

        private void OnCollisionEnter(Collision collision)
        {
            if (Time.time - lastPlay < cooldown || ImpactAudioSystem.Instance == null) return;
            lastPlay = Time.time;
            ImpactAudioSystem.Instance.Report(collision.GetContact(0).point,
                                              collision.impulse.magnitude, surface);
        }
    }
}
