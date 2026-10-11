using System;

namespace GME;

public struct NativeTranscriberMessage
{
	public IntPtr segment_id;

	public IntPtr speaker_user_id;

	public IntPtr source_text;

	public IntPtr translation_texts_keys;

	public IntPtr translation_texts_values;

	public int translation_texts_count;

	public long timestamp;

	public int is_completed;
}
