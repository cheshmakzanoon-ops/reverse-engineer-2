using System;

namespace GME;

[Serializable]
public class PTTUploadCompleteCallbackInfo
{
	public int result;

	public string file_path;

	public string file_id;

	public string audit_result;
}
