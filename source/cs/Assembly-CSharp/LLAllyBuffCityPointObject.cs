using System;
using System.Collections.Generic;
using UnityEngine;

public class LLAllyBuffCityPointObject : WorldPointObject, IWorldLodWatcher
{
	private string prefabPath;

	private LLAllyCityPointInfo ptInfo;

	protected InstanceRequest instanceEffect;

	protected InstanceRequest buffInstanceEffect;

	private ParticleSystem buffParticleSystem;

	private SuperTextMesh nameText;

	private long bUuid;

	private long _refreshTime;

	private ITimer _buffTimer;

	public AsyncMono<WorldAssistanceLabelAsync>.Handle AssistanceLabel { get; private set; }

	public AsyncMono<WorldAssistanceHeroAsync>.Handle AssistanceHero { get; private set; }

	public AsyncMono<LLWorldOccupyAsync>.Handle OccupyLabel { get; private set; }

	public AsyncMono<LLWorldBuffCityAsync>.Handle BuffLabel { get; private set; }

	public override int AutoLookAtThreshold => 5;

	public long Uid => (long)pointType * 10000000L + pointIndex;

	public LLAllyBuffCityPointObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		ptInfo = world.GetPointInfo(pointIndex) as LLAllyCityPointInfo;
		if (ptInfo == null || ptInfo == null || pointIndex != ptInfo.mainIndex)
		{
			return;
		}
		bUuid = ptInfo.uuid;
		world?.RegisterLodWatcher(this);
		AddOldObject();
		if (instance != null)
		{
			return;
		}
		string str = GameEntry.Lua.CallWithReturn<string, PointInfo>("CSharpCallLuaInterface.GetWorldPointModelPath", ptInfo);
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
				OnGameObjectCreated();
			}
		};
	}

	private void OnGameObjectCreated()
	{
		gameObject.transform.SetParent(world.DynamicObjNode);
		gameObject.transform.position = base.WorldPosition;
		gameObject.SetActive(isVisible);
		nameText = gameObject.transform.Find("Model/CityLabel/NameLabel/NameText").GetComponent<SuperTextMesh>();
		SetAutoAdjustLod();
		UpdateGameObject();
		CheckShowTroopDestination();
		if (pointType != 57)
		{
			return;
		}
		if (_touchObject != null)
		{
			_touchObject.previewIconPath = "Assets/Main/Sprites/LodIcon/cfm_daditu_chengshi_01.png";
		}
		if (ptInfo != null && _touchObject != null)
		{
			int cityId = ptInfo.cityId;
			string cityTableName = LandlordManager.Instance.cityTableName;
			if (!string.IsNullOrEmpty(cityTableName))
			{
				string templateData = GameEntry.ConfigCache.GetTemplateData(cityTableName, cityId, "city_field");
				int result = 0;
				if (!templateData.IsNullOrEmpty() && int.TryParse(templateData, out result))
				{
					ptInfo.CityField = result;
				}
				else
				{
					ptInfo.CityField = 12;
				}
				string templateData2 = GameEntry.ConfigCache.GetTemplateData(cityTableName, cityId, "level");
				string templateData3 = GameEntry.ConfigCache.GetTemplateData(cityTableName, cityId, "name");
				if (!templateData2.IsNullOrEmpty())
				{
					ptInfo.CityLevel = templateData2.ToInt();
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

	public override void Destroy()
	{
		world?.UnregisterLodWatcher(this);
		GameEntry.Event.Fire(EventId.WORLD_BUILD_OUT_VIEW, bUuid);
		AssistanceLabel?.Destroy();
		AssistanceLabel = null;
		AssistanceHero?.Destroy();
		AssistanceHero = null;
		OccupyLabel?.Destroy();
		OccupyLabel = null;
		BuffLabel?.Destroy();
		BuffLabel = null;
		if (_buffTimer != null)
		{
			GameEntry.Timer.CancelTimer(_buffTimer);
			_buffTimer = null;
		}
		if (instanceEffect != null)
		{
			instanceEffect.Destroy();
			instanceEffect = null;
		}
		if (buffInstanceEffect != null)
		{
			buffInstanceEffect.Destroy();
			buffInstanceEffect = null;
		}
		base.Destroy();
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		if (world.GetPointInfo(pointIndex) is LLAllyCityPointInfo { pointType: WorldPointType.ZWL_BUILDING_BUFF } lLAllyCityPointInfo)
		{
			OwnerChanged(lLAllyCityPointInfo.ownerCampId);
			if (_refreshTime != lLAllyCityPointInfo.refreshTime && _refreshTime > 0)
			{
				if (buffParticleSystem != null)
				{
					buffParticleSystem.Clear();
					buffParticleSystem.Play();
				}
				else
				{
					ShowRefreshBuffEffect();
				}
			}
			_refreshTime = lLAllyCityPointInfo.refreshTime;
			RefreshAssistanceCount();
			RefreshAssistanceHero();
			RefreshOccupy();
			RefreshBuff();
			string text = GameEntry.Lua.CallWithReturn<string, PointInfo>("CSharpCallLuaInterface.GetWorldPointModelPath", lLAllyCityPointInfo);
			if (prefabPath != text)
			{
				base.Destroy();
				CreateGameObject();
				bool flag = false;
				SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
				if (curSkinMeta != null)
				{
					flag = !curSkinMeta.IsNotSeason();
				}
				if (flag && instanceEffect == null)
				{
					string text2 = "Assets/Main/Prefabs/World/Saiji/Eff_saiji_dsj_shengshi_zhanling.prefab";
					instanceEffect = GameEntry.Resource.InstantiateAsync(text2);
					instanceEffect.completed += delegate
					{
						GameObject gameObject = instanceEffect.gameObject;
						if (gameObject != null && world != null && world.DynamicObjNode != null)
						{
							gameObject.transform.SetParent(world.DynamicObjNode);
							gameObject.transform.position = base.WorldPosition;
							gameObject.SetActive(value: true);
						}
					};
				}
			}
		}
		GameEntry.Event.Fire(EventId.WorldCityBuildObjUpdate, bUuid);
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
		RefreshAssistanceCount();
		RefreshAssistanceHero();
		RefreshOccupy();
		RefreshBuff();
	}

	private void RefreshAssistanceCount()
	{
		int currentLodLevel = world.CurrentLodLevel;
		int num = 0;
		bool num2 = currentLodLevel >= 1 && currentLodLevel <= 5;
		LLAllyCityPointInfo lLAllyCityPointInfo = null;
		if (num2)
		{
			lLAllyCityPointInfo = world.GetPointInfo(pointIndex) as LLAllyCityPointInfo;
			num = lLAllyCityPointInfo?.assistanceCount ?? 0;
		}
		if (num > 0)
		{
			if (AssistanceLabel == null)
			{
				AssistanceLabel = AsyncMono<WorldAssistanceLabelAsync>.Handle.Load("Assets/Main/Prefabs/MainCity/WorldAssistanceLabelAllianceBuilding.prefab", world.DynamicObjNode, delegate
				{
					Transform transform = AssistanceLabel.Transform;
					if (transform != null)
					{
						transform.localPosition = new Vector3(base.WorldPosition.x, base.WorldPosition.y - 3f, base.WorldPosition.z);
					}
					RefreshAssistanceCount();
				});
			}
			else if (AssistanceLabel.MonoInstance != null)
			{
				AssistanceLabel.SetActive(active: true);
				WorldAssistanceLabelAsync monoInstance = AssistanceLabel.MonoInstance;
				int count = num;
				int maxAssistanceCount = lLAllyCityPointInfo.maxAssistanceCount;
				bool showMax = currentLodLevel < 4;
				WorldScene worldScene = world;
				monoInstance.SetAssistance(count, maxAssistanceCount, showMax, (object)worldScene != null && worldScene.GetMyAssistanceCount(pointIndex) > 0);
			}
		}
		else
		{
			AssistanceLabel?.SetActive(active: false);
		}
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
						transform.localPosition = new Vector3(base.WorldPosition.x, base.WorldPosition.y - 9f, base.WorldPosition.z);
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

	public void ShowRefreshBuffEffect()
	{
		if (ptInfo == null || buffInstanceEffect != null)
		{
			return;
		}
		string text = "Assets/Main/Prefabs/World/Landlord/Eff_Glodenbattle_Buff_Wrapper.prefab";
		buffInstanceEffect = GameEntry.Resource.InstantiateAsync(text);
		buffInstanceEffect.completed += delegate
		{
			GameObject gameObject = buffInstanceEffect.gameObject;
			buffParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
			if (gameObject != null && world != null && world.DynamicObjNode != null)
			{
				gameObject.transform.SetParent(world.DynamicObjNode);
				gameObject.transform.position = base.WorldPosition;
				gameObject.SetActive(value: true);
			}
		};
	}

	public void OwnerChanged(int theOwnerCampId)
	{
		if (!(nameText != null))
		{
			return;
		}
		if (theOwnerCampId > 0)
		{
			if (GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.LLGetSelfCampId") == theOwnerCampId)
			{
				nameText.color = new Color32(84, 196, 242, byte.MaxValue);
			}
			else
			{
				nameText.color = new Color32(229, 39, 39, byte.MaxValue);
			}
		}
		else
		{
			nameText.color = Color.white;
		}
		string cityTableName = LandlordManager.Instance.cityTableName;
		string text = "";
		if (!string.IsNullOrEmpty(cityTableName))
		{
			text = GameEntry.ConfigCache.GetTemplateData(cityTableName, ptInfo.cityId, "name");
		}
		nameText.text = text;
		nameText.Rebuild();
	}

	private void RefreshOccupy()
	{
		if (world.GetPointInfo(pointIndex) is LLAllyCityPointInfo lLAllyCityPointInfo)
		{
			int currentLodLevel = world.CurrentLodLevel;
			if (currentLodLevel >= 3 && currentLodLevel <= 5 && lLAllyCityPointInfo.ownerCampId != 0)
			{
				if (OccupyLabel == null)
				{
					OccupyLabel = AsyncMono<LLWorldOccupyAsync>.Handle.Load("Assets/Main/Prefabs/World/Landlord/LLWorldOccupyLabel.prefab", world.DynamicObjNode, delegate
					{
						Transform transform = OccupyLabel.Transform;
						if (transform != null)
						{
							transform.localPosition = base.WorldPosition;
						}
						RefreshOccupy();
					});
				}
				else if (OccupyLabel.MonoInstance != null)
				{
					OccupyLabel.SetActive(active: true);
					bool isEnemy = lLAllyCityPointInfo.ownerCampId != LandlordManager.Instance.myCampId;
					string cityTableName = LandlordManager.Instance.cityTableName;
					string templateData = GameEntry.ConfigCache.GetTemplateData(cityTableName, ptInfo.cityId, "name");
					OccupyLabel.MonoInstance.Refresh(isEnemy, templateData);
				}
			}
			else
			{
				OccupyLabel?.SetActive(active: false);
			}
		}
		else
		{
			OccupyLabel?.SetActive(active: false);
		}
	}

	private void RefreshBuff()
	{
		if (world.GetPointInfo(pointIndex) is LLAllyCityPointInfo lLAllyCityPointInfo)
		{
			if (world.CurrentLodLevel < 3 && lLAllyCityPointInfo.curClientState == 3)
			{
				if (BuffLabel == null)
				{
					BuffLabel = AsyncMono<LLWorldBuffCityAsync>.Handle.Load("Assets/Main/Prefabs/World/Landlord/LLWorldBuffCityLabel.prefab", world.DynamicObjNode, delegate
					{
						Transform transform = BuffLabel.Transform;
						if (transform != null)
						{
							transform.localPosition = base.WorldPosition;
						}
						RefreshBuff();
					});
				}
				else if (BuffLabel.MonoInstance != null)
				{
					BuffLabel.SetActive(active: true);
					BuffLabel.MonoInstance.Init(lLAllyCityPointInfo.buffId);
					RefreshBuffText(lLAllyCityPointInfo);
					StartBuffTimer();
				}
			}
			else
			{
				BuffLabel?.SetActive(active: false);
				StopBuffTimer();
			}
		}
		else
		{
			BuffLabel?.SetActive(active: false);
			StopBuffTimer();
		}
	}

	private void StartBuffTimer()
	{
		if (_buffTimer != null)
		{
			return;
		}
		_buffTimer = GameEntry.Timer.RegisterTimerRepeat(1f, 1f, delegate
		{
			if (BuffLabel == null || BuffLabel.MonoInstance == null)
			{
				StopBuffTimer();
			}
			else if (!(world.GetPointInfo(pointIndex) is LLAllyCityPointInfo lLAllyCityPointInfo))
			{
				StopBuffTimer();
			}
			else if (world.CurrentLodLevel >= 3 || lLAllyCityPointInfo.curClientState != 3)
			{
				BuffLabel.SetActive(active: false);
				StopBuffTimer();
			}
			else if (RefreshBuffText(lLAllyCityPointInfo) <= 0)
			{
				StopBuffTimer();
			}
		});
	}

	private void StopBuffTimer()
	{
		if (_buffTimer != null)
		{
			GameEntry.Timer.CancelTimer(_buffTimer);
			_buffTimer = null;
		}
	}

	private long RefreshBuffText(LLAllyCityPointInfo info)
	{
		if (BuffLabel == null || BuffLabel.MonoInstance == null || info == null)
		{
			return 0L;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		long num = Math.Max(info.refreshTime - serverTime, 0L);
		BuffLabel.MonoInstance.RefreshTxt(GameEntry.Timer.SecondsToStringWithoutHour(num / 1000, ":"));
		return num;
	}
}
