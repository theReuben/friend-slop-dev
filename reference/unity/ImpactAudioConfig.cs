using UnityEngine;

namespace Friendslop.Reference
{
    /// <summary>
    /// Impact-audio tuning data (framework/07): bands map collision impulse to
    /// clip family + volume. Own file — Unity requires ScriptableObject class
    /// names to match their file name or the asset won't serialize.
    /// </summary>
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
}
