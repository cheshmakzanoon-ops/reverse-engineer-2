using System;
using UnityEngine;

[Serializable]
public class ZoneTypeData
{
	public int zoneType;

	public Mesh mesh;

	public Vector3 eulerAngles;

	public Vector2 meshOffset;

	public int zoneMeshType;

	public SubTileData[] subTiles;
}
