using System;
using System.Runtime.InteropServices;

namespace GME;

public class AITranscriberManagerNative : TMGBaseNative
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void UnityAITranscriberOnStarted(IntPtr instance, IntPtr roomId, IntPtr transcriberRobotId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void UnityAITranscriberOnMessageReceived(IntPtr instance, IntPtr roomId, IntPtr messagePtr);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void UnityAITranscriberOnStopped(IntPtr instance, IntPtr roomId, IntPtr transcriberRobotId, int reason);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void UnityAITranscriberOnError(IntPtr instance, IntPtr roomId, IntPtr transcriberRobotId, int error, IntPtr errorInfo);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr tx_ai_transcriber_manager_create_transcriber_listener(IntPtr instance, UnityAITranscriberOnStarted onStarted, UnityAITranscriberOnMessageReceived onMessageReceived, UnityAITranscriberOnStopped onStopped, UnityAITranscriberOnError onError);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern void tx_ai_transcriber_manager_add_transcriber_listener(IntPtr instance, IntPtr listener);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern void tx_ai_transcriber_manager_remove_transcriber_listener(IntPtr instance, IntPtr listener);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern void tx_ai_transcriber_manager_destroy_transcriber_listener(IntPtr listener);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int tx_ai_transcriber_manager_start_realtime_transcriber(IntPtr instance, ref NativeTranscriberParams transcriberParams);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int tx_ai_transcriber_manager_stop_realtime_transcriber(IntPtr instance, IntPtr transcriberRobotId);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int tx_ai_transcriber_manager_pause_receiving_message(IntPtr instance);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int tx_ai_transcriber_manager_resume_receiving_message(IntPtr instance);
}
