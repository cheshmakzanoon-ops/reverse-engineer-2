namespace GME;

public abstract class ITMGContext
{
	public abstract event QAVEnterRoomComplete OnEnterRoomCompleteEvent;

	public abstract event QAVExitRoomComplete OnExitRoomCompleteEvent;

	public abstract event QAVRoomDisconnect OnRoomDisconnectEvent;

	public abstract event QAVEndpointsUpdateInfo OnEndpointsUpdateInfoEvent;

	public abstract event QAVOnRoomTypeChangedEvent OnRoomTypeChangedEvent;

	public abstract event QAVRoomChangeQualityCallback OnRoomChangeQualityEvent;

	public abstract event QAVNetworkQualityStatistics OnNetworkQualityStatisticsEvent;

	public abstract event QAVOnEventCallBack onEventCallBack;

	public static ITMGContext GetInstance()
	{
		return TMGContext.GetInstance();
	}

	public static void SetLibraryPath(string libPath)
	{
		TMGContext.SetLibraryPath(libPath);
	}

	public abstract ITMGContext CreateSubInstance();

	public abstract void DestroySubInstance(ITMGContext subInstance);

	public abstract void SetRegion(string region);

	public abstract int Init(string appId, string userID);

	public abstract int Poll();

	public abstract int Pause();

	public abstract int Resume();

	public abstract int Uninit();

	public abstract int SetScene(ITMG_APP_SCENE scene);

	public abstract int SetAudioRole(ITMG_AUDIO_MEMBER_ROLE role);

	public abstract int SetRangeAudioMode(ITMG_RANGE_AUDIO_MODE rangeAudioMode);

	public abstract int SetRangeAudioTeamID(int teamID);

	public abstract int EnterRoom(string roomId, ITMGRoomType roomType, string userSig);

	public abstract int ExitRoom();

	public abstract bool IsRoomEntered();

	public abstract int SetLogLevel(int levelWrite, int levelPrint);

	public abstract int SetLogPath(string logDir);

	public abstract string GetLogPath();

	public abstract string GetSDKVersion();

	public abstract void SetAppVersion(string appVersion);

	public abstract int ShowDebugView(bool show);

	public abstract ITMGRoom GetRoom();

	public abstract ITMGPTT GetPttCtrl();

	public abstract ITMGAudioCtrl GetAudioCtrl();

	public abstract ITMGAudioEffectCtrl GetAudioEffectCtrl();

	public abstract int SetRecvMixStreamCount(int count);

	public abstract int SetAdvanceParams(string key, string value);

	public abstract string GetAdvanceParams(string key);

	public abstract ITMG_MIC_PERMISSION CheckMicPermission();
}
