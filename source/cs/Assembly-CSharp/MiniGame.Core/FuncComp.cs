using Leopotam.EcsLite;

namespace MiniGame.Core;

public static class FuncComp
{
	public static void ChangeLayer<T>(EcsWorld world, int entity, bool add) where T : struct, IComponentLayer
	{
		if (add)
		{
			AddLayer<T>(world, entity);
		}
		else
		{
			RemoveLayer<T>(world, entity);
		}
	}

	public static void AddLayer<T>(EcsWorld world, int entity) where T : struct, IComponentLayer
	{
		world.GetPool<T>().GetOrAdd(entity).Layer++;
	}

	public static void RemoveLayer<T>(EcsWorld world, int entity) where T : struct, IComponentLayer
	{
		EcsPool<T> pool = world.GetPool<T>();
		if (pool.Has(entity))
		{
			ref T reference = ref pool.Get(entity);
			reference.Layer--;
			if (reference.Layer <= 0)
			{
				pool.Del(entity);
			}
		}
	}
}
