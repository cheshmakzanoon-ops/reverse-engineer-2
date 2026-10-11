using System;

namespace Leopotam.EcsLite;

public class EcsPoolSnapshotRuntimeRef<T> : IEcsPoolSnapshotRuntime where T : struct
{
	private int SparseItemCount;

	private object[] DenseItems;

	private int[] SparseItems;

	private int DenseItemsCount;

	private int[] RecycleItems;

	private int RecycleItemsCount;

	public void TakeSnapshot(IEcsPool ipool)
	{
		EcsPool<T> ecsPool = ipool as EcsPool<T>;
		EcsWorld world = ecsPool._world;
		int usedEntitiesCount = world.GetUsedEntitiesCount();
		int denseItemsCount = ecsPool._denseItemsCount;
		int recycledItemsCount = ecsPool._recycledItemsCount;
		EnsureCapacity(usedEntitiesCount, denseItemsCount, recycledItemsCount);
		Array.Copy(ecsPool._sparseItems, 0, SparseItems, 0, SparseItemCount);
		Array.Copy(ecsPool._recycledItems, 0, RecycleItems, 0, RecycleItemsCount);
		for (int i = 0; i < SparseItemCount; i++)
		{
			int num = SparseItems[i];
			if (num > 0)
			{
				ref T c = ref ecsPool._denseItems[num];
				DenseItems[num] = ecsPool._autoTakeSnapshotHandler(ref c, world, i, world.GetShared());
			}
		}
	}

	public void RestoreSnapshot(IEcsPool ipool)
	{
		EcsPool<T> ecsPool = ipool as EcsPool<T>;
		_ = ecsPool._world;
		ecsPool._denseItemsCount = DenseItemsCount;
		ecsPool._recycledItemsCount = RecycleItemsCount;
		Array.Copy(SparseItems, 0, ecsPool._sparseItems, 0, SparseItemCount);
		Array.Copy(RecycleItems, 0, ecsPool._recycledItems, 0, RecycleItemsCount);
	}

	public void RestoreEntityDelegate(IEcsPool ipool, int entity)
	{
		EcsPool<T> obj = (EcsPool<T>)ipool;
		EcsWorld world = obj._world;
		int num = SparseItems[entity];
		ref T c = ref obj._denseItems[num];
		obj._autoRestoreSnapshotHandler(ref c, world, entity, DenseItems[num], world.GetShared());
	}

	public void EnsureCapacity(int entityCount, int denseCount, int recycleCount)
	{
		SparseItemCount = entityCount;
		DenseItemsCount = denseCount;
		RecycleItemsCount = recycleCount;
		if (SparseItems == null || SparseItems.Length < entityCount)
		{
			SparseItems = new int[entityCount * 2];
		}
		if (DenseItems == null || DenseItems.Length < denseCount)
		{
			DenseItems = new object[denseCount * 2];
		}
		if (RecycleItems == null || RecycleItems.Length < recycleCount)
		{
			RecycleItems = new int[recycleCount * 2];
		}
	}

	public void Clear()
	{
		if (DenseItems != null)
		{
			Array.Clear(DenseItems, 0, DenseItems.Length);
		}
		if (SparseItems != null)
		{
			Array.Clear(SparseItems, 0, SparseItems.Length);
		}
		if (RecycleItems != null)
		{
			Array.Clear(RecycleItems, 0, RecycleItems.Length);
		}
		DenseItemsCount = 0;
		RecycleItemsCount = 0;
	}
}
