using GameFramework;
using ProtoBufNet;
using UnityEngine;

public class CrossServerConnectGame : FsmBaseState
{
	private float _timeout = 6f;

	private float _elapseTime;

	public override int id => 3;

	public CrossServerConnectGame(CrossServerFsmManager mgr)
		: base(mgr)
	{
	}

	public override void OnEnter(params object[] args)
	{
		_elapseTime = 0f;
		string[] array = ((string)args[0]).Split(new char[1] { '|' });
		if (NetPacketConst.useForwardServerId)
		{
			OnGameConnection("E000", "");
			_mgr.crossing = true;
			return;
		}
		GameEntry.Network.Disconnect();
		GameEntry.Network.OnConnectionEvent = OnGameConnection;
		string[] array2 = array;
		int num = (int)args[1];
		string text = (string)args[2];
		string text2 = (string)args[3];
		int connectionType = (int)args[4];
		Log.Info($"[Net] [cross] ConnectGameState::{array2},port:{num},zone:{text},uid:{text2}");
		GameEntry.Network.Connect(array, num, text, connectionType);
		_mgr.crossing = true;
		if (text.StartsWith("APS"))
		{
			text = text.Substring(3);
		}
		CrossServerUtil.ShowCloud(text);
	}

	private void OnGameConnection(string err, string errorMessage)
	{
		if (string.IsNullOrEmpty(err) || err == "E000")
		{
			_mgr.SetState(4);
		}
		else
		{
			GotoLoadingError(7);
		}
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
		_elapseTime += Time.deltaTime;
		if (_elapseTime > _timeout)
		{
			GotoLoadingError(8);
		}
	}
}
