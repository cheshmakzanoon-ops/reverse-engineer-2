using Box2DSharp.Common;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class GGGoEnv : GameSharedEnv
{
	public bool ClientGameOver;

	public bool ServerGameOver;

	public bool CanCheckGameOver;

	public FP RollSpeed;

	public FP Distance;

	public EGameType GameType = EGameType.PveClient;

	public GGGoGameResult GameResult;

	public GGGoInitData InitData;

	public GGGoLevelConfig Level = new GGGoLevelConfig();

	public BuffData[] BuffDatas = new BuffData[30];

	public SpawnCacheEntry[] SpawnCache;

	public static bool Dump;

	public GGGoCommand Commands { get; set; }

	public override IResourceLoader ResourceLoader { get; set; }

	public override object TakeSnapshot()
	{
		return Level;
	}

	public override void RestoreSnapshot(object snapshot)
	{
		Level = snapshot as GGGoLevelConfig;
	}

	public override IGameLevelConfig GetLevelConfig()
	{
		return Level;
	}
}
