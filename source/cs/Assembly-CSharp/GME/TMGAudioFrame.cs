using System;

namespace GME;

public struct TMGAudioFrame
{
	public IntPtr data;

	public uint length;

	public uint sample_rate;

	public uint channel;

	public ulong timestamp;

	public ITMG_PCM_BITS_TYPE bits_type;
}
