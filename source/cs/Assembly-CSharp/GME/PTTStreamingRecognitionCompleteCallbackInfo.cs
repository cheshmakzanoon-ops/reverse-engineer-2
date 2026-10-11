using System;

namespace GME;

[Serializable]
public class PTTStreamingRecognitionCompleteCallbackInfo
{
	public int result;

	public string file_id;

	public string file_path;

	public int file_size;

	public int duration;

	public string text;

	public string audit_result;
}
