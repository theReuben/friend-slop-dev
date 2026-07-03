using UnityEngine;

namespace Friendslop.Reference
{
    /// <summary>
    /// Put on every thud-worthy prefab; routes collisions to ImpactAudioSystem.
    /// Cooldown prevents contact-jitter machine-gunning. Own file — Unity
    /// requires MonoBehaviour class names to match their file name or the
    /// component can't be added to a GameObject.
    /// </summary>
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
