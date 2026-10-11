using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Joker;

internal static class IKCP
{
	private unsafe static void memcpy(void* dest, void* src, int n)
	{
		Unsafe.CopyBlock(dest, src, (uint)n);
	}

	private unsafe static void memcpy(void* dest, void* src, uint n)
	{
		Unsafe.CopyBlock(dest, src, n);
	}

	private unsafe static void* malloc(int size)
	{
		return (void*)Marshal.AllocHGlobal(size);
	}

	private unsafe static void* malloc(uint size)
	{
		return (void*)Marshal.AllocHGlobal((int)size);
	}

	private unsafe static void free(void* ptr)
	{
		Marshal.FreeHGlobal((IntPtr)ptr);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static byte* ikcp_encode8u(byte* p, byte c)
	{
		*(p++) = c;
		return p;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static byte* ikcp_decode8u(byte* p, byte* c)
	{
		*c = *(p++);
		return p;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static byte* ikcp_encode16u(byte* p, ushort w)
	{
		memcpy(p, &w, 2);
		p += 2;
		return p;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static byte* ikcp_decode16u(byte* p, ushort* w)
	{
		memcpy(w, p, 2);
		p += 2;
		return p;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static byte* ikcp_encode32u(byte* p, uint l)
	{
		memcpy(p, &l, 4);
		p += 4;
		return p;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static byte* ikcp_decode32u(byte* p, uint* l)
	{
		memcpy(l, p, 4);
		p += 4;
		return p;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static uint _imin_(uint a, uint b)
	{
		if (a > b)
		{
			return b;
		}
		return a;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static uint _imax_(uint a, uint b)
	{
		if (a < b)
		{
			return b;
		}
		return a;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static uint _ibound_(uint lower, uint middle, uint upper)
	{
		return _imin_(_imax_(lower, middle), upper);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int _iclamp_(int x, uint min, uint max)
	{
		if (x >= min)
		{
			if (x <= max)
			{
				return x;
			}
			return (int)max;
		}
		return (int)min;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static uint _iceilpow2_(uint x)
	{
		x--;
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		x++;
		return x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int _itimediff(uint later, uint earlier)
	{
		return (int)(later - earlier);
	}

	private unsafe static void* ikcp_malloc(int size)
	{
		return malloc(size);
	}

	private unsafe static void* ikcp_malloc(uint size)
	{
		return malloc(size);
	}

	private unsafe static void ikcp_free(void* ptr)
	{
		free(ptr);
	}

	private unsafe static IKCPSEG* ikcp_segment_new(IKCPCB* kcp, int size)
	{
		return (IKCPSEG*)ikcp_malloc(sizeof(IKCPSEG) + size);
	}

	private unsafe static void ikcp_segment_delete(IKCPCB* kcp, IKCPSEG* seg)
	{
		ikcp_free(seg);
	}

	private static void ikcp_output(KcpCallback output, byte[] data, int size)
	{
		if (size != 0)
		{
			output(data, size);
		}
	}

	public unsafe static IKCPCB* ikcp_create(uint conv, ref byte[] buffer)
	{
		IKCPCB* ptr = (IKCPCB*)ikcp_malloc(sizeof(IKCPCB));
		ptr->conv = conv;
		ptr->snd_una = 0u;
		ptr->snd_nxt = 0u;
		ptr->rcv_nxt = 0u;
		ptr->ts_probe = 0u;
		ptr->probe_wait = 0u;
		ptr->snd_wnd = 32u;
		ptr->rcv_wnd = 128u;
		ptr->rmt_wnd = 128u;
		ptr->cwnd = 0u;
		ptr->incr = 0u;
		ptr->probe = 0u;
		ptr->mtu = 1400u;
		ptr->mss = ptr->mtu - 24;
		ptr->stream = 0;
		buffer = new byte[(ptr->mtu + 24) * 3];
		IQUEUEHEAD.iqueue_init(&ptr->snd_queue);
		IQUEUEHEAD.iqueue_init(&ptr->rcv_queue);
		IQUEUEHEAD.iqueue_init(&ptr->snd_buf);
		IQUEUEHEAD.iqueue_init(&ptr->rcv_buf);
		ptr->nrcv_buf = 0u;
		ptr->nsnd_buf = 0u;
		ptr->nrcv_que = 0u;
		ptr->nsnd_que = 0u;
		ptr->state = 0;
		ptr->acklist = null;
		ptr->ackblock = 0u;
		ptr->ackcount = 0u;
		ptr->rx_srtt = 0;
		ptr->rx_rttval = 0;
		ptr->rx_rto = 200;
		ptr->rx_minrto = 100;
		ptr->current = 0u;
		ptr->interval = 100u;
		ptr->ts_flush = 100u;
		ptr->nodelay = 0u;
		ptr->updated = 0u;
		ptr->ssthresh = 2u;
		ptr->fastresend = 0;
		ptr->fastlimit = 5;
		ptr->nocwnd = 0;
		ptr->xmit = 0u;
		ptr->dead_link = 20u;
		for (int i = 0; i < 100; i++)
		{
			ptr->xmit_history[i] = 0;
		}
		ptr->xmit_index = 0;
		ptr->xmit_count = 0;
		ptr->lost_count = 0;
		ptr->sent_count = 0;
		ptr->last_update_time = 0;
		ptr->estimated_loss_rate = 0f;
		return ptr;
	}

	public unsafe static void ikcp_release(IKCPCB* kcp)
	{
		if (kcp != null)
		{
			while (!IQUEUEHEAD.iqueue_is_empty(&kcp->snd_buf))
			{
				IKCPSEG* ptr = IQUEUEHEAD.iqueue_entry(kcp->snd_buf.next);
				IQUEUEHEAD.iqueue_del(&ptr->node);
				ikcp_segment_delete(kcp, ptr);
			}
			while (!IQUEUEHEAD.iqueue_is_empty(&kcp->rcv_buf))
			{
				IKCPSEG* ptr = IQUEUEHEAD.iqueue_entry(kcp->rcv_buf.next);
				IQUEUEHEAD.iqueue_del(&ptr->node);
				ikcp_segment_delete(kcp, ptr);
			}
			while (!IQUEUEHEAD.iqueue_is_empty(&kcp->snd_queue))
			{
				IKCPSEG* ptr = IQUEUEHEAD.iqueue_entry(kcp->snd_queue.next);
				IQUEUEHEAD.iqueue_del(&ptr->node);
				ikcp_segment_delete(kcp, ptr);
			}
			while (!IQUEUEHEAD.iqueue_is_empty(&kcp->rcv_queue))
			{
				IKCPSEG* ptr = IQUEUEHEAD.iqueue_entry(kcp->rcv_queue.next);
				IQUEUEHEAD.iqueue_del(&ptr->node);
				ikcp_segment_delete(kcp, ptr);
			}
			if (kcp->acklist != null)
			{
				ikcp_free(kcp->acklist);
			}
			kcp->nrcv_buf = 0u;
			kcp->nsnd_buf = 0u;
			kcp->nrcv_que = 0u;
			kcp->nsnd_que = 0u;
			kcp->ackcount = 0u;
			kcp->acklist = null;
			ikcp_free(kcp);
		}
	}

	public unsafe static int ikcp_recv(IKCPCB* kcp, byte* buffer, int len)
	{
		if (IQUEUEHEAD.iqueue_is_empty(&kcp->rcv_queue))
		{
			return -1;
		}
		int num = ikcp_peeksize_internal(kcp);
		if (num < 0)
		{
			return -2;
		}
		int num2;
		if (len < 0)
		{
			len = -len;
			if (num > len)
			{
				return -3;
			}
			num2 = ((kcp->nrcv_que >= kcp->rcv_wnd) ? 1 : 0);
			IQUEUEHEAD* next = kcp->rcv_queue.next;
			len = 0;
			while (next != &kcp->rcv_queue)
			{
				IKCPSEG* ptr = IQUEUEHEAD.iqueue_entry(next);
				next = next->next;
				if (buffer != null)
				{
					memcpy(buffer, ptr->data, ptr->len);
					buffer += ptr->len;
				}
				len += (int)ptr->len;
				if (ptr->frg == 0)
				{
					break;
				}
			}
		}
		else
		{
			if (num > len)
			{
				return -3;
			}
			num2 = ((kcp->nrcv_que >= kcp->rcv_wnd) ? 1 : 0);
			IQUEUEHEAD* next = kcp->rcv_queue.next;
			len = 0;
			while (next != &kcp->rcv_queue)
			{
				IKCPSEG* ptr = IQUEUEHEAD.iqueue_entry(next);
				next = next->next;
				if (buffer != null)
				{
					memcpy(buffer, ptr->data, ptr->len);
					buffer += ptr->len;
				}
				len += (int)ptr->len;
				uint frg = ptr->frg;
				IQUEUEHEAD.iqueue_del(&ptr->node);
				ikcp_segment_delete(kcp, ptr);
				kcp->nrcv_que--;
				if (frg == 0)
				{
					break;
				}
			}
		}
		while (!IQUEUEHEAD.iqueue_is_empty(&kcp->rcv_buf))
		{
			IKCPSEG* ptr = IQUEUEHEAD.iqueue_entry(kcp->rcv_buf.next);
			if (ptr->sn != kcp->rcv_nxt || kcp->nrcv_que >= kcp->rcv_wnd)
			{
				break;
			}
			IQUEUEHEAD.iqueue_del(&ptr->node);
			kcp->nrcv_buf--;
			IQUEUEHEAD.iqueue_add_tail(&ptr->node, &kcp->rcv_queue);
			kcp->nrcv_que++;
			kcp->rcv_nxt++;
		}
		if (kcp->nrcv_que < kcp->rcv_wnd && num2 != 0)
		{
			kcp->probe |= 2u;
		}
		return len;
	}

	public unsafe static int ikcp_peeksize(IKCPCB* kcp)
	{
		if (!IQUEUEHEAD.iqueue_is_empty(&kcp->rcv_queue))
		{
			return ikcp_peeksize_internal(kcp);
		}
		return -1;
	}

	private unsafe static int ikcp_peeksize_internal(IKCPCB* kcp)
	{
		IKCPSEG* ptr = IQUEUEHEAD.iqueue_entry(kcp->rcv_queue.next);
		if (ptr->frg == 0)
		{
			return (int)ptr->len;
		}
		if (kcp->nrcv_que < ptr->frg + 1)
		{
			return -1;
		}
		int num = 0;
		for (IQUEUEHEAD* next = kcp->rcv_queue.next; next != &kcp->rcv_queue; next = next->next)
		{
			ptr = IQUEUEHEAD.iqueue_entry(next);
			num += (int)ptr->len;
			if (ptr->frg == 0)
			{
				break;
			}
		}
		return num;
	}

	public unsafe static int ikcp_send(IKCPCB* kcp, byte* buffer, int len)
	{
		if (len < 0)
		{
			return -1;
		}
		int num = 0;
		if (kcp->stream != 0)
		{
			if (!IQUEUEHEAD.iqueue_is_empty(&kcp->snd_queue))
			{
				IKCPSEG* ptr = IQUEUEHEAD.iqueue_entry(kcp->snd_queue.prev);
				if (ptr->len < kcp->mss)
				{
					int num2 = (int)(kcp->mss - ptr->len);
					int num3 = ((len < num2) ? len : num2);
					IKCPSEG* ptr2 = ikcp_segment_new(kcp, (int)ptr->len + num3);
					IQUEUEHEAD.iqueue_add_tail(&ptr2->node, &kcp->snd_queue);
					memcpy(ptr2->data, ptr->data, ptr->len);
					if (buffer != null)
					{
						memcpy(ptr2->data + ptr->len, buffer, num3);
						buffer += num3;
					}
					ptr2->len = ptr->len + (uint)num3;
					ptr2->frg = 0u;
					len -= num3;
					IQUEUEHEAD.iqueue_del_init(&ptr->node);
					ikcp_segment_delete(kcp, ptr);
					num = num3;
				}
			}
			if (len <= 0)
			{
				return num;
			}
			int num4;
			if (len <= (int)kcp->mss)
			{
				num4 = 1;
			}
			else
			{
				num4 = (int)((len + kcp->mss - 1) / kcp->mss);
				if (num4 >= (int)kcp->rcv_wnd)
				{
					if (num <= 0)
					{
						return -2;
					}
					return num;
				}
				if (num4 == 0)
				{
					num4 = 1;
				}
			}
			for (int i = 0; i < num4; i++)
			{
				int num5 = ((len > (int)kcp->mss) ? ((int)kcp->mss) : len);
				IKCPSEG* ptr2 = ikcp_segment_new(kcp, num5);
				if (buffer != null && len > 0)
				{
					memcpy(ptr2->data, buffer, num5);
				}
				ptr2->len = (uint)num5;
				ptr2->frg = 0u;
				IQUEUEHEAD.iqueue_init(&ptr2->node);
				IQUEUEHEAD.iqueue_add_tail(&ptr2->node, &kcp->snd_queue);
				kcp->nsnd_que++;
				if (buffer != null)
				{
					buffer += num5;
				}
				len -= num5;
				num += num5;
			}
		}
		else
		{
			int num6;
			if (len <= (int)kcp->mss)
			{
				num6 = 1;
			}
			else
			{
				num6 = (int)((len + kcp->mss - 1) / kcp->mss);
				if ((long)num6 > 255L || num6 >= (int)kcp->rcv_wnd)
				{
					return -2;
				}
				if (num6 == 0)
				{
					num6 = 1;
				}
			}
			for (int j = 0; j < num6; j++)
			{
				int num7 = ((len > (int)kcp->mss) ? ((int)kcp->mss) : len);
				IKCPSEG* ptr2 = ikcp_segment_new(kcp, num7);
				if (buffer != null && len > 0)
				{
					memcpy(ptr2->data, buffer, num7);
				}
				ptr2->len = (uint)num7;
				ptr2->frg = (uint)(num6 - j - 1);
				IQUEUEHEAD.iqueue_init(&ptr2->node);
				IQUEUEHEAD.iqueue_add_tail(&ptr2->node, &kcp->snd_queue);
				kcp->nsnd_que++;
				if (buffer != null)
				{
					buffer += num7;
				}
				len -= num7;
				num += num7;
			}
		}
		return num;
	}

	public unsafe static int ikcp_send_with_user_header(IKCPCB* kcp, byte* buffer, int len, byte* header, int header_len)
	{
		if (len < 0)
		{
			return -1;
		}
		int num = 0;
		if (kcp->stream != 0)
		{
			return -1;
		}
		int num2 = len + header_len;
		int num3;
		if (num2 <= (int)kcp->mss)
		{
			num3 = 1;
		}
		else
		{
			num3 = (int)((num2 + kcp->mss - 1) / kcp->mss);
			if ((long)num3 > 255L || num3 >= (int)kcp->rcv_wnd)
			{
				return -2;
			}
			if (num3 == 0)
			{
				num3 = 1;
			}
		}
		for (int i = 0; i < num3; i++)
		{
			int num4 = ((num2 > (int)kcp->mss) ? ((int)kcp->mss) : num2);
			int num5 = num4;
			IKCPSEG* ptr = ikcp_segment_new(kcp, num4);
			if (buffer != null && num2 > 0)
			{
				if (i == 0)
				{
					num5 = num4 - header_len;
					memcpy(ptr->data, header, header_len);
					memcpy(ptr->data + header_len, buffer, num5);
				}
				else
				{
					memcpy(ptr->data, buffer, num4);
				}
			}
			ptr->len = (uint)num4;
			ptr->frg = (uint)(num3 - i - 1);
			IQUEUEHEAD.iqueue_init(&ptr->node);
			IQUEUEHEAD.iqueue_add_tail(&ptr->node, &kcp->snd_queue);
			kcp->nsnd_que++;
			if (buffer != null)
			{
				buffer += num5;
			}
			num2 -= num4;
			num += num4;
		}
		return num;
	}

	private unsafe static void ikcp_update_ack(IKCPCB* kcp, int rtt)
	{
		if (kcp->rx_srtt == 0)
		{
			kcp->rx_srtt = rtt;
			kcp->rx_rttval = rtt / 2;
		}
		else
		{
			int num = rtt - kcp->rx_srtt;
			if (num < 0)
			{
				num = -num;
			}
			kcp->rx_rttval = (3 * kcp->rx_rttval + num) / 4;
			kcp->rx_srtt = (7 * kcp->rx_srtt + rtt) / 8;
			if (kcp->rx_srtt < 1)
			{
				kcp->rx_srtt = 1;
			}
		}
		int middle = (int)(kcp->rx_srtt + _imax_(kcp->interval, (uint)(4 * kcp->rx_rttval)));
		kcp->rx_rto = (int)_ibound_((uint)kcp->rx_minrto, (uint)middle, 60000u);
	}

	private unsafe static void ikcp_shrink_buf(IKCPCB* kcp)
	{
		IQUEUEHEAD* next = kcp->snd_buf.next;
		if (next != &kcp->snd_buf)
		{
			IKCPSEG* ptr = IQUEUEHEAD.iqueue_entry(next);
			kcp->snd_una = ptr->sn;
		}
		else
		{
			kcp->snd_una = kcp->snd_nxt;
		}
	}

	private unsafe static void ikcp_parse_ack(IKCPCB* kcp, uint sn)
	{
		if (_itimediff(sn, kcp->snd_una) < 0 || _itimediff(sn, kcp->snd_nxt) >= 0)
		{
			return;
		}
		IQUEUEHEAD* ptr = kcp->snd_buf.next;
		while (ptr != &kcp->snd_buf)
		{
			IKCPSEG* ptr2 = IQUEUEHEAD.iqueue_entry(ptr);
			IQUEUEHEAD* next = ptr->next;
			if (sn == ptr2->sn)
			{
				IQUEUEHEAD.iqueue_del(ptr);
				ikcp_segment_delete(kcp, ptr2);
				kcp->nsnd_buf--;
				break;
			}
			if (_itimediff(sn, ptr2->sn) >= 0)
			{
				ptr = next;
				continue;
			}
			break;
		}
	}

	private unsafe static void ikcp_parse_una(IKCPCB* kcp, uint una)
	{
		IQUEUEHEAD* ptr = kcp->snd_buf.next;
		while (ptr != &kcp->snd_buf)
		{
			IKCPSEG* ptr2 = IQUEUEHEAD.iqueue_entry(ptr);
			IQUEUEHEAD* next = ptr->next;
			if (_itimediff(una, ptr2->sn) > 0)
			{
				IQUEUEHEAD.iqueue_del(ptr);
				ikcp_segment_delete(kcp, ptr2);
				kcp->nsnd_buf--;
				ptr = next;
				continue;
			}
			break;
		}
	}

	private unsafe static void ikcp_parse_fastack(IKCPCB* kcp, uint sn, uint ts)
	{
		if (_itimediff(sn, kcp->snd_una) < 0 || _itimediff(sn, kcp->snd_nxt) >= 0)
		{
			return;
		}
		IQUEUEHEAD* ptr = kcp->snd_buf.next;
		while (ptr != &kcp->snd_buf)
		{
			IKCPSEG* ptr2 = IQUEUEHEAD.iqueue_entry(ptr);
			IQUEUEHEAD* next = ptr->next;
			if (_itimediff(sn, ptr2->sn) >= 0)
			{
				if (sn != ptr2->sn && _itimediff(ts, ptr2->ts) >= 0)
				{
					ptr2->fastack++;
				}
				ptr = next;
				continue;
			}
			break;
		}
	}

	private unsafe static int ikcp_ack_push(IKCPCB* kcp, uint sn, uint ts)
	{
		uint num = kcp->ackcount + 1;
		if (num > kcp->ackblock)
		{
			uint num2 = ((num <= 8) ? 8u : _iceilpow2_(num));
			uint* ptr = (uint*)ikcp_malloc(num2 << 3);
			if (kcp->acklist != null)
			{
				for (uint num3 = 0u; num3 < kcp->ackcount; num3++)
				{
					ptr[num3 * 2] = kcp->acklist[num3 * 2];
					ptr[num3 * 2 + 1] = kcp->acklist[num3 * 2 + 1];
				}
				ikcp_free(kcp->acklist);
			}
			kcp->acklist = ptr;
			kcp->ackblock = num2;
		}
		uint* num4 = kcp->acklist + kcp->ackcount * 2;
		*num4 = sn;
		num4[1] = ts;
		kcp->ackcount++;
		return 0;
	}

	private unsafe static void ikcp_ack_get(IKCPCB* kcp, int p, uint* sn, uint* ts)
	{
		if (sn != null)
		{
			*sn = kcp->acklist[p * 2];
		}
		if (ts != null)
		{
			*ts = kcp->acklist[p * 2 + 1];
		}
	}

	private unsafe static void ikcp_parse_data(IKCPCB* kcp, IKCPSEG* newseg)
	{
		uint sn = newseg->sn;
		if (_itimediff(sn, kcp->rcv_nxt + kcp->rcv_wnd) >= 0 || _itimediff(sn, kcp->rcv_nxt) < 0)
		{
			ikcp_segment_delete(kcp, newseg);
			return;
		}
		int num = 0;
		IQUEUEHEAD* ptr = kcp->rcv_buf.prev;
		while (ptr != &kcp->rcv_buf)
		{
			IKCPSEG* ptr2 = IQUEUEHEAD.iqueue_entry(ptr);
			IQUEUEHEAD* prev = ptr->prev;
			if (ptr2->sn == sn)
			{
				num = 1;
				break;
			}
			if (_itimediff(sn, ptr2->sn) > 0)
			{
				break;
			}
			ptr = prev;
		}
		if (num == 0)
		{
			IQUEUEHEAD.iqueue_init(&newseg->node);
			IQUEUEHEAD.iqueue_add(&newseg->node, ptr);
			kcp->nrcv_buf++;
		}
		else
		{
			ikcp_segment_delete(kcp, newseg);
		}
		while (!IQUEUEHEAD.iqueue_is_empty(&kcp->rcv_buf))
		{
			IKCPSEG* ptr3 = IQUEUEHEAD.iqueue_entry(kcp->rcv_buf.next);
			if (ptr3->sn == kcp->rcv_nxt && kcp->nrcv_que < kcp->rcv_wnd)
			{
				IQUEUEHEAD.iqueue_del(&ptr3->node);
				kcp->nrcv_buf--;
				IQUEUEHEAD.iqueue_add_tail(&ptr3->node, &kcp->rcv_queue);
				kcp->nrcv_que++;
				kcp->rcv_nxt++;
				continue;
			}
			break;
		}
	}

	public unsafe static int ikcp_input(IKCPCB* kcp, byte* data, int size)
	{
		if (data == null || size < 24)
		{
			return -1;
		}
		uint snd_una = kcp->snd_una;
		uint num = 0u;
		uint num2 = 0u;
		int num3 = 0;
		uint num4 = default(uint);
		byte b = default(byte);
		byte frg = default(byte);
		ushort num5 = default(ushort);
		uint num6 = default(uint);
		uint num7 = default(uint);
		uint una = default(uint);
		uint num8 = default(uint);
		while (size >= 24)
		{
			data = ikcp_decode32u(data, &num4);
			if (num4 != kcp->conv)
			{
				return -1;
			}
			data = ikcp_decode8u(data, &b);
			data = ikcp_decode8u(data, &frg);
			data = ikcp_decode16u(data, &num5);
			data = ikcp_decode32u(data, &num6);
			data = ikcp_decode32u(data, &num7);
			data = ikcp_decode32u(data, &una);
			data = ikcp_decode32u(data, &num8);
			size -= 24;
			if (size < num8 || (int)num8 < 0)
			{
				return -2;
			}
			if (b != 81 && b != 82 && b != 83 && b != 84)
			{
				return -3;
			}
			kcp->rmt_wnd = num5;
			ikcp_parse_una(kcp, una);
			ikcp_shrink_buf(kcp);
			switch (b)
			{
			case 82:
				if (_itimediff(kcp->current, num6) >= 0)
				{
					ikcp_update_ack(kcp, _itimediff(kcp->current, num6));
				}
				ikcp_parse_ack(kcp, num7);
				ikcp_shrink_buf(kcp);
				if (num3 == 0)
				{
					num3 = 1;
					num = num7;
					num2 = num6;
				}
				else if (_itimediff(num7, num) > 0 && _itimediff(num6, num2) > 0)
				{
					num = num7;
					num2 = num6;
				}
				break;
			case 81:
				if (_itimediff(num7, kcp->rcv_nxt + kcp->rcv_wnd) >= 0)
				{
					break;
				}
				if (ikcp_ack_push(kcp, num7, num6) != 0)
				{
					return -4;
				}
				if (_itimediff(num7, kcp->rcv_nxt) >= 0)
				{
					IKCPSEG* ptr = ikcp_segment_new(kcp, (int)num8);
					ptr->conv = num4;
					ptr->cmd = b;
					ptr->frg = frg;
					ptr->wnd = num5;
					ptr->ts = num6;
					ptr->sn = num7;
					ptr->una = una;
					ptr->len = num8;
					if (num8 != 0)
					{
						memcpy(ptr->data, data, num8);
					}
					ikcp_parse_data(kcp, ptr);
				}
				break;
			case 83:
				kcp->probe |= 2u;
				break;
			default:
				return -3;
			case 84:
				break;
			}
			data += num8;
			size -= (int)num8;
		}
		if (num3 != 0)
		{
			ikcp_parse_fastack(kcp, num, num2);
		}
		if (_itimediff(kcp->snd_una, snd_una) > 0 && kcp->cwnd < kcp->rmt_wnd)
		{
			uint mss = kcp->mss;
			if (kcp->cwnd < kcp->ssthresh)
			{
				kcp->cwnd++;
				kcp->incr += mss;
			}
			else
			{
				if (kcp->incr < mss)
				{
					kcp->incr = mss;
				}
				kcp->incr += mss * mss / kcp->incr + mss / 16;
				if ((kcp->cwnd + 1) * mss <= kcp->incr)
				{
					kcp->cwnd = (kcp->incr + mss - 1) / ((mss == 0) ? 1 : mss);
				}
			}
			if (kcp->cwnd > kcp->rmt_wnd)
			{
				kcp->cwnd = kcp->rmt_wnd;
				kcp->incr = kcp->rmt_wnd * mss;
			}
		}
		return 0;
	}

	private unsafe static byte* ikcp_encode_seg(byte* ptr, IKCPSEG* seg)
	{
		ptr = ikcp_encode32u(ptr, seg->conv);
		ptr = ikcp_encode8u(ptr, (byte)seg->cmd);
		ptr = ikcp_encode8u(ptr, (byte)seg->frg);
		ptr = ikcp_encode16u(ptr, (ushort)seg->wnd);
		ptr = ikcp_encode32u(ptr, seg->ts);
		ptr = ikcp_encode32u(ptr, seg->sn);
		ptr = ikcp_encode32u(ptr, seg->una);
		ptr = ikcp_encode32u(ptr, seg->len);
		return ptr;
	}

	private unsafe static int ikcp_wnd_unused(IKCPCB* kcp)
	{
		if (kcp->nrcv_que >= kcp->rcv_wnd)
		{
			return 0;
		}
		return (int)(kcp->rcv_wnd - kcp->nrcv_que);
	}

	public unsafe static void ikcp_flush(IKCPCB* kcp, KcpCallback output, byte[] bytes)
	{
		if (kcp->updated != 0)
		{
			ikcp_flush_internal(kcp, output, bytes);
		}
	}

	private unsafe static void ikcp_flush_internal(IKCPCB* kcp, KcpCallback output, byte[] bytes)
	{
		uint current = kcp->current;
		fixed (byte* ptr = bytes)
		{
			byte* ptr2 = ptr;
			int num = 0;
			int num2 = 0;
			IKCPSEG iKCPSEG = default(IKCPSEG);
			iKCPSEG.conv = kcp->conv;
			iKCPSEG.cmd = 82u;
			iKCPSEG.frg = 0u;
			iKCPSEG.wnd = (uint)ikcp_wnd_unused(kcp);
			iKCPSEG.una = kcp->rcv_nxt;
			iKCPSEG.len = 0u;
			iKCPSEG.sn = 0u;
			iKCPSEG.ts = 0u;
			int ackcount = (int)kcp->ackcount;
			int num3;
			for (int i = 0; i < ackcount; i++)
			{
				num3 = (int)(ptr2 - ptr);
				if (num3 + 24 > (int)kcp->mtu)
				{
					ikcp_output(output, bytes, num3);
					ptr2 = ptr;
				}
				ikcp_ack_get(kcp, i, &iKCPSEG.sn, &iKCPSEG.ts);
				ptr2 = ikcp_encode_seg(ptr2, &iKCPSEG);
			}
			kcp->ackcount = 0u;
			if (kcp->rmt_wnd == 0)
			{
				if (kcp->probe_wait == 0)
				{
					kcp->probe_wait = 7000u;
					kcp->ts_probe = kcp->current + kcp->probe_wait;
				}
				else if (_itimediff(kcp->current, kcp->ts_probe) >= 0)
				{
					if (kcp->probe_wait < 7000)
					{
						kcp->probe_wait = 7000u;
					}
					kcp->probe_wait += kcp->probe_wait / 2;
					if (kcp->probe_wait > 120000)
					{
						kcp->probe_wait = 120000u;
					}
					kcp->ts_probe = kcp->current + kcp->probe_wait;
					kcp->probe |= 1u;
				}
			}
			else
			{
				kcp->ts_probe = 0u;
				kcp->probe_wait = 0u;
			}
			if (kcp->probe != 0)
			{
				iKCPSEG.cmd = 83u;
				num3 = (int)(ptr2 - ptr);
				if (num3 + 24 > (int)kcp->mtu)
				{
					ikcp_output(output, bytes, num3);
					ptr2 = ptr;
				}
				ptr2 = ikcp_encode_seg(ptr2, &iKCPSEG);
			}
			if (kcp->probe != 0)
			{
				iKCPSEG.cmd = 84u;
				num3 = (int)(ptr2 - ptr);
				if (num3 + 24 > (int)kcp->mtu)
				{
					ikcp_output(output, bytes, num3);
					ptr2 = ptr;
				}
				ptr2 = ikcp_encode_seg(ptr2, &iKCPSEG);
			}
			kcp->probe = 0u;
			uint num4 = _imin_(kcp->snd_wnd, kcp->rmt_wnd);
			if (kcp->nocwnd == 0)
			{
				num4 = _imin_(kcp->cwnd, num4);
			}
			while (_itimediff(kcp->snd_nxt, kcp->snd_una + num4) < 0 && !IQUEUEHEAD.iqueue_is_empty(&kcp->snd_queue))
			{
				IKCPSEG* intPtr = IQUEUEHEAD.iqueue_entry(kcp->snd_queue.next);
				IQUEUEHEAD.iqueue_del(&intPtr->node);
				IQUEUEHEAD.iqueue_add_tail(&intPtr->node, &kcp->snd_buf);
				kcp->nsnd_que--;
				kcp->nsnd_buf++;
				intPtr->conv = kcp->conv;
				intPtr->cmd = 81u;
				intPtr->wnd = iKCPSEG.wnd;
				intPtr->ts = current;
				intPtr->sn = kcp->snd_nxt++;
				intPtr->una = kcp->rcv_nxt;
				intPtr->resendts = current;
				intPtr->rto = (uint)kcp->rx_rto;
				intPtr->fastack = 0u;
				intPtr->xmit = 0u;
			}
			uint num5 = ((kcp->fastresend > 0) ? ((uint)kcp->fastresend) : uint.MaxValue);
			if (kcp->nodelay == 0)
			{
				uint num6 = (uint)(kcp->rx_rto >> 3);
				for (IQUEUEHEAD* next = kcp->snd_buf.next; next != &kcp->snd_buf; next = next->next)
				{
					IKCPSEG* ptr3 = IQUEUEHEAD.iqueue_entry(next);
					int num7 = 0;
					if (ptr3->xmit == 0)
					{
						num7 = 1;
						ptr3->xmit++;
						ptr3->rto = (uint)kcp->rx_rto;
						ptr3->resendts = current + ptr3->rto + num6;
					}
					else if (_itimediff(current, ptr3->resendts) >= 0)
					{
						num7 = 1;
						ptr3->xmit++;
						kcp->xmit++;
						ptr3->rto += _imax_(ptr3->rto, (uint)kcp->rx_rto);
						ptr3->resendts = current + ptr3->rto;
						num2 = 1;
					}
					else if (ptr3->fastack >= num5 && ((int)ptr3->xmit <= kcp->fastlimit || kcp->fastlimit == 0))
					{
						num7 = 1;
						ptr3->xmit++;
						ptr3->fastack = 0u;
						ptr3->resendts = current + ptr3->rto;
						num++;
					}
					if (num7 != 0)
					{
						ptr3->ts = current;
						ptr3->wnd = iKCPSEG.wnd;
						ptr3->una = kcp->rcv_nxt;
						num3 = (int)(ptr2 - ptr);
						int num8 = (int)(24 + ptr3->len);
						if (num3 + num8 > (int)kcp->mtu)
						{
							ikcp_output(output, bytes, num3);
							ptr2 = ptr;
						}
						ptr2 = ikcp_encode_seg(ptr2, ptr3);
						if (ptr3->len != 0)
						{
							memcpy(ptr2, ptr3->data, ptr3->len);
							ptr2 += ptr3->len;
						}
						if (ptr3->xmit >= kcp->dead_link)
						{
							kcp->state = -1;
						}
					}
				}
			}
			else if (kcp->nodelay == 1)
			{
				for (IQUEUEHEAD* next = kcp->snd_buf.next; next != &kcp->snd_buf; next = next->next)
				{
					IKCPSEG* ptr4 = IQUEUEHEAD.iqueue_entry(next);
					int num9 = 0;
					if (ptr4->xmit == 0)
					{
						num9 = 1;
						ptr4->xmit++;
						ptr4->rto = (uint)kcp->rx_rto;
						ptr4->resendts = current + ptr4->rto;
					}
					else if (_itimediff(current, ptr4->resendts) >= 0)
					{
						num9 = 1;
						ptr4->xmit++;
						kcp->xmit++;
						int rto = (int)ptr4->rto;
						ptr4->rto += (uint)(rto / 2);
						ptr4->resendts = current + ptr4->rto;
						num2 = 1;
					}
					else if (ptr4->fastack >= num5 && ((int)ptr4->xmit <= kcp->fastlimit || kcp->fastlimit == 0))
					{
						num9 = 1;
						ptr4->xmit++;
						ptr4->fastack = 0u;
						ptr4->resendts = current + ptr4->rto;
						num++;
					}
					if (num9 != 0)
					{
						ptr4->ts = current;
						ptr4->wnd = iKCPSEG.wnd;
						ptr4->una = kcp->rcv_nxt;
						num3 = (int)(ptr2 - ptr);
						int num10 = (int)(24 + ptr4->len);
						if (num3 + num10 > (int)kcp->mtu)
						{
							ikcp_output(output, bytes, num3);
							ptr2 = ptr;
						}
						ptr2 = ikcp_encode_seg(ptr2, ptr4);
						if (ptr4->len != 0)
						{
							memcpy(ptr2, ptr4->data, ptr4->len);
							ptr2 += ptr4->len;
						}
						if (ptr4->xmit >= kcp->dead_link)
						{
							kcp->state = -1;
						}
					}
				}
			}
			else
			{
				for (IQUEUEHEAD* next = kcp->snd_buf.next; next != &kcp->snd_buf; next = next->next)
				{
					IKCPSEG* ptr5 = IQUEUEHEAD.iqueue_entry(next);
					int num11 = 0;
					if (ptr5->xmit == 0)
					{
						num11 = 1;
						ptr5->xmit++;
						ptr5->rto = (uint)kcp->rx_rto;
						ptr5->resendts = current + ptr5->rto;
					}
					else if (_itimediff(current, ptr5->resendts) >= 0)
					{
						num11 = 1;
						ptr5->xmit++;
						kcp->xmit++;
						int rto2 = (int)ptr5->rto;
						ptr5->rto += (uint)(rto2 / 2);
						ptr5->resendts = current + ptr5->rto;
						num2 = 1;
					}
					else if (ptr5->fastack >= num5 && ((int)ptr5->xmit <= kcp->fastlimit || kcp->fastlimit == 0))
					{
						num11 = 1;
						ptr5->xmit++;
						ptr5->fastack = 0u;
						ptr5->resendts = current + ptr5->rto;
						num++;
					}
					if (num11 != 0)
					{
						ptr5->ts = current;
						ptr5->wnd = iKCPSEG.wnd;
						ptr5->una = kcp->rcv_nxt;
						num3 = (int)(ptr2 - ptr);
						int num12 = (int)(24 + ptr5->len);
						if (num3 + num12 > (int)kcp->mtu)
						{
							ikcp_output(output, bytes, num3);
							ptr2 = ptr;
						}
						ptr2 = ikcp_encode_seg(ptr2, ptr5);
						if (ptr5->len != 0)
						{
							memcpy(ptr2, ptr5->data, ptr5->len);
							ptr2 += ptr5->len;
						}
						if (ptr5->xmit >= kcp->dead_link)
						{
							kcp->state = -1;
						}
					}
				}
			}
			num3 = (int)(ptr2 - ptr);
			if (num3 > 0)
			{
				ikcp_output(output, bytes, num3);
			}
			if (num != 0)
			{
				uint num13 = kcp->snd_nxt - kcp->snd_una;
				kcp->ssthresh = num13 / 2;
				if (kcp->ssthresh < 2)
				{
					kcp->ssthresh = 2u;
				}
				kcp->cwnd = kcp->ssthresh + num5;
				kcp->incr = kcp->cwnd * kcp->mss;
			}
			if (num2 != 0)
			{
				kcp->ssthresh = num4 / 2;
				if (kcp->ssthresh < 2)
				{
					kcp->ssthresh = 2u;
				}
				kcp->cwnd = 1u;
				kcp->incr = kcp->mss;
			}
			if (kcp->cwnd < 1)
			{
				kcp->cwnd = 1u;
				kcp->incr = kcp->mss;
			}
		}
	}

	public unsafe static void ikcp_update(IKCPCB* kcp, uint current, KcpCallback output, byte[] bytes)
	{
		kcp->current = current;
		if (kcp->updated == 0)
		{
			kcp->updated = 1u;
			kcp->ts_flush = kcp->current;
		}
		int num = _itimediff(kcp->current, kcp->ts_flush);
		if (num >= 10000 || num < -10000)
		{
			kcp->ts_flush = kcp->current;
			num = 0;
		}
		if (num >= 0)
		{
			kcp->ts_flush += kcp->interval;
			if (_itimediff(kcp->current, kcp->ts_flush) >= 0)
			{
				kcp->ts_flush = kcp->current + kcp->interval;
			}
			ikcp_flush_internal(kcp, output, bytes);
		}
	}

	public unsafe static uint ikcp_check(IKCPCB* kcp, uint current)
	{
		if (kcp->updated == 0)
		{
			return current;
		}
		uint num = kcp->ts_flush;
		if (_itimediff(current, num) >= 10000 || _itimediff(current, num) < -10000)
		{
			num = current;
		}
		if (_itimediff(current, num) >= 0)
		{
			return current;
		}
		int num2 = int.MaxValue;
		int num3 = _itimediff(num, current);
		for (IQUEUEHEAD* next = kcp->snd_buf.next; next != &kcp->snd_buf; next = next->next)
		{
			int num4 = _itimediff(IQUEUEHEAD.iqueue_entry(next)->resendts, current);
			if (num4 <= 0)
			{
				return current;
			}
			if (num4 < num2)
			{
				num2 = num4;
			}
		}
		uint num5 = (uint)((num2 < num3) ? num2 : num3);
		if (num5 >= kcp->interval)
		{
			num5 = kcp->interval;
		}
		return current + num5;
	}

	public unsafe static int ikcp_setmtu(IKCPCB* kcp, int mtu, ref byte[] buffer)
	{
		if (kcp->mtu == (uint)mtu)
		{
			return 0;
		}
		if (mtu < 24)
		{
			return -1;
		}
		buffer = new byte[((long)mtu + 24L) * 3];
		kcp->mtu = (uint)mtu;
		kcp->mss = kcp->mtu - 24;
		return 0;
	}

	public unsafe static void ikcp_interval(IKCPCB* kcp, int interval)
	{
		interval = _iclamp_(interval, 1u, 5000u);
		kcp->interval = (uint)interval;
	}

	public unsafe static void ikcp_nodelay(IKCPCB* kcp, int nodelay, int interval, int resend, int nc)
	{
		nodelay = _iclamp_(nodelay, 0u, 2u);
		kcp->nodelay = (uint)nodelay;
		if (nodelay != 0)
		{
			kcp->rx_minrto = 30;
		}
		else
		{
			kcp->rx_minrto = 100;
		}
		interval = _iclamp_(interval, 1u, 5000u);
		kcp->interval = (uint)interval;
		resend = _iclamp_(resend, 0u, uint.MaxValue);
		kcp->fastresend = resend;
		kcp->nocwnd = ((nc == 1) ? 1 : 0);
	}

	public unsafe static void ikcp_wndsize(IKCPCB* kcp, int sndwnd, int rcvwnd)
	{
		sndwnd = _iclamp_(sndwnd, 32u, 2147483647u);
		rcvwnd = _iclamp_(rcvwnd, 128u, 2147483647u);
		kcp->snd_wnd = (uint)sndwnd;
		kcp->rcv_wnd = (uint)rcvwnd;
	}

	public unsafe static void ikcp_fastresendlimit(IKCPCB* kcp, int fastlimit)
	{
		fastlimit = _iclamp_(fastlimit, 0u, 5u);
		kcp->fastlimit = fastlimit;
	}

	public unsafe static void ikcp_streammode(IKCPCB* kcp, int stream)
	{
		kcp->stream = ((stream == 1) ? 1 : 0);
	}

	public unsafe static void ikcp_minrto(IKCPCB* kcp, int minrto)
	{
		minrto = _iclamp_(minrto, 1u, 60000u);
		kcp->rx_minrto = minrto;
	}
}
