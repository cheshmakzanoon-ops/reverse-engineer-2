using MiniGame.Core.Server;

namespace MiniGame.GGGo;

public class GGGoMsgCheckValidationResp : IMessageVerify
{
	public int Code;

	public string Message;
}
