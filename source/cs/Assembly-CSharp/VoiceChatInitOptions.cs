using XLua;

[LuaCallCSharp(GenFlag.No)]
public class VoiceChatInitOptions
{
	public string SdkAppId;

	public string OpenId;

	public VoiceChatAppScene AppScene = VoiceChatAppScene.Rtc;

	public bool EnableSpeakerOnEnter = true;

	public bool EnableMicOnEnter;
}
