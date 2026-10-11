using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GME;

public class TMGContext : ITMGContext
{
	private QAVRoom mAVRoom;

	private QAVAudioCtrl mAVAudioCtrl;

	private QAVAudioEffectCtrl mAVAudioEffectCtrl;

	private readonly object _actionQueueLock = new object();

	private volatile TRTCActionQueue _actionQueue;

	private static TMGContext sInstance;

	private IntPtr nativeInstance;

	public static Dictionary<IntPtr, TMGContext> nativeInstanceMap;

	private static readonly object nativeInstanceMapLock;

	public override event QAVEnterRoomComplete OnEnterRoomCompleteEvent;

	public override event QAVExitRoomComplete OnExitRoomCompleteEvent;

	public override event QAVRoomDisconnect OnRoomDisconnectEvent;

	public override event QAVEndpointsUpdateInfo OnEndpointsUpdateInfoEvent;

	public override event QAVOnRoomTypeChangedEvent OnRoomTypeChangedEvent;

	public override event QAVRoomChangeQualityCallback OnRoomChangeQualityEvent;

	public override event QAVNetworkQualityStatistics OnNetworkQualityStatisticsEvent;

	public override event QAVOnEventCallBack onEventCallBack;

	static TMGContext()
	{
		nativeInstanceMap = new Dictionary<IntPtr, TMGContext>();
		nativeInstanceMapLock = new object();
		TMGBaseNative.InitSDK();
	}

	public new static void SetLibraryPath(string libPath)
	{
		new AndroidJavaClass("com.gme.av.utils.GMELibLoader").CallStatic("setLibraryPath", libPath);
	}

	public new static TMGContext GetInstance()
	{
		if (sInstance == null)
		{
			sInstance = new TMGContext(mainInstance: true);
		}
		return sInstance;
	}

	public IntPtr GetNativeInstance()
	{
		_ = nativeInstance;
		return nativeInstance;
	}

