using System;
using GameFramework;
using UnityEngine;
using UnityEngine.Playables;

public class S0AllianceBossObject : WorldPointObject
{
	private S0AllianceBossWorldPointInfo pointInfo;

	private Transform transform;

	private TimelinePlayer timelinePlayer;

	private ITimer delayTimer;

	private InstanceRequest effectReq;

	private TextMeshProEx timeTMPEx;

	private InstanceRequest _modelReq;

	private InstanceRequest _modelLabelReq;

	private static Color nameColorBlue = new Color(0.329f, 0.768f, 0.949f);

	private readonly string worldModelPath = "Model/ModelLabel";

	private string _modelPath;

	private long _battleStartTime;

	private long _actEndTime;

	private long _uuid;

	private Transform _labelTrans;

	private string _curAnimName;

	public S0AllianceBossObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
		if (world.GetPointInfo(pointIndex) is S0AllianceBossWorldPointInfo s0AllianceBossWorldPointInfo)
		{
			serverId = s0AllianceBossWorldPointInfo.serverId;
			pointInfo = s0AllianceBossWorldPointInfo;
			_modelPath = pointInfo.GetModelPath();
			_battleStartTime = pointInfo.buildPointInfo.startTime;
			_actEndTime = pointInfo.buildPointInfo.actEndTime;
			_uuid = s0AllianceBossWorldPointInfo.uuid;
		}
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		Create();
	}

	public override void SetAutoAdjustLod()
	{
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null)
		{
			serverId = pointInfo.serverId;
			adjuster = gameObject.GetComponent<AutoAdjustLod>();
			if (adjuster == null)
			{
				adjuster = gameObject.AddComponent<AutoAdjustLod>();
			}
			adjuster.SetLodType(LodType.Resource);
		}
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null)
		{
			serverId = pointInfo.serverId;
			this.pointInfo = pointInfo as S0AllianceBossWorldPointInfo;
			if (this.pointInfo != null)
			{
				_battleStartTime = this.pointInfo.buildPointInfo.startTime;
				_actEndTime = this.pointInfo.buildPointInfo.actEndTime;
			}
			RefreshAnim();
			InitModelLabel();
			GameEntry.Lua.Call("CSharpCallLuaInterface.CreateS0AllianceBossBuildingCtrl", _uuid, transform);
		}
	}

	private void Create()
	{
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo == null)
		{
			return;
		}
		serverId = pointInfo.serverId;
		this.pointInfo = pointInfo as S0AllianceBossWorldPointInfo;
		AddOldObject();
		if (string.IsNullOrEmpty(_modelPath))
		{
			Log.Error("S0AllianceBoss -- _modelPath is nil！");
			return;
		}
		instance = GameEntry.Resource.InstantiateAsync(_modelPath);
		instance.completed += delegate
		{
			ClearOldObject();
			gameObject = instance.gameObject;
			if (!(gameObject == null))
			{
				transform = gameObject.transform;
				GameEntry.Lua.Call("CSharpCallLuaInterface.CreateS0AllianceBossBuildingCtrl", _uuid, transform);
				transform.SetParent(world.DynamicObjNode);
				transform.position = base.WorldPosition;
				Transform goNode = transform.Find("Model/WorldModel/Go");
				if (!(goNode == null))
				{
					string prefabPath = "Assets/Main/Prefabs/Building/Building_S0AllianceBoss_01.prefab";
					if (!GameEntry.Resource.PrefabAssetsDownloaded(prefabPath))
					{
						goNode.gameObject.SetActive(value: true);
					}
					else
					{
						goNode.gameObject.SetActive(value: false);
					}
					Transform worldModelNode = transform.Find("Model/WorldModel");
					if (!(worldModelNode == null))
					{
						_modelReq = GameEntry.Resource.InstantiateAsync(prefabPath);
						_modelReq.completed += delegate
						{
							GameObject gameObject = _modelReq.gameObject;
							if (!(gameObject == null))
							{
								goNode.gameObject.SetActive(value: false);
								gameObject.SetActive(value: true);
								Transform obj = gameObject.transform;
								obj.SetParent(worldModelNode);
								obj.Set_localScale(Vector3.one.x, Vector3.one.y, Vector3.one.z);
								obj.Set_localPosition(Vector3.zero.x, Vector3.zero.y, Vector3.zero.z);
								timelinePlayer = base.gameObject.GetComponentInChildren<TimelinePlayer>();
								RefreshAnim();
							}
						};
						_modelLabelReq = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/World/S0AllianceBoss/ModelLabel_Build_Variant.prefab");
						_modelLabelReq.completed += delegate
						{
							GameObject gameObject = _modelLabelReq.gameObject;
							if (!(gameObject == null))
							{
								Transform transform = this.transform.Find("Model");
								if (!(transform == null))
								{
									gameObject.SetActive(value: true);
									Transform obj = gameObject.transform;
									obj.name = "ModelLabel";
									obj.SetParent(transform);
									obj.Set_localPosition(0f, 3.61f, -1.58f);
									Quaternion quaternion = Quaternion.Euler(45f, 0f, 0f);
									obj.Set_localRotation(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
									obj.Set_localScale(1.5f, 1.5f, 1.5f);
									InitModelLabel();
								}
							}
						};
						SetAutoAdjustLod();
						SetClickEvent();
					}
				}
			}
		};
	}

	private void InitModelLabel()
	{
		if (this.transform == null)
		{
			return;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		if (_battleStartTime > 0 && serverTime >= _battleStartTime)
		{
			return;
		}
		_labelTrans = this.transform.Find(worldModelPath);
		if (_labelTrans == null)
		{
			return;
		}
		_labelTrans.SetLocalPositionX(0f);
		Transform transform = this.transform.Find("Icon/IconSprite");
		if (transform != null)
		{
			SpriteRenderer component = transform.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				string buildingLodIconPath = pointInfo.GetBuildingLodIconPath();
				if (!string.IsNullOrEmpty(buildingLodIconPath))
				{
					component.LoadSprite(buildingLodIconPath);
				}
			}
		}
		Transform transform2 = _labelTrans.Find("Node/txt_boss_name");
		if (transform2 == null)
		{
			return;
		}
		TextMeshProEx component2 = transform2.GetComponent<TextMeshProEx>();
		if (component2 == null)
		{
			return;
		}
		string text = (component2.text = string.Format(GameEntry.Localization.GetString("s0_alliance_boss_placeholder"), pointInfo.buildPointInfo.abbr, pointInfo.GetBuildingName()));
		Transform transform3 = _labelTrans.Find("Node/txt_finish");
		if (transform3 == null)
		{
			return;
		}
		Transform transform4 = _labelTrans.Find("Node/txt_battle_time");
		if (transform4 == null)
		{
			return;
		}
		Transform transform5 = _labelTrans.Find("Node/txt_time");
		if (transform5 == null)
		{
			return;
		}
		Transform transform6 = _labelTrans.Find("NameLabel");
		if (transform6 == null)
		{
			return;
		}
		if (_battleStartTime <= 0)
		{
			TextMeshProEx component3 = transform3.GetComponent<TextMeshProEx>();
			string text3 = GameEntry.Localization.GetString("s0_alliance_boss_waiting_schedule");
			component3.text = text3;
			transform3.gameObject.SetActive(value: true);
			transform4.gameObject.SetActive(value: false);
			transform5.gameObject.SetActive(value: false);
			transform6.gameObject.SetActive(value: false);
			return;
		}
		transform3.gameObject.SetActive(value: false);
		transform4.gameObject.SetActive(value: true);
		transform5.gameObject.SetActive(value: true);
		transform6.gameObject.SetActive(value: true);
		TextMeshProEx component4 = transform4.GetComponent<TextMeshProEx>();
		string text4 = GameEntry.Localization.GetString("s0_alliance_boss_challenge_soon");
		component4.text = text4;
		timeTMPEx = transform5.GetComponent<TextMeshProEx>();
		if (timeTMPEx == null)
		{
			return;
		}
		timeTMPEx.text = GameEntry.Timer.MilliSecondToFmtString(_battleStartTime - serverTime);
		Transform transform7 = transform6.Find("NameText");
		if (!(transform7 == null))
		{
			TextMeshProEx component5 = transform7.GetComponent<TextMeshProEx>();
			if (!(component5 == null))
			{
				component5.text = text;
				string allianceId = GameEntry.Data.Player.GetAllianceId();
				component5.color = ((pointInfo.buildPointInfo.allianceId == allianceId) ? nameColorBlue : Color.white);
			}
		}
	}

	private void RefreshAnim()
	{
		if (!timelinePlayer || pointInfo == null || pointInfo.buildPointInfo == null)
		{
			return;
		}
		if (_battleStartTime <= 0)
		{
			PlayAnim("hold", loop: false);
			return;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		long lastReserveTime = pointInfo.buildPointInfo.lastReserveTime;
		if (lastReserveTime > 0)
		{
			long num = serverTime - lastReserveTime;
			gameObject.SetActive(isVisible);
			if (num <= 5100)
			{
				if (isVisible)
				{
					PlayAnim("buildBorn", loop: false, 0f, delegate
					{
						PlayAnim("buildIdle", loop: true);
					});
				}
			}
			else if (isVisible)
			{
				PlayAnim("buildIdle", loop: true);
			}
		}
		else if (isVisible)
		{
			PlayAnim("buildIdle", loop: true);
		}
	}

	private void PlayAnim(string name, bool loop, float startTime = 0f, Action callback = null)
	{
		if (string.Equals(name, _curAnimName))
		{
			return;
		}
		_curAnimName = name;
		if (delayTimer != null)
		{
			GameEntry.Timer.CancelTimer(delayTimer);
			delayTimer = null;
		}
		DirectorWrapMode mode = (loop ? DirectorWrapMode.Loop : DirectorWrapMode.Hold);
		double? num = timelinePlayer?.PlayTimelineAt(name, mode, rewind: true, startTime);
		if (callback != null)
		{
			if (num > 0.0)
			{
				delayTimer = GameEntry.Timer.RegisterTimer((float)num.Value, callback);
			}
			else
			{
				callback?.Invoke();
			}
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
		if (_battleStartTime > 0)
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			long num = _battleStartTime - serverTime;
			if (num >= 0)
			{
				UpdateCutDownTiming(num);
			}
			else if (_labelTrans != null)
			{
				_labelTrans.SetLocalPositionX(1000f);
			}
		}
	}

	private void UpdateCutDownTiming(long offset)
	{
		if (timeTMPEx != null)
		{
			if (offset < 0)
			{
				offset = 0L;
			}
			timeTMPEx.text = GameEntry.Timer.MilliSecondToFmtString(offset);
		}
	}

	public override void Destroy()
	{
		_labelTrans?.SetLocalPositionX(1000f);
		GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveS0AllianceBossBuildingCtrl", _uuid);
		if (_battleStartTime > 0)
		{
			GameEntry.Timer.RegisterTimer(0.5f, delegate
			{
				OnDestroy();
				base.Destroy();
			});
		}
		else if (_actEndTime > 0)
		{
			if (GameEntry.Timer.GetServerTime() - _actEndTime >= 0)
			{
				PlayAnim("buildLeave", loop: false, 0f, delegate
				{
					OnDestroy();
					base.Destroy();
				});
			}
			else
			{
				OnDestroy();
				base.Destroy();
			}
		}
		else
		{
			OnDestroy();
			base.Destroy();
		}
	}

	private void OnDestroy()
	{
		_modelReq?.Destroy();
		_modelLabelReq?.Destroy();
		if (delayTimer != null)
		{
			GameEntry.Timer.CancelTimer(delayTimer);
			delayTimer = null;
		}
	}
}
