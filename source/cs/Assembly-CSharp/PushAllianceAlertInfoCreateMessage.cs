using Sfs2X.Entities.Data;

public class PushAllianceAlertInfoCreateMessage : BaseMessage
{
	private static PushAllianceAlertInfoCreateMessage _instance;

	public static PushAllianceAlertInfoCreateMessage Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = MessageFactory.GetMessage<PushAllianceAlertInfoCreateMessage>();
			}
			return _instance;
		}
	}

	public override string GetMsgId()
	{
		return "push.lw.alliance.alert.info.create";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		PushAllianceAlertInfoManager.Instance.AddAlertInfoMsg(GetMsgId(), message);
	}
}
