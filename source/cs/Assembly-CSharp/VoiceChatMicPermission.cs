using XLua;

[LuaCallCSharp(GenFlag.No)]
public enum VoiceChatMicPermission
{
	Granted = 0,
	Denied = 1,
	NotDetermined = 2,
	Unknown = -1
}
