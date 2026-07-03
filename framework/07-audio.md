# 07 — Audio: comedy's delivery mechanism

Half of every viral friendslop clip is the AUDIO: the scream, the crunch, the
silence before the fall. Audio is sourced free (05) and mixed with intent.

## Priorities (in order, budget time accordingly)

1. **Impact/failure sounds** — falls, breaks, bonks, splats. Exaggerated,
   slightly cartoonish, layered (thud + character grunt + debris). These are
   the clip soundtrack; iterate on them like gameplay.
2. **Character vocalizations** — effort grunts, panic yelps, landing "oof".
   Source: CC0 human vocalization packs on Freesound, or record ourselves
   (a $0 mic take, pitch-shifted, beats a mismatched library scream). Recorded
   own-voice assets are also a strong human-made signal.
3. **Proximity voice chat** — see `04-netcode.md`; it's netcode's job but
   audio-designer owns rolloff curves, occlusion filter tuning, and making
   voice sit above SFX in the mix.
4. **Interaction feedback** — grab, drop, throw, ping, UI. Kenney UI audio
   covers most of this.
5. **Ambience** — one loop per area/biome + a wind/weather layer. Cheap,
   massive presence gain.
6. **Music** — LAST and least: menu theme, results sting, optional low-key
   in-run tension layer. Friendslop runs are mostly music-free (voice chat is
   the music). 2–4 tracks total, CC-BY (credit) or public domain.

## Mixer architecture (fixed)

`Master → { Voice, SFX → {Impacts, Foley, UI}, Ambience, Music }`

- Exposed volume sliders for Voice / SFX / Music in settings (ship-check item).
- Sidechain-style duck: Ambience+Music duck −6 dB while Voice is active (script
  the duck off voice activity; Unity mixer snapshots are fine).
- Limiter on Master. Loudness sanity: dialogue-free game, aim ~ −16 LUFS
  gameplay average, impacts peaking well above bed. Test at streamer levels —
  clipping on stream kills clips.

## Implementation rules

- One pooled `AudioSource` player utility; no AudioSources added ad hoc.
- Every physics impact routes through a single `ImpactAudio` system: maps
  (mass × velocity) → clip family + volume + pitch. Random pitch ±10% on
  everything; nothing repeats identically twice.
- 3D spatial for world sounds (rolloff matched to voice so the soundscape is
  coherent); 2D only for UI + music.
- Keep source WAVs out of git if huge; import settings: Vorbis, quality 70%,
  Decompress-on-load only for tiny frequent clips.

## Silence design

The funniest sound is often none: a beat of silence at the start of a long fall
(cut ambience, keep voice) makes the impact land harder. Script the fall-silence
moment for any game with falling — it's our signature move.
