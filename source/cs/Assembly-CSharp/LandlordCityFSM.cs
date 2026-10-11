using System;

public class LandlordCityFSM : IDisposable
{
	private LLAllyCityPointInfo _pointInfo;

	private int _prepareBoomTime;

	private ITimer _timer;

	public LandlordConst.LLBuildingState State { get; private set; }

	public int RemainTime { get; private set; }

	public int PrepareBoomTime => _prepareBoomTime;

	public LandlordCityFSM(LLAllyCityPointInfo pointInfo)
	{
		if (pointInfo != null)
		{
			string cityTableName = LandlordManager.Instance.cityTableName;
			if (!string.IsNullOrEmpty(cityTableName))
			{
				_prepareBoomTime = GameEntry.ConfigCache.GetTemplateData(cityTableName, pointInfo.cityId, "boom_time").ToInt();
			}
			else
			{
				_prepareBoomTime = 10;
			}
			State = LandlordConst.LLBuildingState.None;
			UpdateByPointInfo(pointInfo);
		}
	}

	public void Dispose()
	{
		ClearStateTimers();
	}

	public void UpdateByPointInfo(LLAllyCityPointInfo pointInfo)
	{
		_pointInfo = pointInfo;
		DeriveState();
	}

	private bool IsExploding(long now)
	{
		int num;
		if (_pointInfo.ownerCampId == 2 && _pointInfo.state == 4 && _pointInfo.overTime + _prepareBoomTime * 1000 <= now)
		{
			num = ((_pointInfo.overTime + (_prepareBoomTime + 10) * 1000 > now) ? 1 : 0);
			if (num != 0 && _timer == null)
			{
				long num2 = (_pointInfo.overTime + (_prepareBoomTime + 10) * 1000 - now) / 1000 + 1;
				_timer = GameEntry.Timer.RegisterTimer(num2, delegate
				{
					GameEntry.Timer.CancelTimer(_timer);
					_timer = null;
					DeriveState();
				});
			}
		}
		else
		{
			num = 0;
		}
		return (byte)num != 0;
	}

	private bool IsWillExplode(long now)
	{
		int num;
		if (_pointInfo.ownerCampId == 2 && _pointInfo.state == 4 && _pointInfo.overTime <= now)
		{
			num = ((_pointInfo.overTime + _prepareBoomTime * 1000 > now) ? 1 : 0);
			if (num != 0 && _timer == null)
			{
				long num2 = (_pointInfo.overTime + _prepareBoomTime * 1000 - now) / 1000 + 1;
				_timer = GameEntry.Timer.RegisterTimer(num2, delegate
				{
					GameEntry.Timer.CancelTimer(_timer);
					_timer = null;
					DeriveState();
				});
			}
		}
		else
		{
			num = 0;
		}
		return (byte)num != 0;
	}

	private bool IsRuins(long now)
	{
		if ((_pointInfo.type != 101 && _pointInfo.type != 105) || _pointInfo.state != 1)
		{
			if (_pointInfo.state == 4 && _pointInfo.ownerCampId == 2)
			{
				return _pointInfo.overTime + (_prepareBoomTime + 10) * 1000 <= now;
			}
			return false;
		}
		return true;
	}

	private bool IsRebuilding(long now)
	{
		return _pointInfo.state == 2;
	}

	private bool IsFighting(long now)
	{
		bool flag = GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsLandlordBattleStageAndWeekUnlock", _pointInfo.cityId);
		bool flag2 = _pointInfo.state == 3 && flag && now > _pointInfo.unlockTime;
		long num = (GameEntry.Lua.CallWithReturn<long>("CSharpCallLuaInterface.GetLLCurActEndTime") - now) / 1000 + 1;
		if (num > 0 && flag2)
		{
			_timer = GameEntry.Timer.RegisterTimer(num, delegate
			{
				GameEntry.Timer.CancelTimer(_timer);
				_timer = null;
				DeriveState();
			});
		}
		return flag2;
	}

	private bool IsNotOpen(long now)
	{
		long num = GameEntry.Lua.CallWithReturn<long, int>("CSharpCallLuaInterface.GetLLCityNextOpenTimeByCityId", _pointInfo.cityId);
		if (num > 0)
		{
			long num2 = (num - now) / 1000 + 1;
			if (num2 > 0)
			{
				_timer = GameEntry.Timer.RegisterTimer(num2, delegate
				{
					GameEntry.Timer.CancelTimer(_timer);
					_timer = null;
					DeriveState();
				});
			}
			return num2 > 0;
		}
		return false;
	}

	private bool IsOpenButShield(long now)
	{
		if (_pointInfo.unlockTime > 0)
		{
			bool num = GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsLandlordBattleStageAndWeekUnlock", _pointInfo.cityId);
			long num2 = (_pointInfo.unlockTime - now) / 1000 + 1;
			if (num && num2 > 0)
			{
				_timer = GameEntry.Timer.RegisterTimer(num2, delegate
				{
					GameEntry.Timer.CancelTimer(_timer);
					_timer = null;
					DeriveState();
				});
				return true;
			}
		}
		return false;
	}

	private void ClearStateTimers()
	{
		if (_timer != null)
		{
			GameEntry.Timer.CancelTimer(_timer);
			_timer = null;
		}
	}

	private void DeriveState()
	{
		ClearStateTimers();
		long serverTime = GameEntry.Timer.GetServerTime();
		LandlordConst.LLBuildingState lLBuildingState = LandlordConst.LLBuildingState.NotOpen;
		if (IsExploding(serverTime))
		{
			lLBuildingState = LandlordConst.LLBuildingState.Exploding;
		}
		else if (IsWillExplode(serverTime))
		{
			lLBuildingState = LandlordConst.LLBuildingState.WillExplode;
		}
		else if (IsRuins(serverTime))
		{
			lLBuildingState = LandlordConst.LLBuildingState.Ruins;
		}
		else if (IsRebuilding(serverTime))
		{
			lLBuildingState = LandlordConst.LLBuildingState.Rebuilding;
		}
		else if (IsFighting(serverTime))
		{
			lLBuildingState = LandlordConst.LLBuildingState.Fighting;
		}
		else if (IsOpenButShield(serverTime))
		{
			lLBuildingState = LandlordConst.LLBuildingState.OpenButShield;
		}
		else if (IsNotOpen(serverTime))
		{
			lLBuildingState = LandlordConst.LLBuildingState.NotOpen;
		}
		if (lLBuildingState != State)
		{
			bool num = State != LandlordConst.LLBuildingState.None;
			State = lLBuildingState;
			if (num)
			{
				GameEntry.Event.Fire(EventId.LandlordCityPointChangeClientState, _pointInfo?.pointIndex);
			}
		}
	}
}
