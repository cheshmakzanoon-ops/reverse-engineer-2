using System;

namespace GME;

[Serializable]
public class RoomManagerOperateCallbackInfo
{
	public int operate_type;

	public string sender_id;

	public string receiver_id;

	public int result;

	public bool operate_value;
}
