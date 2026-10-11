using System;
using GameFramework;
using UnityEngine;
using VEngine;
using XLua;

[LuaCallCSharp(GenFlag.No)]
public class VoiceChatManager
{
	private sealed class RoomState
	{
		public string ChatRoomId = string.Empty;

		public string VoiceRoomId = string.Empty;
	}

	private static VoiceChatManager _instance;

	private IVoiceChatService _service;

	private readonly RoomState _joined = new RoomState();

	private readonly RoomState _pendingEnter = new RoomState();

	private readonly RoomState _pendingExit = new RoomState();

	private bool _isUpdateRegistered;

	private bool _isWaitingExitCallback;

	private bool _isSwitchingRoom;

	private bool _shouldUninitAfterExit;

	private bool _isRoomOpPending;

	private float _roomOpPendingSetRealtime;

	private const float RoomOpPendingTimeoutSeconds = 60f;

	private static int cachedMicEnable = -1;

	public static VoiceChatManager Instance => _instance ?? (_instance = new VoiceChatManager());

	public bool IsInitialized { get; private set; }

	public bool IsRoomOpPending
	{
		get
		{
			if (!_isRoomOpPending)
			{
				return false;
			}
			if (_roomOpPendingSetRealtime + 60f > Time.realtimeSinceStartup)
			{
				return true;
			}
			_isRoomOpPending = false;
			_roomOpPendingSetRealtime = 0f;
			return false;
		}
		private set
		{
			if (value)
			{
				_isRoomOpPending = true;
				_roomOpPendingSetRealtime = Time.realtimeSinceStartup;
			}
			else
			{
				_isRoomOpPending = false;
				_roomOpPendingSetRealtime = 0f;
			}
		}
	}

	public IVoiceChatService Service => _service;

	public event Action<VoiceChatEnterRoomResult> EnterRoomCompleted;

	public event Action<VoiceChatExitRoomResult> ExitRoomCompleted;

	public event Action<VoiceChatDisconnectResult> RoomDisconnected;

	public event Action<VoiceChatUserUpdate> UserUpdated;

	public event Action<VoiceChatCustomEvent> CustomEventReceived;

	private VoiceChatManager()
	{
	}

	public void AddEnterRoomCompletedListener(Action<VoiceChatEnterRoomResult> handler)
	{
		EnterRoomCompleted += handler;
	}

	public void RemoveEnterRoomCompletedListener(Action<VoiceChatEnterRoomResult> handler)
	{
		EnterRoomCompleted -= handler;
	}

	public void AddExitRoomCompletedListener(Action<VoiceChatExitRoomResult> handler)
	{
		ExitRoomCompleted += handler;
	}

	public void RemoveExitRoomCompletedListener(Action<VoiceChatExitRoomResult> handler)
	{
		ExitRoomCompleted -= handler;
	}

	public void AddRoomDisconnectedListener(Action<VoiceChatDisconnectResult> handler)
	{
		RoomDisconnected += handler;
	}

	public void RemoveRoomDisconnectedListener(Action<VoiceChatDisconnectResult> handler)
	{
		RoomDisconnected -= handler;
	}

	public void AddUserUpdatedListener(Action<VoiceChatUserUpdate> handler)
	{
		UserUpdated += handler;
	}

	public void RemoveUserUpdatedListener(Action<VoiceChatUserUpdate> handler)
	{
		UserUpdated -= handler;
	}

	public void AddCustomEventReceivedListener(Action<VoiceChatCustomEvent> handler)
	{
		CustomEventReceived += handler;
	}

	public void RemoveCustomEventReceivedListener(Action<VoiceChatCustomEvent> handler)
	{
		CustomEventReceived -= handler;
	}

	public void SetService(IVoiceChatService service)
	{
		if (_service != service)
		{
			UnbindServiceEvents(_service);
			_service = service;
			BindServiceEvents(_service);
		}
	}

	public int Init(VoiceChatInitOptions options)
	{
		if (options == null)
		{
			return -1;
		}
		if (_service == null)
		{
			SetService(new GmeVoiceChatService());
		}
		int num = _service.Init(options);
		IsInitialized = num == 0;
		if (IsInitialized)
		{
			RegisterUpdate();
		}
		else
		{
			UnregisterUpdate();
		}
		cachedMicEnable = -1;
		return num;
	}

	public int InitWithParams(string sdkAppId, string openId, int appScene, bool enableSpeakerOnEnter, bool enableMicOnEnter)
	{
		VoiceChatInitOptions options = new VoiceChatInitOptions
		{
			SdkAppId = sdkAppId,
			OpenId = openId,
			AppScene = (VoiceChatAppScene)appScene,
			EnableSpeakerOnEnter = enableSpeakerOnEnter,
			EnableMicOnEnter = enableMicOnEnter
		};
		return Init(options);
	}

