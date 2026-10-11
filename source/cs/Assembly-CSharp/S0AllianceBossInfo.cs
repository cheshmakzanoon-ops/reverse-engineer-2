using System;
using Sfs2X.Entities.Data;

public class S0AllianceBossInfo : IDisposable
{
	public int cfgId;

	public long battleStartTime;

	public long battleEndTime;

	public long damage;

	public long actEndTime;

	public int rewardProgress;

	public long maxDamage;

	public int bossType;

	public long lastStarTime;

	public void Update(ISFSObject q)
	{
		cfgId = q.TryGetInt("cfgId");
		battleStartTime = q.TryGetLong("battleStartTime");
		battleEndTime = q.TryGetLong("battleEndTime");
		rewardProgress = q.TryGetInt("rewardProgress");
		damage = q.TryGetLong("damage");
		actEndTime = q.TryGetLong("shieldEndTime");
		maxDamage = q.TryGetLong("maxDamage");
		bossType = q.TryGetInt("bossType");
		lastStarTime = q.TryGetLong("lastStarTime");
	}

	public void Dispose()
	{
	}
}
