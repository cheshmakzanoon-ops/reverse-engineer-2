using System;
using System.Runtime.InteropServices;

namespace GME;

public class TMGContextNative
{
	static TMGContextNative()
	{
	}

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr GMEUnity_CreateInstance();

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern void GMEUnity_DestroyInstance(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern void GMEUnity_SetRegion(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string region);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_Init(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string sdk_app_id, [MarshalAs(UnmanagedType.LPStr)] string userID);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_Poll(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_Pause(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_Resume(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_Uninit(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetScene(IntPtr ins, int scene);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetAudioRole(IntPtr ins, int role_type);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetRangeAudioMode(IntPtr ins, int audio_mode);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetRangeAudioTeamID(IntPtr ins, int team_id);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_EnterRoom(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string room_id, int room_type, [MarshalAs(UnmanagedType.LPStr)] string user_sig);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_ExitRoom(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool GMEUnity_IsRoomEntered(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetLogLevel(IntPtr ins, int levelWrite, int levelPrint);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetLogPath(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string logPath);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_WriteLog(int logLevel, [MarshalAs(UnmanagedType.LPStr)] string message);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr GMEUnity_GetLogPath(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr GMEUnity_GetSDKVersion(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern void GMEUnity_SetAppVersion(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string appVersion);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_ShowDebugView(IntPtr ins, bool show);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetAdvanceParams(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr GMEUnity_GetAdvanceParams(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string key);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_CheckMicPermission(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern void GMEUnity_SetDelegate(IntPtr nativeInstance, NativeOnEventCallBack onEventCallback);
}
