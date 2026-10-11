using System;
using System.Runtime.InteropServices;

namespace GME;

public class TMGAudioFrameCallbackNative
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void OnCapturedAudioFrameHandler(IntPtr instance, ref TMGAudioFrame audioFrame);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void OnLocalProcessedAudioFrameHandler(IntPtr instance, ref TMGAudioFrame frame);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void OnPlayAudioFrameHandler(IntPtr instance, ref TMGAudioFrame frame, string userId);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void OnMixedPlayAudioFrameHandler(IntPtr instance, ref TMGAudioFrame frame);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void OnMixedAllAudioFrameHandler(IntPtr instance, ref TMGAudioFrame frame);
}
