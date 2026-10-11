namespace Joker;

internal struct IKCPSEG
{
	public IQUEUEHEAD node;

	public uint conv;

	public uint cmd;

	public uint frg;

	public uint wnd;

	public uint ts;

	public uint sn;

	public uint una;

	public uint len;

	public uint resendts;

	public uint rto;

	public uint fastack;

	public uint xmit;

	public unsafe fixed byte data[1];
}
