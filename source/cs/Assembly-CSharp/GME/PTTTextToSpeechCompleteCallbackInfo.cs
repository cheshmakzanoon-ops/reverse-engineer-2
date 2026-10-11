using System;

namespace GME;

[Serializable]
public class PTTTextToSpeechCompleteCallbackInfo
{
	public int result;

	public int serial_number;

	public string file_id;
}
