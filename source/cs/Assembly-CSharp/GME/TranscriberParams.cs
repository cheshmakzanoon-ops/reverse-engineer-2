using System.Collections.Generic;

namespace GME;

public struct TranscriberParams
{
	public string transcriberRobotId;

	public string sourceLanguage;

	public List<string> userIdsToTranscribe;

	public List<string> translationLanguages;
}
