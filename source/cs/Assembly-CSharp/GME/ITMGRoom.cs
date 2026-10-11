namespace GME;

public abstract class ITMGRoom
{
	public abstract string GetRoomID();

	public abstract int ChangeRoomType(ITMGRoomType roomType);

	public abstract int GetRoomType();

	public abstract int StartRoomSharing(string targetRoomID, string targetUserID);

	public abstract int StopRoomSharing();

	public abstract int SwitchRoom(string targetRoomID, string userSig);

	public abstract int SendCustomData(byte[] data, int length, int repeatCount);

	public abstract int StopSendCustomData();

	public abstract int UpdateAudioRecvRange(float range);

	public abstract int UpdateSpatializerRecvRange(float range);

	public abstract int UpdateSelfPosition(float[] position, float[] axisForward, float[] axisRight, float[] axisUp);

	public abstract int UpdateOtherPosition(string userID, float[] position);

	public abstract int SendSEIMsg(string message, int repeatCount);

	public abstract IAITranscriberManager GetAITranscriberManager();
}
