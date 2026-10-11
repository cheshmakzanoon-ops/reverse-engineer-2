namespace GME;

public abstract class ITMGPTT
{
	public abstract event QAVRecordFirstAudioFrameCallback OnRecordFirstAudioFrame;

	public abstract event QAVRecordFileCompleteCallback OnRecordFileComplete;

	public abstract event QAVUploadFileCompleteCallback OnUploadFileComplete;

	public abstract event QAVDownloadFileCompleteCallback OnDownloadFileComplete;

	public abstract event QAVPlayFileCompleteCallback OnPlayFileComplete;

	public abstract event QAVSpeechToTextCallback OnSpeechToTextComplete;

	public abstract event QAVStreamingRecognitionRunningCallback OnStreamingSpeechisRunning;

	public abstract event QAVStreamingRecognitionCompleteCallback OnStreamingSpeechComplete;

	public abstract event QAVDownloadFileWithAuditCompleteCallback OnDownloadFileAuditComplete;

	public abstract event QAVSpeechToTextWithAuditCallback OnSpeechToTextAuditComplete;

	public abstract event QAVSpeechToTextWithTargetTextCallback OnSpeechToTextTargetTextComplete;

	public abstract event QAVTranslateTextCallback OnTranslateTextComplete;

	public abstract event QAVTextToSpeechCallback OnTextToSpeechComplete;

	static ITMGPTT()
	{
		ITMGContext.GetInstance().GetPttCtrl();
	}

	public static ITMGPTT GetInstance()
	{
		return ITMGContext.GetInstance().GetPttCtrl();
	}

	public abstract int ApplyPTTAuthbuffer(string userSig);

	public abstract int SetMaxMessageLength(int msTime);

	public abstract int StartRecording(string filePath);

	public abstract int StopRecording();

	public abstract int CancelRecording();

	public abstract int UploadRecordedFile(string filePath);

	public abstract int DownloadRecordedFile(string fileId, string filePath);

	public abstract int PlayRecordedFile(string filePath);

	public abstract int PlayRecordedFile(string filePath, ITMG_VOICE_TYPE voiceType);

	public abstract int StopPlayFile();

	public abstract int SpeechToText(string fileID);

	public abstract int SpeechToText(string fileID, string speechLanguage);

	public abstract int SpeechToText(string fileID, string speechLanguage, string translatelanguage);

	public abstract int TranslateText(string text, string sourceLanguage, string translateLanguage);

	public abstract int GetFileSize(string filePath);

	public abstract int GetVoiceFileDuration(string filePath);

	public abstract int StartRecordingWithStreamingRecognition(string filePath);

	public abstract int StartRecordingWithStreamingRecognition(string filePath, string speechLanguage);

	public abstract int StartRecordingWithStreamingRecognition(string filePath, string speechLanguage, string translatelanguage);

	public abstract int SetPTTSourceLanguage(string sourceLanguage);

	public abstract int TextToSpeech(int serialNumber, string text, string voiceName, string languageCode, float speakingRate);
}
