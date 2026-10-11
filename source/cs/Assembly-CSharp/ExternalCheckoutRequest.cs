using System;

[Serializable]
public class ExternalCheckoutRequest
{
	public string url;

	public string storefrontCountryCode;

	public ExternalCheckoutMode mode;

	public ExternalCheckoutProgram program;

	public bool preferWebView = true;

	public bool allowWebView = true;
}
