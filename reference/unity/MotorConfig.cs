using UnityEngine;

namespace Friendslop.Reference
{
    /// <summary>
    /// Every feel tunable for the character motor. ScriptableObject so tuning
    /// is a data edit, never a code change (framework/03). Defaults below are
    /// the tested starting points from TUNING_DEFAULTS.md for an 80 kg player.
    /// Tune ONE value per playtest and log it in the production log.
    /// </summary>
    [CreateAssetMenu(menuName = "Friendslop/Motor Config")]
    public class MotorConfig : ScriptableObject
    {
        [Header("Hover spring (keeps capsule floating at ride height)")]
        public float rideHeight = 0.6f;        // meters from capsule center to ground
        public float probeExtra = 0.4f;        // extra ray length so downhill slopes don't flicker grounded state
        public float springStrength = 12000f;  // N per meter of compression (80 kg body)
        public float springDamper = 1200f;     // N per m/s of vertical velocity

        [Header("Locomotion")]
        public float maxSpeed = 5.5f;          // m/s
        public float acceleration = 45f;       // how hard we push toward desired velocity
        public float airControl = 0.25f;       // fraction of acceleration while airborne

        [Header("Upright torque (wobble, don't snap — the clumsiness aesthetic)")]
        public float uprightSpring = 800f;     // torque per radian of lean
        public float uprightDamper = 90f;      // torque per rad/s — LOW damping = comedic wobble
        [Range(0f, 90f)]
        public float giveUpAngle = 70f;        // beyond this lean, stop fighting — let them fall (funnier)

        [Header("Jump")]
        public float jumpImpulse = 480f;       // N*s; 480 on 80 kg ≈ 6 m/s takeoff ≈ 1.8 m apex
        public float jumpCooldown = 0.25f;
        public float coyoteTime = 0.12f;       // grace period after leaving ground — forgiveness reads as "good controls"
    }
}
