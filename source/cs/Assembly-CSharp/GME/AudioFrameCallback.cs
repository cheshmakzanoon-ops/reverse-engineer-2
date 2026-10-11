using System;
using AOT;

namespace GME;

public static class AudioFrameCallback
{
	[MonoPInvokeCallback(typeof(TMGAudioFrameCallbackNative.OnCapturedAudioFrameHandler))]
	public static void OnCapturedAudioFrameHandler(IntPtr nativeInstance, ref TMGAudioFrame audioFrame)
	{
		TMGContext.GetTMGContextForNativeInstance(nativeInstance)?.GetAudioCtrlInner()?._audioFrameCallback?.OnCapturedAudioFrame(audioFrame);
	}

	[MonoPInvokeCallback(typeof(TMGAudioFrameCallbackNative.OnLocalProcessedAudioFrameHandler))]
	public static void OnLocalProcessedAudioFrameHandler(IntPtr nativeInstance, ref TMGAudioFrame audioFrame)
	{
		TMGContext.GetTMGContextForNativeInstance(nativeInstance)?.GetAudioCtrlInner()?._audioFrameCallback?.OnLocalProcessedAudioFrame(audioFrame);
	}

	[MonoPInvokeCallback(typeof(TMGAudioFrameCallbackNative.OnPlayAudioFrameHandler))]
	public static void OnPlayAudioFrameHandler(IntPtr nativeInstance, ref TMGAudioFrame audioFrame, string userId)
	{
		TMGContext.GetTMGContextForNativeInstance(nativeInstance)?.GetAudioCtrlInner()?._audioFrameCallback?.OnPlayAudioFrame(audioFrame, userId);
	}

	[MonoPInvokeCallback(typeof(TMGAudioFrameCallbackNative.OnMixedPlayAudioFrameHandler))]
	public static void OnMixedPlayAudioFrameHandler(IntPtr nativeInstance, ref TMGAudioFrame audioFrame)
	{
		TMGContext.GetTMGContextForNativeInstance(nativeInstance)?.GetAudioCtrlInner()?._audioFrameCallback?.OnMixedPlayAudioFrame(audioFrame);
	}

	[MonoPInvokeCallback(typeof(TMGAudioFrameCallbackNative.OnMixedAllAudioFrameHandler))]
	public static void OnMixedAllAudioFrameHandler(IntPtr nativeInstance, ref TMGAudioFrame audioFrame)
	{
		TMGContext.GetTMGContextForNativeInstance(nativeInstance)?.GetAudioCtrlInner()?._audioFrameCallback?.OnMixedAllAudioFrame(audioFrame);
	}
}
