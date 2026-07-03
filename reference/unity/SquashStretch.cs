using System.Collections;
using UnityEngine;

namespace Friendslop.Reference
{
    /// <summary>
    /// The 10-line charm layer (framework/06 cheap moves): scale-punch a visual
    /// transform on jumps, landings, grabs, impacts. Attach to the VISUAL child
    /// (never the physics body — colliders must not scale). No tween library.
    /// Usage: squash.Punch(0.25f) on land; squash.Punch(-0.18f) on jump (stretch).
    /// </summary>
    public class SquashStretch : MonoBehaviour
    {
        [SerializeField] private float duration = 0.28f;
        [SerializeField] private float frequency = 14f;   // damped-sine wobbles

        private Vector3 baseScale;
        private Coroutine running;

        private void Awake() => baseScale = transform.localScale;

        /// <param name="amount">+ squashes (wide/short, landings), - stretches (tall/thin, jumps).</param>
        public void Punch(float amount)
        {
            if (running != null) StopCoroutine(running);
            running = StartCoroutine(Animate(amount));
        }

        private IEnumerator Animate(float amount)
        {
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float k = amount * Mathf.Sin(t * frequency) * (1f - t / duration);
                transform.localScale = new Vector3(baseScale.x * (1f + k),
                                                   baseScale.y * (1f - k),
                                                   baseScale.z * (1f + k));
                yield return null;
            }
            transform.localScale = baseScale;
            running = null;
        }
    }
}
