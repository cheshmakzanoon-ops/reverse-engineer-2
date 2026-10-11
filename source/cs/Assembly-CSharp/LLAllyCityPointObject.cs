using System.Collections.Generic;
using UnityEngine;

public class LLAllyCityPointObject : WorldPointObject, IWorldLodWatcher
{
	private string prefabPath;

	protected InstanceRequest boomInstanceEffect;

	private SimpleAnimation _boomInstanceAnim;

	private long bUuid;

	public AsyncMono<WorldAssistanceHeroAsync>.Handle AssistanceHero { get; private set; }

	public override int AutoLookAtThreshold => 5;

	public long Uid => (long)pointType * 10000000L + pointIndex;

	public LLAllyCityPointObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		PointInfo info = world.GetPointInfo(pointIndex);
		if (info == null || pointIndex != info.mainIndex)
		{
			return;
		}
		bUuid = info.uuid;
		GameEntry.Event.Subscribe(EventId.LandlordCityPointChangeClientState, OnClientStateChanged);
		world?.RegisterLodWatcher(this);
		AddOldObject();
		if (instance != null)
		{
			return;
		}
		string str = GameEntry.Lua.CallWithReturn<string, PointInfo>("CSharpCallLuaInterface.GetWorldPointModelPath", info);
		if (str.IsNullOrEmpty())
		{
			return;
		}
		prefabPath = str;
		instance = world.InstantiateAsyncDynamicObj(str);
		instance.completed += delegate
		{
			gameObject = instance.gameObject;
			if (gameObject != null)
			{
				OnGameObjectCreated(info);
			}
		};
	}

	private void OnGameObjectCreated(PointInfo info)
	{
		gameObject.transform.SetParent(world.DynamicObjNode);
		gameObject.transform.position = base.WorldPosition;
		gameObject.SetActive(isVisible);
		SetAutoAdjustLod();
		UpdateGameObject();
		CheckShowTroopDestination();
		SetAllianceCityClickEvent();
		if (pointType != 57 && pointType != 56 && pointType != 55)
		{
			return;
		}
		if (_touchObject != null)
		{
			_touchObject.previewIconPath = "Assets/Main/Sprites/LodIcon/cfm_daditu_chengshi_01.png";
		}
		if (info is LLAllyCityPointInfo lLAllyCityPointInfo && _touchObject != null)
		{
			int cityId = lLAllyCityPointInfo.cityId;
			string cityTableName = LandlordManager.Instance.cityTableName;
			if (!string.IsNullOrEmpty(cityTableName))
			{
				string templateData = GameEntry.ConfigCache.GetTemplateData(cityTableName, cityId, "city_field");
				int result = 0;
				if (!templateData.IsNullOrEmpty() && int.TryParse(templateData, out result))
				{
					lLAllyCityPointInfo.CityField = result;
				}
				else
				{
					lLAllyCityPointInfo.CityField = 12;
				}
				string templateData2 = GameEntry.ConfigCache.GetTemplateData(cityTableName, cityId, "level");
				string templateData3 = GameEntry.ConfigCache.GetTemplateData(cityTableName, cityId, "name");
				if (!templateData2.IsNullOrEmpty())
				{
					lLAllyCityPointInfo.CityLevel = templateData2.ToInt();
				}
				if (_touchObject != null)
				{
					_touchObject.previewName = GameEntry.Localization.GetString("140205", templateData2, GameEntry.Localization.GetString(templateData3));
					_touchObject.previewType = WorldPreviewType.AllianceCity;
				}
			}
		}
		GameEntry.Event.Fire(EventId.CityDomeShow, pointIndex);
	}

	private Transform SetAllianceCityClickEvent()
	{
		if (gameObject == null)
		{
			return null;
		}
		Transform transform = gameObject.transform.Find("Model");
		if ((pointType == 55 || pointType == 57 || pointType == 56) && transform != null)
		{
			_touchObject = transform.GetComponent<TouchObjectEventTrigger>();
		}
		else
		{
			_touchObject = gameObject.GetComponent<TouchObjectEventTrigger>();
		}
		if (_touchObject == null)
		{
			return transform;
		}
		_touchObject.onPointerClick = base.OnClickPoint;
		return transform;
	}

	public override void Destroy()
	{
		world?.UnregisterLodWatcher(this);
		GameEntry.Event.Unsubscribe(EventId.LandlordCityPointChangeClientState, OnClientStateChanged);
		GameEntry.Event.Fire(EventId.WORLD_BUILD_OUT_VIEW, bUuid);
		AssistanceHero?.Destroy();
		AssistanceHero = null;
		_boomInstanceAnim = null;
		if (gameObject != null)
		{
			WorldPointCityStronghold componentInChildren = gameObject.GetComponentInChildren<WorldPointCityStronghold>();
			if (componentInChildren != null)
			{
				componentInChildren.UnInit();
			}
		}
		if (boomInstanceEffect != null)
		{
			boomInstanceEffect.Destroy();
			boomInstanceEffect = null;
		}
		base.Destroy();
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		if (world.GetPointInfo(pointIndex) is LLAllyCityPointInfo lLAllyCityPointInfo && (lLAllyCityPointInfo.pointType == WorldPointType.ZWL_BUILDING || lLAllyCityPointInfo.pointType == WorldPointType.ZWL_BUILDING_BUFF || lLAllyCityPointInfo.pointType == WorldPointType.ZWL_BUILDING_THRONE))
		{
			RefreshAssistanceHero();
			string text = GameEntry.Lua.CallWithReturn<string, PointInfo>("CSharpCallLuaInterface.GetWorldPointModelPath", lLAllyCityPointInfo);
			if (prefabPath != text)
			{
				Destroy();
				CreateGameObject();
			}
			UpdateState();
			GameEntry.Event.Fire(EventId.WorldCityBuildObjUpdate, bUuid);
			GameEntry.Event.Fire(EventId.LandlordCityPointObjectUpdate, bUuid);
		}
	}

	public void UpdateState()
	{
		if (world.GetPointInfo(pointIndex) is LLAllyCityPointInfo lLAllyCityPointInfo)
		{
			LandlordConst.LLBuildingState curClientState = (LandlordConst.LLBuildingState)lLAllyCityPointInfo.curClientState;
			if (curClientState == LandlordConst.LLBuildingState.Exploding)
			{
				ShowBoomEffect();
			}
			else
			{
				_boomInstanceAnim?.Play("Default");
			}
		}
	}

	public override void CheckShowTroopDestination()
	{
		bool flag = isSHowDestination;
		List<WorldMarch> ownerMarches = world.GetOwnerMarches(GameEntry.Data.Player.Uid);
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		foreach (WorldMarch item in ownerMarches)
		{
			if (item.IsVisibleMarch() && pointInfo != null && pointInfo.uuid == item.targetUuid && item.targetUuid != 0L)
			{
				flag = false;
				Vector3 realPos = base.WorldPosition;
				int num = 1;
				EnumDestinationSignalType destinationType = world.GetDestinationType(item.uuid, item.targetUuid, pointIndex, item.target, isFormation: false, ref realPos, ref num);
				ShowTroopDestinationSignal(realPos, destinationType, num);
				break;
			}
		}
		if (flag)
		{
			HideTroopDestinationSignal();
		}
	}

	public void UpdateLod(int lod)
	{
		RefreshAssistanceHero();
	}

	private void RefreshAssistanceHero()
	{
		int currentLodLevel = world.CurrentLodLevel;
		int num = 0;
		if (currentLodLevel >= 3 && currentLodLevel <= 4)
		{
			num = world?.GetMyAssistanceFirstHero(pointIndex) ?? 0;
		}
		if (num > 0)
		{
			if (AssistanceHero == null)
			{
				AssistanceHero = AsyncMono<WorldAssistanceHeroAsync>.Handle.Load("Assets/Main/Prefabs/MainCity/WorldAssistanceHeroAllianceTrade.prefab", world?.DynamicObjNode, delegate
				{
					Transform transform = AssistanceHero?.Transform;
					if (transform != null)
					{
						transform.localPosition = base.WorldPosition;
						RefreshAssistanceHero();
					}
				});
			}
			else if (AssistanceHero.MonoInstance != null)
			{
				AssistanceHero.SetActive(active: true);
				AssistanceHero.MonoInstance.SetHeroHead(num);
			}
		}
		else
		{
			AssistanceHero?.SetActive(active: false);
		}
	}

	public void ShowBoomEffect()
	{
		LLAllyCityPointInfo info;
		if ((info = world.GetPointInfo(pointIndex) as LLAllyCityPointInfo) == null || boomInstanceEffect != null)
		{
			return;
		}
		string text = "Assets/Main/Prefabs/World/Landlord/Eff_Glodenbattle_Explosion_Wrapper.prefab";
		boomInstanceEffect = GameEntry.Resource.InstantiateAsync(text);
		boomInstanceEffect.completed += delegate
		{
			GameObject gameObject = boomInstanceEffect.gameObject;
			_boomInstanceAnim = gameObject.GetComponentInChildren<SimpleAnimation>();
			if (gameObject != null && world != null && world.DynamicObjNode != null)
			{
				gameObject.transform.SetParent(world.DynamicObjNode);
				gameObject.transform.position = base.WorldPosition;
				gameObject.SetActive(value: true);
				if (!gameObject.TryGetComponent<FireworkParticleController>(out var component))
				{
					component = gameObject.AddComponent<FireworkParticleController>();
				}
				int serverTimeSeconds = GameEntry.Timer.GetServerTimeSeconds();
				string cityTableName = LandlordManager.Instance.cityTableName;
				int num = 10;
				if (!string.IsNullOrEmpty(cityTableName))
				{
					num = GameEntry.ConfigCache.GetTemplateData(cityTableName, info.cityId, "boom_time").ToInt();
				}
				int num2 = 11;
				long num3 = serverTimeSeconds - (info.overTime / 1000 + num) - 1;
				component.Configure(num2, num2, 1f * (float)num3 / (float)num2);
			}
		};
	}

	private void OnClientStateChanged(object pointId)
	{
		if (pointId is int num && pointIndex == num)
		{
			UpdateGameObject();
		}
	}
}
