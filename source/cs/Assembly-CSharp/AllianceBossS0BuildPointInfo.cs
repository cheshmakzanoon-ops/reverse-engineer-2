using Protobuf;
using Sfs2X.Entities.Data;

public class AllianceBossS0BuildPointInfo
{
	public long startTime;

	public string allianceId;

	public int cfgId;

	public string abbr;

	public long lastReserveTime;

	public long actEndTime;

	public AllianceBossS0BuildPointInfo()
	{
	}

	public AllianceBossS0BuildPointInfo(ISFSObject msg)
	{
		ParseData(msg);
	}

	public AllianceBossS0BuildPointInfo(Protobuf.AllianceBossS0BuildPointInfo msg)
	{
		ParseData(msg);
	}

	public virtual void ParseData(ISFSObject msg)
	{
		startTime = msg.TryGetLong("startTime");
		allianceId = msg.TryGetString("allianceId");
		cfgId = msg.TryGetInt("cfgId");
		abbr = msg.TryGetString("abbr");
		lastReserveTime = msg.TryGetLong("lastReserveTime");
		actEndTime = msg.TryGetLong("actEndTime");
	}

	protected virtual void ParseData(Protobuf.AllianceBossS0BuildPointInfo msg)
	{
		allianceId = msg.AllianceId;
		startTime = msg.StartTime;
		cfgId = msg.CfgId;
		abbr = msg.Abbr;
		lastReserveTime = msg.LastReserveTime;
		actEndTime = msg.ActEndTime;
	}
}
