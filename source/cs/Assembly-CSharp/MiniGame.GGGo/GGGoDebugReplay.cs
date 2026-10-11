using System.Collections.Generic;

namespace MiniGame.GGGo;

internal class GGGoDebugReplay
{
	public string LevelPath;

	public List<SyncCommand> Commands;

	public List<int> Frames = new List<int>();
}
