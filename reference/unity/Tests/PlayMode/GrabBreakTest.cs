using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Friendslop.Reference.Tests
{
    /// <summary>
    /// THE level-2 pattern (framework/12): a physics invariant tested with a
    /// PROGRAMMATICALLY built scenario — no test scene assets to rot. Three
    /// things every physics test here must do:
    ///   1. build primitives + components in code,
    ///   2. wait in FixedUpdate ticks (physics doesn't advance in yield null),
    ///   3. assert RANGES and outcomes, never exact floats.
    ///
    /// This is also the jank-guard shape: when a funny exploit is promoted to
    /// a feature (PROTECTED JANK, framework/08), the guard test looks exactly
    /// like this — recreate the exploit, assert it still works.
    /// </summary>
    public class GrabBreakTest
    {
        private GameObject holder, box;

        [TearDown]
        public void Cleanup()
        {
            if (holder != null) Object.Destroy(holder);
            if (box != null) Object.Destroy(box);
        }

        [UnityTest]
        public IEnumerator Joint_BreaksUnderOverload_AndReleaseFires()
        {
            // -- build the scenario in code ---------------------------------
            holder = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            holder.transform.position = Vector3.zero;
            var holderBody = holder.AddComponent<Rigidbody>();
            holderBody.mass = 80f;                       // player mass (TUNING_DEFAULTS)
            holderBody.useGravity = false;               // isolate the invariant under test

            box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.transform.position = new Vector3(0f, 0f, 1f);
            var boxBody = box.AddComponent<Rigidbody>();
            boxBody.mass = 10f;
            boxBody.useGravity = false;

            var joint = holder.AddComponent<ConfigurableJoint>();
            joint.connectedBody = boxBody;
            joint.xMotion = joint.yMotion = joint.zMotion = ConfigurableJointMotion.Limited;
            joint.linearLimit = new SoftJointLimit { limit = 0.1f };
            joint.breakForce = 2500f;                    // the GrabSystem default

            bool released = false;
            var listener = holder.AddComponent<JointBreakListener>();
            listener.OnBroke += () => released = true;

            // settle a few ticks so the joint is live before we abuse it
            for (int i = 0; i < 5; i++) yield return new WaitForFixedUpdate();

            // -- act: yank far beyond break force ---------------------------
            boxBody.AddForce(Vector3.forward * 20000f, ForceMode.Force);
            for (int i = 0; i < 25 && !released; i++)    // ~0.5 s of physics
                yield return new WaitForFixedUpdate();

            // -- assert outcomes, not exact numbers -------------------------
            Assert.IsTrue(released, "breakForce exceeded but OnJointBreak never fired");
            Assert.IsNull(holder.GetComponent<ConfigurableJoint>(),
                "Unity should destroy the joint component on break");
            Assert.Greater(boxBody.linearVelocity.magnitude, 1f,
                "freed box should carry comedic momentum");
        }

        /// <summary>Minimal relay so the test can observe OnJointBreak.</summary>
        private class JointBreakListener : MonoBehaviour
        {
            public event System.Action OnBroke;
            private void OnJointBreak(float force) => OnBroke?.Invoke();
        }
    }
}
