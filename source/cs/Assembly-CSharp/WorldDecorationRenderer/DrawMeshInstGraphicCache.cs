using System.Collections.Generic;
using UnityEngine;

namespace WorldDecorationRenderer;

public sealed class DrawMeshInstGraphicCache
{
	private readonly Dictionary<DrawKey, DrawMeshInstGraphic> _cache = new Dictionary<DrawKey, DrawMeshInstGraphic>(256);

	public DrawMeshInstGraphic GetOrCreate(in DrawKey drawKey, Mesh mesh, Material mat, int layer)
	{
		if (_cache.TryGetValue(drawKey, out var value))
		{
			return value;
		}
		value = new DrawMeshInstGraphic(mesh, mat, layer);
		_cache.Add(drawKey, value);
		return value;
	}

	public void Remove(in DrawKey drawKey)
	{
		_cache.Remove(drawKey);
	}

	public void Clear()
	{
		_cache.Clear();
	}
}
