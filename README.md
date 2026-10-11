# reverse-engineer-2 — Unity APK recovery

Unity 2019.4.41f1 Mono build. Authorized recovery — full asset + source extraction.

## Layout
- `source/cs/` — decompiled C# (ILSpy 9.1, 117 DLLs decrypted, ~9600 files; System/UnityEngine framework skipped)
- `source/dlls/` — decrypted .NET DLLs (LFS)
- `source/lua/` — 18,300 XLua `.luac` (LWLF v1, game version 759)
- `assets/bundles/` — 7,868 carved UnityFS bundles from `BundleFragment0.bytes` (LFS)
- `assets/extracted/` — Textures/PNG, Sprites, Audio/WAV, Meshes/OBJ, Shaders, TextAssets
- `assets/table/` — 1,275 game data tables (zip)
- `assets/locale/` — 19 decompressed locale bins (gzip)
- `meta/` — VERSION.txt, gameres/dllres manifests, offset-table versions
- `docs/EXTRACTION.md` — methods
- `tools/` — decrypt/carve/extract scripts

## Key findings
- `.mdl` = DLL with first `K` bytes XOR `K`, `K = data[0] ^ 0x4D`
- `BundleFragment0.bytes` = 7,868 concatenated UnityFS bundles, indexed by `BundleOffsetTable.bytes` / `AliasOffsetTable.bytes` (fragment, offset)
- `LWScripts.data` = LWLF: magic, fileVersion=1 (unencrypted), version=759, 18,300 entries (name + length + luac)
- `table_*.data` = ZIP; `locale/*.bin` = gzip
- Audio via UnityPy `AudioClip.samples` → WAV; Textures → PNG; Meshes → OBJ

See `docs/EXTRACTION.md`.
