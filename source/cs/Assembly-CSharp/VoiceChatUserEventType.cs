using XLua;

[LuaCallCSharp(GenFlag.No)]
public enum VoiceChatUserEventType
{
	Unknown,
	UserEnter,
	UserExit,
	UserMicOpened,
	UserMicClosed,
	UserHasAudio,
	UserNoAudio
}
