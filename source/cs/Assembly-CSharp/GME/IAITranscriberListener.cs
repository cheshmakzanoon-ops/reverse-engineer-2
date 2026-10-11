namespace GME;

public interface IAITranscriberListener
{
	void onRealtimeTranscriberStarted(string roomId, string transcriberRobotId);

	void onReceiveTranscriberMessage(string roomId, TranscriberMessage message);

	void onRealtimeTranscriberStopped(string roomId, string transcriberRobotId, int reason);

	void onRealtimeTranscriberError(string roomId, string transcriberRobotId, int error, string errorInfo);
}
