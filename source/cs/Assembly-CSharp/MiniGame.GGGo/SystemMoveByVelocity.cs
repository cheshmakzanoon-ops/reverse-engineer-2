using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.GGGo;

public class SystemMoveByVelocity : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnv> _env;

	protected readonly EcsFilterInject<Inc<ComponentPosition, ComponentVelocity>, Exc<ComponentStatic>> _filterPos;

	protected readonly EcsFilterInject<Inc<ComponentPosition, ComponentMoveDirection>, Exc<ComponentStatic>> _filterDirection;

	protected readonly EcsPoolInject<ComponentPosition> _poolPos;

	protected readonly EcsPoolInject<ComponentVelocity> _poolVelocity;

	protected readonly EcsPoolInject<ComponentMoveDirection> _poolDirection;

	public void Run(IEcsSystems systems)
	{
		FP physicsTickDelta = _env.Value.PhysicsTickDelta;
		foreach (int item in _filterPos.Value)
		{
			ref ComponentPosition reference = ref _poolPos.Value.Get(item);
			ComponentVelocity componentVelocity = _poolVelocity.Value.Get(item);
			if (_poolDirection.Value.Has(item))
			{
				componentVelocity.Velocity += _poolDirection.Value.Get(item).Value;
			}
			reference.PrevPosition = reference.Position;
			reference.Position += componentVelocity.Velocity * physicsTickDelta;
		}
		FP logicTime = _env.Value.LogicTime;
		foreach (int item2 in _filterDirection.Value)
		{
			ComponentMoveDirection componentMoveDirection = _poolDirection.Value.Get(item2);
			if (componentMoveDirection.EndTime > FP.Zero && logicTime >= componentMoveDirection.EndTime)
			{
				_poolDirection.Value.Del(item2);
			}
		}
	}
}
