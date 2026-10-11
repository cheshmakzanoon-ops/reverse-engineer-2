using System.Collections.Generic;

namespace GME;

public struct TranscriberMessage
{
	public string segmentId;

	public string speakerUserId;

	public string sourceText;

	public Dictionary<string, string> translationTexts;

	public long timestamp;

	public bool isCompleted;
}
