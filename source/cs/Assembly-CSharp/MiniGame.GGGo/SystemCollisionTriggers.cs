using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class SystemCollisionTriggers : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnv> _env;

	protected readonly EcsFilterInject<Inc<ComponentPosition, ComponentCollider, ComponentRigidBody>, Exc<ComponentStatic>> _filterRig;

	protected readonly EcsFilterInject<Inc<ComponentPosition, ComponentColliderTrigger>, Exc<ComponentColliderDisable>> _filterCollider;

	protected readonly EcsPoolInject<ComponentPosition> _poolPos;

	protected readonly EcsPoolInject<ComponentCollider> _poolCollider;

	protected readonly EcsPoolInject<ComponentColliderDisable> _poolColliderDisable;

	protected readonly EcsPoolInject<ComponentColliderTrigger> _poolColliderTrigger;

	protected readonly EcsPoolInject<ComponentRigidBody> _poolRigid;

	private const int MaxPrevTriggers = 32;

	private const int MaxCurrentPerBody = 64;

	private readonly int[] _curIds = new int[64];

	private readonly FVector2[] _curCenters = new FVector2[64];

	public void Run(IEcsSystems systems)
	{
		EcsWorld value = _world.Value;
		foreach (int item in _filterRig.Value)
		{
			ref ComponentPosition reference = ref _poolPos.Value.Get(item);
			ref ComponentCollider reference2 = ref _poolCollider.Value.Get(item);
			ref ComponentRigidBody reference3 = ref _poolRigid.Value.Get(item);
			if (reference3.PrevTriggerIds == null)
			{
				reference3.PrevTriggerIds = new int[32];
				reference3.PrevTriggerCount = 0;
			}
			FVector2 fVector = reference.Position + reference2.Offset;
			FVector2 halfSize = reference2.HalfSize;
			int num = Collision(value, item, fVector, halfSize);
			int prevTriggerCount = reference3.PrevTriggerCount;
			for (int i = 0; i < num; i++)
			{
				int num2 = _curIds[i];
				if (!PrevContains(reference3.PrevTriggerIds, prevTriggerCount, num2))
				{
					FVector2 senderCenter = _curCenters[i];
					SendEnterEvent(value, num2, item, fVector, senderCenter, reference2.Layer, GetLayerOfEntity(num2));
				}
			}
			for (int j = 0; j < prevTriggerCount; j++)
			{
				int num3 = reference3.PrevTriggerIds[j];
				if (!CurrentContains(num, num3))
				{
					SendExitEvent(value, num3, item);
				}
			}
			if (num > 32)
			{
				value.LogError("SystemCollisionTriggers: curCount exceeds MaxPrevTriggers while updating prev for entity " + item);
				num = 32;
			}
			reference3.PrevTriggerCount = num;
			for (int k = 0; k < num; k++)
			{
				reference3.PrevTriggerIds[k] = _curIds[k];
			}
		}
	}

	private int Collision(EcsWorld world, int entity, FVector2 centerA, FVector2 halfA)
	{
		if (_poolColliderDisable.Value.Has(entity))
		{
			return 0;
		}
		int num = 0;
		foreach (int item in _filterCollider.Value)
		{
			if (item == entity)
			{
				continue;
			}
			ref ComponentColliderTrigger reference = ref _poolColliderTrigger.Value.Get(item);
			FVector2 fVector = _poolPos.Value.Get(item).Position + reference.Offset;
			FVector2 halfSize = reference.HalfSize;
			if (ComponentCollisionExtensions.RectOverlaps(centerA, halfA, fVector, halfSize))
			{
				if (num >= 64)
				{
					world.LogError("SystemCollisionTriggers: current triggers capacity exceeded for entity " + entity);
					break;
				}
				_curIds[num] = item;
				_curCenters[num] = fVector;
				num++;
			}
		}
		return num;
	}

	private bool PrevContains(int[] prevIds, int prevCount, int src)
	{
		for (int i = 0; i < prevCount; i++)
		{
			if (prevIds[i] == src)
			{
				return true;
			}
		}
		return false;
	}

	private bool CurrentContains(int curCount, int src)
	{
		for (int i = 0; i < curCount; i++)
		{
			if (_curIds[i] == src)
			{
				return true;
			}
		}
		return false;
	}

	private ColliderLayer GetLayerOfEntity(int entity)
	{
		if (_poolCollider.Value.Has(entity))
		{
			return _poolCollider.Value.Get(entity).Layer;
		}
		return ColliderLayer.None;
	}

	private void SendEnterEvent(EcsWorld world, int senderEntity, int targetEntity, FVector2 targetCenter, FVector2 senderCenter, ColliderLayer targetLayer, ColliderLayer senderLayer)
	{
		CollisionDirection direction = CalcDirection(targetCenter, senderCenter);
		EcsPackedEntity sender = world.PackEntity(senderEntity);
		EcsPackedEntity target = world.PackEntity(targetEntity);
		FuncEvent.Broadcast(world, new EventTriggerEnter(sender, target, direction, senderLayer, targetLayer));
	}

	private void SendExitEvent(EcsWorld world, int senderEntity, int targetEntity)
	{
		EcsPackedEntity target = world.PackEntity(targetEntity);
		if (!_world.Value.IsEntityAliveInternal(senderEntity))
		{
			FuncEvent.Broadcast(world, new EventTriggerExit(EcsPackedEntity.Invalid, target));
			return;
		}
		EcsPackedEntity sender = world.PackEntity(senderEntity);
		FuncEvent.Broadcast(world, new EventTriggerExit(sender, target));
	}

	private CollisionDirection CalcDirection(FVector2 centerA, FVector2 centerB)
	{
		FP fP = centerA.X - centerB.X;
		FP fP2 = centerA.Y - centerB.Y;
		if (FP.Abs(fP) > FP.Abs(fP2))
		{
			if (!(fP > FP.Zero))
			{
				return CollisionDirection.Left;
			}
			return CollisionDirection.Right;
		}
		if (!(fP2 > FP.Zero))
		{
			return CollisionDirection.Bottom;
		}
		return CollisionDirection.Top;
	}
}
