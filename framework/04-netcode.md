# 04 — Netcode: free, host-authoritative, Steam-native

## Fixed stack (do not re-litigate per game)

- **Netcode for GameObjects (NGO)** — Unity's first-party high-level netcode.
  Chosen for documentation abundance (matters for model-driven development),
  not raw performance.
- **Facepunch.Steamworks** for Steam API (lobbies, friends, voice, achievements)
  — MIT licensed, C#-idiomatic.
- **Community Facepunch transport for NGO** over **Steam Datagram Relay (SDR)**
  — free NAT punching + relay, no server costs, IPs hidden. This is the entire
  reason "ship and forget" multiplayer is viable.
- **Host = a player** (listen server). No dedicated servers, ever.
- Player count: design for 4, cap at 4–6. Never promise 8+.

Alternative on record: FishNet + FishySteamworks if NGO hits a wall. Switching
is a producer decision logged with reasons.

## Architecture rules

1. **Host-authoritative physics.** The full physics sim runs on the host only.
   Clients send inputs (`ServerRpc`), host simulates, `NetworkTransform`
   (or a custom compressed sync) replicates results. Do NOT attempt client-side
   prediction/rollback for the physics sim — friendslop tolerates 60–120 ms of
   input latency on clients; it reads as part of the clumsiness. Getting
   prediction right is a scope trap that has killed better teams.
2. **Client-local cosmetics.** Camera, ragdoll *visuals* on the local player's
   corpse-cam, particles, UI, sound — all local, never synced.
3. **Sync budget:** ≤ 30 active NetworkObjects. Props become network-synced only
   while held/disturbed (dynamic spawn or ownership of a pooled set); at-rest
   props are static scene objects.
4. **Everything gameplay-relevant flows through the host.** Scores, run state,
   hazard triggers: `NetworkVariable`s on a single `RunManager` NetworkObject.
   No client ever decides an outcome.
5. **Determinism is not assumed.** Random seeds are host-rolled and replicated.

## Voice (a core mechanic, build in Phase 2, not last)

- Capture via Facepunch `SteamUser.VoiceRecord`, ship compressed voice bytes as
  unreliable custom messages, decode and play through an `AudioSource` **on the
  speaker's character head** with 3D spatialization + rolloff = proximity voice
  for free.
- Occlusion: single low-pass filter driven by a raycast between heads. Cheap,
  huge comedy payoff.
- Always-audible fallback toggle (accessibility + streamer preference) and a
  push-to-talk/open-mic option. Mute-player UI is mandatory (ship-check item).

## Session flow

```
Boot: SteamClient.Init(appId) → fail → offline mode banner, solo still works
Menu: Host → SteamMatchmaking.CreateLobby(4, FriendsOnly)
      Join → friends list / Steam overlay invite / lobby browser (public optional)
Lobby joined → transport connects via host SteamId → NGO StartClient
In-run join: default OFF (join between runs only) — mid-run join is a scope trap
```

## Failure handling (Gate 4 tests all of these)

- **Client disconnect mid-run:** their character ragdolls and despawns after
  10 s; their held joints break gracefully; run continues.
- **Host quits/crashes:** clients get a clear "Host left" screen → back to menu.
  Host migration is OUT OF SCOPE (documented in store FAQ). Encourage the most
  stable connection to host via lobby UI hint.
- **Join failures:** every failure path shows a human-readable message
  (lobby full, version mismatch, Steam offline). Version-gate the lobby with
  the build id in lobby metadata.
- **Timeouts:** 10 s connection timeout with retry button, never an infinite
  spinner.

## Testing netcode without friends

- Multiplayer Play Mode (Unity 6) or ParrelSync-style clones for local testing —
  but Steam transport needs distinct Steam accounts, so keep a **UTP/local
  transport toggle** (`DEV_BUILD` only) for everyday iteration; test the real
  Facepunch transport at every gate with two real machines/accounts.
- Simulate latency + loss with Unity Transport's simulator pipeline at
  100 ms / 5% loss — the game must remain playable and funny.
- `AppId 480` (Spacewar) works for pre-Steamworks-approval lobby testing;
  switch to the real AppId the day it exists and never ship 480.

## What NOT to build

No accounts, no persistence beyond local save files, no matchmaking with
strangers by default (friends-first; a public lobby list is optional and cheap,
skill matchmaking is not), no anti-cheat (co-op vs. physics: cheaters only ruin
their own party), no chat text moderation (voice is Steam's problem, text chat
is friends-only or absent).
