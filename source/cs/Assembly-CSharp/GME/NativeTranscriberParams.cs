using System;

namespace GME;

public struct NativeTranscriberParams
{
	public IntPtr transcriberRobotId;

	public IntPtr sourceLanguage;

	public IntPtr userIds;

	public int userIdsCount;

	public IntPtr translationLanguages;

	public int translationLanguagesCount;
}
