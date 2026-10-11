using System;

namespace GME;

[Serializable]
public class PTTStreamingRecognitionRunningCallbackInfo
{
	public int index;

	public int slice_type;

	public string text;
}
