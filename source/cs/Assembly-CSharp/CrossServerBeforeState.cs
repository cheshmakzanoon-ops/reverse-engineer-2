using GameFramework;
using GameKit.Base;

public class CrossServerBeforeState : FsmBaseState
{
	public override int id => 0;

	public CrossServerBeforeState(CrossServerFsmManager mgr)
		: base(mgr)
	{
	}

	public override void OnEnter(params object[] args)
	{
		string zone = AccountCredentialManager.ServerInfo.zone;
		if (!string.IsNullOrEmpty(zone) && ushort.TryParse(zone.Substring(3), out var result))
		{
			GameEntry.Network.Send(new UserDisconnectRequest());
			GameEntry.Network.UpdateSrcServerId(result);
		}
		else
		{
			Log.Error("CrossServerBeforeState::ZoneName format error :" + zone);
		}
		_mgr.SetState(1);
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
	}
}