	private TMGContext(bool mainInstance)
	{
		if (mainInstance)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
			new AndroidJavaClass("com.gme.TMG.ITMGContext").CallStatic<AndroidJavaObject>("GetInstance", new object[1] { androidJavaObject });
			TMGContextNative.GMEUnity_WriteLog(2, "Unity Main TMGContext created");
		}
		else
		{
			TMGContextNative.GMEUnity_WriteLog(2, "Unity Sub TMGContext created");
		}
		nativeInstance = TMGContextNative.GMEUnity_CreateInstance();
	}

	~TMGContext()
	{
		TMGContextNative.GMEUnity_DestroyInstance(nativeInstance);
		nativeInstance = IntPtr.Zero;
	}

	public static TMGContext GetTMGContextForNativeInstance(IntPtr nativeInstance)
	{
		lock (nativeInstanceMapLock)
		{
			nativeInstanceMap.TryGetValue(nativeInstance, out var value);
			return value;
		}
	}

	public override ITMGContext CreateSubInstance()
	{
		if (sInstance != this)
		{
			return null;
		}
		return new TMGContext(mainInstance: false);
	}

	public override void DestroySubInstance(ITMGContext subInstance)
	{
		TMGContextNative.GMEUnity_WriteLog(2, "Unity DestroySubInstance");
		if (sInstance != this || subInstance == null || !(subInstance is TMGContext tMGContext))
		{
			return;
		}
		TMGContextNative.GMEUnity_Uninit(tMGContext.nativeInstance);
		lock (nativeInstanceMapLock)
		{
			if (nativeInstanceMap.ContainsKey(tMGContext.nativeInstance))
			{
				nativeInstanceMap.Remove(tMGContext.nativeInstance);
			}
		}
	}

	public override int Poll()
	{
		_actionQueue?.Update();
		return TMGContextNative.GMEUnity_Poll(nativeInstance);
	}

	public override int Pause()
	{
		_actionQueue?.Pause();
		return TMGContextNative.GMEUnity_Pause(nativeInstance);
	}

	public override int Resume()
	{
		_actionQueue?.Resume();
		return TMGContextNative.GMEUnity_Resume(nativeInstance);
	}

	public override int ShowDebugView(bool show)
	{
		return TMGContextNative.GMEUnity_ShowDebugView(nativeInstance, show);
	}

	public override int Init(string sdkAppID, string userID)
	{
		TMGContextNative.GMEUnity_SetDelegate(nativeInstance, GMECSharpEventCallback.OnEventCallBack);
		lock (nativeInstanceMapLock)
		{
			if (!nativeInstanceMap.ContainsKey(nativeInstance))
			{
				nativeInstanceMap.Add(nativeInstance, this);
			}
		}
		return TMGContextNative.GMEUnity_Init(nativeInstance, sdkAppID, userID);
	}

	public override int SetScene(ITMG_APP_SCENE scene)
	{
		return TMGContextNative.GMEUnity_SetScene(nativeInstance, (int)scene);
	}

	public override int Uninit()
	{
		mAVRoom?.Uninit();
		return TMGContextNative.GMEUnity_Uninit(nativeInstance);
	}

	public override string GetSDKVersion()
	{
		return Marshal.PtrToStringAnsi(TMGContextNative.GMEUnity_GetSDKVersion(nativeInstance));
	}

	public override void SetAppVersion(string sAppVersion)
	{
		TMGContextNative.GMEUnity_SetAppVersion(nativeInstance, sAppVersion);
	}

	public override void SetRegion(string region)
	{
		TMGContextNative.GMEUnity_SetRegion(nativeInstance, region);
	}

	public override int SetLogLevel(int levelWrite, int levelPrint)
	{
		return TMGContextNative.GMEUnity_SetLogLevel(nativeInstance, levelWrite, levelPrint);
	}

	public override int SetLogPath(string logDir)
	{
		return TMGContextNative.GMEUnity_SetLogPath(nativeInstance, logDir);
	}

	public override string GetLogPath()
	{
		return Marshal.PtrToStringAnsi(TMGContextNative.GMEUnity_GetLogPath(nativeInstance));
	}

	public override int SetRecvMixStreamCount(int nCount)
	{
		return TMGContextNative.GMEUnity_SetAdvanceParams(nativeInstance, "RecvMixStreamCount", Convert.ToString(nCount));
	}

	public override int SetAudioRole(ITMG_AUDIO_MEMBER_ROLE role)
	{
		return TMGContextNative.GMEUnity_SetAudioRole(nativeInstance, (int)role);
	}

	public override bool IsRoomEntered()
	{
		return TMGContextNative.GMEUnity_IsRoomEntered(nativeInstance);
	}

	public override int EnterRoom(string roomID, ITMGRoomType roomtype, string userSig)
	{
		return TMGContextNative.GMEUnity_EnterRoom(nativeInstance, roomID, (int)roomtype, userSig);
	}

	public override int ExitRoom()
	{
		return TMGContextNative.GMEUnity_ExitRoom(nativeInstance);
	}

	public override ITMGRoom GetRoom()
	{
		return GetRoomInner();
	}

	public override string GetAdvanceParams(string KeyCode)
	{
		return Marshal.PtrToStringAnsi(TMGContextNative.GMEUnity_GetAdvanceParams(nativeInstance, KeyCode));
	}

	public override int SetAdvanceParams(string KeyCode, string value)
	{
		return TMGContextNative.GMEUnity_SetAdvanceParams(nativeInstance, KeyCode, value);
	}

	public override ITMG_MIC_PERMISSION CheckMicPermission()
	{
		return (ITMG_MIC_PERMISSION)TMGContextNative.GMEUnity_CheckMicPermission(nativeInstance);
	}

	public QAVRoom GetRoomInner()
	{
		if (mAVRoom == null)
		{
			mAVRoom = new QAVRoom(this);
		}
		return mAVRoom;
	}

	public override ITMGAudioCtrl GetAudioCtrl()
	{
		return GetAudioCtrlInner();
	}

	public QAVAudioCtrl GetAudioCtrlInner()
	{
		if (mAVAudioCtrl == null)
		{
			mAVAudioCtrl = new QAVAudioCtrl(this);
		}
		return mAVAudioCtrl;
	}

	public override ITMGAudioEffectCtrl GetAudioEffectCtrl()
	{
		return GetAudioEffectCtrlInner();
	}

	public QAVAudioEffectCtrl GetAudioEffectCtrlInner()
	{
		if (mAVAudioEffectCtrl == null)
		{
			mAVAudioEffectCtrl = new QAVAudioEffectCtrl(this);
		}
		return mAVAudioEffectCtrl;
	}

	public override ITMGPTT GetPttCtrl()
	{
		return QAVPTT.GetInstance();
	}

	public override int SetRangeAudioMode(ITMG_RANGE_AUDIO_MODE gameAudioMode)
	{
		return TMGContextNative.GMEUnity_SetRangeAudioMode(nativeInstance, (int)gameAudioMode);
	}

	public override int SetRangeAudioTeamID(int teamID)
	{
		return TMGContextNative.GMEUnity_SetRangeAudioTeamID(nativeInstance, teamID);
	}

	public TRTCActionQueue GetActionQueue()
	{
		if (_actionQueue == null)
		{
			lock (_actionQueueLock)
			{
				if (_actionQueue == null)
				{
					_actionQueue = new TRTCActionQueue();
				}
			}
		}
		return _actionQueue;
	}

	public void InvokeEnterRoomEvent(EventCallbackInfo callbackInfo)
	{
		OnEnterRoomCompleteEvent?.Invoke(callbackInfo.result, callbackInfo.error_info);
	}

	public void InvokeExitRoomEvent()
	{
		OnExitRoomCompleteEvent?.Invoke();
	}

	public void InvokeRoomDisconnectEvent(EventCallbackInfo callbackInfo)
	{
		OnRoomDisconnectEvent?.Invoke(callbackInfo.result, callbackInfo.error_info);
	}

	public void InvokeUserInfoUpdateEvent(UserInfoUpdateCallbackInfo callbackInfo)
	{
		OnEndpointsUpdateInfoEvent?.Invoke(callbackInfo.event_id, callbackInfo.user_list.Length, callbackInfo.user_list);
	}

	public void InvokeNetworkQualityStatisticsEvent(NetworkQualityCallbackInfo callbackInfo)
	{
		TMGNetworkQuality tMGNetworkQuality = new TMGNetworkQuality();
		tMGNetworkQuality.quality = (ITMG_NETWORK_QUALITY)callbackInfo.local.quality;
		tMGNetworkQuality.rtt = callbackInfo.local.rtt;
		tMGNetworkQuality.upLoss = callbackInfo.local.up_loss;
		tMGNetworkQuality.downLoss = callbackInfo.local.down_loss;
		List<TMGNetworkQuality> list = new List<TMGNetworkQuality>();
		for (int i = 0; i < callbackInfo.remote.Length; i++)
		{
			TMGNetworkQuality tMGNetworkQuality2 = new TMGNetworkQuality();
			tMGNetworkQuality2.userID = callbackInfo.remote[i].user_id;
			tMGNetworkQuality2.quality = (ITMG_NETWORK_QUALITY)callbackInfo.remote[i].quality;
			tMGNetworkQuality2.rtt = callbackInfo.remote[i].rtt;
			tMGNetworkQuality2.upLoss = callbackInfo.remote[i].up_loss;
			tMGNetworkQuality2.downLoss = callbackInfo.remote[i].down_loss;
			list.Add(tMGNetworkQuality2);
		}
		OnNetworkQualityStatisticsEvent?.Invoke(tMGNetworkQuality, list);
	}

	public void InvokeCommonEvent(int eventType, int subEventType, string data)
	{
		onEventCallBack?.Invoke(eventType, subEventType, data);
	}
}
