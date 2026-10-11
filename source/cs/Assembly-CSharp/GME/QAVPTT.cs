using System;

namespace GME;

public class QAVPTT : ITMGPTT
{
	private IntPtr nativeInstance = IntPtr.Zero;

	private static QAVPTT sInstance;

	private static readonly object sLock = new object();

	public override event QAVRecordFirstAudioFrameCallback OnRecordFirstAudioFrame;

	public override event QAVRecordFileCompleteCallback OnRecordFileComplete;

	public override event QAVUploadFileCompleteCallback OnUploadFileComplete;

	public override event QAVDownloadFileCompleteCallback OnDownloadFileComplete;

	public override event QAVPlayFileCompleteCallback OnPlayFileComplete;

	public override event QAVSpeechToTextCallback OnSpeechToTextComplete;

	public override event QAVStreamingRecognitionCompleteCallback OnStreamingSpeechComplete;

	public override event QAVStreamingRecognitionRunningCallback OnStreamingSpeechisRunning;

	public override event QAVDownloadFileWithAuditCompleteCallback OnDownloadFileAuditComplete;

	public override event QAVSpeechToTextWithAuditCallback OnSpeechToTextAuditComplete;

	public override event QAVSpeechToTextWithTargetTextCallback OnSpeechToTextTargetTextComplete;

	public override event QAVTranslateTextCallback OnTranslateTextComplete;

	public override event QAVTextToSpeechCallback OnTextToSpeechComplete;

	public new static QAVPTT GetInstance()
	{
		if (sInstance == null)
		{
			sInstance = new QAVPTT();
		}
		return sInstance;
	}

	public override int ApplyPTTAuthbuffer(string authBuffer)
	{
		return TMGPTTNative.GMEUnity_ApplyPTTAuthbuffer(nativeInstance, authBuffer);
	}

	public override int SetMaxMessageLength(int msTime)
	{
		return TMGPTTNative.GMEUnity_SetMaxMessageLength(nativeInstance, msTime);
	}

	public override int SetPTTSourceLanguage(string sourceLanguage)
	{
		return TMGPTTNative.GMEUnity_SetPTTSourceLanguage(nativeInstance, sourceLanguage);
	}

	public override int StartRecording(string filePath)
	{
		return TMGPTTNative.GMEUnity_StartRecording(nativeInstance, filePath);
	}

	public override int StopRecording()
	{
		return TMGPTTNative.GMEUnity_StopRecording(nativeInstance);
	}

	public override int CancelRecording()
	{
		return TMGPTTNative.GMEUnity_CancelRecording(nativeInstance);
	}

	public override int UploadRecordedFile(string filePath)
	{
		return TMGPTTNative.GMEUnity_UploadRecordedFile(nativeInstance, filePath);
	}

	public override int DownloadRecordedFile(string fileID, string downloadFilePath)
	{
		return TMGPTTNative.GMEUnity_DownloadRecordedFile(nativeInstance, fileID, downloadFilePath);
	}

	public override int PlayRecordedFile(string filePath)
	{
		return TMGPTTNative.GMEUnity_StartPlayFile(nativeInstance, filePath, ITMG_VOICE_TYPE.ITMG_VOICE_TYPE_ORIGINAL_SOUND);
	}

	public override int PlayRecordedFile(string filePath, ITMG_VOICE_TYPE voiceType)
	{
		return TMGPTTNative.GMEUnity_StartPlayFile(nativeInstance, filePath, voiceType);
	}

	public override int StopPlayFile()
	{
		return TMGPTTNative.GMEUnity_StopPlayFile(nativeInstance);
	}

	public override int GetFileSize(string filePath)
	{
		return TMGPTTNative.GMEUnity_GetFileSize(nativeInstance, filePath);
	}

	public override int GetVoiceFileDuration(string filePath)
	{
		return TMGPTTNative.GMEUnity_GetVoiceFileDuration(nativeInstance, filePath);
	}

	public override int SpeechToText(string fileID)
	{
		return TMGPTTNative.GMEUnity_SpeechToText(nativeInstance, fileID, "cmn-Hans-CN", "cmn-Hans-CN");
	}

	public override int SpeechToText(string fileID, string speechLanguage)
	{
		return TMGPTTNative.GMEUnity_SpeechToText(nativeInstance, fileID, speechLanguage, "cmn-Hans-CN");
	}

	public override int SpeechToText(string fileID, string speechLanguage, string translatelanguage)
	{
		return TMGPTTNative.GMEUnity_SpeechToText(nativeInstance, fileID, speechLanguage, translatelanguage);
	}

	public override int StartRecordingWithStreamingRecognition(string filePath)
	{
		return TMGPTTNative.GMEUnity_StartRecordingWithStreamingRecognition(nativeInstance, filePath, "cmn-Hans-CN", "cmn-Hans-CN");
	}

