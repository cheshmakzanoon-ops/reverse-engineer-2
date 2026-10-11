using System;

namespace GME;

[Serializable]
public class PlayMusicFinishCallbackInfo
{
	public int result;

	public long sound_id;

	public bool is_finished;

	public string file_path;
}
