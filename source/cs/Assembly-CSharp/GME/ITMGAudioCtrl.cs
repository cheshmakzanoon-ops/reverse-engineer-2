using System.Collections.Generic;

namespace GME;

public abstract class ITMGAudioCtrl
{
	public abstract int EnableMic(bool enable);

	public abstract int GetMicState();

	public abstract int EnableSpeaker(bool enable);

	public abstract int GetSpeakerState();

	public abstract int EnableAudioCaptureDevice(bool enable);

	public abstract bool IsAudioCaptureDeviceEnabled();

	public abstract int EnableAudioPlayDevice(bool enable);

	public abstract bool IsAudioPlayDeviceEnabled();

	public abstract int EnableAudioSend(bool enable);

	public abstract bool IsAudioSendEnabled();

	public abstract int EnableAudioRecv(bool enable);

	public abstract bool IsAudioRecvEnabled();

	public abstract int SetMicVolume(int volume);

	public abstract int GetMicVolume();

	public abstract int SetSpeakerVolume(int volume);

	public abstract int GetSpeakerVolume();

	public abstract int SetSpeakerVolumeByUserID(string userID, int volume);

	public abstract int GetSpeakerVolumeByUserID(string userID);

	public abstract int EnableLoopBack(bool enable);

	public abstract int SetLoopBackVolume(int volume);

	public abstract int AddAudioBlackList(string userID);

	public abstract int RemoveAudioBlackList(string userID);

	public abstract bool IsUserIDInAudioBlackList(string userID);

	public abstract int InitSpatializer(string modelPath);

	public abstract int EnableSpatializer(bool enable);

	public abstract bool IsEnableSpatializer();

	public abstract int GetMicListCount();

	public abstract int GetMicList(out List<TMGAudioDeviceInfo> ppDeviceInfoList, int nCount);

	public abstract int SelectMic(string micId);

	public abstract int GetCurrentMic(out TMGAudioDeviceInfo pDeviceInfo);

	public abstract int GetSpeakerListCount();

	public abstract int GetSpeakerList(out List<TMGAudioDeviceInfo> ppDeviceInfoList, int nCount);

	public abstract int SelectSpeaker(string pSpeakerID);

	public abstract int GetCurrentSpeaker(out TMGAudioDeviceInfo pDeviceInfo);

	public abstract int EnableCustomAudioCapture(bool enable);

	public abstract int SetAudioRoute(ITMG_AUDIO_ROUTE route);

	public abstract int SendCustomAudioData(ref TMGAudioFrame frame);

	public abstract int EnableCustomAudioRendering(bool enable);

	public abstract int GetCustomAudioRenderingFrame(ref TMGAudioFrame frame);

	public abstract int TrackingVolume(float interval);

	public abstract int StopTrackingVolume();

	public abstract int GetMicLevel();

	public abstract int GetSpeakerLevel();

	public abstract int GetSendStreamLevel();

	public abstract int GetRecvStreamLevel(string userID);

	public abstract int StartMicDeviceTest(int interval);

	public abstract int StopMicDeviceTest();

	public abstract int StartSpeakerDeviceTest(string filePath);

	public abstract int StopSpeakerDeviceTest();

	public abstract void SetAudioFrameCallback(ITMGAudioFrameCallback callback);
}
