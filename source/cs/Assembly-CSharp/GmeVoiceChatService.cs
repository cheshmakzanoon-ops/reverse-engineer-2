using System;
using GME;
using UnityEngine;

public class GmeVoiceChatService : IVoiceChatService
{
	private ITMGContext _context;

	private ITMGAudioCtrl _audioCtrl;

	private ITMGAudioEffectCtrl _audioEffectCtrl;

	private VoiceChatInitOptions _options;

	private bool _inited;

	public event Action<VoiceChatEnterRoomResult, bool, bool> EnterRoomCompleted;

	public event Action<VoiceChatExitRoomResult> ExitRoomCompleted;

	public event Action<VoiceChatDisconnectResult> RoomDisconnected;

	public event Action<VoiceChatUserUpdate> UserUpdated;

	public event Action<VoiceChatCustomEvent> CustomEventReceived;

	public int Init(VoiceChatInitOptions options)
	{
		if (options == null || string.IsNullOrEmpty(options.SdkAppId) || string.IsNullOrEmpty(options.OpenId))
		{
			Debug.LogError("[VoiceChat] Init failed, options/sdkAppId/openId invalid.");
			return -1;
		}
		if (_inited)
		{
			Uninit();
		}
		_options = options;
		_context = ITMGContext.GetInstance();
		if (_context == null)
		{
			Debug.LogError("[VoiceChat] Init failed, ITMGContext.GetInstance() is null.");
			return -2;
		}
		BindContextEvents();
		int num = _context.Init(options.SdkAppId, options.OpenId);
		if (num != 0)
		{
			Debug.LogError($"[VoiceChat] GME Init failed, ret={num}");
			UnbindContextEvents();
			_context = null;
			_audioCtrl = null;
			_audioEffectCtrl = null;
			_options = null;
			return num;
		}
		num = _context.SetScene(ToGmeScene(options.AppScene));
		if (num != 0)
		{
			Debug.LogWarning($"[VoiceChat] SetScene failed, ret={num}, scene={options.AppScene}");
		}
		_audioCtrl = _context.GetAudioCtrl();
		_audioEffectCtrl = _context.GetAudioEffectCtrl();
		_inited = true;
		return 0;
	}

	public int Uninit()
	{
		if (_context == null)
		{
			_inited = false;
			_audioCtrl = null;
			_audioEffectCtrl = null;
			_options = null;
			return 0;
		}
		UnbindContextEvents();
		int result = _context.Uninit();
		_context = null;
		_audioCtrl = null;
		_audioEffectCtrl = null;
		_options = null;
		_inited = false;
		return result;
	}

	public int Poll()
	{
		if (_context == null || !_inited)
		{
			return -1;
		}
		return _context.Poll();
	}

	public int EnterRoom(string roomId, VoiceChatRoomType roomType, string userSig)
	{
		if (_context == null || !_inited)
		{
			Debug.LogError("[VoiceChat] EnterRoom failed, service not initialized.");
			return -1;
		}
		if (string.IsNullOrEmpty(roomId) || string.IsNullOrEmpty(userSig))
		{
			Debug.LogError("[VoiceChat] EnterRoom failed, roomId/userSig invalid.");
			return -2;
		}
		return _context.EnterRoom(roomId, ToGmeRoomType(roomType), userSig);
	}

	private static string GetAuthBuffer(string sdkAppID, string roomID, string userID, string authKey)
	{
		return QAVAuthBuffer.GenAuthBuffer(int.Parse(sdkAppID), roomID, userID, authKey);
	}

	public int ExitRoom()
	{
		if (_context == null || !_inited)
		{
			return -1;
		}
		return _context.ExitRoom();
	}

	public bool IsRoomEntered()
	{
		if (_context == null || !_inited)
		{
			return false;
		}
		return _context.IsRoomEntered();
	}

	public int SetMicEnabled(bool enabled)
	{
		if (_audioCtrl == null || !_inited)
		{
			return -1;
		}
		return _audioCtrl.EnableMic(enabled);
	}

