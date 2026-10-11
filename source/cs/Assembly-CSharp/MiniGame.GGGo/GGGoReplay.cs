using System.Collections.Generic;

namespace MiniGame.GGGo;

public class GGGoReplay
{
	public string LevelPath;

	public string MD5;

	public List<SyncCommand> Commands = new List<SyncCommand>();
}
