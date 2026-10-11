using System;
using UnityEngine;
using UnityEngine.Rendering;
using WorldDecorationRenderer;

[Serializable]
public class DecorationRenderInfo
{
	public Mesh mesh;

	public int subMeshIndex;

	public Material material;

	public ShadowCastingMode shadowCastingMode;

	public bool receiveShadows;

	public AABB localBounds;

	public int layer;

	public DecorationTransformInfo localTransformInfo;

	public string texGuid;

	public string atlasName;

	[NonSerialized]
	public int texIndexInAtlas;

	[NonSerialized]
	public Vector4 texAtlasST;

	[NonSerialized]
	public DrawKey drawKey;
}
