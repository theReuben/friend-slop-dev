using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Friendslop.Reference.Tests
{
    /// <summary>
    /// THE gate test (framework/08): boot the game, host, spawn 4 simulated
    /// players, feed 60 s of random intents, assert zero errors/exceptions.
    /// Green smoke test is a precondition for every phase gate.
    ///
    /// Project must provide one hook: an ISmokeDriver implementation that
    /// knows how to reach "hosting with N fake players" (using NGO's
    /// NetworkManager.StartHost + spawning player prefabs directly — no Steam
    /// in tests; the DEV_BUILD local transport makes this possible).
    /// Headless: Unity -runTests -testPlatform PlayMode -batchmode
    /// </summary>
    public interface ISmokeDriver
    {
        IEnumerator BootToHosting(int fakePlayers);
        void ApplyRandomIntent(int playerIndex, System.Random rng); // set a random PlayerIntent on player i
    }

    public class SmokeTest
    {
        private readonly List<string> errors = new();

        private void OnLog(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
                errors.Add($"{type}: {condition}");
        }

        [UnityTest]
        public IEnumerator FourPlayers_SixtySeconds_NoErrors()
        {
            Application.logMessageReceived += OnLog;
            try
            {
                yield return SceneManager.LoadSceneAsync("Boot", LoadSceneMode.Single);

                var driverBehaviour = Object.FindFirstObjectByType<MonoBehaviour>(FindObjectsInactive.Exclude);
                ISmokeDriver driver = driverBehaviour as ISmokeDriver
                    ?? throw new System.Exception("Scene needs an ISmokeDriver (add SmokeDriver to Boot).");

                yield return driver.BootToHosting(4);

                var rng = new System.Random(12345);              // deterministic chaos: failures reproduce
                float deadline = Time.time + 60f;
                while (Time.time < deadline)
                {
                    for (int i = 0; i < 4; i++)
                        if (rng.NextDouble() < 0.3) driver.ApplyRandomIntent(i, rng);
                    yield return new WaitForSeconds(0.1f);
                    Assert.IsTrue(errors.Count == 0,
                        "Errors during smoke run:\n" + string.Join("\n", errors));
                }
            }
            finally
            {
                Application.logMessageReceived -= OnLog;
            }
        }
    }
}
