using System;

namespace GME;

[Serializable]
public class PTTTranslateTextCompleteCallbackInfo
{
	public int result;

	public string source_language_code;

	public string source_text;

	public PTTTranslateTextItem[] target_text;
}
