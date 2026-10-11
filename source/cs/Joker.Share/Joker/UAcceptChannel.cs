using System.Net;
using System.Net.Sockets;

namespace Joker;

public sealed class UAcceptChannel : UChannel
{
	public UAcceptChannel(long id, Socket socket, IPEndPoint endpoint, UService service)
		: base(id, endpoint, service)
	{
		base.ChannelType = ChannelType.Accept;
		_socket = socket;
	}

	protected override bool OnAuthenticated(MemoryBuffer buffer)
	{
		uint num = buffer.Read<uint>();
		InitKCP(num);
		Log.Info($"UChannel accepted authenticated channel {num}");
		SendConnect();
		_state = EState.Authenticated;
		_service?.OnAcceptComplete(this);
		return true;
	}

	protected override void OnDisconnected()
	{
		_service?.OnDisconnectComplete(this);
	}

	protected override void HandlePing(long time)
	{
	}

	protected override void HandleRetryConnect()
	{
	}
}
