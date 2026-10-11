using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("游戏结束检查", "功能")]
public struct CheckGameOverAction : IAction
{
	public int UniqueID;

	public bool IsDestination;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		GGGoEnv shared = world.GetShared<GGGoEnv>();
		shared.CanCheckGameOver = true;
		if (action is CheckGameOverAction checkGameOverAction)
		{
			shared.GameResult.IsDestination = checkGameOverAction.IsDestination;
		}
	}
}
