using System.Collections;
using UnityEngine;

namespace Friendslop.Reference
{
    /// <summary>
    /// Impact -> full ragdoll -> comedic recovery.
    ///
    /// Honest scope note: proper physics-to-animation blending is a rabbit
    /// hole. This does the friendslop-sufficient version: flop convincingly,
    /// lie there for a beat (that's the clip), then stand back up with a quick
    /// root-snap + get-up masking. Do NOT upgrade this to full active-ragdoll
    /// without a producer note — it's a classic scope trap.
    ///
    /// Setup: character prefab has an animated visual rig whose bones carry
    /// pre-configured ragdoll Rigidbodies+Colliders (all kinematic while
    /// animated), plus the gameplay capsule (HoverCapsuleMotor) as the root.
    /// Runs host-side; remote clients see the result via synced bone roots or
    /// simply the capsule + a local-cosmetic ragdoll (framework/04: cosmetics
    /// are client-local — a slightly different flop per screen is fine).
    /// </summary>
    public class RagdollBlender : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody hips;                 // ragdoll root bone
        [SerializeField] private Rigidbody[] ragdollBodies;      // all bone bodies incl. hips
        [SerializeField] private HoverCapsuleMotor motor;
        [SerializeField] private float impactThreshold = 350f;   // N*s of collision impulse to trigger flop
        [SerializeField] private float minDownTime = 1.6f;       // the beat where the camera lingers
        [SerializeField] private float getUpDuration = 0.6f;

        public bool IsRagdolled { get; private set; }
        public event System.Action OnFlop;                       // camera linger + impact audio hook
        public event System.Action OnRecovered;

        private void Awake() => SetRagdoll(false);

        private void OnCollisionEnter(Collision collision)
        {
            if (!IsRagdolled && collision.impulse.magnitude > impactThreshold)
                Flop(collision.impulse / Time.fixedDeltaTime * 0.001f);
        }

        public void Flop(Vector3 launchForce = default)
        {
            if (IsRagdolled) return;
            IsRagdolled = true;
            motor.enabled = false;
            animator.enabled = false;
            SetRagdoll(true);

            // Hand the capsule's momentum to the bones so the flop continues the motion.
            var capsule = motor.GetComponent<Rigidbody>();
            foreach (var rb in ragdollBodies)
                rb.linearVelocity = capsule.linearVelocity;
            hips.AddForce(launchForce, ForceMode.Impulse);
            capsule.isKinematic = true;                          // park the gameplay capsule during the flop

            OnFlop?.Invoke();
            StartCoroutine(RecoverAfterBeat());
        }

        private IEnumerator RecoverAfterBeat()
        {
            yield return new WaitForSeconds(minDownTime);
            // Wait until the pile settles (or timeout — someone may be juggling the body, which is allowed).
            float deadline = Time.time + 4f;
            while (hips.linearVelocity.sqrMagnitude > 0.4f && Time.time < deadline)
                yield return new WaitForFixedUpdate();

            // Teleport the gameplay capsule to where the body ended up, slightly raised.
            var capsule = motor.GetComponent<Rigidbody>();
            capsule.position = hips.position + Vector3.up * 0.7f;
            capsule.linearVelocity = Vector3.zero;
            capsule.isKinematic = false;

            SetRagdoll(false);
            animator.enabled = true;
            animator.CrossFade("GetUp", 0.1f);                   // any short get-up clip masks the snap
            motor.enabled = true;
            IsRagdolled = false;
            OnRecovered?.Invoke();
            yield return new WaitForSeconds(getUpDuration);
        }

        private void SetRagdoll(bool on)
        {
            foreach (var rb in ragdollBodies)
            {
                rb.isKinematic = !on;
                rb.detectCollisions = on;
            }
        }
    }
}
