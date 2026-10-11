"""Decrypt Unity .mdl (XOR header) -> .dll. Key = first_byte ^ 0x4D, decrypt first `key` bytes."""
import glob, os, sys
src=sys.argv[1] if len(sys.argv)>1 else "assets/Assemblies"
out=sys.argv[2] if len(sys.argv)>2 else "source/dlls"
os.makedirs(out, exist_ok=True)
for f in glob.glob(f"{src}/*.mdl"):
    data=bytearray(open(f,'rb').read())
    key=data[0]^0x4D
    for i in range(key): data[i]^=key
    assert data[:4]==b'MZ\x90\x00', f
    open(os.path.join(out, os.path.basename(f).replace('.mdl','.dll')),'wb').write(data)
print("done")
