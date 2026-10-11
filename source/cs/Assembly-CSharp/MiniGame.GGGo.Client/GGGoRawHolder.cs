using System;
using MiniGame.Core;

namespace MiniGame.GGGo.Client;

public class GGGoRawHolder : IResourceHolder
{
	public object Data { get; protected set; }

	public bool IsDone => Data != null;

	public bool IsError => Data == null;

	public bool IsValid => Data != null;

	public float Progress => 1f;

	public string ErrorMessage { get; private set; }

	public Action<IResourceHolder> OnDone { get; set; }

	public GGGoRawHolder(object data)
	{
		Data = data;
	}

	public T As<T>() where T : class
	{
		return Data as T;
	}

	public void Dispose()
	{
	}
}
