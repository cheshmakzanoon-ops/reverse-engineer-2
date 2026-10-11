using System;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public class ResourceLoaderJson : IResourceLoader, IDisposable
{
	private readonly IGameSerializer _serializer;

	private readonly string _rootPath;

	private readonly string _localPath;

	private readonly string _fmtTemplate;

	private readonly string _fmtLevel;

	public ResourceLoaderJson(string rootPath, string localPath, IGameSerializer serializer, string suffix = ".txt")
	{
		_serializer = serializer;
		_rootPath = rootPath;
		_localPath = localPath;
		_fmtTemplate = rootPath + "Template/{0}" + suffix;
		_fmtLevel = rootPath + "Map/{0}" + suffix;
	}

	public IGameSerializer GetSerializer()
	{
		return _serializer;
	}

	public IResourceHolder LoadAsset<T>(string name)
	{
		Type typeFromHandle = typeof(T);
		if (typeFromHandle == typeof(EcsEntitySnapshot))
		{
			name = GetFinalPath(name, _fmtTemplate);
			return new TDataHolderJson<EcsEntitySnapshot>(name, _serializer);
		}
		if (typeFromHandle == typeof(GameLevel))
		{
			name = GetFinalPath(name, _fmtLevel);
			return new TDataHolderJson<GameLevel>(name, _serializer);
		}
		throw new NotSupportedException($"{typeFromHandle} not supported");
	}

	private string GetFinalPath(string name, string format)
	{
		if (!name.StartsWith(_rootPath))
		{
			name = string.Format(format, name);
		}
		return name.Replace(_rootPath, _localPath);
	}

	public IResourceHolder LoadAssetAsync<T>(string name, Action<IResourceHolder> callback = null)
	{
		throw new NotSupportedException();
	}

	public IResourceHolder LoadAsset<T>(EcsPackedEntityWithWorld owner, string name)
	{
		return LoadAsset<T>(name);
	}

	public IResourceHolder LoadAssetAsync<T>(EcsPackedEntityWithWorld owner, string name, Action<IResourceHolder> callback = null)
	{
		return LoadAssetAsync<T>(name, callback);
	}

	public object LoadConfig(int id, Type type)
	{
		throw new NotSupportedException();
	}

	public T LoadConfig<T>(int id) where T : class
	{
		return LoadConfig(id, typeof(T)) as T;
	}

	public void Dispose()
	{
	}
}
