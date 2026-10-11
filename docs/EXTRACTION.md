# Extraction notes

Unity version: 2019.4.41f1 (from globalgamemanagers)
Build: Mono (assets/Assemblies/*.mdl + libmonoboehm), XLua, XAssetPro (VEngine/LocalBundleOffsetMapper)

## 1. DLL decrypt (.mdl -> .dll)
Each .mdl: key K = byte0 ^ 0x4D ('M'). Decrypt only first K bytes with XOR K.
Verified on all 117 files (MZ + e_lfanew -> PE).
See tools/decrypt_mdl.py.

Decompiled with ilspycmd 9.1.0.7988 (.NET 8):
ilspycmd -p -o source/cs/<Name> source/dlls/<Name>.dll
System/UnityEngine/Mono framework DLLs skipped (standard, not game code).

## 2. Bundles
assets/AssetBundles/BundleFragment0.bytes (522MB) is concatenation of 7,868 UnityFS bundles.
Index: BundleOffsetTable.bytes / AliasOffsetTable.bytes
Outer: int32 fragCount, frag names (C# BinaryReader strings), int32 tableLen, tableBytes
Inner tableBytes: int32 count, per entry: string bundleName, int64 packed (low32=offset, high32=fragIdx)
Carve by sorted offsets (last to EOF). Each chunk starts with UnityFS.
Real names from BundleOffsetTable; hashed aliases from AliasOffsetTable (same offsets).

## 3. Unity assets
UnityPy with FALLBACK_UNITY_VERSION=2019.4.41f1.
- Texture2D/Sprite -> .image.save PNG
- AudioClip -> .samples dict -> WAV bytes
- Mesh -> .export() -> OBJ
- TextAsset/Shader -> raw/script
See tools/extract_assets.py.

## 4. Lua
assets/lwScripts/LWScripts.data: LWLF magic, int32 fileVersion=1 (Original, not Encrypted=2), int32 version=759, int32 count=18300.
Per entry: C# string name (.luac path), int32 length, bytes (XLua compiled, header 1b 4c 75 61 53).
No SuperDecrypt needed (fileVersion 1).

## 5. Table / locale
table_*.data is ZIP (1,275 files). locale/23175/*.bin is gzip (19 langs).
bin/Data hashed files are Unity SerializedFiles (UI prefabs); Textures merged into assets/extracted.

## 6. LFS
Binaries via Git LFS (.gitattributes): dll, bundle, png/jpg, wav/mp3/ogg, obj/fbx, luac, bin/bytes, shader, ttf, mp4.
