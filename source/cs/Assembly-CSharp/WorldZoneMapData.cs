using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VEngine;

public class WorldZoneMapData
{
	public int width;

	public int height;

	private short[] gridToZone;

	private ushort[] gridToZoneIdArray;

	private short[] landlordGridToZone;

	public int mapPointCount;

	public SeasonType mSeasonType;

	public Dictionary<int, WorldZoneData> zones = new Dictionary<int, WorldZoneData>(256);

	private static readonly Dictionary<SeasonType, WorldZoneMapData> __cache__ = new Dictionary<SeasonType, WorldZoneMapData>();

	private static bool _isLoadingLandlordData;

	public bool isInitLandlordData;

	public void Load(BinaryReader br, SeasonType seasonType = SeasonType.Unknown)
	{
		width = br.ReadInt32();
		height = br.ReadInt32();
		br.ReadInt32();
		if (seasonType == SeasonType.NineNationRainforest)
		{
			gridToZoneIdArray = LoadZoneIdData(width, height, br);
		}
		mSeasonType = seasonType;
		int num = br.ReadInt32();
		zones.Clear();
		for (int i = 0; i < num; i++)
		{
			WorldZoneData worldZoneData = new WorldZoneData();
			worldZoneData.Load(br);
			zones.Add(worldZoneData.ZoneId, worldZoneData);
		}
		mapPointCount = br.ReadInt32();
		if (mapPointCount == 0)
		{
			return;
		}
		if (width == 3000 && height == 3000)
		{
			gridToZone = null;
			return;
		}
		gridToZone = new short[mapPointCount + 1];
		int num2 = br.ReadInt32();
		for (int j = 0; j < num2; j++)
		{
			int num3 = br.ReadInt32();
			short num4 = br.ReadInt16();
			short num5 = br.ReadInt16();
			for (int k = 1; k <= num4; k++)
			{
				gridToZone[num3 + k] = num5;
			}
		}
	}

	public ushort[] LoadZoneIdData(int width, int height, BinaryReader br)
	{
		int num = br.ReadInt32();
		ushort[] array = new ushort[num];
		for (int i = 0; i < num; i++)
		{
			ushort num2 = br.ReadUInt16();
			array[i] = num2;
		}
		return array;
	}

	public WorldZoneData GetZoneData(int zoneId)
	{
		if (zones.TryGetValue(zoneId, out var value))
		{
			return value;
		}
		return null;
	}

