using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.GGGo.Client;

public class SystemFireRender : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnvClient> _env;

	public void Run(IEcsSystems systems)
	{
		GGGoEnvClient value = _env.Value;
		Queue<IRender> renderQueue = value.RenderQueue;
		while (renderQueue.Count > 0)
		{
			IRender render = renderQueue.Dequeue();
			value.UIAction?.Invoke(render);
			render.Recycle();
		}
	}
}
