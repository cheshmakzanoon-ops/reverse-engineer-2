import UnityPy.config
UnityPy.config.FALLBACK_UNITY_VERSION="2019.4.41f1"
import UnityPy, os, sys, glob, json, traceback
SRC="bundles"
OUT="extracted/assets"
os.makedirs(OUT, exist_ok=True)
for sub in ["Textures","Audio","Meshes","TextAssets","Shaders","Sprites"]:
    os.makedirs(os.path.join(OUT,sub), exist_ok=True)
files=sorted(glob.glob(f"{SRC}/*.bundle"))
start=int(sys.argv[1]) if len(sys.argv)>1 else 0
end=int(sys.argv[2]) if len(sys.argv)>2 else len(files)
print(f"processing {start}:{end} of {len(files)}")
counts={"tex":0,"audio":0,"mesh":0,"text":0,"shader":0,"sprite":0,"err":0}
for idx in range(start, min(end,len(files))):
    p=files[idx]
    bname=os.path.splitext(os.path.basename(p))[0]
    try:
        env=UnityPy.load(p)
        for o in env.objects:
            t=o.type.name
            try:
                if t=="Texture2D":
                    d=o.read()
                    name=d.m_Name or f"{bname}_{o.path_id}"
                    # sanitize
                    name="".join(c if c.isalnum() or c in "._-" else "_" for c in name)
                    out=os.path.join(OUT,"Textures",f"{name}.png")
                    if not os.path.exists(out):
                        d.image.save(out)
                    counts["tex"]+=1
                elif t=="AudioClip":
                    d=o.read()
                    for aname,adata in d.samples.items():
                        out=os.path.join(OUT,"Audio",aname)
                        if not os.path.exists(out):
                            open(out,'wb').write(adata)
                        counts["audio"]+=1
                elif t=="Mesh":
                    d=o.read()
                    name=d.m_Name or f"{bname}_{o.path_id}"
                    name="".join(c if c.isalnum() or c in "._-" else "_" for c in name)
                    out=os.path.join(OUT,"Meshes",f"{name}.obj")
                    if not os.path.exists(out):
                        open(out,'w').write(d.export())
                    counts["mesh"]+=1
                elif t=="TextAsset":
                    d=o.read()
                    name=d.m_Name or f"{bname}_{o.path_id}"
                    name="".join(c if c.isalnum() or c in "._-" else "_" for c in name)
                    out=os.path.join(OUT,"TextAssets",f"{name}.bytes")
                    if not os.path.exists(out):
                        data=d.script if isinstance(d.script,bytes) else str(d.script).encode()
                        open(out,'wb').write(data)
                    counts["text"]+=1
                elif t=="Shader":
                    d=o.read()
                    name=getattr(d,'m_ParsedForm',None) and getattr(d.m_ParsedForm,'m_Name',None) or getattr(d,'m_Name',f"{bname}_{o.path_id}")
                    name="".join(c if c.isalnum() or c in "._-" else "_" for c in str(name))
                    out=os.path.join(OUT,"Shaders",f"{name}.shader")
                    if not os.path.exists(out):
                        open(out,'w',errors='ignore').write(str(d.script) if hasattr(d,'script') else str(d))
                    counts["shader"]+=1
                elif t=="Sprite":
                    d=o.read()
                    try:
                        img=d.image
                        if img:
                            name=d.m_Name or f"{bname}_{o.path_id}"
                            name="".join(c if c.isalnum() or c in "._-" else "_" for c in name)
                            out=os.path.join(OUT,"Sprites",f"{name}.png")
                            if not os.path.exists(out):
                                img.save(out)
                            counts["sprite"]+=1
                    except: pass
            except Exception as e:
                pass
    except Exception as e:
        counts["err"]+=1
    if (idx-start)%200==0:
        print(f"{idx} {counts}")
print("DONE", counts)
