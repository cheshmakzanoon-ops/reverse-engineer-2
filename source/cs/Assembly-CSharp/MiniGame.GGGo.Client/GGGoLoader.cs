using System;
using System.Collections.Generic;
using System.IO;
using Leopotam.EcsLite;
using MiniGame.Core;
using UnityEngine;
using VEngine;

namespace MiniGame.GGGo.Client;

public class GGGoLoader : IResourceLoader, IDisposable
{
	protected IGameSerializer _serializer;

	protected bool _cache;

	protected string _template;

	protected string _level;

	protected Dictionary<string, GGGoRawHolder> _txtCached = new Dictionary<string, GGGoRawHolder>();

	protected Dictionary<string, List<GGGoCacheableResource>> _prefabCache = new Dictionary<string, List<GGGoCacheableResource>>();

	protected Dictionary<EcsPackedEntity, IGGGoCacheableResourceState> _stateCache = new Dictionary<EcsPackedEntity, IGGGoCacheableResourceState>();

	public GGGoLoader(IGameSerializer serializer, string template, string level, bool cacheTemplate)
	{
		_cache = cacheTemplate;
		_template = template;
		_level = level;
		_serializer = serializer;
	}

	public IGameSerializer GetSerializer()
	{
		return _serializer;
	}

	public IResourceHolder LoadAsset<T>(string name)
	{
		if (typeof(T) == typeof(GameLevel))
		{
			string path = ((!string.IsNullOrEmpty(_level)) ? string.Format(_level, Path.GetFileNameWithoutExtension(name)) : name);
			IResourceHolder result = null;
			Asset asset = ResourceManager.LoadAssetStatic(path, typeof(TextAsset));
			if (asset != null)
			{
				TextAsset textAsset = asset.asset as TextAsset;
				if (textAsset != null)
				{
					result = new GGGoRawHolder(_serializer.FromJson<GameLevel>(textAsset.text));
				}
				asset.Release();
			}
			return result;
		}
		if (typeof(T) == typeof(EcsEntitySnapshot))
		{
			if (_cache && _txtCached.TryGetValue(name, out var value))
			{
				return value;
			}
			string path2 = ((!string.IsNullOrEmpty(_template)) ? string.Format(_template, Path.GetFileNameWithoutExtension(name)) : name);
			value = null;
			Asset asset2 = ResourceManager.LoadAssetStatic(path2, typeof(TextAsset));
			if (asset2 != null)
			{
				TextAsset textAsset2 = asset2.asset as TextAsset;
				if (textAsset2 != null)
				{
					value = new GGGoRawHolder(_serializer.FromJson<EcsEntitySnapshot>(textAsset2.text));
					if (_cache)
					{
						_txtCached.Add(name, value);
					}
				}
				asset2.Release();
			}
			return value;
		}
		if (typeof(T) == typeof(string))
		{
			Asset asset3 = ResourceManager.LoadAssetStatic(name, typeof(TextAsset));
			if (asset3 != null)
			{
				new GGGoRawHolder(asset3.asset);
				asset3.Release();
			}
		}
		return new GGGoAssetHolder(this, name, typeof(T), sync: true);
	}

	public IResourceHolder LoadAssetAsync<T>(string name, Action<IResourceHolder> callback = null)
	{
		if (typeof(T) == typeof(GameObject))
		{
			return new GGGoAssetHolder(this, name, typeof(T), sync: false);
		}
		if (typeof(T) == typeof(GameLevel))
		{
			throw new NotSupportedException();
		}
		if (typeof(T) == typeof(EcsEntitySnapshot))
		{
			throw new NotSupportedException();
		}
		_ = typeof(T) == typeof(string);
		throw new NotImplementedException();
	}

	public IResourceHolder LoadAsset<T>(EcsPackedEntityWithWorld owner, string name)
	{
		return LoadAsset<T>(name);
	}

	public IResourceHolder LoadAssetAsync<T>(EcsPackedEntityWithWorld owner, string name, Action<IResourceHolder> callback = null)
	{
		if (typeof(T) == typeof(GameObject))
		{
			return new GGGoAssetHolder(this, name, typeof(T), sync: false, owner);
		}
		if (typeof(T) == typeof(GameLevel))
		{
			throw new NotSupportedException();
		}
		if (typeof(T) == typeof(EcsEntitySnapshot))
		{
			throw new NotSupportedException();
		}
		_ = typeof(T) == typeof(string);
		throw new NotImplementedException();
	}

	public object LoadConfig(int id, Type type)
	{
		throw new NotImplementedException();
	}

	public T LoadConfig<T>(int id) where T : class
	{
		throw new NotImplementedException();
	}

	public GGGoCacheableResource AcquireFromCache(string path, EcsPackedEntityWithWorld owner)
	{
		if (!_prefabCache.TryGetValue(path, out var value) || value.Count == 0)
		{
			return null;
		}
		int num = -1;
		int num2 = -1;
		for (int num3 = value.Count - 1; num3 >= 0; num3--)
		{
			EcsPackedEntityWithWorld a = value[num3].GetOwner();
			if (a.EqualsTo(in owner))
			{
				num = num3;
				break;
			}
			if (num2 < 0 && !a.IsValid())
			{
				num2 = num3;
			}
		}
		if (num < 0)
		{
			num = ((num2 < 0) ? (value.Count - 1) : num2);
		}
		GGGoCacheableResource gGGoCacheableResource = value[num];
		value.RemoveAt(num);
		gGGoCacheableResource.SetActiveAtLateUpdate(active: true);
		return gGGoCacheableResource;
	}

	public void ReleaseToCache(GGGoCacheableResource obj, EcsPackedEntityWithWorld owner)
	{
		obj.SetActiveAtLateUpdate(active: false);
		if (!_prefabCache.TryGetValue(obj.Path, out var value))
		{
			value = new List<GGGoCacheableResource>();
			_prefabCache.Add(obj.Path, value);
		}
		value.Add(obj);
	}

	public void SaveCachedResourceState(EcsPackedEntity entity, GGGoCacheableResource cache)
	{
		IGGGoCacheableResourceState value = cache.SaveState();
		_stateCache[entity] = value;
	}

	public IGGGoCacheableResourceState LoadCachedResourceState(EcsPackedEntity entity)
	{
		if (_stateCache.TryGetValue(entity, out var value))
		{
			return value;
		}
		return null;
	}

	public void Dispose()
	{
		_stateCache.Clear();
		foreach (List<GGGoCacheableResource> value in _prefabCache.Values)
		{
			foreach (GGGoCacheableResource item in value)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(item);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(item);
				}
			}
		}
		_prefabCache.Clear();
	}
}
