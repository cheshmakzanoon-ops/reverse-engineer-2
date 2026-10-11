using System;

namespace GME;

[Serializable]
public class PTTDownloadCompleteCallbackInfo
{
	public int result;

	public string file_path;

	public string file_id;

	public string audit_result;
}
