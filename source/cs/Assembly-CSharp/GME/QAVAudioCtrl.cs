using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GME;

public class QAVAudioCtrl : ITMGAudioCtrl
{
	private const uint _deviceInfoLen = 512u;

	public ITMGAudioFrameCallback _audioFrameCallback;

	private IntPtr nativeInstance = IntPtr.Zero;

	public override int EnableAudioCaptureDevice(bool enabled)
	{
		Debug.LogFormat("EnableMic GMEUnity_EnableAudioCaptureDevice start");
		return TMGAudioCtrlNative.GMEUnity_EnableAudioCaptureDevice(nativeInstance, enabled);
	}

	public override int EnableAudioPlayDevice(bool enabled)
	{
		return TMGAudioCtrlNative.GMEUnity_EnableAudioPlayDevice(nativeInstance, enabled);
	}

	public override bool IsAudioCaptureDeviceEnabled()
	{
		return TMGAudioCtrlNative.GMEUnity_IsAudioCaptureDeviceEnabled(nativeInstance);
	}

	public override bool IsAudioPlayDeviceEnabled()
	{
		return TMGAudioCtrlNative.GMEUnity_IsAudioPlayDeviceEnabled(nativeInstance);
	}

	public override int EnableAudioSend(bool isEnabled)
	{
		return TMGAudioCtrlNative.GMEUnity_EnableAudioSend(nativeInstance, isEnabled);
	}

	public override int EnableAudioRecv(bool isEnabled)
	{
		return TMGAudioCtrlNative.GMEUnity_EnableAudioRecv(nativeInstance, isEnabled);
	}

	public override bool IsAudioSendEnabled()
	{
		return TMGAudioCtrlNative.GMEUnity_IsAudioSendEnabled(nativeInstance);
	}

	public override bool IsAudioRecvEnabled()
	{
		return TMGAudioCtrlNative.GMEUnity_IsAudioRecvEnabled(nativeInstance);
	}

	public override int EnableMic(bool isEnabled)
	{
		int num = EnableAudioCaptureDevice(isEnabled);
		int num2 = EnableAudioSend(isEnabled);
		if (num == QAVError.OK && num2 == QAVError.OK)
		{
			return QAVError.OK;
		}
		if (num != QAVError.OK)
		{
			return num;
		}
		return num2;
	}

	public override int GetMicState()
	{
		if (!IsAudioCaptureDeviceEnabled() || !IsAudioSendEnabled())
		{
			return 0;
		}
		return 1;
	}

	public override int EnableSpeaker(bool isEnabled)
	{
		int num = EnableAudioPlayDevice(isEnabled);
		int num2 = EnableAudioRecv(isEnabled);
		if (num == QAVError.OK && num2 == QAVError.OK)
		{
			return QAVError.OK;
		}
		if (num != QAVError.OK)
		{
			return num;
		}
		return num2;
	}

	public override int GetSpeakerState()
	{
		if (!IsAudioPlayDeviceEnabled() || !IsAudioRecvEnabled())
		{
			return 0;
		}
		return 1;
	}

	public override int SetMicVolume(int volume)
	{
		return TMGAudioCtrlNative.GMEUnity_SetMicVolume(nativeInstance, volume);
	}

	public override int GetMicVolume()
	{
		return TMGAudioCtrlNative.GMEUnity_GetMicVolume(nativeInstance);
	}

	public override int SetSpeakerVolume(int volume)
	{
		return TMGAudioCtrlNative.GMEUnity_SetSpeakerVolume(nativeInstance, volume);
	}

	public override int GetSpeakerVolume()
	{
		return TMGAudioCtrlNative.GMEUnity_GetSpeakerVolume(nativeInstance);
	}

	public override int SetSpeakerVolumeByUserID(string userID, int volume)
	{
		return TMGAudioCtrlNative.GMEUnity_SetSpeakerVolumeByUserID(nativeInstance, userID, volume);
	}

	public override int GetSpeakerVolumeByUserID(string userID)
	{
		return TMGAudioCtrlNative.GMEUnity_GetSpeakerVolumeByUserID(nativeInstance, userID);
	}

	public override int EnableLoopBack(bool enable)
	{
		return TMGAudioCtrlNative.GMEUnity_EnableLoopBack(nativeInstance, enable);
	}

	public override int SetLoopBackVolume(int volume)
	{
		return TMGAudioCtrlNative.GMEUnity_SetLoopBackVolume(nativeInstance, volume);
	}

