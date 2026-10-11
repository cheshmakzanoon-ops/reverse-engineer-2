using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Newtonsoft.Json;

namespace MiniGame.Core;

public class GameLevel : IDisposable
{
	[JsonProperty]
	public object _levelEnv;

	[JsonProperty]
	public EcsAliveEntitiesSnapshot _levelEntities;

	[JsonIgnore]
	private Dictionary<string, GameEntityTemplate> _entityTemplates;

	public T GetEnv<T>() where T : class
	{
		return _levelEnv as T;
	}

	public virtual void SaveLevel(GameWorld world, IGameSaveFilter filter, bool saveEnv = true)
	{
		filter?.Init(world);
		if (saveEnv)
		{
			_levelEnv = SaveLevelEnv(world);
		}
		_levelEntities = SaveLevelEntities(world, filter);
	}

	public virtual void LoadLevel(GameWorld world, bool loadEnv = true)
	{
		if (loadEnv)
		{
			LoadLevelEnv(world, _levelEnv);
		}
		LoadLevelEntities(world, _levelEntities);
	}

	public virtual void UpdateTemplates(IResourceLoader loader)
	{
		for (int i = 0; i < _levelEntities.AllEntities.Length; i++)
		{
			EcsEntitySnapshot ecsEntitySnapshot = _levelEntities.AllEntities[i];
			if (GameEntityTemplate.TryGetTemplate(ecsEntitySnapshot, out var template) && !string.IsNullOrEmpty(template.Name) && template.ComponentsVersion != 0L)
			{
				GameEntityTemplate entityTemplate = GetEntityTemplate(loader, template.Name);
				if (entityTemplate != null && entityTemplate.ComponentsVersion != template.ComponentsVersion)
				{
					entityTemplate.Update(ecsEntitySnapshot);
				}
			}
		}
	}

	public virtual GameEntityTemplate GetEntityTemplate(IResourceLoader loader, string name)
	{
		IResourceHolder resourceHolder = loader.LoadAsset<EcsEntitySnapshot>(name);
		if (resourceHolder == null)
		{
			AddEntityTemplate(name, null);
			return null;
		}
		if (!(resourceHolder.Data is EcsEntitySnapshot template))
		{
			AddEntityTemplate(name, null);
			return null;
		}
		GameEntityTemplate gameEntityTemplate = GameEntityTemplate.Create(template);
		AddEntityTemplate(name, gameEntityTemplate);
		return gameEntityTemplate;
	}

	public void AddEntityTemplate(string templateName, GameEntityTemplate template)
	{
		if (_entityTemplates == null)
		{
			_entityTemplates = new Dictionary<string, GameEntityTemplate>();
		}
		_entityTemplates[templateName] = template;
	}

	protected virtual object SaveLevelEnv(GameWorld world)
	{
		return world.Env.TakeSnapshot();
	}

	protected virtual void LoadLevelEnv(GameWorld world, object env)
	{
		world.Env.RestoreSnapshot(env);
	}

	public static bool SaveLevelEntity(IGameSaveFilter filter, EcsEntitySnapshot entity)
	{
		if (!filter.FilterEntity(entity))
		{
			return false;
		}
		(short, Type, object)[] array = entity.Components;
		int num = array.Length;
		for (int num2 = num - 1; num2 >= 0; num2--)
		{
			ref(short, Type, object) reference = ref array[num2];
			if (!filter.FilterComponent(entity.EntityID, reference.Item1, reference.Item2, ref reference.Item3))
			{
				num--;
				if (num - num2 > 0)
				{
					Array.ConstrainedCopy(array, num2 + 1, array, num2, num - num2);
				}
			}
		}
		if (num == 0)
		{
			return false;
		}
		if (num != array.Length)
		{
			Array.Resize(ref array, num);
			entity.Components = array;
		}
		return true;
	}

	protected virtual EcsAliveEntitiesSnapshot SaveLevelEntities(GameWorld world, IGameSaveFilter filter)
	{
		EcsAliveEntitiesSnapshot ecsAliveEntitiesSnapshot = world._world.TakeEntitiesSnapshot();
		List<EcsEntitySnapshot> list = new List<EcsEntitySnapshot>(ecsAliveEntitiesSnapshot.AllEntities);
		for (int num = ecsAliveEntitiesSnapshot.EntitiesCount - ecsAliveEntitiesSnapshot.RecycledEntitiesCount - 1; num >= 0; num--)
		{
			EcsEntitySnapshot entity = list[num];
			if (filter != null && !SaveLevelEntity(filter, entity))
			{
				ecsAliveEntitiesSnapshot.RecycledEntitiesCount++;
				if (ecsAliveEntitiesSnapshot.RecycledEntitiesCount > ecsAliveEntitiesSnapshot.RecycledEntitiesCapacity)
				{
					ecsAliveEntitiesSnapshot.RecycledEntitiesCapacity <<= 1;
				}
				list.RemoveAt(num);
			}
		}
		ecsAliveEntitiesSnapshot.AllEntities = list.ToArray();
		return ecsAliveEntitiesSnapshot;
	}

	protected virtual void LoadLevelEntities(GameWorld world, EcsAliveEntitiesSnapshot entities)
	{
		world._world.RestoreEntitiesSnapshot(entities);
	}

	public void Dispose()
	{
		if (_levelEnv is IDisposable disposable)
		{
			disposable.Dispose();
		}
		_levelEnv = null;
		IDisposable levelEntities;
		if ((levelEntities = _levelEntities) != null)
		{
			levelEntities.Dispose();
		}
		_levelEntities = null;
		_entityTemplates?.Clear();
	}
}
