using System;
using AOT;

namespace GME;

public class GMECSharpEventCallback
{
	[MonoPInvokeCallback(typeof(NativeOnEventCallBack))]
	public static void OnEventCallBack(IntPtr nativeInstance, int event_type, string data)
	{
		TMGContext tMGContextForNativeInstance = TMGContext.GetTMGContextForNativeInstance(nativeInstance);
		switch (event_type)
		{
		case 1:
			HandleEnterRoomCallback(tMGContextForNativeInstance, event_type, data);
			break;
		case 2:
			HandleExitRoomCallback(tMGContextForNativeInstance, event_type, data);
			break;
		case 3:
			HandleRoomDisconnectCallback(tMGContextForNativeInstance, event_type, data);
			break;
		case 4:
			HandleUserInfoUpdateCallback(tMGContextForNativeInstance, event_type, data);
			break;
		case 30:
			HandleRecordCompletedCallback(tMGContextForNativeInstance, event_type, data);
			break;
		case 6000:
			HandleRoomManagerCallback(tMGContextForNativeInstance, event_type, data);
			break;
		case 1087:
			HandlePlayMusicStartCallback(tMGContextForNativeInstance, event_type, data);
			break;
		case 1088:
			HandlePlayMusicProgressCallback(tMGContextForNativeInstance, event_type, data);
			break;
		case 1089:
			HandlePlayMusicPauseCallback(tMGContextForNativeInstance, event_type, data);
			break;
		case 1090:
			HandlePlayMusicResumeCallback(tMGContextForNativeInstance, event_type, data);
			break;
		case 1091:
			HandlePlayMusicFinishCallback(tMGContextForNativeInstance, event_type, data);
			break;
		case 13:
			HandleSwitchRoomCallback(tMGContextForNativeInstance, event_type, data);
			break;
		case 23:
		case 24:
			HandleRoomSharingCallback(tMGContextForNativeInstance, event_type, data);
			break;
		case 1021:
			HandleNetworkQualityStatisticsCallback(tMGContextForNativeInstance, event_type, data);
			break;
		case 5000:
			HandlePTTRecordFirstAudioFrameCallback();
			break;
		case 5001:
			HandlePTTRecordCompleteCallback(event_type, data);
			break;
		case 5002:
			HandlePTTUploadCompleteCallback(event_type, data);
			break;
		case 5003:
			HandlePTTDownloadCompleteCallback(event_type, data);
			break;
		case 5004:
			HandlePTTPlayCompleteCallback(event_type, data);
			break;
		case 5005:
			HandlePTTSpeech2TextCompleteCallback(event_type, data);
			break;
		case 5007:
			HandlePTTStreamingRecognitionRunningCallback(event_type, data);
			break;
		case 5006:
			HandlePTTStreamingRecognitionCompleteCallback(event_type, data);
			break;
		case 5008:
			HandlePTTTextToSpeechCompleteCallback(event_type, data);
			break;
		case 5009:
			HandlePTTTranslateTextCompleteCallback(event_type, data);
			break;
		case 1092:
			HandleCutomDataCallback(tMGContextForNativeInstance, event_type, data);
			break;
		default:
			HandleCommonCallback(tMGContextForNativeInstance, event_type, 0, data);
			break;
		}
	}

	private static void HandleEnterRoomCallback(TMGContext tmgContext, int eventType, string data)
	{
		EventCallbackInfo eventCallbackInfo = JsonParser.FromJson<EventCallbackInfo>(data);
		if (eventCallbackInfo != null)
		{
			tmgContext?.InvokeEnterRoomEvent(eventCallbackInfo);
		}
	}

	private static void HandleExitRoomCallback(TMGContext tmgContext, int eventType, string data)
	{
		tmgContext?.InvokeExitRoomEvent();
	}

	private static void HandleRoomDisconnectCallback(TMGContext tmgContext, int eventType, string data)
	{
		EventCallbackInfo eventCallbackInfo = JsonParser.FromJson<EventCallbackInfo>(data);
		if (eventCallbackInfo != null)
		{
			tmgContext?.InvokeRoomDisconnectEvent(eventCallbackInfo);
		}
	}

	private static void HandleUserInfoUpdateCallback(TMGContext tmgContext, int eventType, string data)
	{
		UserInfoUpdateCallbackInfo userInfoUpdateCallbackInfo = JsonParser.FromJson<UserInfoUpdateCallbackInfo>(data);
		if (userInfoUpdateCallbackInfo != null)
		{
			tmgContext?.InvokeUserInfoUpdateEvent(userInfoUpdateCallbackInfo);
		}
	}

	private static void HandleRoomManagerCallback(TMGContext tmgContext, int eventType, string data)
	{
		RoomManagerOperateCallbackInfo roomManagerOperateCallbackInfo = JsonParser.FromJson<RoomManagerOperateCallbackInfo>(data);
		if (roomManagerOperateCallbackInfo != null)
		{
			HandleCommonCallback(tmgContext, eventType, roomManagerOperateCallbackInfo.operate_type, data);
		}
	}

