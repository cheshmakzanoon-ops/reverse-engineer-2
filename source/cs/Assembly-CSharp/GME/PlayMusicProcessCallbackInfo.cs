using System;

namespace GME;

[Serializable]
public class PlayMusicProcessCallbackInfo
{
	public long sound_id;

	public long process_ms;

	public long duration_ms;
}
