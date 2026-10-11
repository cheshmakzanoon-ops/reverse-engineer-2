namespace GME;

public abstract class ITMGAudioEffectCtrl
{
	public abstract event QAVPlayMusicStartCallback OnPlayMusicStartEvent;

	public abstract event QAVPlayMusicProgressCallback OnPlayMusicProgressEvent;

	public abstract event QAVPlayMusicPauseCallback OnPlayMusicPauseEvent;

	public abstract event QAVPlayMusicResumeCallback OnPlayMusicResumeEvent;

	public abstract event QAVPlayMusicFinishCallback OnPlayMusicFinishEvent;

	public abstract event QAVRecordCompleteCallback OnRecordCompleteEvent;

	public abstract int StartPlayMusic(int soundId, string filePath, int loopCount);

	public abstract int StopPlayMusic(int soundId);

	public abstract int PausePlayMusic(int soundId);

	public abstract int ResumePlayMusic(int soundId);

	public abstract int IsMusicPlayEnd(int soundId);

	public abstract int SetAllMusicVolume(int volume);

	public abstract int SetMusicPublishVolume(int soundId, int volume);

	public abstract int GetMusicPublishVolume(int soundId);

	public abstract int SetMusicPlayoutVolume(int soundId, int volume);

	public abstract int GetMusicPlayoutVolume(int soundId);

	public abstract int SetMusicPitch(int soundId, float pitch);

	public abstract int GetMusicDurationInMS(int soundId);

	public abstract int GetMusicCurrentPosInMS(int soundId);

	public abstract int SeekMusicToPosInTime(int soundId, int time);

	public abstract int EnableMusicPublish(int soundId, bool enable);

	public abstract int EnableMusicPlayout(int soundId, bool enable);

	public abstract int SetVoiceType(ITMG_VOICE_TYPE voiceType);

	public abstract int SetKaraokeType(ITMG_KARAOKE_TYPE type);

	public abstract int StartRecord(string filePath, ITMG_AUDIO_RECORDING_CONTENT content);

	public abstract int StopRecord();

	public abstract int StartSystemAudioLoopback(string playerPath);

	public abstract int StopSystemAudioLoopback();

	public abstract int SetSystemAudioLoopbackVolume(int volume);
}
