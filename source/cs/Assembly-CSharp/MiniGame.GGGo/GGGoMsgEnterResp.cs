using MiniGame.Core.Server;

namespace MiniGame.GGGo;

public class GGGoMsgEnterResp : IMessageEnter
{
	public string LevelPath { get; set; }

	public int MaxPlayers { get; set; }
}
