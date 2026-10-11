using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class SystemAcceleration : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnv> _env;

	protected readonly EcsFilterInject<Inc<ComponentAcceleration, ComponentVelocity>, Exc<ComponentStatic>> _filter;

	protected readonly EcsFilterInject<Inc<ComponentAcceleration, ComponentVelocity, ComponentControlDisable>> _filterControlDisable;

	protected readonly EcsPoolInject<ComponentAcceleration> _poolAcc;

	protected readonly EcsPoolInject<ComponentVelocity> _poolVelocity;

	protected readonly EcsPoolInject<ComponentStatic> _poolStatic;

	protected readonly EcsPoolInject<ComponentData> _poolData;

	public void Run(IEcsSystems systems)
	{
		FP x = _env.Value.PhysicsTickDelta;
		FP slowDownFactor = x * (FP)10;
		foreach (int item in _filterControlDisable.Value)
		{
			ref ComponentAcceleration reference = ref _poolAcc.Value.Get(item);
			ref ComponentVelocity reference2 = ref _poolVelocity.Value.Get(item);
			if (_poolStatic.Value.Has(item))
			{
				reference.Value = FVector2.Zero;
				reference2.Velocity = FVector2.Zero;
			}
			else
			{
				reference.Value.X = FP.Zero;
			}
		}
		foreach (int item2 in _filter.Value)
		{
			ref ComponentAcceleration reference3 = ref _poolAcc.Value.Get(item2);
			ref ComponentVelocity reference4 = ref _poolVelocity.Value.Get(item2);
			FP x2 = FP.One;
			if (_poolData.Value.Has(item2))
			{
				FP y = _poolData.Value.Get(item2).GetPropertyValue(PropertyID.MoveSpeedRate);
				if (y != FP.Zero)
				{
					x2 += y;
				}
			}
			if (reference3.Value.X == FP.Zero)
			{
				SlowDown(ref reference4, reference3, slowDownFactor);
			}
			else
			{
				ref FP x3 = ref reference4.Velocity.X;
				x3 += reference3.Value.X * x * x2;
			}
			ref FP y2 = ref reference4.Velocity.Y;
			y2 += reference3.Value.Y * x;
			FP y3 = reference4.MaxSpeed.X * x2;
			if (y3 > FP.Zero)
			{
				if (FP.Abs(reference4.Velocity.X) > y3)
				{
					reference4.Velocity.X = (FP)FP.Sign(reference4.Velocity.X) * y3;
				}
				if (FP.Abs(reference4.Velocity.Y) > reference4.MaxSpeed.Y)
				{
					reference4.Velocity.Y = (FP)FP.Sign(reference4.Velocity.Y) * reference4.MaxSpeed.Y;
				}
			}
		}
	}

	private void SlowDown(ref ComponentVelocity velocity, ComponentAcceleration acc, FP slowDownFactor)
	{
		if (!(velocity.Velocity.X == FP.Zero))
		{
			FP x = velocity.MaxSpeed.X * slowDownFactor;
			if (FP.Abs(velocity.Velocity.X) <= x + FP.EN1)
			{
				velocity.Velocity.X = FP.Zero;
			}
			else if (velocity.Velocity.X > FP.Zero)
			{
				ref FP x2 = ref velocity.Velocity.X;
				x2 -= x;
			}
			else
			{
				ref FP x3 = ref velocity.Velocity.X;
				x3 += x;
			}
		}
	}
}
