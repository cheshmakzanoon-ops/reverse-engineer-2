using System;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public interface IResourceLoader : IDisposable
{
	IGameSerializer GetSerializer();

	IResourceHolder LoadAsset<T>(string name);

	IResourceHolder LoadAsset<T>(EcsPackedEntityWithWorld owner, string name);

	IResourceHolder LoadAssetAsync<T>(string name, Action<IResourceHolder> callback = null);

	IResourceHolder LoadAssetAsync<T>(EcsPackedEntityWithWorld owner, string name, Action<IResourceHolder> callback = null);

	object LoadConfig(int id, Type type);

	T LoadConfig<T>(int id) where T : class;
}
