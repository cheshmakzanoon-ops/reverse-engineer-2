namespace GME;

public interface ITMGAudioFrameCallback
{
	void OnCapturedAudioFrame(TMGAudioFrame frame);

	void OnLocalProcessedAudioFrame(TMGAudioFrame frame);

	void OnPlayAudioFrame(TMGAudioFrame frame, string userID);

	void OnMixedPlayAudioFrame(TMGAudioFrame frame);

	void OnMixedAllAudioFrame(TMGAudioFrame frame);
}
