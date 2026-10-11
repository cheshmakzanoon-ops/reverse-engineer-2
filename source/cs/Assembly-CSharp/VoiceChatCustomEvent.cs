using XLua;

[LuaCallCSharp(GenFlag.No)]
public class VoiceChatCustomEvent
{
	public int Type;

	public int SubType;

	public string Data;

	public string ChatRoomId;

	public string VoiceRoomId;
}