	public override int AddAudioBlackList(string userID)
	{
		return TMGAudioCtrlNative.GMEUnity_AddAudioBlackList(nativeInstance, userID);
	}

	public override int RemoveAudioBlackList(string userID)
	{
		return TMGAudioCtrlNative.GMEUnity_RemoveAudioBlackList(nativeInstance, userID);
	}

	public override bool IsUserIDInAudioBlackList(string userID)
	{
		return TMGAudioCtrlNative.GMEUnity_IsUserIDInAudioBlackList(nativeInstance, userID);
	}

	public override int GetMicListCount()
	{
		return TMGAudioCtrlNative.GMEUnity_GetMicListCount(nativeInstance);
	}

	public override int GetMicList(out List<TMGAudioDeviceInfo> devicesInfo, int count)
	{
		devicesInfo = new List<TMGAudioDeviceInfo>();
		if (count <= 0)
		{
			return -1;
		}
		StringBuilder stringBuilder = new StringBuilder(count * 512);
		int num = TMGAudioCtrlNative.GMEUnity_GetMicList(nativeInstance, stringBuilder, count, (uint)(count * 512));
		if (num != 0 || stringBuilder == null)
		{
			return -1;
		}
		string[] array = stringBuilder.ToString().Split(new char[1] { '~' });
		if (array.Length != count)
		{
			return -1;
		}
		for (int i = 0; i < count; i++)
		{
			string[] array2 = array[i].Split(new char[1] { '$' });
			if (array2 == null || array2.Length != 2)
			{
				return -1;
			}
			devicesInfo.Add(new TMGAudioDeviceInfo(array2[0], array2[1]));
		}
		return num;
	}

	public override int SelectMic(string micID)
	{
		return TMGAudioCtrlNative.GMEUnity_SelectMic(nativeInstance, micID);
	}

	public override int GetCurrentMic(out TMGAudioDeviceInfo deviceInfo)
	{
		StringBuilder stringBuilder = new StringBuilder(512);
		int num = TMGAudioCtrlNative.GMEUnity_GetCurrentMic(nativeInstance, stringBuilder, 512u);
		if (num != 0 || stringBuilder == null)
		{
			deviceInfo = new TMGAudioDeviceInfo("", "");
			return -1;
		}
		string[] array = stringBuilder.ToString().Split(new char[1] { '$' });
		if (array == null || array.Length != 2)
		{
			deviceInfo = new TMGAudioDeviceInfo("", "");
			return -1;
		}
		deviceInfo = new TMGAudioDeviceInfo(array[0], array[1]);
		return num;
	}

	public override int GetSpeakerListCount()
	{
		return TMGAudioCtrlNative.GMEUnity_GetSpeakerListCount(nativeInstance);
	}

	public override int GetSpeakerList(out List<TMGAudioDeviceInfo> devicesInfo, int count)
	{
		devicesInfo = new List<TMGAudioDeviceInfo>();
		if (count <= 0)
		{
			return -1;
		}
		StringBuilder stringBuilder = new StringBuilder(count * 512);
		int num = TMGAudioCtrlNative.GMEUnity_GetSpeakerList(nativeInstance, stringBuilder, count, (uint)(count * 512));
		if (num != 0 || stringBuilder == null)
		{
			return -1;
		}
		string[] array = stringBuilder.ToString().Split(new char[1] { '~' });
		if (array.Length != count)
		{
			return -1;
		}
		for (int i = 0; i < count; i++)
		{
			string[] array2 = array[i].Split(new char[1] { '$' });
			if (array2 == null || array2.Length != 2)
			{
				return -1;
			}
			devicesInfo.Add(new TMGAudioDeviceInfo(array2[0], array2[1]));
		}
		return num;
	}

	public override int SelectSpeaker(string speaker)
	{
		return TMGAudioCtrlNative.GMEUnity_SelectSpeaker(nativeInstance, speaker);
	}

	public override int GetCurrentSpeaker(out TMGAudioDeviceInfo deviceInfo)
	{
		StringBuilder stringBuilder = new StringBuilder(512);
		int num = TMGAudioCtrlNative.GMEUnity_GetCurrentSpeaker(nativeInstance, stringBuilder, 512u);
		if (num != 0 || stringBuilder == null)
		{
			deviceInfo = new TMGAudioDeviceInfo("", "");
			return -1;
		}
		string[] array = stringBuilder.ToString().Split(new char[1] { '$' });
		if (array == null || array.Length != 2)
		{
			deviceInfo = new TMGAudioDeviceInfo("", "");
			return -1;
		}
		deviceInfo = new TMGAudioDeviceInfo(array[0], array[1]);
		return num;
	}

