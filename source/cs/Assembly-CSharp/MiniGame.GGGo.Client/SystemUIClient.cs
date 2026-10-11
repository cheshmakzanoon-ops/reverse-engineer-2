using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.GGGo.Client;

public class SystemUIClient : IEcsInitSystem, IEcsSystem, IEcsDestroySystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnvClient> _env;

	protected readonly EcsFilterInject<Inc<ComponentPlayer, ComponentUnityPrefab>, Exc<ComponentControllerClient>> _filterPlayer;

	private readonly EcsPoolInject<ComponentPlayer> _poolPlayer;

	private readonly EcsPoolInject<ComponentUnityPrefab> _poolPrefab;

	private readonly EcsPoolInject<ComponentControllerClient> _poolController;

	public void Init(IEcsSystems systems)
	{
		FuncTCClient.InitAction(_world.Value);
	}

	public void Destroy(IEcsSystems systems)
	{
		FuncTCClient.ClearAction(_world.Value);
	}
}
