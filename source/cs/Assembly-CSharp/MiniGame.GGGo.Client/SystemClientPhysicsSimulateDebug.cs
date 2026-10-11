using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.GGGo.Client;

public class SystemClientPhysicsSimulateDebug : IEcsInitSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnvClient> _env;

	public void Init(IEcsSystems systems)
	{
	}
}
