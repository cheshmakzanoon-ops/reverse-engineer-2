using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using XLua;

[Hotfix(HotfixFlag.Stateless)]
public class GetFullPushWorldMarchMessage : BaseMessage
{
	private static GetFullPushWorldMarchMessage _instance;

	public static GetFullPushWorldMarchMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<GetFullPushWorldMarchMessage>());

	public override string GetMsgId()
	{
		return "world.march.full.info.view";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		long val = (long)args[0];
		ISFSObject iSFSObject = new SFSObject();
		iSFSObject.PutLong("uuid", val);
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}
}
