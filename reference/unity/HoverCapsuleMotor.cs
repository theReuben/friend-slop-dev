using UnityEngine;

namespace Friendslop.Reference
{
    /// <summary>
    /// The friendslop character: a DYNAMIC Rigidbody capsule that hovers on a
    /// downward spring and is held upright by a torque spring.
    ///
    /// WHY this and not CharacterController:
    ///  - dynamic bodies are pushable, stackable, grabbable, and ragdoll-able —
    ///    players colliding with players IS the game.
    ///  - the hover spring gives smooth ground handling (steps, slopes) without
    ///    kinematic special-casing.
    ///  - the upright torque spring, deliberately under-damped, produces the
    ///    recoverable-clumsiness wobble that reads as charm on stream.
    ///
    /// Runs identically on host-simulated remote players (feed intents from
    /// RPCs) and the local player (feed intents from input). No prediction.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class HoverCapsuleMotor : MonoBehaviour
    {
        [SerializeField] private MotorConfig config;
        [SerializeField] private LayerMask groundMask = ~0; // exclude the player layer in the project!

        private Rigidbody body;
        private PlayerIntent intent;
        private float lastGroundedTime = float.NegativeInfinity;
        private float lastJumpTime = float.NegativeInfinity;

        public bool Grounded { get; private set; }
        public Vector3 GroundNormal { get; private set; } = Vector3.up;

        /// <summary>Set once per tick by input (local) or by the received RPC (remote). </summary>
        public void SetIntent(in PlayerIntent newIntent) => intent = newIntent;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            if (config == null) { Debug.LogError($"{name}: MotorConfig not assigned", this); enabled = false; return; }
            body.interpolation = RigidbodyInterpolation.Interpolate; // mandatory: physics at 50 Hz, render faster
            body.constraints = RigidbodyConstraints.None;            // upright is a SPRING, not a constraint
        }

        private void FixedUpdate()
        {
            HoverSpring();
            Locomote();
            StayUpright();
            TryJump();
            intent.Jump = false; // edge-triggered inputs are consumed once
        }

        private void HoverSpring()
        {
            float rayLength = config.rideHeight + config.probeExtra;
            if (!Physics.Raycast(body.position, Vector3.down, out RaycastHit hit, rayLength,
                                 groundMask, QueryTriggerInteraction.Ignore))
            {
                Grounded = false;
                return;
            }

            Grounded = hit.distance <= config.rideHeight + 0.15f;
            GroundNormal = hit.normal;
            if (Grounded) lastGroundedTime = Time.time;

            float compression = config.rideHeight - hit.distance;                  // + when too low
            float verticalVel = Vector3.Dot(body.linearVelocity, Vector3.up);
            float force = compression * config.springStrength - verticalVel * config.springDamper;
            if (force > 0f || compression > 0f)                                     // pull-down only when compressed:
                body.AddForce(Vector3.up * force);                                  // never suck the player onto ledges

            // Newton's third law makes standing on props/friends push them down — free comedy:
            if (hit.rigidbody != null)
                hit.rigidbody.AddForceAtPosition(Vector3.down * Mathf.Max(force, 0f), hit.point);
        }

        private void Locomote()
        {
            Vector3 wishDir = Quaternion.Euler(0f, intent.LookYaw, 0f)
                              * new Vector3(intent.Move.x, 0f, intent.Move.y);
            wishDir = Vector3.ClampMagnitude(wishDir, 1f);

            Vector3 velocity = body.linearVelocity;
            Vector3 horizontal = new Vector3(velocity.x, 0f, velocity.z);
            Vector3 desired = wishDir * config.maxSpeed;

            float control = Grounded ? config.acceleration : config.acceleration * config.airControl;
            Vector3 push = Vector3.ClampMagnitude(desired - horizontal, control * Time.fixedDeltaTime * 10f);
            body.AddForce(push * body.mass, ForceMode.Force);
        }

        private void StayUpright()
        {
            // Torque-spring toward world up. Under-damped on purpose: the wobble is the aesthetic.
            Vector3 axis = Vector3.Cross(transform.up, Vector3.up);
            float angleRad = Mathf.Asin(Mathf.Clamp(axis.magnitude, -1f, 1f));

            // Past the give-up angle the character commits to the fall — much funnier than
            // an invisible force dragging them back to their feet.
            if (angleRad * Mathf.Rad2Deg > config.giveUpAngle) return;

            Vector3 torque = axis.normalized * (angleRad * config.uprightSpring)
                             - body.angularVelocity * config.uprightDamper;
            body.AddTorque(torque);

            // Face movement direction with a gentle yaw spring (visual only, never snaps).
            Vector3 flatVel = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z);
            if (flatVel.sqrMagnitude > 0.5f)
            {
                float targetYaw = Mathf.Atan2(flatVel.x, flatVel.z) * Mathf.Rad2Deg;
                float yawError = Mathf.DeltaAngle(body.rotation.eulerAngles.y, targetYaw);
                body.AddTorque(Vector3.up * (yawError * Mathf.Deg2Rad * config.uprightSpring * 0.15f));
            }
        }

        private void TryJump()
        {
            bool coyoteGrounded = Time.time - lastGroundedTime <= config.coyoteTime;
            if (!intent.Jump || !coyoteGrounded || Time.time - lastJumpTime < config.jumpCooldown) return;

            lastJumpTime = Time.time;
            lastGroundedTime = float.NegativeInfinity;
            Vector3 velocity = body.linearVelocity;
            if (velocity.y < 0f) { velocity.y = 0f; body.linearVelocity = velocity; } // don't fight downward momentum
            body.AddForce(Vector3.up * config.jumpImpulse, ForceMode.Impulse);
        }
    }
}
