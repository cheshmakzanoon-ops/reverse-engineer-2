using System.Collections.Generic;
using Sfs2X.Entities.Data;

public class PushAllianceAlertInfoManager
{
	private static PushAllianceAlertInfoManager _instance;

	private Queue<ISFSObject> _msgs = new Queue<ISFSObject>();

	private Queue<string> _names = new Queue<string>();

	private ITimer _timer;

	public static PushAllianceAlertInfoManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PushAllianceAlertInfoManager();
			}
			return _instance;
		}
	}

	public void AddAlertInfoMsg(string msgName, ISFSObject message)
	{
		if (GameEntry.Data?.Player?.CheckSwitch("opt_push_alliance_alert", defaultVal: false) ?? false)
		{
			_names.Enqueue(msgName);
			_msgs.Enqueue(message);
			AddTimer();
		}
		else
		{
			SendByPCallStackTable(msgName, message);
		}
	}

	public void AddTimer()
	{
		if (_timer == null)
		{
			_timer = GameEntry.Timer.RegisterTimerRepeat(0.1f, 0.02f, DelayHandleMsgs);
		}
	}

	public void DeleteTimer()
	{
		if (_timer != null)
		{
			GameEntry.Timer.CancelTimer(_timer);
			_timer = null;
		}
	}

	private void DelayHandleMsgs()
	{
		ISFSObject iSFSObject = null;
		string msgName = "";
		if (_msgs.Count > 0)
		{
			iSFSObject = _msgs.Dequeue();
			msgName = _names.Dequeue();
		}
		if (iSFSObject == null)
		{
			DeleteTimer();
		}
		else
		{
			SendByPCallStackTable(msgName, iSFSObject);
		}
	}

	private void SendByPCallStackTable(string msgName, ISFSObject message)
	{
		LuaStackTable luaStackTable = (message as SFSObject).ToLuaTable(GameEntry.Lua.Env);
		GameEntry.Lua.DispatchResponse(msgName, luaStackTable);
	}
}
