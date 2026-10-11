using System.Collections.Generic;

public class LuaCacheDataManager : WorldManagerBase
{
	private Dictionary<long, Dictionary<string, PlayerType>> playerTypeCache = new Dictionary<long, Dictionary<string, PlayerType>>();

	public LuaCacheDataManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
	}

	public override void UnInit()
	{
		CleanAllianceCacheData();
	}

	public void CleanAllianceCacheData()
	{
		foreach (Dictionary<string, PlayerType> value in playerTypeCache.Values)
		{
			value.Clear();
		}
		playerTypeCache.Clear();
	}

	public void OnWorldColorDirty(string allianceId)
	{
		foreach (KeyValuePair<long, Dictionary<string, PlayerType>> item in playerTypeCache)
		{
			if (item.Value.ContainsKey(allianceId))
			{
				item.Value.Remove(allianceId);
			}
		}
	}

	public PlayerType IsMyEnemy(int serverId, string allianceId, int currentServerId = 0)
	{
		if (serverId <= 0 && allianceId.IsNullOrEmpty())
		{
			return PlayerType.PlayerNone;
		}
		long key = currentServerId | ((long)serverId << 32);
		if (!playerTypeCache.TryGetValue(key, out var value))
		{
			value = new Dictionary<string, PlayerType>();
			playerTypeCache[key] = value;
		}
		if (!value.TryGetValue(allianceId, out var value2))
		{
			value2 = (value[allianceId] = GameEntry.Lua.CallWithReturn<PlayerType, int, string, string, int>("CSharpCallLuaInterface.CheckPlayerType", serverId, allianceId, null, currentServerId));
		}
		return value2;
	}
}
