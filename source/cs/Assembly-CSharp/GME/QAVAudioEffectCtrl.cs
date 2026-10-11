using System;

namespace GME;

public class QAVAudioEffectCtrl : ITMGAudioEffectCtrl
{
	private IntPtr nativeInstance = IntPtr.Zero;

	public override event QAVPlayMusicStartCallback OnPlayMusicStartEvent;

	public override event QAVPlayMusicProgressCallback OnPlayMusicProgressEvent;

	public override event QAVPlayMusicPauseCallback OnPlayMusicPauseEvent;

	public override event QAVPlayMusicResumeCallback OnPlayMusicResumeEvent;

	public override event QAVPlayMusicFinishCallback OnPlayMusicFinishEvent;

	public override event QAVRecordCompleteCallback OnRecordCompleteEvent;

	public void InvokePlayMusicStartEvent(PlayMusicCallbackInfo callbackInfo)
	{
		OnPlayMusicStartEvent?.Invoke(callbackInfo.sound_id);
	}

	public void InvokePlayMusicProcessEvent(PlayMusicProcessCallbackInfo callbackInfo)
	{
		OnPlayMusicProgressEvent?.Invoke(callbackInfo.sound_id, callbackInfo.process_ms, callbackInfo.duration_ms);
	}

	public void InvokePlayMusicPauseEvent(PlayMusicCallbackInfo callbackInfo)
	{
		OnPlayMusicPauseEvent?.Invoke(callbackInfo.sound_id);
	}

	public void InvokePlayMusicResumeEvent(PlayMusicCallbackInfo callbackInfo)
	{
		OnPlayMusicResumeEvent?.Invoke(callbackInfo.sound_id);
	}

	public void InvokePlayMusicFinishEvent(PlayMusicFinishCallbackInfo callbackInfo)
	{
		OnPlayMusicFinishEvent?.Invoke(callbackInfo.result, callbackInfo.sound_id, callbackInfo.is_finished, callbackInfo.file_path);
	}

	public void InvokeRecordCompleteEvent(AudioRecordCompleteCallbackInfo callbackInfo)
	{
		OnRecordCompleteEvent?.Invoke(callbackInfo.result, callbackInfo.event_id, callbackInfo.file_path);
	}

	public override int SetMusicPitch(int sound_id, float pitch)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_SetMusicPitch(nativeInstance, sound_id, pitch);
	}

	public override int SetMusicPublishVolume(int sound_id, int volume)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_SetMusicPublishVolume(nativeInstance, sound_id, volume);
	}

	public override int GetMusicPublishVolume(int sound_id)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_GetMusicPublishVolume(nativeInstance, sound_id);
	}

	public override int SetMusicPlayoutVolume(int sound_id, int volume)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_SetMusicPlayoutVolume(nativeInstance, sound_id, volume);
	}

	public override int GetMusicPlayoutVolume(int sound_id)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_GetMusicPlayoutVolume(nativeInstance, sound_id);
	}

	public override int GetMusicDurationInMS(int sound_id)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_GetMusicDurationInMS(nativeInstance, sound_id);
	}

	public override int GetMusicCurrentPosInMS(int sound_id)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_GetMusicCurrentPosInMS(nativeInstance, sound_id);
	}

	public override int SetAllMusicVolume(int volume)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_SetAllMusicVolume(nativeInstance, volume);
	}

	public override int StartPlayMusic(int soundId, string filePath, int loopCount)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_StartPlayMusic(nativeInstance, soundId, filePath, loopCount);
	}

	public override int PausePlayMusic(int soundId)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_PausePlayMusic(nativeInstance, soundId);
	}

	public override int ResumePlayMusic(int soundId)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_ResumePlayMusic(nativeInstance, soundId);
	}

	public override int StopPlayMusic(int soundId)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_StopPlayMusic(nativeInstance, soundId);
	}

	public override int SeekMusicToPosInTime(int soundId, int timeMs)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_SeekMusicToPosInTime(nativeInstance, soundId, timeMs);
	}

	public override int EnableMusicPublish(int soundId, bool enable)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_EnableMusicPublish(nativeInstance, soundId, enable);
	}

	public override int EnableMusicPlayout(int soundId, bool enable)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_EnableMusicPlayout(nativeInstance, soundId, enable);
	}

	public override int StartRecord(string filePath, ITMG_AUDIO_RECORDING_CONTENT content)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_StartRecord(nativeInstance, filePath, (int)content);
	}

	public override int StopRecord()
	{
		return TMGAudioEffectCtrlNative.GMEUnity_StopRecord(nativeInstance);
	}

	public override int SetVoiceType(ITMG_VOICE_TYPE voiceType)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_SetVoiceType(nativeInstance, voiceType);
	}

	public override int SetKaraokeType(ITMG_KARAOKE_TYPE type)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_SetKaraokeType(nativeInstance, type);
	}

	public override int IsMusicPlayEnd(int sound_id)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_IsMusicPlayEnd(nativeInstance, sound_id);
	}

	public override int StartSystemAudioLoopback(string playerPath)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_StartSystemAudioLoopback(nativeInstance, playerPath);
	}

	public override int StopSystemAudioLoopback()
	{
		return TMGAudioEffectCtrlNative.GMEUnity_StopSystemAudioLoopback(nativeInstance);
	}

	public override int SetSystemAudioLoopbackVolume(int volume)
	{
		return TMGAudioEffectCtrlNative.GMEUnity_SetSystemAudioLoopbackVolume(nativeInstance, volume);
	}

	public QAVAudioEffectCtrl(TMGContext tmgContext)
	{
		nativeInstance = tmgContext.GetNativeInstance();
	}
}
