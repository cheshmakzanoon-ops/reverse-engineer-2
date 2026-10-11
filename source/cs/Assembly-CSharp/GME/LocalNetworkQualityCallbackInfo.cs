using System;

namespace GME;

[Serializable]
public class LocalNetworkQualityCallbackInfo
{
	public int quality;

	public int rtt;

	public int up_loss;

	public int down_loss;
}
