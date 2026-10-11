using System;
using System.Runtime.InteropServices;

namespace GME;

public class TMGPTTNative
{
	static TMGPTTNative()
	{
	}

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_ApplyPTTAuthbuffer(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string userSig);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetMaxMessageLength(IntPtr ins, int msTime);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StartRecording(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string filePath);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StopRecording(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_CancelRecording(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_UploadRecordedFile(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string filePath);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_DownloadRecordedFile(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string fileID, [MarshalAs(UnmanagedType.LPStr)] string filePath);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StartPlayFile(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string filePath, ITMG_VOICE_TYPE voiceType);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StopPlayFile(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SpeechToText(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string fileID, [MarshalAs(UnmanagedType.LPStr)] string speechLanguage, [MarshalAs(UnmanagedType.LPStr)] string translateLanguage);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetFileSize(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string filePath);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetVoiceFileDuration(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string filePath);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetPTTSourceLanguage(IntPtr ins, string sourceLanguage);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StartRecordingWithStreamingRecognition(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string filePath, [MarshalAs(UnmanagedType.LPStr)] string speechLanguage, [MarshalAs(UnmanagedType.LPStr)] string translatelanguage);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_TranslateText(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string text, [MarshalAs(UnmanagedType.LPStr)] string sourceLanguage, [MarshalAs(UnmanagedType.LPStr)] string translateLanguage);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_TextToSpeech(IntPtr ins, int serialNumber, [MarshalAs(UnmanagedType.LPStr)] string text, [MarshalAs(UnmanagedType.LPStr)] string voiceName, [MarshalAs(UnmanagedType.LPStr)] string languageCode, float speakingRate);
}
