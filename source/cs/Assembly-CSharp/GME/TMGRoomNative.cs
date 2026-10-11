using System;
using System.Runtime.InteropServices;

namespace GME;

public class TMGRoomNative
{
	static TMGRoomNative()
	{
	}

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr GMEUnity_GetRoomID(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_ChangeRoomType(IntPtr ins, int room_type);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetRoomType(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StartRoomSharing(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string target_room_id, [MarshalAs(UnmanagedType.LPStr)] string target_userID);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StopRoomSharing(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SwitchRoom(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string room_id, [MarshalAs(UnmanagedType.LPStr)] string user_sig);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SendCustomData(IntPtr ins, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] customdata, int length, int repeatCout);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StopSendCustomData(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_UpdateAudioRecvRange(IntPtr ins, float range);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_UpdateSpatializerRecvRange(IntPtr ins, float range);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_UpdateSelfPosition(IntPtr ins, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] float[] position, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] float[] axisForward, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] float[] axisRight, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] float[] axisUp, int len);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_UpdateOtherPosition(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string userID, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] float[] position, int len);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SendSEIMsg(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string message, int repeatCout);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr GMEUnity_GetAITranscriberManager(IntPtr ins);
}
