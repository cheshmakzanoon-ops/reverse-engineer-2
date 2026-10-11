using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.GGGo;

public class SystemGravity : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnv> _env;

	protected readonly EcsFilterInject<Inc<ComponentGravity, ComponentVelocity>, Exc<ComponentStatic>> _filter;

	protected readonly EcsPoolInject<ComponentVelocity> _poolVelocity;

	protected readonly EcsPoolInject<ComponentRigidBody> _poolRig;

	public void Run(IEcsSystems systems)
	{
		FP y = _env.Value.PhysicsTickDelta;
		foreach (int item in _filter.Value)
		{
			if (!_poolRig.Value.Has(item) || !_poolRig.Value.Get(item).IsGrounded)
			{
				ref ComponentGravity reference = ref _filter.Pools.Inc1.Get(item);
				ref FP y2 = ref _poolVelocity.Value.Get(item).Velocity.Y;
				y2 += reference.Value * y;
			}
		}
	}
}
