namespace Leopotam.EcsLite;

public class EcsFilterSnapshotRuntime
{
	private const int kCapacityReserved = 16;

	public int[] DenseEntities;

	public int EntitiesCount;

	public int[] SparseEntities;

	public void EnsureCapacity(int worldEntityCount, int filterEntityCount)
	{
		if (SparseEntities == null || SparseEntities.Length < worldEntityCount)
		{
			SparseEntities = new int[worldEntityCount + 16];
		}
		if (DenseEntities == null || DenseEntities.Length < filterEntityCount)
		{
			DenseEntities = new int[filterEntityCount + 16];
		}
		EntitiesCount = filterEntityCount;
	}

	public void Clear()
	{
		EntitiesCount = 0;
	}
}
