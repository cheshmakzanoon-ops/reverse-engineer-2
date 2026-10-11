using MiniGame.Core.Server;

namespace MiniGame.GGGo;

public class GGGoMsgStartResp : IMessageStart
{
	public string[] PlayerSessionIDs { get; set; }
}
