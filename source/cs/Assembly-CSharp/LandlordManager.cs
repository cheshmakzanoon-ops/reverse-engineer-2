using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class LandlordManager
{
	private static LandlordManager _instance;

	private string _cityTableName;

	private bool _isNewCenterMap;

	private readonly List<Vector2> _speedCalculateList = new List<Vector2>();

	private bool _speedInit;

	private const string CenterMapRandomFxPrefabPath = "Assets/Main/Prefabs/World/BuildFireEffectMainBuild.prefab";

	private const int CenterMapRandomFxCount = 20;

	private const int CenterMapRandomFxMinTile = 1000;

	private const int CenterMapRandomFxMaxTileExclusive = 2000;

	private const int CenterMapFxScaleFactor = 30;

	private const float CenterMapRandomFxCheckInterval = 1f;

	private bool _centerMapRandomFxInited;

	private bool _centerMapRandomFxVisible;

	private bool _centerMapRandomFxPathWarned;

	private ITimer _centerMapRandomFxTimer;

	private readonly List<InstanceRequest> _centerMapRandomFxReqs = new List<InstanceRequest>();

	public static LandlordManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new LandlordManager();
			}
			return _instance;
		}
	}

	public string cityTableName
	{
		get
		{
			if (_cityTableName == null)
			{
				_cityTableName = GameEntry.Lua.CallWithReturn<string>("CSharpCallLuaInterface.GetLLCurCityTableName");
			}
			return _cityTableName;
		}
	}

	public bool isLandlordActOpenAndNewMap
	{
		get
		{
			if (!string.IsNullOrEmpty(cityTableName))
			{
				return isNewCenterMap;
			}
			return false;
		}
	}

	public bool isNewCenterMap => _isNewCenterMap;

	public int myCampId { get; set; }

	public long previewBoomTime { get; set; }

	public int centerServerId { get; set; }

	public static void Purge()
	{
		_instance?.DisposeCenterMapRandomFxSystem();
		ClearLandlordCache();
		_instance = null;
	}

	public static void ClearLandlordCache()
	{
		WorldZoneMapData.ClearLandlordCache();
		WorldZoneEdgeDataCache.CleanLandlordCache();
	}

	public void RefreshTableNameFromWorld()
	{
		_cityTableName = GameEntry.Lua.CallWithReturn<string>("CSharpCallLuaInterface.GetLLCurCityTableName");
	}

	public void SetIsNewCenterMapFlag(bool isNewCenterMapValue, bool isFromSrcServerPush)
	{
		bool num = _isNewCenterMap;
		_isNewCenterMap = isNewCenterMapValue;
		RefreshCenterMapRandomFx();
		if (num != isNewCenterMapValue && isNewCenterMapValue && isFromSrcServerPush)
		{
			WorldScene worldScene = SceneManager.World as WorldScene;
			if (worldScene != null)
			{
				worldScene.OnSkinChangeByLandlordData();
			}
		}
	}

	private void InitSpeedFromConfig()
	{
		if (_speedInit)
		{
			return;
		}
		_speedInit = true;
		_speedCalculateList.Clear();
		string text = GameEntry.Lua.CallWithReturn<string, string, string, string>("CSharpCallLuaInterface.GetConfigStr", "zonewar_landlord", "k2", string.Empty);
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		string[] array = text.Split(new char[1] { '|' });
		foreach (string text2 in array)
		{
			if (!string.IsNullOrEmpty(text2))
			{
				string[] array2 = text2.Split(new char[1] { ';' });
				if (array2.Length >= 3 && float.TryParse(array2[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var result) && float.TryParse(array2[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var result2) && result > 0f && result2 > 0f)
				{
					_speedCalculateList.Add(new Vector2(result, result2));
				}
			}
		}
	}

	public void CalculateOccupyCurProgress(long startOccupyTime, float startOccupyProgress, float maxProgress, int campId, float extraEffectValue, out int curProgress, out int timeToComplete)
	{
		curProgress = 0;
		timeToComplete = 0;
		if (_speedCalculateList.Count == 0)
		{
			InitSpeedFromConfig();
		}
		if (_speedCalculateList.Count == 0)
		{
			curProgress = Mathf.RoundToInt(startOccupyProgress);
			timeToComplete = 0;
			return;
		}
		bool flag = campId == 1;
		float num = (float)(GameEntry.Timer.GetServerTime() - startOccupyTime) / 1000f;
		if (num <= 0f)
		{
			float f = 0f;
			if (flag)
			{
				float num2 = startOccupyProgress;
				if (num2 > 0f)
				{
					f = CalculateRemainingTime(0f, num2, extraEffectValue);
				}
			}
			else
			{
				float num2 = maxProgress - startOccupyProgress;
				if (num2 > 0f)
				{
					f = CalculateRemainingTime(0f, num2, extraEffectValue);
				}
			}
			curProgress = Mathf.RoundToInt(startOccupyProgress);
			timeToComplete = Mathf.RoundToInt(f);
			return;
		}
		float num3 = 1f + extraEffectValue;
		float num4 = 0f;
		float num5 = num;
		float num6 = 0f;
		for (int i = 0; i < _speedCalculateList.Count; i++)
		{
			if (num5 <= 0f)
			{
				break;
			}
			float x = _speedCalculateList[i].x;
			float y = _speedCalculateList[i].y;
			if (num6 < x)
			{
				float num7 = ((i < _speedCalculateList.Count - 1) ? _speedCalculateList[i + 1].x : float.PositiveInfinity);
				float num8 = num6;
				float num9 = Mathf.Min(num6 + num5, x, num7);
				if (num9 > num8)
				{
					float num10 = num9 - num8;
					num4 += y * num10 * num3;
					num5 -= num10;
					num6 = num9;
				}
			}
		}
		if (num5 > 0f)
		{
			float y2 = _speedCalculateList[_speedCalculateList.Count - 1].y;
			num4 += y2 * num5 * num3;
		}
		float f2 = 0f;
		float b;
		if (flag)
		{
			b = startOccupyProgress - num4;
			b = Mathf.Max(0f, b);
			float num11 = b;
			if (num11 > 0f)
			{
				f2 = CalculateRemainingTime(num, num11, extraEffectValue);
			}
		}
		else
		{
			b = Mathf.Min(startOccupyProgress + num4, maxProgress);
			float num11 = maxProgress - b;
			if (num11 > 0f)
			{
				f2 = CalculateRemainingTime(num, num11, extraEffectValue);
			}
		}
		curProgress = Mathf.RoundToInt(b);
		timeToComplete = Mathf.RoundToInt(f2);
	}

	private int CalculateRemainingTime(float startDuration, float targetProgress, float extraEffectValue)
	{
		if (_speedCalculateList.Count == 0)
		{
			InitSpeedFromConfig();
		}
		if (_speedCalculateList.Count == 0)
		{
			return 0;
		}
		float num = 1f + extraEffectValue;
		float num2 = targetProgress;
		float num3 = 0f;
		float num4 = startDuration;
		for (int i = 0; i < _speedCalculateList.Count; i++)
		{
			if (num2 <= 0f)
			{
				break;
			}
			float x = _speedCalculateList[i].x;
			float y = _speedCalculateList[i].y;
			if (num4 < x)
			{
				float b = ((i < _speedCalculateList.Count - 1) ? _speedCalculateList[i + 1].x : float.PositiveInfinity);
				float num5 = Mathf.Min(x, b);
				float num6 = num5 - num4;
				float num7 = y * num;
				if (num7 <= 0f)
				{
					break;
				}
				float num8 = num2 / num7;
				if (num8 <= num6)
				{
					num3 += num8;
					num2 = 0f;
					break;
				}
				float num9 = num7 * num6;
				num3 += num6;
				num2 -= num9;
				num4 = num5;
			}
		}
		if (num2 > 0f)
		{
			float num10 = _speedCalculateList[_speedCalculateList.Count - 1].y * num;
			if (num10 > 0f)
			{
				num3 += num2 / num10;
			}
		}
		return Mathf.RoundToInt(num3);
	}

	public void EnsureCenterMapRandomFxSystem()
	{
		if (isNewCenterMap || previewBoomTime <= 0)
		{
			if (_centerMapRandomFxInited)
			{
				DisposeCenterMapRandomFxSystem();
			}
		}
		else if (!_centerMapRandomFxInited)
		{
			_centerMapRandomFxInited = true;
			GameEntry.Event.Subscribe(EventId.ChangeCameraLod, OnCenterMapRandomFxLodChanged);
			_centerMapRandomFxTimer = GameEntry.Timer.RegisterTimerRepeat(0f, 1f, RefreshCenterMapRandomFx);
		}
	}

	public void DisposeCenterMapRandomFxSystem()
	{
		if (_centerMapRandomFxInited)
		{
			_centerMapRandomFxInited = false;
			GameEntry.Event.Unsubscribe(EventId.ChangeCameraLod, OnCenterMapRandomFxLodChanged);
		}
		if (_centerMapRandomFxTimer != null)
		{
			GameEntry.Timer.CancelTimer(_centerMapRandomFxTimer);
			_centerMapRandomFxTimer = null;
		}
		DestroyCenterMapRandomFxInstances();
	}

	private void OnCenterMapRandomFxLodChanged(object userdata)
	{
		RefreshCenterMapRandomFx();
	}

	private void RefreshCenterMapRandomFx()
	{
		if (!ShouldShowCenterMapRandomFx())
		{
			SetCenterMapRandomFxVisible(visible: false);
			if (isNewCenterMap)
			{
				DisposeCenterMapRandomFxSystem();
			}
		}
		else
		{
			if (_centerMapRandomFxReqs.Count == 0)
			{
				CreateCenterMapRandomFxInstances();
			}
			SetCenterMapRandomFxVisible(visible: true);
		}
	}

	private bool ShouldShowCenterMapRandomFx()
	{
		if (string.IsNullOrEmpty(cityTableName) || isNewCenterMap)
		{
			return false;
		}
		WorldScene worldScene = SceneManager.World as WorldScene;
		if (worldScene == null)
		{
			return false;
		}
		if (worldScene.CurrentLodLevel < 5)
		{
			return false;
		}
		if (centerServerId == 0 || GameEntry.Data.Player.GetCurServerId() != centerServerId)
		{
			return false;
		}
		if (previewBoomTime <= 0)
		{
			return false;
		}
		return GameEntry.Timer.GetServerTime() >= previewBoomTime;
	}

	private void CreateCenterMapRandomFxInstances()
	{
		WorldScene worldScene = SceneManager.World as WorldScene;
		if (worldScene == null)
		{
			return;
		}
		if (string.IsNullOrEmpty("Assets/Main/Prefabs/World/BuildFireEffectMainBuild.prefab"))
		{
			if (!_centerMapRandomFxPathWarned)
			{
				_centerMapRandomFxPathWarned = true;
				Debug.LogWarning("[LandlordCenterRandomFx] Prefab path is empty.");
			}
			return;
		}
		for (int i = 0; i < 20; i++)
		{
			int tilePosX = Random.Range(1000, 2000);
			int tilePosY = Random.Range(1000, 2000);
			Vector3 worldPos = TileCoord.TileToWorld(tilePosX, tilePosY, 0);
			InstanceRequest req = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/World/BuildFireEffectMainBuild.prefab");
			if (req == null)
			{
				continue;
			}
			_centerMapRandomFxReqs.Add(req);
			req.completed += delegate
			{
				GameObject gameObject = req.gameObject;
				if (gameObject != null && worldScene != null && worldScene.DynamicObjNode != null)
				{
					gameObject.transform.SetParent(worldScene.DynamicObjNode);
					gameObject.transform.position = worldPos;
					gameObject.transform.localScale = Vector3.one * 30f;
					gameObject.SetActive(_centerMapRandomFxVisible);
				}
			};
		}
	}

	private void SetCenterMapRandomFxVisible(bool visible)
	{
		_centerMapRandomFxVisible = visible;
		for (int i = 0; i < _centerMapRandomFxReqs.Count; i++)
		{
			InstanceRequest instanceRequest = _centerMapRandomFxReqs[i];
			if (instanceRequest != null && instanceRequest.gameObject != null)
			{
				instanceRequest.gameObject.SetActive(visible);
			}
		}
	}

	private void DestroyCenterMapRandomFxInstances()
	{
		for (int i = 0; i < _centerMapRandomFxReqs.Count; i++)
		{
			_centerMapRandomFxReqs[i]?.Destroy();
		}
		_centerMapRandomFxReqs.Clear();
	}

	public static bool TriggerCityExplosion(int pointIndex)
	{
		WorldScene worldScene = SceneManager.World as WorldScene;
		if (worldScene == null)
		{
			Debug.LogWarning("[LandlordCityGM] WorldScene未初始化");
			return false;
		}
		PointInfo pointInfo = worldScene.GetPointInfo(pointIndex);
		if (pointInfo == null)
		{
			Debug.LogWarning($"[LandlordCityGM] 未找到点位索引为 {pointIndex} 的点位");
			return false;
		}
		if (!(pointInfo is LLAllyCityPointInfo lLAllyCityPointInfo))
		{
			Debug.LogWarning($"[LandlordCityGM] 点位 {pointIndex} 不是金脉城市");
			return false;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		lLAllyCityPointInfo.ownerCampId = 2;
		lLAllyCityPointInfo.state = 4;
		lLAllyCityPointInfo.overTime = serverTime;
		lLAllyCityPointInfo.UpdateFSM();
		GameEntry.Event.Fire(EventId.LandlordCityPointChangeClientState, lLAllyCityPointInfo.pointIndex);
		int prepareBoomTime = lLAllyCityPointInfo.prepareBoomTime;
		int num = 10;
		Debug.Log("[LandlordCityGM] 城市爆炸流程已触发 - " + $"CityId:{lLAllyCityPointInfo.cityId}, " + $"PointIndex:{lLAllyCityPointInfo.pointIndex}, " + $"PrepareBoomTime:{prepareBoomTime}秒, " + $"BoomingTime:{num}秒, " + $"当前状态:{lLAllyCityPointInfo.curClientState}");
		return true;
	}
}
