using System;
using UnityEngine;

namespace WorldDecorationRenderer;

public readonly struct DrawKey : IEquatable<DrawKey>
{
	public readonly int meshId;

	public readonly int matId;

	public readonly int layer;

	public DrawKey(Mesh mesh, Material mat, int layer)
	{
		meshId = (mesh ? mesh.GetInstanceID() : 0);
		matId = (mat ? mat.GetInstanceID() : 0);
		this.layer = layer;
	}

	public bool Equals(DrawKey other)
	{
		if (meshId == other.meshId && matId == other.matId)
		{
			return layer == other.layer;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is DrawKey other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (meshId * 397) ^ matId ^ (layer * 31);
	}
}
