using UnityEngine;
using UnityEngine.Audio;

namespace Friendslop.Reference
{
    /// <summary>
    /// The signature move (framework/07): when the LOCAL player commits to a
    /// long fall, cut ambience/music (keep voice chat!) so the eventual impact
    /// lands twice as hard. Purely client-local cosmetic — never synced.
    ///
    /// Setup: two AudioMixer snapshots — "Normal" and "Falling" (Falling ==
    /// Normal with Ambience & Music at -80 dB; Voice and SFX untouched).
    /// </summary>
    public class FallSilence : MonoBehaviour
    {
        [SerializeField] private HoverCapsuleMotor localMotor;   // assign the LOCAL player's motor at spawn
        [SerializeField] private AudioMixerSnapshot normal;
        [SerializeField] private AudioMixerSnapshot falling;
        [SerializeField] private float triggerVelocity = -9f;    // m/s downward ≈ falling for >0.9 s
        [SerializeField] private float armDelay = 0.35f;         // don't trigger on hops
        [SerializeField] private float fadeIn = 0.4f;            // ambience fades out over this
        [SerializeField] private float fadeOut = 0.15f;          // slam back on landing

        private Rigidbody body;
        private float airborneTime;
        private bool silenced;

        public void Bind(HoverCapsuleMotor motor)                // call from local player spawn code
        {
            localMotor = motor;
            body = motor.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (localMotor == null) return;
            if (body == null) body = localMotor.GetComponent<Rigidbody>();

            airborneTime = localMotor.Grounded ? 0f : airborneTime + Time.deltaTime;
            bool shouldSilence = airborneTime > armDelay && body.linearVelocity.y < triggerVelocity;

            if (shouldSilence && !silenced) { falling.TransitionTo(fadeIn); silenced = true; }
            else if (!shouldSilence && silenced) { normal.TransitionTo(fadeOut); silenced = false; }
        }
    }
}
