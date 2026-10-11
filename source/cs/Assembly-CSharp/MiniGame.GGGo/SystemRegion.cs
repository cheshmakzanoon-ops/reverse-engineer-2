using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class SystemRegion : IEcsInitSystem, IEcsSystem, IEcsDestroySystem, IEcsRunSystem
{
	private EcsWorldInject world;

	private readonly EcsSharedInject<GGGoEnv> _env;

	protected readonly EcsFilterInject<Inc<ComponentRegion>> _filterRegion;

	protected readonly EcsFilterInject<Inc<ComponentPosition, ComponentPlayer>> _filterPlayer;

	protected readonly EcsPoolInject<ComponentPosition> _poolPos;

	public void Init(IEcsSystems systems)
	{
		InitPlayerPos();
	}

	public void Destroy(IEcsSystems systems)
	{
	}

	public void Run(IEcsSystems systems)
	{
		Roll();
	}

	private void InitPlayerPos()
	{
		GGGoEnv value = _env.Value;
		if (value == null || value.GameState == EGameWorldState.Settlement)
		{
			return;
		}
		EcsFilter ecsFilter = world.Value.Filter<ComponentPlayer>().End();
		int num = 0;
		if (ecsFilter.GetEntitiesCount() > 1)
		{
			foreach (int item in ecsFilter)
			{
				num++;
				_poolPos.Value.Get(item).Position = ((num == 1) ? new FVector2(-0.6f, -1.5) : new FVector2(0.6f, -1.5));
			}
			return;
		}
		foreach (int item2 in ecsFilter)
		{
			_poolPos.Value.Get(item2).Position = new FVector2(-1.35, -2.6);
		}
	}

	private void Roll()
	{
		int lastEntity;
		FP fP = FuncRegion.GetRegionsLastPos(world.Value, out lastEntity) + _env.Value.Level.RangeVertical.Y;
		if (_env.Value.Distance <= fP)
		{
			_env.Value.Distance = fP;
			_env.Value.RollSpeed = FP.Zero;
			return;
		}
		if (_filterRegion.Value.GetEntitiesCount() > 0)
		{
			FP lowerPlayerPos = GetLowerPlayerPos();
			FP x = FuncRegion.GetCurSpeed(distance: _env.Value.Distance + _env.Value.Level.RangeVertical.X, world: world.Value, entity: out lastEntity);
			GGGoLevelConfig level = _env.Value.Level;
			FP x2 = level.RangeVertical.Y / FP._0_5;
			FP fP2 = _env.Value.Distance - level.RangeVertical.Y + x2 * level.LowerBound;
			if (lowerPlayerPos < fP2)
			{
				x *= level.LowerBoundSpeedRate;
			}
			_env.Value.RollSpeed = x;
		}
		if (FuncGame.IsPlaying(world.Value))
		{
			GGGoEnv value = _env.Value;
			value.Distance += _env.Value.RollSpeed * _env.Value.LogicTickDelta;
		}
	}

	private FP GetLowerPlayerPos()
	{
		FP fP = FP.MaxValue;
		foreach (int item in _filterPlayer.Value)
		{
			ComponentPosition componentPosition = _poolPos.Value.Get(item);
			if (componentPosition.Position.Y < fP)
			{
				fP = componentPosition.Position.Y;
			}
		}
		return fP;
	}
}