	public bool IsMicEnabled()
	{
		if (_audioCtrl == null || !_inited)
		{
			return false;
		}
		return _audioCtrl.GetMicState() == 1;
	}

	public int GetMicListCount()
	{
		return 1;
	}

	public int SetSpeakerEnabled(bool enabled)
	{
		if (_audioCtrl == null || !_inited)
		{
			return -1;
		}
		return _audioCtrl.EnableSpeaker(enabled);
	}

	public bool IsSpeakerEnabled()
	{
		if (_audioCtrl == null || !_inited)
		{
			return false;
		}
		return _audioCtrl.GetSpeakerState() == 1;
	}

	public int GetSpeakerListCount()
	{
		return 1;
	}

	public int SetSpeakerVolumeByUserId(string userId, int volume)
	{
		if (_audioCtrl == null || !_inited)
		{
			return -1;
		}
		if (string.IsNullOrEmpty(userId))
		{
			return -2;
		}
		int volume2 = Mathf.Clamp(volume, 0, 200);
		return _audioCtrl.SetSpeakerVolumeByUserID(userId, volume2);
	}

	public int StartPlayMusic(int soundId, string filePath, int loopCount)
	{
		if (_audioEffectCtrl == null || !_inited)
		{
			return -1;
		}
		if (string.IsNullOrEmpty(filePath))
		{
			return -2;
		}
		return _audioEffectCtrl.StartPlayMusic(soundId, filePath, loopCount);
	}

	public int StopPlayMusic(int soundId)
	{
		if (_audioEffectCtrl == null || !_inited)
		{
			return -1;
		}
		return _audioEffectCtrl.StopPlayMusic(soundId);
	}

	public int IsMusicPlayEnd(int soundId)
	{
		if (_audioEffectCtrl == null || !_inited)
		{
			return -1;
		}
		return _audioEffectCtrl.IsMusicPlayEnd(soundId);
	}

	public VoiceChatMicPermission CheckMicPermission()
	{
		if (_context == null || !_inited)
		{
			return VoiceChatMicPermission.Unknown;
		}
		return _context.CheckMicPermission() switch
		{
			ITMG_MIC_PERMISSION.ITMG_PERMISSION_GRANTED => VoiceChatMicPermission.Granted, 
			ITMG_MIC_PERMISSION.ITMG_PERMISSION_Denied => VoiceChatMicPermission.Denied, 
			ITMG_MIC_PERMISSION.ITMG_PERMISSION_NotDetermined => VoiceChatMicPermission.NotDetermined, 
			_ => VoiceChatMicPermission.Unknown, 
		};
	}

	public string GetSdkVersion()
	{
		if (_context == null || !_inited)
		{
			return string.Empty;
		}
		return _context.GetSDKVersion();
	}

	public void OnApplicationFocus(bool hasFocus)
	{
		if (_context != null && _inited)
		{
			Debug.Log($"[VoiceChat] OnApplicationFocus {hasFocus}");
			if (hasFocus)
			{
				_context.Resume();
			}
			else
			{
				_context.Pause();
			}
		}
	}

	public string GenAuthBuffer(string roomId, string openId)
	{
		if (string.IsNullOrEmpty(roomId) || string.IsNullOrEmpty(openId))
		{
			return string.Empty;
		}
		if (_options == null || string.IsNullOrEmpty(_options.SdkAppId))
		{
			Debug.LogError("[VoiceChat] GenAuthBuffer failed, sdkAppId invalid.");
			return string.Empty;
		}
		return string.Empty;
	}

	private void BindContextEvents()
	{
		if (_context != null)
		{
			_context.OnEnterRoomCompleteEvent += HandleEnterRoomComplete;
			_context.OnExitRoomCompleteEvent += HandleExitRoomComplete;
			_context.OnRoomDisconnectEvent += HandleRoomDisconnect;
			_context.OnEndpointsUpdateInfoEvent += HandleEndpointsUpdateInfo;
			_context.onEventCallBack += HandleOnEventCallback;
		}
	}

