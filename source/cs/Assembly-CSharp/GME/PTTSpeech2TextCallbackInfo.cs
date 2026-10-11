using System;

namespace GME;

[Serializable]
public class PTTSpeech2TextCallbackInfo
{
	public int result;

	public string file_id;

	public string text;

	public string target_text;

	public string audit_result;
}
