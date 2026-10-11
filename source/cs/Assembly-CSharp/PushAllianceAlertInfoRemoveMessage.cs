using Sfs2X.Entities.Data;

public class PushAllianceAlertInfoRemoveMessage : BaseMessage
{
	private static PushAllianceAlertInfoRemoveMessage _instance;

	public static PushAllianceAlertInfoRemoveMessage Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = MessageFactory.GetMessage<PushAllianceAlertInfoRemoveMessage>();
			}
			return _instance;
		}
	}

	public override string GetMsgId()
	{
		return "push.lw.alliance.alert.info.remove";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		PushAllianceAlertInfoManager.Instance.AddAlertInfoMsg(GetMsgId(), message);
	}
}
