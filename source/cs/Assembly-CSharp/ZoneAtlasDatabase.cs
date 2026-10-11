using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Zone/ZoneAtlasDatabase")]
public class ZoneAtlasDatabase : ScriptableObject
{
	[Header("Tile Size")]
	public int tileSize = 50;

	public float tileScale = 1f;

	public float textureScale = 15f / 128f;

	public Vector2 zonePosOffset = new Vector2(0.5f, 0.5f);

	public Material ZoneRTMaterial;

	public ZoneTypeData[] zoneDatas;

	public float zoneCampChangeTime = 1f;

	public Material terrainRTMaterial;

	public float terrainMeshSize = 1f;

	private Dictionary<int, ZoneTypeData> _map;

	public ZoneTypeData Get(int t)
	{
		if (_map == null)
		{
			_map = BuildMap();
		}
		if (!_map.TryGetValue(t, out var value))
		{
			return null;
		}
		return value;
	}

	private Dictionary<int, ZoneTypeData> BuildMap()
	{
		if (zoneDatas != null && zoneDatas.Length != 0)
		{
			Dictionary<int, ZoneTypeData> dictionary = new Dictionary<int, ZoneTypeData>(zoneDatas.Length);
			ZoneTypeData[] array = zoneDatas;
			foreach (ZoneTypeData zoneTypeData in array)
			{
				if (zoneTypeData != null)
				{
					dictionary[zoneTypeData.zoneType] = zoneTypeData;
				}
			}
			return dictionary;
		}
		return null;
	}
}
