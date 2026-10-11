using System;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public class SnapshotWorldRuntime : IDisposable
{
	private object _envData;

	private EcsWorldSnapshotRuntime _worldSnapshot;

	public void TakeSnapshot(GameWorld world)
	{
		ISnapshotRuntime snapshotRuntime = world.Env as ISnapshotRuntime;
		_envData = snapshotRuntime.TakeSnapshotRuntime(_envData);
		if (_worldSnapshot == null)
		{
			_worldSnapshot = new EcsWorldSnapshotRuntime();
		}
		world._world.TakeSnapshotRuntime(_worldSnapshot);
	}

	public void RestoreSnapshot(GameWorld world)
	{
		(world.Env as ISnapshotRuntime).RestoreSnapshotRuntime(_envData);
		world._world.RestoreSnapshotRuntime(_worldSnapshot);
	}

	public string ToJson()
	{
		throw new NotSupportedException("SnapshotWorldRuntime does not support JSON serialization");
	}

	public void FromJson(string json)
	{
		throw new NotSupportedException("SnapshotWorldRuntime does not support JSON deserialization");
	}

	public void Dispose()
	{
		if (_envData is IDisposable disposable)
		{
			disposable.Dispose();
		}
		_envData = null;
		_worldSnapshot?.Dispose();
		_worldSnapshot = null;
	}
}