	public int Uninit()
	{
		if (_service == null)
		{
			IsInitialized = false;
			UnregisterUpdate();
			ClearRoomState(_joined);
			ClearRoomState(_pendingEnter);
			ClearRoomState(_pendingExit);
			_isWaitingExitCallback = false;
			_isSwitchingRoom = false;
			_shouldUninitAfterExit = false;
			IsRoomOpPending = false;
			cachedMicEnable = -1;
			return 0;
		}
		Poll();
		int result = _service.Uninit();
		IsInitialized = false;
		UnregisterUpdate();
		ClearRoomState(_joined);
		ClearRoomState(_pendingEnter);
		ClearRoomState(_pendingExit);
		_isWaitingExitCallback = false;
		_isSwitchingRoom = false;
		_shouldUninitAfterExit = false;
		IsRoomOpPending = false;
		return result;
	}

	public void ResetForReload()
	{
		try
		{
			Uninit();
		}
		catch (Exception ex)
		{
			Log.Error("[VoiceChat] ResetForReload uninit exception: " + ex.Message);
		}
		this.EnterRoomCompleted = null;
		this.ExitRoomCompleted = null;
		this.RoomDisconnected = null;
		this.UserUpdated = null;
		this.CustomEventReceived = null;
		Log.Info("[VoiceChat] ResetForReload");
	}

	public int Poll()
	{
		if (_service == null)
		{
			return -1;
		}
		return _service.Poll();
	}

	private void Tick()
	{
		Poll();
	}

	public int EnterRoom(string chatRoomId, string voiceRoomId, int roomType, string userSig)
	{
		Log.Info("[VoiceChat] EnterRoom request, chatRoomId=" + (chatRoomId ?? string.Empty) + ", voiceRoomId=" + (voiceRoomId ?? string.Empty));
		if (_service == null)
		{
			return -1;
		}
		if (string.IsNullOrEmpty(chatRoomId) || string.IsNullOrEmpty(voiceRoomId))
		{
			return -2;
		}
		if (string.Equals(_joined.ChatRoomId, chatRoomId, StringComparison.Ordinal) && string.Equals(_joined.VoiceRoomId, voiceRoomId, StringComparison.Ordinal) && _service.IsRoomEntered())
		{
			return 0;
		}
		bool flag = !string.IsNullOrEmpty(_joined.VoiceRoomId) && !string.Equals(_joined.VoiceRoomId, voiceRoomId, StringComparison.Ordinal);
		if (flag)
		{
			int num = ExitRoom(uninitService: false);
			if (num != 0)
			{
				return num;
			}
		}
		_isSwitchingRoom = flag;
		SetRoomState(_pendingEnter, chatRoomId, voiceRoomId);
		IsRoomOpPending = true;
		int num2 = _service.EnterRoom(voiceRoomId, (VoiceChatRoomType)roomType, userSig);
		if (num2 != 0)
		{
			ClearRoomState(_pendingEnter);
			_isSwitchingRoom = false;
			IsRoomOpPending = false;
		}
		return num2;
	}

	public int EnterRoom(string voiceRoomId, int roomType, string userSig)
	{
		return EnterRoom(voiceRoomId, voiceRoomId, roomType, userSig);
	}

	public int ExitRoom(bool uninitService)
	{
		Log.Info($"[VoiceChat] ExitRoom request, chatRoomId={_joined.ChatRoomId}, voiceRoomId={_joined.VoiceRoomId}, uninitService={uninitService}");
		if (_service == null)
		{
			return -1;
		}
		IsRoomOpPending = true;
		int num = _service.ExitRoom();
		if (num != 0)
		{
			IsRoomOpPending = false;
			return num;
		}
		ClearRoomState(_pendingExit);
		if (!IsRoomStateEmpty(_joined))
		{
			CopyRoomState(_joined, _pendingExit);
		}
		_isWaitingExitCallback = true;
		_shouldUninitAfterExit = uninitService;
		if (uninitService)
		{
			VoiceChatExitRoomResult result = new VoiceChatExitRoomResult
			{
				Result = 0,
				ErrorInfo = "LocalExitFallback"
			};
			DispatchExitRoomCompleted(result);
		}
		return num;
	}

	public int ExitRoom()
	{
		return ExitRoom(uninitService: true);
	}

	public bool IsRoomEntered()
	{
		if (_service != null)
		{
			return _service.IsRoomEntered();
		}
		return false;
	}

	public int SetMicEnabled(bool enabled)
	{
		if (_service == null)
		{
			return -1;
		}
		if (GetMicListCount() <= 0)
		{
			UIUtils.ShowTips("voice_room_tips11", 3f);
			return -2;
		}
		if (GameEntry.Sdk.CheckSelfPermission("android.permission.RECORD_AUDIO") == "0")
		{
			return _service.SetMicEnabled(enabled);
		}
		GameEntry.Sdk.SendDataToNative("RequestRecordAudioPermission", 1003.ToString());
		cachedMicEnable = (enabled ? 1 : 0);
		return -1;
	}