	public void InitLandlordGridToZone(BinaryReader br)
	{
		br.ReadInt32();
		br.ReadInt32();
		br.ReadInt32();
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			WorldZoneData worldZoneData = new WorldZoneData();
			worldZoneData.Load(br);
			zones.Add(worldZoneData.ZoneId, worldZoneData);
		}
		mapPointCount = br.ReadInt32();
		if (mapPointCount == 0)
		{
			return;
		}
		landlordGridToZone = new short[mapPointCount + 1];
		int num2 = br.ReadInt32();
		for (int j = 0; j < num2; j++)
		{
			int num3 = br.ReadInt32();
			short num4 = br.ReadInt16();
			short num5 = br.ReadInt16();
			for (int k = 1; k <= num4; k++)
			{
				landlordGridToZone[num3 + k] = num5;
			}
		}
	}

	public void ClearCenterServerData()
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, WorldZoneData> zone in zones)
		{
			int cityPos = zone.Value.CityPos;
			int num = cityPos % 3000;
			int num2 = cityPos / 3000;
			if (num < 2000 && num >= 1000 && num2 < 2000 && num2 >= 1000)
			{
				list.Add(zone.Key);
			}
		}
		foreach (int item in list)
		{
			zones.Remove(item);
		}
	}

	public WorldZoneData GetZoneByPosId(int pointId)
	{
		int num = -1;
		if (width == 3000 && height == 3000)
		{
			if (LandlordManager.Instance.isLandlordActOpenAndNewMap)
			{
				int num2 = pointId % 3000;
				int num3 = pointId / 3000;
				if (landlordGridToZone != null && num2 >= 1000 && num2 < 2000 && num3 >= 1000 && num3 < 2000)
				{
					int num4 = (num3 - 1000) * 1000 + (num2 - 1000);
					if (num4 > 0 && num4 < landlordGridToZone.Length)
					{
						num = landlordGridToZone[num4];
						return GetZoneData(num);
					}
				}
			}
			if (gridToZone == null)
			{
				int curServerId = GameEntry.Data.Player.GetCurServerId();
				Vector3 worldPos = TileCoord.TileIndexToWorld(pointId, ForceChangeScene.World, curServerId);
				return GetZoneIdByWorldPos(worldPos);
			}
		}
		if (gridToZone == null || pointId < 0 || pointId >= gridToZone.Length)
		{
			return null;
		}
		num = gridToZone[pointId];
		return GetZoneData(num);
	}

	public int GetZoneIdByTilePos(int tileX, int tileY)
	{
		if (gridToZoneIdArray == null)
		{
			return -1;
		}
		if (tileX < 0 || tileY < 0)
		{
			return -1;
		}
		int num = tileY / 25;
		int num2 = tileX / 25;
		int num3 = num * 120 + num2;
		if (num3 < 0 || num3 >= gridToZoneIdArray.Length)
		{
			return -1;
		}
		return gridToZoneIdArray[num3];
	}

	public int CalcZoneId3000(int tileX, int tileY)
	{
		int result = -1;
		if (width == 3000 && height == 3000)
		{
			int num = tileX % 1000;
			int num2 = tileY % 1000;
			int num3 = tileX / 1000;
			int num4 = tileY / 1000;
			if (num2 >= 25 && num2 < 975 && num >= 25 && num < 975)
			{
				int num5 = (num2 - 25) / 50;
				int num6 = (num - 25) / 50;
				if (num5 % 2 == 0 && num6 % 2 == 0)
				{
					result = 34 + 663 * num4 + 10 * num3 + num6 / 2 + num5 / 2 * 63;
					goto IL_01d0;
				}
			}
			if (num2 < 50)
			{
				if (num < 50)
				{
					result = 1 + 663 * num4 + 11 * num3;
				}
				else if (num > 950)
				{
					result = 11 + 663 * num4 + 11 * num3;
				}
				else
				{
					int num7 = (num - 50) / 100;
					result = 2 + 663 * num4 + 11 * num3 + num7;
				}
			}
			else if (num2 > 950)
			{
				if (num < 50)
				{
					result = 631 + 663 * num4 + 11 * num3;
				}
				else if (num > 950)
				{
					result = 641 + 663 * num4 + 11 * num3;
				}
				else
				{
					int num8 = (num - 50) / 100;
					result = 632 + 663 * num4 + 11 * num3 + num8;
				}
			}
			else
			{
				int num9 = (num2 - 50) / 100;
				if (num < 50)
				{
					result = 64 + 663 * num4 + 11 * num3 + num9 * 63;
				}
				else if (num > 950)
				{
					result = 74 + 663 * num4 + 11 * num3 + num9 * 63;
				}
				else
				{
					int num10 = (num - 50) / 100;
					result = 65 + 663 * num4 + 11 * num3 + num9 * 63 + num10;
				}
			}
		}
		goto IL_01d0;
		IL_01d0:
		return result;
	}

	public WorldZoneData GetZoneIdByWorldPos(Vector3 worldPos)
	{
		int num = -1;
		int num2 = Mathf.Clamp((int)(worldPos.x / 2f), 0, width - 1);
		int num3 = Mathf.Clamp((int)(worldPos.z / 2f), 0, height - 1);
		if (landlordGridToZone != null && num2 >= 1000 && num2 < 2000 && num3 >= 1000 && num3 < 2000)
		{
			int num4 = (num3 - 1000) * 1000 + (num2 - 1000) + 1;
			if (num4 > 0 && num4 < landlordGridToZone.Length)
			{
				num = landlordGridToZone[num4];
				return GetZoneData(num);
			}
		}
		if (gridToZone == null && width == 3000 && height == 3000)
		{
			num = ((mSeasonType != SeasonType.NineNationRainforest) ? CalcZoneId3000(num2, num3) : GetZoneIdByTilePos(num2, num3));
			if (num > 0)
			{
				return GetZoneData(num);
			}
		}
		int num5 = num3 * width + num2 + 1;
		if (gridToZone == null || num5 < 0 || num5 >= gridToZone.Length)
		{
			return null;
		}
		num = gridToZone[num5];
		return GetZoneData(num);
	}

	public static WorldZoneMapData TryLoadWorldZoneMapData(SeasonType type, bool async, Action<WorldZoneMapData> callback)
	{
		WorldZoneMapData worldZoneMapData = null;
		if (__cache__.ContainsKey(type))
		{
			worldZoneMapData = __cache__[type];
		}
		else
		{
			string path = "Assets/Main/Scenes/Zone/zone.bytes";
			switch (type)
			{
			case SeasonType.NineNationRainforest:
				path = "Assets/Main/SeasonRes/S6/Scenes/Zone/zone_S6.bytes";
				break;
			case SeasonType.NineNation:
				path = "Assets/Main/SeasonRes/S5/Scenes/Zone/zone_S5.bytes";
				break;
			case SeasonType.Darkness:
				path = "Assets/Main/SeasonRes/S4/Scenes/Zone/zone_S4.bytes";
				break;
			case SeasonType.Mummy:
				path = "Assets/Main/SeasonRes/S3/Scenes/Zone/zone_S3.bytes";
				break;
			case SeasonType.Snow:
				path = "Assets/Main/SeasonRes/S2/Scenes/zone_S2.bytes";
				if (!GameEntry.Resource.HasAsset(path))
				{
					path = "Assets/Main/Scenes/Zone/zone_S2.bytes";
				}
				break;
			case SeasonType.CityStronghold:
				path = "Assets/Main/SeasonRes/S1/Scenes/zone_S1.bytes";
				if (!GameEntry.Resource.HasAsset(path))
				{
					path = "Assets/Main/Scenes/Zone/zone_S1.bytes";
				}
				break;
			}
			Asset asset = null;
			asset = ((!async) ? GameEntry.Resource.LoadAsset(path, typeof(TextAsset)) : GameEntry.Resource.LoadAssetAsync(path, typeof(TextAsset)));
			Asset asset2 = asset;
			asset2.completed = (Action<Asset>)Delegate.Combine(asset2.completed, (Action<Asset>)delegate(Asset req)
			{
				TextAsset textAsset = req.asset as TextAsset;
				if (textAsset != null)
				{
					using MemoryStream memoryStream = new MemoryStream(textAsset.bytes);
					BinaryReader binaryReader = new BinaryReader(memoryStream);
					memoryStream.Seek(0L, SeekOrigin.Begin);
					WorldZoneMapData worldZoneMapData2 = new WorldZoneMapData();
					worldZoneMapData2.Load(binaryReader, type);
					binaryReader.Close();
					if (!__cache__.ContainsKey(type))
					{
						__cache__.Add(type, worldZoneMapData2);
					}
					if (!CheckLandlordData((int)type, worldZoneMapData2, isAsync: true, callback))
					{
						callback?.Invoke(worldZoneMapData2);
					}
				}
				req.Release();
			});
			if (__cache__.ContainsKey(type))
			{
				worldZoneMapData = __cache__[type];
			}
		}
		if (worldZoneMapData != null && callback != null && !CheckLandlordData((int)type, worldZoneMapData, isAsync: true, callback))
		{
			callback(worldZoneMapData);
		}
		return worldZoneMapData;
	}

	public static int GetZoneIdByWorldPos(Vector3 worldPos, int type)
	{
		int result = 0;
		WorldZoneMapData worldZoneMapData = TryLoadWorldZoneMapData((SeasonType)type, async: false, null);
		if (worldZoneMapData != null)
		{
			WorldZoneData zoneIdByWorldPos = worldZoneMapData.GetZoneIdByWorldPos(worldPos);
			if (zoneIdByWorldPos != null)
			{
				result = zoneIdByWorldPos.ZoneId;
			}
		}
		return result;
	}

	public static int GetZoneIdByPosId(int pointId, int type)
	{
		int result = 0;
		WorldZoneMapData worldZoneMapData = TryLoadWorldZoneMapData((SeasonType)type, async: false, null);
		if (worldZoneMapData != null)
		{
			WorldZoneData zoneByPosId = worldZoneMapData.GetZoneByPosId(pointId);
			if (zoneByPosId != null)
			{
				result = zoneByPosId.ZoneId;
			}
		}
		return result;
	}

	public static bool CheckLandlordData(int type, WorldZoneMapData data, bool isAsync, Action<WorldZoneMapData> callback = null)
	{
		if (type == 6 && LandlordManager.Instance.isLandlordActOpenAndNewMap)
		{
			return TryLoadLandlordMapData(isAsync, callback);
		}
		return false;
	}

	public static bool TryLoadLandlordMapData(bool async, Action<WorldZoneMapData> callback)
	{
		if (LandlordManager.Instance.isLandlordActOpenAndNewMap)
		{
			if (__cache__.TryGetValue(SeasonType.NineNation, out var value) && value.isInitLandlordData)
			{
				callback?.Invoke(value);
				return false;
			}
			if (_isLoadingLandlordData)
			{
				return true;
			}
			Asset asset = null;
			asset = ((!async) ? GameEntry.Resource.LoadAsset("Assets/Main/SeasonRes/S5/Scenes/Zone_Landlord/zone.bytes", typeof(TextAsset)) : GameEntry.Resource.LoadAssetAsync("Assets/Main/SeasonRes/S5/Scenes/Zone_Landlord/zone.bytes", typeof(TextAsset)));
			if (asset == null)
			{
				return false;
			}
			_isLoadingLandlordData = true;
			Asset asset2 = asset;
			asset2.completed = (Action<Asset>)Delegate.Combine(asset2.completed, (Action<Asset>)delegate(Asset req)
			{
				try
				{
					TextAsset textAsset = req.asset as TextAsset;
					if (textAsset != null && _isLoadingLandlordData)
					{
						using (MemoryStream memoryStream = new MemoryStream(textAsset.bytes))
						{
							BinaryReader binaryReader = new BinaryReader(memoryStream);
							memoryStream.Seek(0L, SeekOrigin.Begin);
							if (__cache__.ContainsKey(SeasonType.NineNation))
							{
								WorldZoneMapData worldZoneMapData = __cache__[SeasonType.NineNation];
								worldZoneMapData.ClearCenterServerData();
								worldZoneMapData.InitLandlordGridToZone(binaryReader);
								callback?.Invoke(worldZoneMapData);
								worldZoneMapData.isInitLandlordData = true;
							}
							binaryReader.Close();
							return;
						}
					}
				}
				finally
				{
					_isLoadingLandlordData = false;
					req.Release();
				}
			});
			return true;
		}
		return false;
	}

	public static void ClearLandlordCache()
	{
		if (__cache__.TryGetValue(SeasonType.NineNation, out var value) && value.isInitLandlordData)
		{
			__cache__.Remove(SeasonType.NineNation);
		}
		_isLoadingLandlordData = false;
	}

	public static void RefreshLandlordMapData(Action<WorldZoneMapData> callback)
	{
		WorldZoneMapData worldZoneMapData = null;
		if (__cache__.ContainsKey(SeasonType.NineNation))
		{
			worldZoneMapData = __cache__[SeasonType.NineNation];
		}
		if (worldZoneMapData != null)
		{
			CheckLandlordData(6, worldZoneMapData, isAsync: true, callback);
		}
	}
}
