using System.Collections.Generic;
using System.Text;
using Protobuf;

public class LLAllyCityPointInfo : PointInfo
{
	public int cityId;

	public int type;

	public int state;

	public int ownerCampId;

	public int tmpOwnerCampId;

	public long fixStartTime;

	public long fixEndTime;

	public int progress;

	public int progressMax;

	public long occupyStartTime;

	public int occupyStartProgress;

	public long overTime;

	public long refreshTime;

	public int buffId;

	public Dictionary<int, float> effects = new Dictionary<int, float>();

	public string alAbbr;

	public int alServerId;

	public long unlockTime;

	public long battleStartTime;

	public long battleEndTime;

	public int CityLevel = -1;

	public int CityField = -1;

	public int assistanceCount;

	public int maxAssistanceCount;

	private LandlordCityFSM _landlordCityFsm;

	public int curClientState => (int)_landlordCityFsm.State;

	public int prepareBoomTime => _landlordCityFsm.PrepareBoomTime;

	public void UpdateFSM()
	{
		_landlordCityFsm?.UpdateByPointInfo(this);
	}

	public void DisposeFSM()
	{
		_landlordCityFsm?.Dispose();
	}

	public void UpdateAssistanceCount()
	{
		base.PointManager?.TryGetAssistanceCountByPointIndex(serverId, pointIndex, out assistanceCount, out maxAssistanceCount);
	}

	public LLAllyCityPointInfo(int theCityId)
	{
		cityId = theCityId;
	}

	public override PointInfo Clone()
	{
		LLAllyCityPointInfo lLAllyCityPointInfo = new LLAllyCityPointInfo(cityId);
		BaseClone(lLAllyCityPointInfo);
		lLAllyCityPointInfo.type = type;
		lLAllyCityPointInfo.state = state;
		lLAllyCityPointInfo.ownerCampId = ownerCampId;
		lLAllyCityPointInfo.tmpOwnerCampId = tmpOwnerCampId;
		lLAllyCityPointInfo.fixStartTime = fixStartTime;
		lLAllyCityPointInfo.fixEndTime = fixEndTime;
		lLAllyCityPointInfo.progress = progress;
		lLAllyCityPointInfo.progressMax = progressMax;
		lLAllyCityPointInfo.occupyStartTime = occupyStartTime;
		lLAllyCityPointInfo.occupyStartProgress = occupyStartProgress;
		lLAllyCityPointInfo.refreshTime = refreshTime;
		lLAllyCityPointInfo.buffId = buffId;
		lLAllyCityPointInfo.overTime = overTime;
		lLAllyCityPointInfo.effects = new Dictionary<int, float>(effects);
		lLAllyCityPointInfo.CityLevel = CityLevel;
		lLAllyCityPointInfo.CityField = CityField;
		lLAllyCityPointInfo.assistanceCount = assistanceCount;
		lLAllyCityPointInfo.maxAssistanceCount = maxAssistanceCount;
		lLAllyCityPointInfo._landlordCityFsm = _landlordCityFsm;
		lLAllyCityPointInfo.alAbbr = alAbbr;
		lLAllyCityPointInfo.alServerId = alServerId;
		lLAllyCityPointInfo.unlockTime = unlockTime;
		lLAllyCityPointInfo.battleStartTime = battleStartTime;
		lLAllyCityPointInfo.battleEndTime = battleEndTime;
		return lLAllyCityPointInfo;
	}

	public LLAllyCityPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		ZWLBuildingPoint zwlBuilding = pi.ZwlBuilding;
		cityId = zwlBuilding.CityId;
		type = zwlBuilding.Type;
		state = zwlBuilding.State;
		ownerCampId = zwlBuilding.OwnerCampId;
		tmpOwnerCampId = zwlBuilding.TmpOwnerCampId;
		fixStartTime = zwlBuilding.FixStartTime;
		fixEndTime = zwlBuilding.FixEndTime;
		progress = zwlBuilding.Progress;
		progressMax = zwlBuilding.ProgressMax;
		occupyStartTime = zwlBuilding.OccupyStartTime;
		occupyStartProgress = zwlBuilding.OccupyStartProgress;
		overTime = zwlBuilding.OverTime;
		alServerId = zwlBuilding.AlServerId;
		unlockTime = zwlBuilding.UnlockTime;
		battleStartTime = zwlBuilding.BattleStartTime;
		battleEndTime = zwlBuilding.BattleEndTime;
		alAbbr = zwlBuilding.AlAbbr;
		if (zwlBuilding.Effect != null)
		{
			effects.Clear();
			foreach (KeyValuePair<int, float> item in zwlBuilding.Effect)
			{
				effects[item.Key] = item.Value;
			}
		}
		refreshBuffInfo buffInfo = zwlBuilding.BuffInfo;
		if (buffInfo != null)
		{
			refreshTime = buffInfo.RefreshTime;
			buffId = buffInfo.BuffId;
		}
		tileSize = SceneManager.World.GetAllianceCitySizeByItemId(cityId);
		PointInfo pointInfoByUuid = SceneManager.World.GetPointInfoByUuid(pi.Uuid);
		if (pointInfoByUuid != null && pointInfoByUuid is LLAllyCityPointInfo lLAllyCityPointInfo)
		{
			CityLevel = lLAllyCityPointInfo.CityLevel;
			CityField = lLAllyCityPointInfo.CityField;
			_landlordCityFsm = lLAllyCityPointInfo._landlordCityFsm;
		}
		if (_landlordCityFsm == null)
		{
			_landlordCityFsm = new LandlordCityFSM(this);
		}
		else
		{
			_landlordCityFsm.UpdateByPointInfo(this);
		}
		UpdateAssistanceCount();
	}

	public int GetOwnerCampId()
	{
		return ownerCampId;
	}

	public bool HasOwner()
	{
		return GetOwnerCampId() > 0;
	}

	public override void OnDescription(StringBuilder sb)
	{
		sb.AppendLine($"服务器状态：{state}");
		sb.AppendLine("服务器状态枚举：1(废墟)、2(修复中)、3(正常)、4(完成)");
		sb.AppendLine($"客户端状态：{curClientState}");
		sb.AppendLine("客户端状态枚举：1(未开放)、2(战斗中)、3(即将炸毁)、4(爆炸中)、5(废墟)、6(重建中)");
		sb.AppendLine($"assistanceCount = {assistanceCount}");
		sb.AppendLine($"maxAssistanceCount = {maxAssistanceCount}");
		sb.AppendLine("alAbbr = " + alAbbr);
		sb.AppendLine($"alServerId = {alServerId}");
	}
}
