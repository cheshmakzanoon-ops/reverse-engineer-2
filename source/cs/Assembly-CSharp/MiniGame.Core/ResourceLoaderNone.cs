using System;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public class ResourceLoaderNone : IResourceLoader, IDisposable
{
	public float Progress { get; } = 1f;

	public bool IsDone { get; } = true;

	public bool IsLoading { get; }

	public bool IsError { get; }

	public IGameSerializer GetSerializer()
	{
		return null;
	}

	public IResourceHolder LoadAsset<T>(string name)
	{
		throw new NotImplementedException();
	}

	public IResourceHolder LoadAssetAsync<T>(string name, Action<IResourceHolder> callback = null)
	{
		throw new NotImplementedException();
	}

	public IResourceHolder LoadAsset<T>(EcsPackedEntityWithWorld owner, string name)
	{
		return LoadAsset<T>(name);
	}

	public IResourceHolder LoadAssetAsync<T>(EcsPackedEntityWithWorld owner, string name, Action<IResourceHolder> callback = null)
	{
		return LoadAssetAsync<T>(name, callback);
	}

	public void UnloadAsset(IResourceHolder resourceHolder)
	{
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

	public void Dispose()
	{
	}
}
