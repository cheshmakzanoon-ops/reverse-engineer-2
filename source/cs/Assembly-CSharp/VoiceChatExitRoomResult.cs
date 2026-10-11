using XLua;

[LuaCallCSharp(GenFlag.No)]
public class VoiceChatExitRoomResult
{
	public int Result;

	public string ErrorInfo;

	public string ChatRoomId;

	public string VoiceRoomId;
}
