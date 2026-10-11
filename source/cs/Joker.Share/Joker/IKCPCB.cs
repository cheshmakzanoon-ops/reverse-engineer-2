namespace Joker;

internal struct IKCPCB
{
	public uint conv;

	public uint mtu;

	public uint mss;

	public int state;

	public uint snd_una;

	public uint snd_nxt;

	public uint rcv_nxt;

	public uint ssthresh;

	public int rx_rttval;

	public int rx_srtt;

	public int rx_rto;

	public int rx_minrto;

	public uint snd_wnd;

	public uint rcv_wnd;

	public uint rmt_wnd;

	public uint cwnd;

	public uint probe;

	public uint current;

	public uint interval;

	public uint ts_flush;

	public uint xmit;

	public uint nrcv_buf;

	public uint nsnd_buf;

	public uint nrcv_que;

	public uint nsnd_que;

	public uint nodelay;

	public uint updated;

	public uint ts_probe;

	public uint probe_wait;

	public uint incr;

	public IQUEUEHEAD snd_queue;

	public IQUEUEHEAD rcv_queue;

	public IQUEUEHEAD snd_buf;

	public IQUEUEHEAD rcv_buf;

	public unsafe uint* acklist;

	public uint ackcount;

	public uint ackblock;

	public uint dead_link;

	public int fastresend;

	public int fastlimit;

	public int nocwnd;

	public int stream;

	public unsafe fixed int xmit_history[100];

	public int xmit_index;

	public int xmit_count;

	public int lost_count;

	public int sent_count;

	public int last_update_time;

	public float estimated_loss_rate;
}
