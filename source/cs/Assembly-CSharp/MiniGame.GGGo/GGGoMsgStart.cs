using MiniGame.Core.Server;

namespace MiniGame.GGGo;

public class GGGoMsgStart : IMessageStart
{
	public string[] PlayerSessionIDs { get; set; }
}
