namespace GME;

public abstract class IAITranscriberManager
{
	public abstract void startRealtimeTranscriber(TranscriberParams param);

	public abstract void stopRealtimeTranscriber(string transcriberRobotId);

	public abstract void pauseReceivingMessage();

	public abstract void resumeReceivingMessage();

	public abstract void addListener(IAITranscriberListener listener);

	public abstract void removeListener(IAITranscriberListener listener);
}
