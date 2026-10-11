using UnityEngine;

namespace WorldDecorationRenderer;

public struct DrawMeshInstGraphic
{
	public int layer;

	public Mesh mesh;

	public Material material;

	public DrawMeshInstGraphic(Mesh mesh, Material material, int layer)
	{
		this.mesh = mesh;
		this.material = material;
		this.layer = layer;
	}
}
