using XLua;

[LuaCallCSharp(GenFlag.No)]
public class VoiceChatUserUpdate
{
	public VoiceChatUserEventType EventType;

	public string[] UserIds;

	public string ChatRoomId;

	public string VoiceRoomId;
}
