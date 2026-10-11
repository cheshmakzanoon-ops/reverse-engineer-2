using System.Net;
using System.Net.Sockets;

namespace Joker;

public sealed class UConnectChannel : UChannel
{
	private EndPoint _receiveEP;

	public UConnectChannel(long id, IPEndPoint endpoint, UService service)
		: base(id, endpoint, service)
	{
		base.ChannelType = ChannelType.Connect;
		_socket = new Socket(endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
		_socket.Blocking = false;
		_receiveEP = new IPEndPoint(IPAddress.Any, 0);
		UService.ConfigureSocketBuffers(_socket);
		UService.ConfigureSioConnReset(_socket);
		InitKCP(UtilsNet.GenerateConv());
	}

	public override void TickIncoming()
	{
		if (_socket != null && _socket.ReceiveFromNonBlocking(_udpRecvBuffer, out var len, ref _receiveEP))
		{
			RawInput(_udpRecvBuffer, len);
		}
		base.TickIncoming();
	}

	protected override bool OnAuthenticated(MemoryBuffer buffer)
	{
		uint num = buffer.Read<uint>();
		if (num != _kcp.ConversationId)
		{
			Log.Warning($"OnAuthenticated called with {num} invalid conversation ID:{_kcp.ConversationId}.");
			return false;
		}
		_state = EState.Authenticated;
		_service.OnConnectComplete(this);
		return true;
	}

	protected override void OnDisconnected()
	{
		_service?.OnDisconnectComplete(this);
	}
}
