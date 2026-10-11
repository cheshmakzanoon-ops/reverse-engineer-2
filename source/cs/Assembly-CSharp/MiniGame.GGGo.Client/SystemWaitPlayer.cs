using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.GGGo.Client;

public class SystemWaitPlayer : IEcsRunSystem, IEcsSystem
{
	private EcsWorldInject world;

	private readonly EcsSharedInject<GGGoEnv> _env;

	private readonly EcsFilterInject<Inc<ComponentPlayer, ComponentData>> _playerFilter;

	public void Run(IEcsSystems systems)
	{
		GGGoEnv value = _env.Value;
		if (value != null && value.GameState == EGameWorldState.Preparing && _playerFilter.Value.GetEntitiesCount() > 0)
		{
			value.GameState = EGameWorldState.Running;
		}
	}
}