	public static void OnPermissionCallback()
	{
		if (_instance != null && _instance._service != null && (cachedMicEnable == 0 || cachedMicEnable == 1))
		{
			_instance._service.SetMicEnabled(cachedMicEnable == 1);
		}
	}

	public bool IsMicEnabled()
	{
		if (_service != null)
		{
			return _service.IsMicEnabled();
		}
		return false;
	}

	public int GetMicListCount()
	{
		if (_service == null)
		{
			return -1;
		}
		return _service.GetMicListCount();
	}

	public int SetSpeakerEnabled(bool enabled)
	{
		if (_service == null)
		{
			return -1;
		}
		if (GetSpeakerListCount() <= 0)
		{
			UIUtils.ShowTips("voice_room_tips12", 3f);
			return -2;
		}
		return _service.SetSpeakerEnabled(enabled);
	}

	public bool IsSpeakerEnabled()
	{
		if (_service != null)
		{
			return _service.IsSpeakerEnabled();
		}
		return false;
	}

	public int GetSpeakerListCount()
	{
		if (_service == null)
		{
			return -1;
		}
		return _service.GetSpeakerListCount();
	}

	public int SetSpeakerVolumeByUserId(string userId, int volume)
	{
		if (_service == null)
		{
			return -1;
		}
		return _service.SetSpeakerVolumeByUserId(userId, volume);
	}

	public int StartPlayMusic(int soundId, string filePath, int loopCount)
	{
		if (_service == null)
		{
			return -1;
		}
		return _service.StartPlayMusic(soundId, filePath, loopCount);
	}

	public int StopPlayMusic(int soundId)
	{
		if (_service == null)
		{
			return -1;
		}
		return _service.StopPlayMusic(soundId);
	}

	public int IsMusicPlayEnd(int soundId)
	{
		if (_service == null)
		{
			return -1;
		}
		return _service.IsMusicPlayEnd(soundId);
	}

	public int CheckMicPermission()
	{
		if (_service == null)
		{
			return -1;
		}
		return (int)_service.CheckMicPermission();
	}

	public string GetSdkVersion()
	{
		if (_service != null)
		{
			return _service.GetSdkVersion();
		}
		return string.Empty;
	}

	public void OnApplicationFocus(bool hasFocus)
	{
		_service?.OnApplicationFocus(hasFocus);
	}

	private void BindServiceEvents(IVoiceChatService service)
	{
		if (service != null)
		{
			service.EnterRoomCompleted += OnEnterRoomCompleted;
			service.ExitRoomCompleted += OnExitRoomCompleted;
			service.RoomDisconnected += OnRoomDisconnected;
			service.UserUpdated += OnUserUpdated;
			service.CustomEventReceived += OnCustomEventReceived;
		}
	}

	private void UnbindServiceEvents(IVoiceChatService service)
	{
		if (service != null)
		{
			service.EnterRoomCompleted -= OnEnterRoomCompleted;
			service.ExitRoomCompleted -= OnExitRoomCompleted;
			service.RoomDisconnected -= OnRoomDisconnected;
			service.UserUpdated -= OnUserUpdated;
			service.CustomEventReceived -= OnCustomEventReceived;
		}
	}

	private void OnEnterRoomCompleted(VoiceChatEnterRoomResult result, bool enableMic, bool enableSpeaker)
	{
		if (result == null)
		{
			result = new VoiceChatEnterRoomResult();
		}
		Log.Info($"[VoiceChat] OnEnterRoomCompleted callback, result={result.Result}, chatRoomId={_pendingEnter.ChatRoomId}, voiceRoomId={_pendingEnter.VoiceRoomId}, errorInfo={result.ErrorInfo ?? string.Empty}");
		if (_isSwitchingRoom && _isWaitingExitCallback && !IsRoomStateEmpty(_pendingExit))
		{
			VoiceChatExitRoomResult result2 = new VoiceChatExitRoomResult
			{
				Result = 0,
				ErrorInfo = "SyntheticExitBeforeEnter"
			};
			DispatchExitRoomCompleted(result2);
		}
		result.ChatRoomId = _pendingEnter.ChatRoomId;
		result.VoiceRoomId = _pendingEnter.VoiceRoomId;
		if (result.Result == 0 || result.Result == 1001 || result.Result == 1003)
		{
			CopyRoomState(_pendingEnter, _joined);
		}
		else
		{
			ClearRoomState(_joined);
		}
		if (result.Result == 0)
		{
			if (enableMic)
			{
				SetMicEnabled(enableMic);
			}
			if (enableSpeaker)
			{
				SetSpeakerEnabled(enableSpeaker);
			}
		}
		ClearRoomState(_pendingEnter);
		_isSwitchingRoom = false;
		IsRoomOpPending = false;
		this.EnterRoomCompleted?.Invoke(result);
	}

