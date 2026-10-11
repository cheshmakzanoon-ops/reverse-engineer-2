using System.Text;
using Google.Protobuf.Collections;
using Protobuf;

public class AllyCityPointInfo : PointInfo
{
	public readonly int CityId;

	public int CityLevel = -1;

	public int CityField = -1;

	public int assistanceCount;

	public int maxAssistanceCount;

	public CityStrongholdPointInfo StrongholdInfo;

	public AllianceCityPointInfo CityInfo;

	public CityTradePointInfo WorldTradeInfo;

	public GoldTreePointInfo GoldTreePointInfo;

	public CityAltarPointInfo AltarPointInfo;

	public void UpdateAssistanceCount()
	{
		base.PointManager?.TryGetAssistanceCountByPointIndex(serverId, pointIndex, out assistanceCount, out maxAssistanceCount);
	}

	public AllyCityPointInfo(int theCityId)
	{
		CityId = theCityId;
	}

	public override PointInfo Clone()
	{
		AllyCityPointInfo allyCityPointInfo = new AllyCityPointInfo(CityId);
		BaseClone(allyCityPointInfo);
		allyCityPointInfo.CityLevel = CityLevel;
		allyCityPointInfo.CityField = CityField;
		allyCityPointInfo.assistanceCount = assistanceCount;
		allyCityPointInfo.maxAssistanceCount = maxAssistanceCount;
		allyCityPointInfo.StrongholdInfo = StrongholdInfo;
		allyCityPointInfo.CityInfo = CityInfo;
		allyCityPointInfo.WorldTradeInfo = WorldTradeInfo;
		allyCityPointInfo.GoldTreePointInfo = GoldTreePointInfo;
		allyCityPointInfo.AltarPointInfo = AltarPointInfo;
		return allyCityPointInfo;
	}

	public AllyCityPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		WorldTradeInfo = null;
		CityInfo = null;
		StrongholdInfo = null;
		AltarPointInfo = null;
		if (pointType == WorldPointType.WORLD_CITY_STRONGHOLD)
		{
			StrongholdInfo = CityStrongholdPointInfo.Parser.ParseFrom(pi.ExtraInfo);
			if (StrongholdInfo != null)
			{
				CityId = StrongholdInfo.StrongholdId;
				if (StrongholdInfo.ThermalConductor != null)
				{
					thermalConductor = new ThermalConductor(StrongholdInfo.ThermalConductor);
				}
			}
		}
		else if (pointType == WorldPointType.WORLD_CITY_TRADE)
		{
			WorldTradeInfo = CityTradePointInfo.Parser.ParseFrom(pi.ExtraInfo);
			if (WorldTradeInfo != null)
			{
				CityId = WorldTradeInfo.TradeId;
			}
		}
		else if (pointType == WorldPointType.CITY_ALTAR)
		{
			AltarPointInfo = CityAltarPointInfo.Parser.ParseFrom(pi.ExtraInfo);
			if (AltarPointInfo != null)
			{
				CityId = AltarPointInfo.Cfgid;
			}
		}
		else if (pointType == WorldPointType.GOLD_TREE)
		{
			GoldTreePointInfo = GoldTreePointInfo.Parser.ParseFrom(pi.ExtraInfo);
			if (GoldTreePointInfo != null)
			{
				CityId = GoldTreePointInfo.TreeId;
			}
		}
		else
		{
			CityInfo = AllianceCityPointInfo.Parser.ParseFrom(pi.ExtraInfo);
			if (CityInfo != null)
			{
				CityId = CityInfo.CityId;
				if (CityInfo.ThermalConductor != null)
				{
					thermalConductor = new ThermalConductor(CityInfo.ThermalConductor);
				}
			}
		}
		tileSize = SceneManager.World.GetAllianceCitySizeByItemId(CityId);
		PointInfo pointInfoByUuid = SceneManager.World.GetPointInfoByUuid(pi.Uuid);
		if (pointInfoByUuid != null && pointInfoByUuid is AllyCityPointInfo allyCityPointInfo)
		{
			CityLevel = allyCityPointInfo.CityLevel;
			CityField = allyCityPointInfo.CityField;
		}
		UpdateAssistanceCount();
	}

	public string GetAllianceId()
	{
		if (CityInfo != null)
		{
			return CityInfo.AllianceId;
		}
		if (StrongholdInfo != null)
		{
			return StrongholdInfo.AllianceId;
		}
		return null;
	}

	public bool HasOwner()
	{
		return !string.IsNullOrEmpty(GetAllianceId());
	}

	public override void OnDescription(StringBuilder sb)
	{
		sb.AppendLine($"assistanceCount = {assistanceCount}");
		sb.AppendLine($"maxAssistanceCount = {maxAssistanceCount}");
		sb.AppendLine($"CityState = {CityInfo?.State ?? (-1)}");
		sb.AppendLine($"StrongholdInfoState = {StrongholdInfo?.State ?? (-1)}");
		sb.AppendLine($"WorldTradeInfo = {WorldTradeInfo?.State ?? (-1)}");
		if (pointType != WorldPointType.CITY_ALTAR || AltarPointInfo == null)
		{
			return;
		}
		sb.AppendLine("========== 祭坛 Altar ==========");
		long serverTime = GameEntry.Timer.GetServerTime();
		string templateData = GameEntry.ConfigCache.GetTemplateData("season_city_s6", AltarPointInfo.Cfgid, "flag");
		string templateData2 = GameEntry.ConfigCache.GetTemplateData("season_city_s6", AltarPointInfo.Cfgid, "mutex_flag");
		sb.AppendLine("Flag: " + templateData + ", 互斥标志: " + templateData2);
		sb.AppendLine("Time: " + GameEntry.Timer.TimeStampToTime(serverTime));
		sb.AppendLine("\tStart: " + GameEntry.Timer.TimeStampToTime(AltarPointInfo.BattleStartTime));
		sb.AppendLine("\tEnd: " + GameEntry.Timer.TimeStampToTime(AltarPointInfo.BattleEndTime));
		if (AltarPointInfo.Owner != null)
		{
			sb.AppendLine("Owner: [" + AltarPointInfo.Owner.AlAbbr + "] " + AltarPointInfo.Owner.AlName);
		}
		if (AltarPointInfo.TmpOwner != null)
		{
			sb.AppendLine("TmpOwner: [" + AltarPointInfo.TmpOwner.AlAbbr + "] " + AltarPointInfo.TmpOwner.AlName);
		}
		RepeatedField<CityAltarOccupyInfo> occupylist = AltarPointInfo.Occupylist;
		if (occupylist != null && occupylist.Count > 0)
		{
			sb.AppendLine("---------- 祭坛占领信息 ----------");
			foreach (CityAltarOccupyInfo item in AltarPointInfo.Occupylist)
			{
				string text = ((item.Speed > 0) ? " <--" : "");
				string text2 = ((item.Et > 0) ? (", 满进度: " + GameEntry.Timer.TimeStampToTime(item.Et)) : "");
				sb.AppendLine($"[{item.UserInfo?.AlAbbr}] Score: {item.Score}, Speed: {item.Speed}{text2}{text}");
			}
		}
		if (AltarPointInfo.GiveupTime > 0)
		{
			sb.AppendLine("放弃时间: " + GameEntry.Timer.TimeStampToTime(AltarPointInfo.GiveupTime));
		}
		else
		{
			sb.AppendLine("没有放弃");
		}
		sb.AppendLine($"渔获状态: {AltarPointInfo.FishState}");
	}
}