	private static void HandlePlayMusicStartCallback(TMGContext tmgContext, int eventType, string data)
	{
		PlayMusicCallbackInfo playMusicCallbackInfo = JsonParser.FromJson<PlayMusicCallbackInfo>(data);
		if (playMusicCallbackInfo != null)
		{
			tmgContext?.GetAudioEffectCtrlInner()?.InvokePlayMusicStartEvent(playMusicCallbackInfo);
		}
	}

	private static void HandlePlayMusicProgressCallback(TMGContext tmgContext, int eventType, string data)
	{
		PlayMusicProcessCallbackInfo playMusicProcessCallbackInfo = JsonParser.FromJson<PlayMusicProcessCallbackInfo>(data);
		if (playMusicProcessCallbackInfo != null)
		{
			tmgContext?.GetAudioEffectCtrlInner()?.InvokePlayMusicProcessEvent(playMusicProcessCallbackInfo);
		}
	}

	private static void HandlePlayMusicPauseCallback(TMGContext tmgContext, int eventType, string data)
	{
		PlayMusicCallbackInfo playMusicCallbackInfo = JsonParser.FromJson<PlayMusicCallbackInfo>(data);
		if (playMusicCallbackInfo != null)
		{
			tmgContext?.GetAudioEffectCtrlInner()?.InvokePlayMusicPauseEvent(playMusicCallbackInfo);
		}
	}

	private static void HandlePlayMusicResumeCallback(TMGContext tmgContext, int eventType, string data)
	{
		PlayMusicCallbackInfo playMusicCallbackInfo = JsonParser.FromJson<PlayMusicCallbackInfo>(data);
		if (playMusicCallbackInfo != null)
		{
			tmgContext?.GetAudioEffectCtrlInner()?.InvokePlayMusicResumeEvent(playMusicCallbackInfo);
		}
	}

	private static void HandlePlayMusicFinishCallback(TMGContext tmgContext, int eventType, string data)
	{
		PlayMusicFinishCallbackInfo playMusicFinishCallbackInfo = JsonParser.FromJson<PlayMusicFinishCallbackInfo>(data);
		if (playMusicFinishCallbackInfo != null)
		{
			tmgContext?.GetAudioEffectCtrlInner()?.InvokePlayMusicFinishEvent(playMusicFinishCallbackInfo);
		}
	}

	private static void HandleRecordCompletedCallback(TMGContext tmgContext, int eventType, string data)
	{
		AudioRecordCompleteCallbackInfo audioRecordCompleteCallbackInfo = JsonParser.FromJson<AudioRecordCompleteCallbackInfo>(data);
		if (audioRecordCompleteCallbackInfo != null)
		{
			tmgContext?.GetAudioEffectCtrlInner()?.InvokeRecordCompleteEvent(audioRecordCompleteCallbackInfo);
		}
	}

	private static void HandleSwitchRoomCallback(TMGContext tmgContext, int eventType, string data)
	{
		EventCallbackInfo eventCallbackInfo = JsonParser.FromJson<EventCallbackInfo>(data);
		if (eventCallbackInfo != null)
		{
			HandleCommonCallback(tmgContext, eventType, eventCallbackInfo.result, eventCallbackInfo.error_info);
		}
	}

	private static void HandleRoomSharingCallback(TMGContext tmgContext, int eventType, string data)
	{
		EventCallbackInfo eventCallbackInfo = JsonParser.FromJson<EventCallbackInfo>(data);
		if (eventCallbackInfo != null)
		{
			HandleCommonCallback(tmgContext, eventType, eventCallbackInfo.result, eventCallbackInfo.error_info);
		}
	}

	private static void HandleNetworkQualityStatisticsCallback(TMGContext tmgContext, int eventType, string data)
	{
		NetworkQualityCallbackInfo networkQualityCallbackInfo = JsonParser.FromJson<NetworkQualityCallbackInfo>(data);
		if (networkQualityCallbackInfo != null)
		{
			tmgContext?.InvokeNetworkQualityStatisticsEvent(networkQualityCallbackInfo);
		}
	}

	private static void HandlePTTRecordFirstAudioFrameCallback()
	{
		QAVPTT.GetInstance().InvokeRecordedFirstAudioFrameEvent();
	}

	private static void HandlePTTRecordCompleteCallback(int event_type, string data)
	{
		PTTRecordCompleteCallbackInfo pTTRecordCompleteCallbackInfo = JsonParser.FromJson<PTTRecordCompleteCallbackInfo>(data);
		if (pTTRecordCompleteCallbackInfo != null)
		{
			QAVPTT.GetInstance().InvokePTTRecordCompleteEvent(pTTRecordCompleteCallbackInfo);
		}
	}