	private void UnbindContextEvents()
	{
		if (_context != null)
		{
			_context.OnEnterRoomCompleteEvent -= HandleEnterRoomComplete;
			_context.OnExitRoomCompleteEvent -= HandleExitRoomComplete;
			_context.OnRoomDisconnectEvent -= HandleRoomDisconnect;
			_context.OnEndpointsUpdateInfoEvent -= HandleEndpointsUpdateInfo;
			_context.onEventCallBack -= HandleOnEventCallback;
		}
	}

	private void HandleEnterRoomComplete(int result, string errorInfo)
	{
		this.EnterRoomCompleted?.Invoke(new VoiceChatEnterRoomResult
		{
			Result = result,
			ErrorInfo = errorInfo
		}, _options.EnableMicOnEnter, _options.EnableSpeakerOnEnter);
	}

	private void HandleExitRoomComplete()
	{
		if (_audioCtrl != null)
		{
			_audioCtrl.EnableMic(enable: false);
		}
		this.ExitRoomCompleted?.Invoke(new VoiceChatExitRoomResult
		{
			Result = 0,
			ErrorInfo = string.Empty
		});
	}

	private void HandleRoomDisconnect(int result, string errorInfo)
	{
		this.RoomDisconnected?.Invoke(new VoiceChatDisconnectResult
		{
			Result = result,
			ErrorInfo = errorInfo
		});
	}

	private void HandleEndpointsUpdateInfo(int eventId, int count, string[] userIdList)
	{
		VoiceChatUserUpdate obj = new VoiceChatUserUpdate
		{
			EventType = ToUserEventType(eventId),
			UserIds = (userIdList ?? Array.Empty<string>())
		};
		this.UserUpdated?.Invoke(obj);
	}

	private void HandleOnEventCallback(int type, int subType, string data)
	{
		this.CustomEventReceived?.Invoke(new VoiceChatCustomEvent
		{
			Type = type,
			SubType = subType,
			Data = data
		});
	}

	private static ITMGRoomType ToGmeRoomType(VoiceChatRoomType roomType)
	{
		return roomType switch
		{
			VoiceChatRoomType.Fluency => ITMGRoomType.ITMG_ROOM_TYPE_FLUENCY, 
			VoiceChatRoomType.Standard => ITMGRoomType.ITMG_ROOM_TYPE_STANDARD, 
			VoiceChatRoomType.HighQuality => ITMGRoomType.ITMG_ROOM_TYPE_HIGHQUALITY, 
			_ => ITMGRoomType.ITMG_ROOM_TYPE_FLUENCY, 
		};
	}

	private static ITMG_APP_SCENE ToGmeScene(VoiceChatAppScene scene)
	{
		if (scene != VoiceChatAppScene.Rtc && scene == VoiceChatAppScene.Live)
		{
			return ITMG_APP_SCENE.ITMG_APP_SCENE_LIVE;
		}
		return ITMG_APP_SCENE.ITMG_APP_SCENE_RTC;
	}

	private static VoiceChatUserEventType ToUserEventType(int eventId)
	{
		return (ITMG_EVENT_ID_USER_UPDATE)eventId switch
		{
			ITMG_EVENT_ID_USER_UPDATE.ITMG_EVENT_ID_USER_ENTER => VoiceChatUserEventType.UserEnter, 
			ITMG_EVENT_ID_USER_UPDATE.ITMG_EVENT_ID_USER_EXIT => VoiceChatUserEventType.UserExit, 
			ITMG_EVENT_ID_USER_UPDATE.ITMG_EVENT_ID_USER_MIC_OPENED => VoiceChatUserEventType.UserMicOpened, 
			ITMG_EVENT_ID_USER_UPDATE.ITMG_EVENT_ID_USER_MIC_CLOSED => VoiceChatUserEventType.UserMicClosed, 
			ITMG_EVENT_ID_USER_UPDATE.ITMG_EVENT_ID_USER_HAS_AUDIO => VoiceChatUserEventType.UserHasAudio, 
			ITMG_EVENT_ID_USER_UPDATE.ITMG_EVENT_ID_USER_NO_AUDIO => VoiceChatUserEventType.UserNoAudio, 
			_ => VoiceChatUserEventType.Unknown, 
		};
	}
}
