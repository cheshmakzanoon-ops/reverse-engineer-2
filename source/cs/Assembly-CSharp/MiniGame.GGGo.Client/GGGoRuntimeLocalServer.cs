using MiniGame.Core;
using MiniGame.Core.Client;

namespace MiniGame.GGGo.Client;

public class GGGoRuntimeLocalServer : GameUnityRuntime
{
	public override bool IsPlaying
	{
		get
		{
			return true;
		}
		protected set
		{
		}
	}

	public override GameWorld Game { get; protected set; }

	public override GameClientPlayer ClientPlayer { get; protected set; }

	public void StartGame(int ping1 = 0, int ping2 = 0, int pingJitter = 0)
	{
		GameUnityApp.Init();
	}

	public override void ExitGame()
	{
		Game = null;
	}
}
