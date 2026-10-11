using System;
using System.Runtime.InteropServices;

namespace GME;

public class TMGAudioEffectCtrlNative
{
	static TMGAudioEffectCtrlNative()
	{
	}

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StartPlayMusic(IntPtr ins, int soundId, [MarshalAs(UnmanagedType.LPStr)] string filePath, int loopCount);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StopPlayMusic(IntPtr ins, int soundId);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_PausePlayMusic(IntPtr ins, int soundId);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_ResumePlayMusic(IntPtr ins, int soundId);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_IsMusicPlayEnd(IntPtr ins, int sound_id);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetAllMusicVolume(IntPtr ins, int vol);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetMusicPublishVolume(IntPtr ins, int sound_id, int volume);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetMusicPublishVolume(IntPtr ins, int sound_id);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetMusicPlayoutVolume(IntPtr ins, int sound_id, int volume);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetMusicPlayoutVolume(IntPtr ins, int sound_id);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetMusicPitch(IntPtr ins, int sound_id, float pitch);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetMusicDurationInMS(IntPtr ins, int sound_id);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_GetMusicCurrentPosInMS(IntPtr ins, int sound_id);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SeekMusicToPosInTime(IntPtr ins, int soundId, int time);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_EnableMusicPublish(IntPtr ins, int sound_id, bool enable);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_EnableMusicPlayout(IntPtr ins, int sound_id, bool enable);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetVoiceType(IntPtr ins, ITMG_VOICE_TYPE voice_type);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetKaraokeType(IntPtr ins, ITMG_KARAOKE_TYPE karaoke_type);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StartRecord(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string filePath, int content);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StopRecord(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StartSystemAudioLoopback(IntPtr ins, [MarshalAs(UnmanagedType.LPStr)] string playerPath);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_StopSystemAudioLoopback(IntPtr ins);

	[DllImport("gmesdk", CallingConvention = CallingConvention.Cdecl)]
	public static extern int GMEUnity_SetSystemAudioLoopbackVolume(IntPtr ins, int volume);
}
