using System.Runtime.InteropServices;
using System.Text;

namespace GME;

public class QAVAuthBuffer
{
	public const string MyLibName = "gmesdk";

	public static string GenAuthBuffer(int appId, string roomID, string openId, string key)
	{
		TMGBaseNative.InitSDK();
		int num = 1024;
		StringBuilder stringBuilder = new StringBuilder(num);
		QAVSDK_AuthBuffer_GenAuthBuffer(appId, roomID, openId, key, stringBuilder, num);
		return stringBuilder.ToString();
	}

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	private static extern int QAVSDK_AuthBuffer_GenAuthBuffer(int appId, [MarshalAs(UnmanagedType.LPStr)] string roomID, [MarshalAs(UnmanagedType.LPStr)] string openID, [MarshalAs(UnmanagedType.LPStr)] string key, StringBuilder userSig, int buffLength);
}
