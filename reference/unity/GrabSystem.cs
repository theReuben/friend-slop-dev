using UnityEngine;

namespace Friendslop.Reference
{
    /// <summary>
    /// Physical grabbing/carrying via ConfigurableJoint.
    ///
    /// WHY joints and never parenting (framework/03, non-negotiable):
    ///  - held objects keep full collision, so the ladder you carry hits your
    ///    friend's head. That's the game.
    ///  - joints have break forces, so overloading/yanking rips grips apart —
    ///    emergent slapstick with zero extra code.
    ///  - two players grabbing the same object Just Works (two joints).
    ///
    /// Grabbable = any Rigidbody on the grabbable layer. Players are grabbable
    /// too (their capsule is a Rigidbody) — do not special-case them out.
    /// In multiplayer this runs HOST-SIDE only, driven by intent.GrabHeld.
    /// </summary>
    public class GrabSystem : MonoBehaviour
    {
        [Header("Config (values from TUNING_DEFAULTS.md)")]
        [SerializeField] private float grabRange = 1.6f;
        [SerializeField] private float grabRadius = 0.45f;      // spherecast: generous aim assist = comedy density
        [SerializeField] private float breakForce = 2500f;      // N; light props never break, yanking a player might
        [SerializeField] private float jointSpring = 3000f;     // drive toward hand anchor
        [SerializeField] private float jointDamper = 300f;
        [SerializeField] private float maxGrabMass = 120f;      // heavier than a player+bit: can't carry the bus
        [SerializeField] private LayerMask grabbableMask;
        [SerializeField] private Transform handAnchor;          // child transform ~0.7 m in front of chest

        private Rigidbody body;
        private ConfigurableJoint joint;
        public Rigidbody Held { get; private set; }
        public event System.Action<Rigidbody> OnGrab;
        public event System.Action<Rigidbody> OnRelease;        // fire audio/VFX from these, not from here

        private void Awake() => body = GetComponent<Rigidbody>();

        /// <summary>Call every host tick with the current intent.</summary>
        public void SetGrabHeld(bool held)
        {
            if (held && Held == null) TryGrab();
            else if (!held && Held != null) Release();
        }

        private void TryGrab()
        {
            Vector3 origin = handAnchor.position - transform.forward * 0.3f;
            if (!Physics.SphereCast(origin, grabRadius, transform.forward, out RaycastHit hit,
                                    grabRange, grabbableMask, QueryTriggerInteraction.Ignore)) return;
            Rigidbody target = hit.rigidbody;
            if (target == null || target == body || target.mass > maxGrabMass) return;

            // Joint lives on the GRABBER, connected to the target at the hit point.
            joint = gameObject.AddComponent<ConfigurableJoint>();
            joint.connectedBody = target;
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = transform.InverseTransformPoint(handAnchor.position);
            joint.connectedAnchor = target.transform.InverseTransformPoint(hit.point);
            joint.xMotion = joint.yMotion = joint.zMotion = ConfigurableJointMotion.Limited;
            joint.linearLimit = new SoftJointLimit { limit = 0.1f };
            var drive = new JointDrive { positionSpring = jointSpring, positionDamper = jointDamper,
                                         maximumForce = float.MaxValue };
            joint.xDrive = joint.yDrive = joint.zDrive = drive;
            joint.breakForce = breakForce;                       // Unity destroys the joint & calls OnJointBreak
            joint.enableCollision = true;                        // held object still collides with holder — mandatory

            Held = target;
            OnGrab?.Invoke(target);
        }

        public void Release()
        {
            if (joint != null) Destroy(joint);
            joint = null;
            var released = Held;
            Held = null;
            if (released != null) OnRelease?.Invoke(released);
        }

        // Unity calls this when breakForce is exceeded (object too heavy, friend yanked away, explosion).
        private void OnJointBreak(float force)
        {
            joint = null;
            var lost = Held;
            Held = null;
            if (lost != null) OnRelease?.Invoke(lost);          // listeners play the "grip lost" yelp here
        }

        private void OnDisable() => Release();                  // disconnects/ragdolls must drop cleanly
    }
}
