using System;
using XLua;

[LuaCallCSharp(GenFlag.No)]
public interface IVoiceChatService
{
	event Action<VoiceChatEnterRoomResult, bool, bool> EnterRoomCompleted;

	event Action<VoiceChatExitRoomResult> ExitRoomCompleted;

	event Action<VoiceChatDisconnectResult> RoomDisconnected;

	event Action<VoiceChatUserUpdate> UserUpdated;

	event Action<VoiceChatCustomEvent> CustomEventReceived;

	int Init(VoiceChatInitOptions options);

	int Uninit();

	int Poll();

	int EnterRoom(string roomId, VoiceChatRoomType roomType, string userSig);

	int ExitRoom();

	bool IsRoomEntered();

	int SetMicEnabled(bool enabled);

	bool IsMicEnabled();

	int GetMicListCount();

	int SetSpeakerEnabled(bool enabled);

	bool IsSpeakerEnabled();

	int GetSpeakerListCount();

	int SetSpeakerVolumeByUserId(string userId, int volume);

	int StartPlayMusic(int soundId, string filePath, int loopCount);

	int StopPlayMusic(int soundId);

	int IsMusicPlayEnd(int soundId);

	VoiceChatMicPermission CheckMicPermission();

	string GetSdkVersion();

	void OnApplicationFocus(bool hasFocus);

	string GenAuthBuffer(string roomId, string openId);
}
