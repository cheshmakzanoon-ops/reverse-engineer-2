using Protobuf;

public class S0AllianceBossWorldPointInfo : PointInfo
{
	public AllianceBossS0BuildPointInfo buildPointInfo;

	public S0AllianceBossWorldPointInfo()
	{
	}

	public S0AllianceBossWorldPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		buildPointInfo = new AllianceBossS0BuildPointInfo(pi.AllianceBossS0BuildPointInfo);
		int.TryParse(GetModelSize(), out tileSize);
	}

	public override PointInfo Clone()
	{
		S0AllianceBossWorldPointInfo s0AllianceBossWorldPointInfo = new S0AllianceBossWorldPointInfo();
		BaseClone(s0AllianceBossWorldPointInfo);
		s0AllianceBossWorldPointInfo.buildPointInfo = buildPointInfo;
		return s0AllianceBossWorldPointInfo;
	}

	public string GetModelPath()
	{
		return GameEntry.ConfigCache.GetTemplateData("activity_alliance_boss_s0", buildPointInfo.cfgId, "building");
	}

	public string GetBuildingLodIconPath()
	{
		return GameEntry.ConfigCache.GetTemplateData("activity_alliance_boss_s0", buildPointInfo.cfgId, "lod_icon");
	}

	public string GetBuildingName()
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData("activity_alliance_boss_s0", buildPointInfo.cfgId, "name");
		string text = GameEntry.Localization.GetString(templateData);
		string templateData2 = GameEntry.ConfigCache.GetTemplateData("activity_alliance_boss_s0", buildPointInfo.cfgId, "difficulty");
		return "Lv." + templateData2 + " " + text;
	}

	public string GetModelSize()
	{
		if (int.TryParse(GameEntry.ConfigCache.GetTemplateData("activity_alliance_boss_s0", buildPointInfo.cfgId, "monsterId"), out var result))
		{
			return GameEntry.ConfigCache.GetTemplateData("lw_world_monster", result, "size");
		}
		return "";
	}
}
