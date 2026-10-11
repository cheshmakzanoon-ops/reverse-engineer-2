using XLua;

[LuaCallCSharp(GenFlag.No)]
public class VoiceChatDisconnectResult
{
	public int Result;

	public string ErrorInfo;

	public string ChatRoomId;

	public string VoiceRoomId;
}
