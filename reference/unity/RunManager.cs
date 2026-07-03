using Unity.Netcode;
using UnityEngine;

namespace Friendslop.Reference
{
    /// <summary>
    /// The single source of truth for game state (framework/04 rule 4): one
    /// NetworkObject in the Game scene whose NetworkVariables ARE the run.
    /// Clients read, host writes; no client ever decides an outcome.
    ///
    /// Deliberately a skeleton — the states are universal to run-based
    /// friendslop, the transitions/win conditions come from the design doc.
    /// Keep the restart path HOT: Results -> InRun must be one input, < 5 s.
    /// </summary>
    public class RunManager : NetworkBehaviour
    {
        public enum RunPhase : byte { Lobby, Countdown, InRun, Results }

        // Host-writes/everyone-reads is the default permission set — keep it.
        public NetworkVariable<RunPhase> Phase = new(RunPhase.Lobby);
        public NetworkVariable<int> Seed = new();          // host-rolled; ALL randomness derives from this
        public NetworkVariable<float> RunTimer = new();
        public NetworkVariable<int> WipeCount = new();     // feeds the end-of-run blame card (framework/10)

        public static RunManager Instance { get; private set; }
        public event System.Action<RunPhase> OnPhaseChanged;

        public override void OnNetworkSpawn()
        {
            Instance = this;
            Phase.OnValueChanged += (_, next) => OnPhaseChanged?.Invoke(next);
        }

        private void Update()
        {
            if (!IsServer || Phase.Value != RunPhase.InRun) return;
            RunTimer.Value += Time.deltaTime;
            // Host-side win/fail checks go here, reading the design doc's conditions.
        }

        // ---- Host-only transitions (guard EVERY mutation with IsServer) ----

        public void StartRun()
        {
            if (!IsServer) return;
            Seed.Value = Random.Range(int.MinValue, int.MaxValue);
            RunTimer.Value = 0f;
            Phase.Value = RunPhase.Countdown;
            // Spawn/reset level from Seed, teleport players to start, then:
            Invoke(nameof(GoLive), 3f);
        }

        private void GoLive() { if (IsServer) Phase.Value = RunPhase.InRun; }

        public void EndRun(bool won)
        {
            if (!IsServer || Phase.Value != RunPhase.InRun) return;
            if (!won) WipeCount.Value++;
            Phase.Value = RunPhase.Results;
            // Results screen shows the blame card; any player's "again" input calls:
        }

        // The restart hook — clients request, host acts. One input, no menu round-trip.
        [ServerRpc(RequireOwnership = false)]
        public void RequestRestartServerRpc()
        {
            if (Phase.Value == RunPhase.Results || Phase.Value == RunPhase.Lobby)
                StartRun();
        }
    }
}
