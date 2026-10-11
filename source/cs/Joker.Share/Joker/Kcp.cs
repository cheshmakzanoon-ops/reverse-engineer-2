using System;
using System.Threading;

namespace Joker;

public sealed class Kcp : IDisposable
{
	private unsafe IKCPCB* _kcp;

	private KcpCallback _output;

	private byte[] _buffer;

	private int _disposed;

	public const int OneM = 1048576;

	public const int InnerMaxWaitSize = 1048576;

	public const int OuterMaxWaitSize = 1048576;

	public unsafe bool IsSet => _kcp != null;

	public unsafe uint ConversationId => _kcp->conv;

	public unsafe uint MaximumTransmissionUnit => _kcp->mtu;

	public unsafe uint MaximumSegmentSize => _kcp->mss;

	public unsafe int State => _kcp->state;

	public unsafe uint SendUna => _kcp->snd_una;

	public unsafe uint SendNext => _kcp->snd_nxt;

	public unsafe uint ReceiveNext => _kcp->rcv_nxt;

	public unsafe uint SlowStartThreshold => _kcp->ssthresh;

	public unsafe int RxRttval => _kcp->rx_rttval;

	public unsafe int RxSrtt => _kcp->rx_srtt;

	public unsafe int RxRto => _kcp->rx_rto;

	public unsafe int RxMinrto => _kcp->rx_minrto;

	public unsafe uint SendWindowSize => _kcp->snd_wnd;

	public unsafe uint ReceiveWindowSize => _kcp->rcv_wnd;

	public unsafe uint RemoteWindowSize => _kcp->rmt_wnd;

	public unsafe uint CongestionWindowSize => _kcp->cwnd;

	public unsafe uint Probe => _kcp->probe;

	public unsafe uint Current => _kcp->current;

	public unsafe uint Interval => _kcp->interval;

	public unsafe uint TimestampFlush => _kcp->ts_flush;

	public unsafe uint Transmissions => _kcp->xmit;

	public unsafe uint ReceiveBufferCount => _kcp->nrcv_buf;

	public unsafe uint ReceiveQueueCount => _kcp->nrcv_que;

	public unsafe uint WaitReceiveCount => _kcp->nrcv_buf + _kcp->nrcv_que;

	public unsafe uint SendBufferCount => _kcp->nsnd_buf;

	public unsafe uint SendQueueCount => _kcp->nsnd_que;

	public unsafe uint WaitSendCount => _kcp->nsnd_buf + _kcp->nsnd_que;

	public unsafe uint NoDelay => _kcp->nodelay;

	public unsafe uint Updated => _kcp->updated;

	public unsafe uint TimestampProbe => _kcp->ts_probe;

	public unsafe uint ProbeWait => _kcp->probe_wait;

	public unsafe uint Increment => _kcp->incr;

	public unsafe uint* AckList => _kcp->acklist;

	public unsafe uint AckCount => _kcp->ackcount;

	public unsafe uint AckBlock => _kcp->ackblock;

	public byte[] Buffer => _buffer;

	public unsafe int FastResend => _kcp->fastresend;

	public unsafe int FastResendLimit => _kcp->fastlimit;

	public unsafe int NoCongestionWindow => _kcp->nocwnd;

	public unsafe int StreamMode => _kcp->stream;

	public unsafe int DeadLink => (int)_kcp->dead_link;

	public KcpCallback Output => _output;

	public Kcp(KcpCallback output)
		: this(0u, output)
	{
	}

	public unsafe Kcp(uint conv, KcpCallback output)
	{
		_kcp = IKCP.ikcp_create(conv, ref _buffer);
		_output = output;
	}

	public unsafe void Dispose()
	{
		if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
		{
			IKCP.ikcp_release(_kcp);
			_kcp = null;
			_output = null;
			_buffer = null;
			GC.SuppressFinalize(this);
		}
	}

	public void SetOutput(KcpCallback output)
	{
		_output = output;
	}

	~Kcp()
	{
		Dispose();
	}

	public unsafe int Send(byte[] buffer)
	{
		fixed (byte* buffer2 = &buffer[0])
		{
			return IKCP.ikcp_send(_kcp, buffer2, buffer.Length);
		}
	}

	public unsafe int Send(byte[] buffer, int length)
	{
		fixed (byte* buffer2 = &buffer[0])
		{
			return IKCP.ikcp_send(_kcp, buffer2, length);
		}
	}

	public unsafe int Send(byte[] buffer, int offset, int length)
	{
		fixed (byte* buffer2 = &buffer[offset])
		{
			return IKCP.ikcp_send(_kcp, buffer2, length);
		}
	}

	public unsafe int SendWithUserHeader(byte[] buffer, int offset, int length, byte[] header, int headerLength)
	{
		_ = header.Length;
		fixed (byte* buffer2 = &buffer[offset])
		{
			fixed (byte* header2 = header)
			{
				return IKCP.ikcp_send_with_user_header(_kcp, buffer2, length, header2, headerLength);
			}
		}
	}

	public unsafe int Send(ReadOnlySpan<byte> buffer)
	{
		fixed (byte* buffer2 = &buffer[0])
		{
			return IKCP.ikcp_send(_kcp, buffer2, buffer.Length);
		}
	}

	public unsafe int Send(ReadOnlyMemory<byte> buffer)
	{
		fixed (byte* buffer2 = &buffer.Span[0])
		{
			return IKCP.ikcp_send(_kcp, buffer2, buffer.Length);
		}
	}