	private void OnExitRoomCompleted(VoiceChatExitRoomResult result)
	{
		string text = ((result != null) ? (result.ChatRoomId ?? string.Empty) : string.Empty);
		string text2 = ((result != null) ? (result.VoiceRoomId ?? string.Empty) : string.Empty);
		Log.Info($"[VoiceChat] OnExitRoomCompleted callback, result={result?.Result ?? 0}, chatRoomId={text}, voiceRoomId={text2}, pendingChatRoomId={_pendingExit.ChatRoomId}, pendingVoiceRoomId={_pendingExit.VoiceRoomId}, errorInfo={((result != null) ? (result.ErrorInfo ?? string.Empty) : string.Empty)}");
		if (_isWaitingExitCallback || !IsRoomStateEmpty(_pendingExit))
		{
			DispatchExitRoomCompleted(result);
		}
	}

	private void OnRoomDisconnected(VoiceChatDisconnectResult result)
	{
		if (result == null)
		{
			result = new VoiceChatDisconnectResult();
		}
		result.ChatRoomId = _joined.ChatRoomId;
		result.VoiceRoomId = _joined.VoiceRoomId;
		ClearRoomState(_joined);
		ClearRoomState(_pendingEnter);
		ClearRoomState(_pendingExit);
		_isWaitingExitCallback = false;
		_isSwitchingRoom = false;
		_shouldUninitAfterExit = false;
		IsRoomOpPending = false;
		this.RoomDisconnected?.Invoke(result);
	}

	private void OnUserUpdated(VoiceChatUserUpdate update)
	{
		if (update == null)
		{
			update = new VoiceChatUserUpdate();
		}
		update.ChatRoomId = _joined.ChatRoomId;
		update.VoiceRoomId = _joined.VoiceRoomId;
		this.UserUpdated?.Invoke(update);
	}

	private void OnCustomEventReceived(VoiceChatCustomEvent evt)
	{
		if (evt == null)
		{
			evt = new VoiceChatCustomEvent();
		}
		evt.ChatRoomId = _joined.ChatRoomId;
		evt.VoiceRoomId = _joined.VoiceRoomId;
		this.CustomEventReceived?.Invoke(evt);
	}

	private void DispatchExitRoomCompleted(VoiceChatExitRoomResult result)
	{
		if (result == null)
		{
			result = new VoiceChatExitRoomResult();
		}
		if (string.IsNullOrEmpty(result.ChatRoomId))
		{
			result.ChatRoomId = _pendingExit.ChatRoomId;
		}
		if (string.IsNullOrEmpty(result.VoiceRoomId))
		{
			result.VoiceRoomId = _pendingExit.VoiceRoomId;
		}
		ClearRoomState(_joined);
		ClearRoomState(_pendingExit);
		_isWaitingExitCallback = false;
		IsRoomOpPending = false;
		bool shouldUninitAfterExit = _shouldUninitAfterExit;
		_shouldUninitAfterExit = false;
		this.ExitRoomCompleted?.Invoke(result);
		if (shouldUninitAfterExit)
		{
			Uninit();
		}
	}

	private void RegisterUpdate()
	{
		if (!_isUpdateRegistered)
		{
			Updater.AddUpdateCallback(Tick);
			_isUpdateRegistered = true;
		}
	}

	private void UnregisterUpdate()
	{
		if (_isUpdateRegistered)
		{
			Updater.RemoveUpdateCallback(Tick);
			_isUpdateRegistered = false;
		}
	}

	private static bool IsRoomStateEmpty(RoomState state)
	{
		if (state != null)
		{
			if (string.IsNullOrEmpty(state.ChatRoomId))
			{
				return string.IsNullOrEmpty(state.VoiceRoomId);
			}
			return false;
		}
		return true;
	}

	private static void SetRoomState(RoomState state, string chatRoomId, string voiceRoomId)
	{
		if (state != null)
		{
			state.ChatRoomId = chatRoomId ?? string.Empty;
			state.VoiceRoomId = voiceRoomId ?? string.Empty;
		}
	}

	private static void CopyRoomState(RoomState source, RoomState target)
	{
		if (target != null)
		{
			if (source == null)
			{
				ClearRoomState(target);
				return;
			}
			target.ChatRoomId = source.ChatRoomId;
			target.VoiceRoomId = source.VoiceRoomId;
		}
	}

	private static void ClearRoomState(RoomState state)
	{
		if (state != null)
		{
			state.ChatRoomId = string.Empty;
			state.VoiceRoomId = string.Empty;
			cachedMicEnable = -1;
		}
	}
}
