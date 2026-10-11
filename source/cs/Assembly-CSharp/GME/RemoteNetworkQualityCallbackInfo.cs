using System;

namespace GME;

[Serializable]
public class RemoteNetworkQualityCallbackInfo
{
	public string user_id;

	public int quality;

	public int rtt;

	public int up_loss;

	public int down_loss;
}