	private static void HandlePTTUploadCompleteCallback(int event_type, string data)
	{
		PTTUploadCompleteCallbackInfo pTTUploadCompleteCallbackInfo = JsonParser.FromJson<PTTUploadCompleteCallbackInfo>(data);
		if (pTTUploadCompleteCallbackInfo != null)
		{
			QAVPTT.GetInstance().InvokePTTUploadCompleteEvent(pTTUploadCompleteCallbackInfo);
		}
	}

	private static void HandlePTTDownloadCompleteCallback(int event_type, string data)
	{
		PTTDownloadCompleteCallbackInfo pTTDownloadCompleteCallbackInfo = JsonParser.FromJson<PTTDownloadCompleteCallbackInfo>(data);
		if (pTTDownloadCompleteCallbackInfo != null)
		{
			QAVPTT.GetInstance().InvokePTTDownloadCompleteEvent(pTTDownloadCompleteCallbackInfo);
		}
	}

	private static void HandlePTTPlayCompleteCallback(int event_type, string data)
	{
		PTTPlayCompleteCallbackInfo pTTPlayCompleteCallbackInfo = JsonParser.FromJson<PTTPlayCompleteCallbackInfo>(data);
		if (pTTPlayCompleteCallbackInfo != null)
		{
			QAVPTT.GetInstance().InvokePTTPlayCompleteEvent(pTTPlayCompleteCallbackInfo);
		}
	}

	private static void HandlePTTSpeech2TextCompleteCallback(int event_type, string data)
	{
		PTTSpeech2TextCallbackInfo pTTSpeech2TextCallbackInfo = JsonParser.FromJson<PTTSpeech2TextCallbackInfo>(data);
		if (pTTSpeech2TextCallbackInfo != null)
		{
			QAVPTT.GetInstance().InvokePTTSpeechToTextCompleteEvent(pTTSpeech2TextCallbackInfo);
		}
	}

	private static void HandlePTTStreamingRecognitionRunningCallback(int event_type, string data)
	{
		PTTStreamingRecognitionRunningCallbackInfo pTTStreamingRecognitionRunningCallbackInfo = JsonParser.FromJson<PTTStreamingRecognitionRunningCallbackInfo>(data);
		if (pTTStreamingRecognitionRunningCallbackInfo != null)
		{
			QAVPTT.GetInstance().InvokePTTStreamingSpeechRunningEvent(pTTStreamingRecognitionRunningCallbackInfo);
		}
	}

	private static void HandlePTTStreamingRecognitionCompleteCallback(int event_type, string data)
	{
		PTTStreamingRecognitionCompleteCallbackInfo pTTStreamingRecognitionCompleteCallbackInfo = JsonParser.FromJson<PTTStreamingRecognitionCompleteCallbackInfo>(data);
		if (pTTStreamingRecognitionCompleteCallbackInfo != null)
		{
			QAVPTT.GetInstance().InvokePTTStreamingSpeechCompleteEvent(pTTStreamingRecognitionCompleteCallbackInfo);
		}
	}

	private static void HandlePTTTranslateTextCompleteCallback(int event_type, string data)
	{
		PTTTranslateTextCompleteCallbackInfo pTTTranslateTextCompleteCallbackInfo = JsonParser.FromJson<PTTTranslateTextCompleteCallbackInfo>(data);
		if (pTTTranslateTextCompleteCallbackInfo != null)
		{
			QAVPTT.GetInstance().InvokePTTTranslateTextCompleteEvent(pTTTranslateTextCompleteCallbackInfo.result, data);
		}
	}

	private static void HandlePTTTextToSpeechCompleteCallback(int event_type, string data)
	{
		PTTTextToSpeechCompleteCallbackInfo pTTTextToSpeechCompleteCallbackInfo = JsonParser.FromJson<PTTTextToSpeechCompleteCallbackInfo>(data);
		if (pTTTextToSpeechCompleteCallbackInfo != null)
		{
			QAVPTT.GetInstance().InvokePTTTextToSpeechCompleteEvent(pTTTextToSpeechCompleteCallbackInfo.result, pTTTextToSpeechCompleteCallbackInfo.serial_number, pTTTextToSpeechCompleteCallbackInfo.file_id);
		}
	}

	private static void HandleCutomDataCallback(TMGContext tmgContext, int event_type, string data)
	{
		CustomDataCallbackInfo customDataCallbackInfo = JsonParser.FromJson<CustomDataCallbackInfo>(data);
		if (customDataCallbackInfo != null)
		{
			tmgContext?.InvokeCommonEvent(event_type, customDataCallbackInfo.sub_type, data);
		}
	}

	private static void HandleCommonCallback(TMGContext tmgContext, int eventType, int subEventType, string data)
	{
		tmgContext?.InvokeCommonEvent(eventType, subEventType, data);
	}
}
