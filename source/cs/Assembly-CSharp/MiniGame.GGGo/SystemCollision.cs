using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.GGGo;

public class SystemCollision : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnv> _env;

	protected readonly EcsFilterInject<Inc<ComponentPosition, ComponentCollider, ComponentRigidBody>, Exc<ComponentStatic>> _filterRig;

	protected readonly EcsFilterInject<Inc<ComponentPosition, ComponentCollider>, Exc<ComponentRigidBody, ComponentColliderDisable>> _filterCollider;

	protected readonly EcsPoolInject<ComponentPosition> _poolPos;

	protected readonly EcsPoolInject<ComponentCollider> _poolCollider;

	protected readonly EcsPoolInject<ComponentColliderDisable> _poolColliderDisable;

	protected readonly EcsPoolInject<ComponentVelocity> _poolVel;

	protected readonly EcsPoolInject<ComponentRigidBody> _poolRigid;

	private const int MaxTempColliders = 64;

	private readonly int[] _tempColliderEntities = new int[64];

	private readonly FVector2[] _tempCenters = new FVector2[64];

	private readonly FVector2[] _tempHalfSizes = new FVector2[64];

	private int _tempIndex;

	private readonly FVector2[] _tempContactPoints = new FVector2[64];

	public void Run(IEcsSystems systems)
	{
		foreach (int item in _filterRig.Value)
		{
			if (!_poolColliderDisable.Value.Has(item))
			{
				ref ComponentRigidBody reference = ref _poolRigid.Value.Get(item);
				ComponentPosition componentPosition = _poolPos.Value.Get(item);
				ref ComponentCollider reference2 = ref _poolCollider.Value.Get(item);
				if (reference.ContinuousDetection)
				{
					ContinuousCollectColliders(item, componentPosition.PrevPosition + reference2.Offset, componentPosition.Position + reference2.Offset, reference2.HalfSize);
				}
				else
				{
					CollectColliders(item, componentPosition.Position + reference2.Offset, reference2.HalfSize);
				}
				if (_tempIndex > 0)
				{
					reference.IsGrounded = ResolveCollisionsY(item);
					_tempIndex = 0;
				}
				else
				{
					reference.IsGrounded = CheckGround(item);
				}
			}
		}
	}

	private bool CheckGround(int entity)
	{
		ref ComponentPosition reference = ref _poolPos.Value.Get(entity);
		ref ComponentCollider reference2 = ref _poolCollider.Value.Get(entity);
		FVector2 fVector = reference.Position + reference2.Offset;
		FVector2 halfSize = reference2.HalfSize;
		if (_poolVel.Value.Has(entity) && _poolVel.Value.Get(entity).Velocity.Y != FP.Zero)
		{
			return false;
		}
		FP eN = FP.EN2;
		FP x = fVector.Y - halfSize.Y;
		FP fP = fVector.X - halfSize.X;
		FP fP2 = fVector.X + halfSize.X;
		foreach (int item in _filterCollider.Value)
		{
			if (item == entity)
			{
				continue;
			}
			ref ComponentCollider reference3 = ref _poolCollider.Value.Get(item);
			FVector2 fVector2 = _poolPos.Value.Get(item).Position + reference3.Offset;
			FVector2 halfSize2 = reference3.HalfSize;
			FP fP3 = x - (fVector2.Y + halfSize2.Y);
			if (!(fP3 > eN) && !(fP3 < -FP.EN3))
			{
				FP fP4 = fVector2.X - halfSize2.X;
				FP fP5 = fVector2.X + halfSize2.X;
				if (!(((fP2 < fP5) ? fP2 : fP5) - ((fP > fP4) ? fP : fP4) < -eN) && fP3 <= eN)
				{
					return true;
				}
			}
		}
		return false;
	}

	private int CollectColliders(int entity, FVector2 pos, FVector2 half, bool reset = true)
	{
		if (reset)
		{
			_tempIndex = 0;
		}
		foreach (int item in _filterCollider.Value)
		{
			if (entity == item)
			{
				continue;
			}
			ref ComponentCollider reference = ref _poolCollider.Value.Get(item);
			FVector2 fVector = _poolPos.Value.Get(item).Position + reference.Offset;
			FVector2 halfSize = reference.HalfSize;
			if (ComponentCollisionExtensions.RectOverlaps(pos, half, fVector, halfSize))
			{
				if (_tempIndex >= _tempColliderEntities.Length)
				{
					_world.Value.LogError("SystemCollision: tempColliders array size exceeded, entity id: " + entity);
					break;
				}
				if (!TempContains(item))
				{
					_tempColliderEntities[_tempIndex] = item;
					_tempCenters[_tempIndex] = fVector;
					_tempHalfSizes[_tempIndex] = halfSize;
					_tempContactPoints[_tempIndex] = pos;
					_tempIndex++;
				}
			}
		}
		return _tempIndex;
	}

	private bool AppendCollider(int entityB, FVector2 centerB, FVector2 halfB, FVector2 contactPoint)
	{
		if (TempContains(entityB))
		{
			return false;
		}
		if (_tempIndex >= _tempColliderEntities.Length)
		{
			_world.Value.LogError("SystemCollision: tempColliders array size exceeded when append, entity id: " + entityB);
			return false;
		}
		_tempColliderEntities[_tempIndex] = entityB;
		_tempCenters[_tempIndex] = centerB;
		_tempHalfSizes[_tempIndex] = halfB;
		_tempContactPoints[_tempIndex] = contactPoint;
		_tempIndex++;
		return true;
	}

	private bool TempContains(int entityId)
	{
		for (int i = 0; i < _tempIndex; i++)
		{
			if (_tempColliderEntities[i] == entityId)
			{
				return true;
			}
		}
		return false;
	}

	private int ContinuousCollectColliders(int entity, FVector2 prevPos, FVector2 currPos, FVector2 half)
	{
		CollectColliders(entity, currPos, half);
		FVector2 fVector = currPos - prevPos;
		FP fP = FP.Abs(fVector.X);
		FP fP2 = FP.Abs(fVector.Y);
		FP fP3 = half.X + half.X;
		FP fP4 = half.Y + half.Y;
		if (fP <= fP3 && fP2 <= fP4)
		{
			return _tempIndex;
		}
		CollectColliders(entity, prevPos, half, reset: false);
		foreach (int item in _filterCollider.Value)
		{
			if (entity == item || TempContains(item))
			{
				continue;
			}
			ref ComponentCollider reference = ref _poolCollider.Value.Get(item);
			FVector2 centerB = _poolPos.Value.Get(item).Position + reference.Offset;
			FVector2 halfSize = reference.HalfSize;
			FVector2 fVector2 = new FVector2(halfSize.X + half.X, halfSize.Y + half.Y);
			FP fP5 = FP.Zero;
			FP fP6 = FP.One;
			bool flag = false;
			FP x = fVector.X;
			FP y = prevPos.X;
			FP x2 = centerB.X - fVector2.X;
			FP x3 = centerB.X + fVector2.X;
			if (x == FP.Zero)
			{
				if (y < x2 || y > x3)
				{
					flag = true;
				}
			}
			else
			{
				FP y2 = FP.One / x;
				FP fP7 = (x2 - y) * y2;
				FP fP8 = (x3 - y) * y2;
				if (fP7 > fP8)
				{
					FP fP9 = fP7;
					fP7 = fP8;
					fP8 = fP9;
				}
				if (fP7 > fP5)
				{
					fP5 = fP7;
				}
				if (fP8 < fP6)
				{
					fP6 = fP8;
				}
				if (fP5 > fP6)
				{
					flag = true;
				}
			}
			if (flag)
			{
				continue;
			}
			FP y3 = fVector.Y;
			FP y4 = prevPos.Y;
			FP x4 = centerB.Y - fVector2.Y;
			FP x5 = centerB.Y + fVector2.Y;
			if (y3 == FP.Zero)
			{
				if (y4 < x4 || y4 > x5)
				{
					flag = true;
				}
			}
			else
			{
				FP y5 = FP.One / y3;
				FP fP10 = (x4 - y4) * y5;
				FP fP11 = (x5 - y4) * y5;
				if (fP10 > fP11)
				{
					FP fP12 = fP10;
					fP10 = fP11;
					fP11 = fP12;
				}
				if (fP10 > fP5)
				{
					fP5 = fP10;
				}
				if (fP11 < fP6)
				{
					fP6 = fP11;
				}
				if (fP5 > fP6)
				{
					flag = true;
				}
			}
			if (!flag && fP6 >= FP.Zero && fP5 <= FP.One)
			{
				FP fP13 = fP5;
				if (fP13 < FP.Zero)
				{
					fP13 = FP.Zero;
				}
				if (fP13 > FP.One)
				{
					fP13 = FP.One;
				}
				FVector2 contactPoint = prevPos + fVector * fP13;
				AppendCollider(item, centerB, halfSize, contactPoint);
			}
		}
		return _tempIndex;
	}

	private bool ResolveCollisionsY(int entity)
	{
		ref ComponentPosition reference = ref _poolPos.Value.Get(entity);
		ref ComponentCollider reference2 = ref _poolCollider.Value.Get(entity);
		FVector2 fVector = reference.Position + reference2.Offset;
		FVector2 fVector2 = reference.PrevPosition + reference2.Offset;
		FVector2 halfSize = reference2.HalfSize;
		bool flag = false;
		FP x = fVector.Y;
		FP y = FP.EN3;
		bool flag2 = _poolVel.Value.Has(entity);
		FP fP = FP.Zero;
		if (flag2)
		{
			fP = _poolVel.Value.Get(entity).Velocity.Y;
		}
		for (int i = 0; i < _tempIndex; i++)
		{
			FVector2 fVector3 = _tempCenters[i];
			FVector2 fVector4 = _tempHalfSizes[i];
			FVector2 fVector5 = _tempContactPoints[i];
			FP x2 = fVector3.Y + fVector4.Y;
			bool num = fVector5.Y >= fVector3.Y - y;
			bool flag3 = fVector2.Y >= x2 - y;
			if (!num && !flag3)
			{
				continue;
			}
			bool flag4 = false;
			if ((!flag2) ? (fVector.Y < fVector2.Y) : (fP < FP.Zero))
			{
				FP fP2 = x2 + halfSize.Y + y;
				if (fP2 > x)
				{
					x = fP2;
					flag = true;
				}
			}
		}
		if (!flag)
		{
			return false;
		}
		FP fP3 = x - fVector.Y;
		if (fP3 <= y)
		{
			return false;
		}
		reference.Position += new FVector2(FP.Zero, fP3);
		if (flag2)
		{
			ref ComponentVelocity reference3 = ref _poolVel.Value.Get(entity);
			reference3.Velocity = new FVector2(reference3.Velocity.X, FP.Zero);
		}
		return fP != FP.Zero;
	}
}
