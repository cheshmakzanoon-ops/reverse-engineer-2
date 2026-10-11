using MiniGame.Core.Server;

namespace MiniGame.GGGo;

public class GGGoMsgEnter : IMessageEnter
{
	public string LevelPath { get; set; }

	public int MaxPlayers { get; set; }
}
