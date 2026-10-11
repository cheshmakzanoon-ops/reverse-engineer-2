using System;

namespace GME;

[Serializable]
public class AudioRecordCompleteCallbackInfo
{
	public int event_id;

	public int result;

	public string file_path;
}
