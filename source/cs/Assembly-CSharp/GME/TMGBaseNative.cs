using System.Runtime.InteropServices;

namespace GME;

public class TMGBaseNative
{
	public const string MyLibName = "gmesdk";

	private static bool inited;

	[DllImport("TXFFmpeg", CallingConvention = CallingConvention.Cdecl)]
	public static extern void GMEUnity_loadTXFFmpeg();

	[DllImport("TXSoundTouch", CallingConvention = CallingConvention.Cdecl)]
	public static extern void GMEUnity_loadTXSoundTouch();

	public static void InitSDK()
	{
		if (!inited)
		{
			inited = true;
		}
	}
}
