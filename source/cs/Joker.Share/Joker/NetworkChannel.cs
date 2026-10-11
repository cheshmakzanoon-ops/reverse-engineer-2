using System;
using System.Net;

namespace Joker;

public abstract class NetworkChannel : IDisposable
{
	private IPEndPoint remoteAddress;

	public long Id { get; protected set; }

	public ChannelType ChannelType { get; protected set; }

	public int Error { get; set; }

	public IPEndPoint RemoteAddress
	{
		get
		{
			return remoteAddress;
		}
		set
		{
			remoteAddress = value;
		}
	}

	public bool IsDisposed => Id == 0;

	public abstract void Dispose();
}
