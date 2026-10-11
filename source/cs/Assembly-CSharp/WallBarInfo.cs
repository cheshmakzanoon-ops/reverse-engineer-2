using Protobuf;

public class WallBarInfo
{
	public int shieldType;

	public long shieldMaxValue;

	public long expireTime;

	public long shieldValue;

	public WallBarInfo(ShieldInfo msg)
	{
		ParseData(msg);
	}

	public void ParseData(ShieldInfo msg)
	{
		shieldType = msg.ShieldType;
		shieldMaxValue = msg.ShieldMaxValue;
		expireTime = msg.ExpireTime;
		shieldValue = msg.ShieldValue;
	}

	public bool IsValid()
	{
		if (expireTime < 0)
		{
			return false;
		}
		if (expireTime == 0L)
		{
			return true;
		}
		if (GameEntry.Timer.GetServerTime() <= expireTime)
		{
			return true;
		}
		expireTime = -1L;
		return false;
	}
}
