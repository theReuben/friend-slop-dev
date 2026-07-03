using UnityEngine;

namespace Friendslop.Reference
{
    /// <summary>
    /// The "WHERE ARE YOU" machine (framework/04): a raycast between the local
    /// listener's head and this speaker's head drives a low-pass filter, so
    /// voices muffle behind walls/terrain. Client-local cosmetic, never synced.
    /// Attach next to SteamVoiceChat's output AudioSource + an AudioLowPassFilter.
    /// </summary>
    [RequireComponent(typeof(AudioLowPassFilter))]
    public class VoiceOcclusion : MonoBehaviour
    {
        [SerializeField] private LayerMask occluderMask;         // world geometry only — NOT players/props
        [SerializeField] private float openCutoff = 22000f;
        [SerializeField] private float occludedCutoff = 900f;    // muffled-through-a-wall character
        [SerializeField] private float smoothTime = 0.15f;       // no clicking as people cross doorways
        [SerializeField] private float checkInterval = 0.1f;     // 10 Hz is plenty; raycasts are cheap but not free

        private AudioLowPassFilter filter;
        private Transform listenerHead;                          // set from local player spawn code
        private float targetCutoff, velocity, nextCheck;

        public void Bind(Transform localListenerHead) => listenerHead = localListenerHead;

        private void Awake()
        {
            filter = GetComponent<AudioLowPassFilter>();
            targetCutoff = openCutoff;
        }

        private void Update()
        {
            if (listenerHead == null) return;
            if (Time.time >= nextCheck)
            {
                nextCheck = Time.time + checkInterval;
                bool blocked = Physics.Linecast(listenerHead.position, transform.position,
                                                occluderMask, QueryTriggerInteraction.Ignore);
                targetCutoff = blocked ? occludedCutoff : openCutoff;
            }
            filter.cutoffFrequency = Mathf.SmoothDamp(filter.cutoffFrequency, targetCutoff,
                                                      ref velocity, smoothTime);
        }
    }
}