	public override int TrackingVolume(float interval)
	{
		return TMGAudioCtrlNative.GMEUnity_TrackingVolume(nativeInstance, interval);
	}

	public override int StopTrackingVolume()
	{
		return TMGAudioCtrlNative.GMEUnity_StopTrackingVolume(nativeInstance);
	}

	public override int GetMicLevel()
	{
		return TMGAudioCtrlNative.GMEUnity_GetMicLevel(nativeInstance);
	}

	public override int GetSpeakerLevel()
	{
		return TMGAudioCtrlNative.GMEUnity_GetSpeakerLevel(nativeInstance);
	}

	public override int GetSendStreamLevel()
	{
		return TMGAudioCtrlNative.GMEUnity_GetSendStreamLevel(nativeInstance);
	}

	public override int GetRecvStreamLevel(string userID)
	{
		return TMGAudioCtrlNative.GMEUnity_GetRecvStreamLevel(nativeInstance, userID);
	}

	public override int StartMicDeviceTest(int interval)
	{
		return TMGAudioCtrlNative.GMEUnity_StartMicDeviceTest(nativeInstance, interval);
	}

	public override int StopMicDeviceTest()
	{
		return TMGAudioCtrlNative.GMEUnity_StopMicDeviceTest(nativeInstance);
	}

	public override int StartSpeakerDeviceTest(string filePath)
	{
		return TMGAudioCtrlNative.GMEUnity_StartSpeakerDeviceTest(nativeInstance, filePath);
	}

	public override int StopSpeakerDeviceTest()
	{
		return TMGAudioCtrlNative.GMEUnity_StopSpeakerDeviceTest(nativeInstance);
	}

	public override int InitSpatializer(string modelPath)
	{
		return QAVError.OK;
	}

	public override int EnableSpatializer(bool enable)
	{
		return TMGAudioCtrlNative.GMEUnity_EnableSpatializer(nativeInstance, enable);
	}

	public override bool IsEnableSpatializer()
	{
		if (TMGAudioCtrlNative.GMEUnity_IsEnableSpatializer(nativeInstance) != 0)
		{
			return true;
		}
		return false;
	}

	public override int EnableCustomAudioCapture(bool enable)
	{
		return TMGAudioCtrlNative.GMEUnity_EnableCustomAudioCapture(nativeInstance, enable);
	}

	public override int SetAudioRoute(ITMG_AUDIO_ROUTE route)
	{
		return TMGAudioCtrlNative.GMEUnity_SetAudioRoute(nativeInstance, (int)route);
	}

	public override int SendCustomAudioData(ref TMGAudioFrame frame)
	{
		return TMGAudioCtrlNative.GMEUnity_SendCustomAudioData(nativeInstance, ref frame);
	}

	public override int EnableCustomAudioRendering(bool enable)
	{
		return TMGAudioCtrlNative.GMEUnity_EnableCustomAudioRendering(nativeInstance, enable);
	}

	public override int GetCustomAudioRenderingFrame(ref TMGAudioFrame frame)
	{
		return TMGAudioCtrlNative.GMEUnity_GetCustomAudioRenderingFrame(nativeInstance, ref frame);
	}

	public override void SetAudioFrameCallback(ITMGAudioFrameCallback callback)
	{
		_audioFrameCallback = callback;
		if (callback == null)
		{
			TMGAudioCtrlNative.GMEUnity_SetAudioFrameCallback(nativeInstance, null, null, null, null, null);
		}
		else
		{
			TMGAudioCtrlNative.GMEUnity_SetAudioFrameCallback(nativeInstance, AudioFrameCallback.OnCapturedAudioFrameHandler, AudioFrameCallback.OnLocalProcessedAudioFrameHandler, AudioFrameCallback.OnPlayAudioFrameHandler, AudioFrameCallback.OnMixedPlayAudioFrameHandler, AudioFrameCallback.OnMixedAllAudioFrameHandler);
		}
	}

	public QAVAudioCtrl(TMGContext tmgContext)
	{
		nativeInstance = tmgContext.GetNativeInstance();
	}

	~QAVAudioCtrl()
	{
	}
}
