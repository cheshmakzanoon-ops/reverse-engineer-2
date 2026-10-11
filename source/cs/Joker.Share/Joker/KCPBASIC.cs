namespace Joker;

public static class KCPBASIC
{
	public const uint RTO_NDL = 30u;

	public const uint RTO_MIN = 100u;

	public const uint RTO_DEF = 200u;

	public const uint RTO_MAX = 60000u;

	public const uint CMD_PUSH = 81u;

	public const uint CMD_ACK = 82u;

	public const uint CMD_WASK = 83u;

	public const uint CMD_WINS = 84u;

	public const uint ASK_SEND = 1u;

	public const uint ASK_TELL = 2u;

	public const uint WND_SND = 32u;

	public const uint WND_RCV = 128u;

	public const uint MTU_DEF = 1400u;

	public const uint MTU_SAFETY = 470u;

	public const uint ACK_FAST = 3u;

	public const uint INTERVAL = 100u;

	public const uint INTERVAL_MIN = 1u;

	public const uint INTERVAL_LIMIT = 5000u;

	public const uint OVERHEAD = 24u;

	public const uint DEADLINK = 20u;

	public const uint THRESH_INIT = 2u;

	public const uint THRESH_MIN = 2u;

	public const uint PROBE_INIT = 7000u;

	public const uint PROBE_LIMIT = 120000u;

	public const uint FRG_LIMIT = 255u;

	public const uint NODELAY_MIN = 0u;

	public const uint NODELAY_LIMIT = 2u;

	public const uint FASTACK_MIN = 0u;

	public const uint FASTACK_LIMIT = 5u;

	public const uint OUTPUT = 1u;

	public const uint INPUT = 2u;

	public const uint SEND = 4u;

	public const uint RECV = 8u;

	public const uint IN_DATA = 16u;

	public const uint IN_ACK = 32u;

	public const uint IN_PROBE = 64u;

	public const uint IN_WINS = 128u;

	public const uint OUT_DATA = 256u;

	public const uint OUT_ACK = 512u;

	public const uint OUT_PROBE = 1024u;

	public const uint OUT_WINS = 2048u;

	public const int XMIT_HISTORY = 100;
}
