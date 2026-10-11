using System;

namespace Leopotam.EcsLite;

public class EcsPoolSnapshotRuntime<T> : IEcsPoolSnapshotRuntime where T : struct
{
	private int SparseItemCount;

	private T[] DenseItems;

	private int[] SparseItems;

	private int DenseItemsCount;

	private int[] RecycleItems;

	private int RecycleItemsCount;

	public void TakeSnapshot(IEcsPool ipool)
	{
		EcsPool<T> obj = ipool as EcsPool<T>;
		int usedEntitiesCount = obj._world.GetUsedEntitiesCount();
		int denseItemsCount = obj._denseItemsCount;
		int recycledItemsCount = obj._recycledItemsCount;
		EnsureCapacity(usedEntitiesCount, denseItemsCount, recycledItemsCount);
		Array.Copy(obj._denseItems, 0, DenseItems, 0, DenseItemsCount);
		Array.Copy(obj._sparseItems, 0, SparseItems, 0, SparseItemCount);
		Array.Copy(obj._recycledItems, 0, RecycleItems, 0, RecycleItemsCount);
	}

	public void RestoreSnapshot(IEcsPool ipool)
	{
		EcsPool<T> ecsPool = ipool as EcsPool<T>;
		_ = ecsPool._world;
		ecsPool._denseItemsCount = DenseItemsCount;
		ecsPool._recycledItemsCount = RecycleItemsCount;
		Array.Copy(DenseItems, 0, ecsPool._denseItems, 0, DenseItemsCount);
		Array.Copy(SparseItems, 0, ecsPool._sparseItems, 0, SparseItemCount);
		Array.Copy(RecycleItems, 0, ecsPool._recycledItems, 0, RecycleItemsCount);
	}

	public void RestoreEntityDelegate(IEcsPool ipool, int entity)
	{
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
			DenseItems = new T[denseCount * 2];
		}
		if (RecycleItems == null || RecycleItems.Length < recycleCount)
		{
			RecycleItems = new int[recycleCount * 2];
		}
	}
}
