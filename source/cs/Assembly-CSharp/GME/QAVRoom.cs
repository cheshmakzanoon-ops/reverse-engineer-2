using System;
using System.Runtime.InteropServices;

namespace GME;

public class QAVRoom : ITMGRoom
{
	private IntPtr nativeInstance = IntPtr.Zero;

	private readonly object _aiTranscriberManagerLock = new object();

	private volatile AITranscriberManagerImplement _aiTranscriberManager;

	private TRTCActionQueue _actionQueue;

	public override int ChangeRoomType(ITMGRoomType roomType)
	{
		return TMGRoomNative.GMEUnity_ChangeRoomType(nativeInstance, (int)roomType);
	}

	public override int GetRoomType()
	{
		return TMGRoomNative.GMEUnity_GetRoomType(nativeInstance);
	}

	public override string GetRoomID()
	{
		return Marshal.PtrToStringAnsi(TMGRoomNative.GMEUnity_GetRoomID(nativeInstance));
	}

	public override int StartRoomSharing(string targetRoomID, string targetuserID)
	{
		return TMGRoomNative.GMEUnity_StartRoomSharing(nativeInstance, targetRoomID, targetuserID);
	}

	public override int StopRoomSharing()
	{
		return TMGRoomNative.GMEUnity_StopRoomSharing(nativeInstance);
	}

	public override int SwitchRoom(string roomID, string authBuffer)
	{
		return TMGRoomNative.GMEUnity_SwitchRoom(nativeInstance, roomID, authBuffer);
	}

	public override int SendCustomData(byte[] customdata, int length, int repeatCout)
	{
		return TMGRoomNative.GMEUnity_SendCustomData(nativeInstance, customdata, length, repeatCout);
	}

	public override int StopSendCustomData()
	{
		return TMGRoomNative.GMEUnity_StopSendCustomData(nativeInstance);
	}

	public override int UpdateSelfPosition(float[] position, float[] axisForward, float[] axisRight, float[] axisUp)
	{
		return TMGRoomNative.GMEUnity_UpdateSelfPosition(nativeInstance, position, axisForward, axisRight, axisUp, 3);
	}

	public override int UpdateAudioRecvRange(float range)
	{
		return TMGRoomNative.GMEUnity_UpdateAudioRecvRange(nativeInstance, range);
	}

	public override int UpdateSpatializerRecvRange(float range)
	{
		return TMGRoomNative.GMEUnity_UpdateSpatializerRecvRange(nativeInstance, range);
	}

	public override int UpdateOtherPosition(string userID, float[] position)
	{
		return TMGRoomNative.GMEUnity_UpdateOtherPosition(nativeInstance, userID, position, 3);
	}

	public override int SendSEIMsg(string message, int repeatCout)
	{
		return TMGRoomNative.GMEUnity_SendSEIMsg(nativeInstance, message, repeatCout);
	}

	public override IAITranscriberManager GetAITranscriberManager()
	{
		if (_aiTranscriberManager == null)
		{
			lock (_aiTranscriberManagerLock)
			{
				if (_aiTranscriberManager == null)
				{
					IntPtr intPtr = TMGRoomNative.GMEUnity_GetAITranscriberManager(nativeInstance);
					if (intPtr != IntPtr.Zero)
					{
						_aiTranscriberManager = new AITranscriberManagerImplement(intPtr, _actionQueue);
					}
				}
			}
		}
		return _aiTranscriberManager;
	}

	public void Uninit()
	{
		_aiTranscriberManager?.Destroy();
		_aiTranscriberManager = null;
	}

	public QAVRoom(TMGContext tmgContext)
	{
		nativeInstance = tmgContext.GetNativeInstance();
		_actionQueue = tmgContext.GetActionQueue();
	}
}
