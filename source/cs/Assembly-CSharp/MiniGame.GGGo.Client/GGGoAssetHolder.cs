using System;
using Leopotam.EcsLite;
using MiniGame.Core;
using UnityEngine;
using VEngine;

namespace MiniGame.GGGo.Client;

public class GGGoAssetHolder : IResourceHolder
{
	private GGGoLoader _loader;

	private string _path;

	private EcsPackedEntityWithWorld _owner;

	private Asset _asset;

	private GameObject _instance;

	public object Data => _asset.asset;

	public bool IsDone => _asset.isDone;

	public bool IsError => _asset.isError;

	public bool IsValid => _asset.asset != null;

	public float Progress => _asset.progress;

	public string ErrorMessage { get; private set; }

	public Action<IResourceHolder> OnDone { get; set; }

	public GGGoAssetHolder(GGGoLoader loader, string path, Type type, bool sync, EcsPackedEntityWithWorld owner = default(EcsPackedEntityWithWorld))
	{
		_loader = loader;
		_path = path;
		_owner = owner;
		GGGoCacheableResource gGGoCacheableResource = _loader.AcquireFromCache(path, owner);
		if (gGGoCacheableResource != null)
		{
			_asset = gGGoCacheableResource.Asset;
			_instance = gGGoCacheableResource.gameObject;
			OnAssetCompleted(_asset);
		}
		else
		{
			_asset = ResourceManager.LoadAssetStatic(path, type);
			OnAssetCompleted(_asset);
		}
	}

	public T As<T>() where T : class
	{
		if (typeof(T) == typeof(string))
		{
			TextAsset textAsset = _asset.asset as TextAsset;
			return ((textAsset != null) ? textAsset.text : string.Empty) as T;
		}
		if (typeof(T) == typeof(GameLevel))
		{
			return null;
		}
		_ = typeof(T) == typeof(EcsEntitySnapshot);
		return (T)Data;
	}

	public T GetInstance<T>() where T : class
	{
		return _instance as T;
	}

	public T AsInstance<T>() where T : class
	{
		if (Data == null)
		{
			return null;
		}
		if (typeof(T) == typeof(GameObject))
		{
			if (_instance == null)
			{
				GameObject original = (GameObject)Data;
				_instance = UnityEngine.Object.Instantiate(original);
			}
			return _instance as T;
		}
		return null;
	}

	public void Dispose()
	{
		Asset asset = _asset;
		asset.completed = (Action<Asset>)Delegate.Remove(asset.completed, new Action<Asset>(OnAssetCompleted));
		if (_instance != null)
		{
			if (_instance.TryGetComponent<GGGoCacheableResource>(out var component))
			{
				component.Asset = _asset;
				component.Path = _path;
				_loader.ReleaseToCache(component, _owner);
				return;
			}
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(_instance);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(_instance);
			}
			_instance = null;
		}
		if (!_asset.isDone)
		{
			Asset asset2 = _asset;
			asset2.completed = (Action<Asset>)Delegate.Combine(asset2.completed, new Action<Asset>(OnLoadingToRelease));
		}
		else
		{
			_asset.Release();
		}
	}

	private void OnAssetCompleted(Asset asset)
	{
		OnDone?.Invoke(this);
	}

	private void OnLoadingToRelease(Asset asset)
	{
		asset.completed = (Action<Asset>)Delegate.Remove(asset.completed, new Action<Asset>(OnLoadingToRelease));
		asset.Release();
	}
}
