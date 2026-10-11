using System;

namespace GME;

[Serializable]
public class NetworkQualityCallbackInfo
{
	public LocalNetworkQualityCallbackInfo local;

	public RemoteNetworkQualityCallbackInfo[] remote;
}