	public override int StartRecordingWithStreamingRecognition(string filePath, string speechLanguage)
	{
		return TMGPTTNative.GMEUnity_StartRecordingWithStreamingRecognition(nativeInstance, filePath, speechLanguage, speechLanguage);
	}

	public override int StartRecordingWithStreamingRecognition(string filePath, string speechLanguage, string translatelanguage)
	{
		return TMGPTTNative.GMEUnity_StartRecordingWithStreamingRecognition(nativeInstance, filePath, speechLanguage, translatelanguage);
	}

	public override int TranslateText(string text, string sourceLanguage, string translatelanguage)
	{
		return TMGPTTNative.GMEUnity_TranslateText(nativeInstance, text, sourceLanguage, translatelanguage);
	}

	public override int TextToSpeech(int serialNumber, string text, string voiceName, string languageCode, float speakingRate)
	{
		return TMGPTTNative.GMEUnity_TextToSpeech(nativeInstance, serialNumber, text, voiceName, languageCode, speakingRate);
	}

	public void InvokeRecordedFirstAudioFrameEvent()
	{
		OnRecordFirstAudioFrame?.Invoke();
	}

	public void InvokePTTRecordCompleteEvent(PTTRecordCompleteCallbackInfo callbackInfo)
	{
		if (callbackInfo != null)
		{
			OnRecordFileComplete?.Invoke(callbackInfo.result, callbackInfo.file_path, callbackInfo.file_size, callbackInfo.duration);
		}
	}

	public void InvokePTTUploadCompleteEvent(PTTUploadCompleteCallbackInfo callbackInfo)
	{
		if (callbackInfo != null)
		{
			OnUploadFileComplete?.Invoke(callbackInfo.result, callbackInfo.file_path, callbackInfo.file_id, callbackInfo.audit_result);
		}
	}

	public void InvokePTTDownloadCompleteEvent(PTTDownloadCompleteCallbackInfo callbackInfo)
	{
		if (callbackInfo != null)
		{
			if (OnDownloadFileComplete != null)
			{
				OnDownloadFileComplete?.Invoke(callbackInfo.result, callbackInfo.file_path, callbackInfo.file_id);
			}
			else if (OnDownloadFileAuditComplete != null)
			{
				OnDownloadFileAuditComplete?.Invoke(callbackInfo.result, callbackInfo.file_path, callbackInfo.file_id, callbackInfo.audit_result);
			}
		}
	}

	public void InvokePTTPlayCompleteEvent(PTTPlayCompleteCallbackInfo callbackInfo)
	{
		if (callbackInfo != null)
		{
			OnPlayFileComplete?.Invoke(callbackInfo.result, callbackInfo.file_path);
		}
	}

	public void InvokePTTSpeechToTextCompleteEvent(PTTSpeech2TextCallbackInfo callbackInfo)
	{
		if (callbackInfo != null)
		{
			if (OnSpeechToTextAuditComplete != null)
			{
				OnSpeechToTextAuditComplete?.Invoke(callbackInfo.result, callbackInfo.file_id, callbackInfo.text, callbackInfo.audit_result);
			}
			else if (OnSpeechToTextComplete != null)
			{
				OnSpeechToTextComplete?.Invoke(callbackInfo.result, callbackInfo.file_id, callbackInfo.text);
			}
			else if (OnSpeechToTextTargetTextComplete != null)
			{
				OnSpeechToTextTargetTextComplete?.Invoke(callbackInfo.result, callbackInfo.file_id, callbackInfo.text, callbackInfo.audit_result, callbackInfo.target_text);
			}
		}
	}

	public void InvokePTTStreamingSpeechRunningEvent(PTTStreamingRecognitionRunningCallbackInfo callbackInfo)
	{
		if (callbackInfo != null)
		{
			OnStreamingSpeechisRunning?.Invoke(callbackInfo.index, callbackInfo.slice_type, callbackInfo.text);
		}
	}

	public void InvokePTTStreamingSpeechCompleteEvent(PTTStreamingRecognitionCompleteCallbackInfo callbackInfo)
	{
		if (callbackInfo != null)
		{
			OnStreamingSpeechComplete?.Invoke(callbackInfo.result, callbackInfo.file_id, callbackInfo.file_path, callbackInfo.file_size, callbackInfo.duration, callbackInfo.text, callbackInfo.audit_result);
		}
	}

	public void InvokePTTTranslateTextCompleteEvent(int errorCode, string targetText)
	{
		OnTranslateTextComplete?.Invoke(errorCode, targetText);
	}

	public void InvokePTTTextToSpeechCompleteEvent(int code, int serialNumber, string fileID)
	{
		OnTextToSpeechComplete?.Invoke(code, serialNumber, fileID);
	}

	public QAVPTT()
	{
		nativeInstance = TMGContext.GetInstance().GetNativeInstance();
	}
}
