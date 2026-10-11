using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GME;

public class TMGAudioCtrlNative
{
	static TMGAudioCtrlNative()
	{
	}

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_EnableAudioCaptureDevice(IntPtr ins, bool enable);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_EnableAudioSend(IntPtr ins, bool bEnable);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool GMEUnity_IsAudioCaptureDeviceEnabled(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool GMEUnity_IsAudioSendEnabled(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_EnableAudioPlayDevice(IntPtr ins, bool enable);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_EnableAudioRecv(IntPtr ins, bool enable);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool GMEUnity_IsAudioPlayDeviceEnabled(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool GMEUnity_IsAudioRecvEnabled(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetMicVolume(IntPtr ins, int vol);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetMicVolume(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetSpeakerVolume(IntPtr ins, int vol);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetSpeakerVolume(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetSpeakerVolumeByUserID(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string userID, int vol);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetSpeakerVolumeByUserID(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string userID);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_EnableLoopBack(IntPtr ins, bool enable);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetLoopBackVolume(IntPtr ins, int volume);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_AddAudioBlackList(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string userID);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_RemoveAudioBlackList(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string userID);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool GMEUnity_IsUserIDInAudioBlackList(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string userID);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_EnableSpatializer(IntPtr ins, bool enable);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_IsEnableSpatializer(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetMicListCount(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetMicList(IntPtr ins, StringBuilder devicesInfo, int max_count, uint devicesInfoLen);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SelectMic(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string device_id);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetCurrentMic(IntPtr ins, StringBuilder devicesInfo, uint device_info_len);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetSpeakerListCount(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetSpeakerList(IntPtr ins, StringBuilder devicesInfo, int max_count, uint devicesInfoLen);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SelectSpeaker(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string device_id);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetCurrentSpeaker(IntPtr ins, StringBuilder devices_info, uint device_info_len);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_TrackingVolume(IntPtr ins, float interval);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StopTrackingVolume(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetMicLevel(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetSpeakerLevel(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetSendStreamLevel(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetRecvStreamLevel(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string userID);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StartMicDeviceTest(IntPtr ins, int interval);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StopMicDeviceTest(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StartSpeakerDeviceTest(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string file_path);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StopSpeakerDeviceTest(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_EnableCustomAudioCapture(IntPtr ins, bool enable);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetAudioRoute(IntPtr ins, int route);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SendCustomAudioData(IntPtr ins, ref TMGAudioFrame frame);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_EnableCustomAudioRendering(IntPtr ins, bool enable);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetCustomAudioRenderingFrame(IntPtr ins, ref TMGAudioFrame frame);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern void GMEUnity_SetAudioFrameCallback(IntPtr ins, TMGAudioFrameCallbackNative.OnCapturedAudioFrameHandler onCapturedAudioFrame, TMGAudioFrameCallbackNative.OnLocalProcessedAudioFrameHandler onLocalProcessedAudioFrame, TMGAudioFrameCallbackNative.OnPlayAudioFrameHandler onPlayAudioFrame, TMGAudioFrameCallbackNative.OnMixedPlayAudioFrameHandler onMixedPlayAudioFrame, TMGAudioFrameCallbackNative.OnMixedAllAudioFrameHandler onMixedAllAudioFrame);
}