	public unsafe int Send(ArraySegment<byte> buffer)
	{
		fixed (byte* buffer2 = &buffer.Array[buffer.Offset])
		{
			return IKCP.ikcp_send(_kcp, buffer2, buffer.Count);
		}
	}

	public unsafe int Send(byte* buffer, int length)
	{
		return IKCP.ikcp_send(_kcp, buffer, length);
	}

	public unsafe int Send(byte* buffer, int offset, int length)
	{
		return IKCP.ikcp_send(_kcp, buffer + offset, length);
	}

	public unsafe int Input(byte[] buffer)
	{
		fixed (byte* data = &buffer[0])
		{
			return IKCP.ikcp_input(_kcp, data, buffer.Length);
		}
	}

	public unsafe int Input(byte[] buffer, int length)
	{
		fixed (byte* data = &buffer[0])
		{
			return IKCP.ikcp_input(_kcp, data, length);
		}
	}

	public unsafe int Input(byte[] buffer, int offset, int length)
	{
		fixed (byte* data = &buffer[offset])
		{
			return IKCP.ikcp_input(_kcp, data, length);
		}
	}

	public unsafe int Input(ReadOnlySpan<byte> buffer)
	{
		fixed (byte* data = &buffer[0])
		{
			return IKCP.ikcp_input(_kcp, data, buffer.Length);
		}
	}

	public unsafe int Input(ReadOnlyMemory<byte> buffer)
	{
		fixed (byte* data = &buffer.Span[0])
		{
			return IKCP.ikcp_input(_kcp, data, buffer.Length);
		}
	}

	public unsafe int Input(ArraySegment<byte> buffer)
	{
		fixed (byte* data = &buffer.Array[buffer.Offset])
		{
			return IKCP.ikcp_input(_kcp, data, buffer.Count);
		}
	}

	public unsafe int Input(byte* buffer, int length)
	{
		return IKCP.ikcp_input(_kcp, buffer, length);
	}

	public unsafe int Input(byte* buffer, int offset, int length)
	{
		return IKCP.ikcp_input(_kcp, buffer + offset, length);
	}

	public unsafe int PeekSize()
	{
		return IKCP.ikcp_peeksize(_kcp);
	}

	public unsafe int Receive(byte[] buffer)
	{
		fixed (byte* buffer2 = &buffer[0])
		{
			return IKCP.ikcp_recv(_kcp, buffer2, buffer.Length);
		}
	}

	public unsafe int Receive(byte[] buffer, int length)
	{
		fixed (byte* buffer2 = &buffer[0])
		{
			return IKCP.ikcp_recv(_kcp, buffer2, length);
		}
	}

	public unsafe int Receive(byte[] buffer, int offset, int length)
	{
		fixed (byte* buffer2 = &buffer[offset])
		{
			return IKCP.ikcp_recv(_kcp, buffer2, length);
		}
	}

	public unsafe int Receive(Span<byte> buffer)
	{
		fixed (byte* buffer2 = &buffer[0])
		{
			return IKCP.ikcp_recv(_kcp, buffer2, buffer.Length);
		}
	}

	public unsafe int Receive(Memory<byte> buffer)
	{
		fixed (byte* buffer2 = &buffer.Span[0])
		{
			return IKCP.ikcp_recv(_kcp, buffer2, buffer.Length);
		}
	}

	public unsafe int Receive(ArraySegment<byte> buffer)
	{
		fixed (byte* buffer2 = &buffer.Array[buffer.Offset])
		{
			return IKCP.ikcp_recv(_kcp, buffer2, buffer.Count);
		}
	}

	public unsafe int Receive(byte* buffer, int length)
	{
		return IKCP.ikcp_recv(_kcp, buffer, length);
	}

	public unsafe int Receive(byte* buffer, int offset, int length)
	{
		return IKCP.ikcp_recv(_kcp, buffer + offset, length);
	}

	public unsafe void Update(uint current)
	{
		IKCP.ikcp_update(_kcp, current, _output, _buffer);
	}

	public unsafe uint Check(uint current)
	{
		return IKCP.ikcp_check(_kcp, current);
	}

	public unsafe void Flush()
	{
		IKCP.ikcp_flush(_kcp, _output, _buffer);
	}

	public unsafe int SetMtu(int mtu)
	{
		return IKCP.ikcp_setmtu(_kcp, mtu, ref _buffer);
	}

	public unsafe void SetInterval(int interval)
	{
		IKCP.ikcp_interval(_kcp, interval);
	}

	public unsafe void SetNoDelay(int nodelay, int interval, int resend, int nc)
	{
		IKCP.ikcp_nodelay(_kcp, nodelay, interval, resend, nc);
	}

	public unsafe void SetWindowSize(int sndwnd, int rcvwnd)
	{
		IKCP.ikcp_wndsize(_kcp, sndwnd, rcvwnd);
	}

	public unsafe void SetFastResendLimit(int fastlimit)
	{
		IKCP.ikcp_fastresendlimit(_kcp, fastlimit);
	}

	public unsafe void SetStreamMode(int stream)
	{
		IKCP.ikcp_streammode(_kcp, stream);
	}

	public unsafe void SetMinrto(int minrto)
	{
		IKCP.ikcp_minrto(_kcp, minrto);
	}

	public unsafe void SetDeadLink(int deadlink)
	{
		_kcp->dead_link = Math.Min((uint)deadlink, 40u);
	}
}
