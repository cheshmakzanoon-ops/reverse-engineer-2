using System;

namespace GME;

[Serializable]
public class PTTRecordCompleteCallbackInfo
{
	public int result;

	public string file_path;

	public int file_size;

	public int duration;
}
