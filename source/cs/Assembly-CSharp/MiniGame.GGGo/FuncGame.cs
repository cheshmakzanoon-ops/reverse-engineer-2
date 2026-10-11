using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

public static class FuncGame
{
	public static bool IsPlaying(EcsWorld world)
	{
		if (world == null)
		{
			return false;
		}
		GGGoEnv shared = world.GetShared<GGGoEnv>();
		if (shared.GameState == EGameWorldState.Running)
		{
			return shared.LogicTime > shared.WaitToStartTime;
		}
		return false;
	}

	public static FP GetPlayingTime(EcsWorld world)
	{
		if (world == null)
		{
			return FP.Zero;
		}
		GGGoEnv shared = world.GetShared<GGGoEnv>();
		return FP.Max(FP.Zero, shared.LogicTime - (FP)shared.WaitToStartTime);
	}
}
