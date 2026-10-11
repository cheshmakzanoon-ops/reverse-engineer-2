using System.Collections.Generic;

public class PayOrderDataManager
{
	private HashSet<string> _consumedOrderList = new HashSet<string>();

	private bool _isConsumedOrderDetectFunctionOpen;

	private string _nativeQueryPriceResult = "";

	private string _storefrontCode = string.Empty;

	private string _externalCheckoutUrl = string.Empty;

	private string _externalCheckoutToken = string.Empty;

	private string _externalCheckoutProgram = string.Empty;

	private string _externalCheckoutLastEvent = string.Empty;

	public bool IsOrderConsumed(string orderId)
	{
		return _consumedOrderList.Contains(orderId);
	}

	public void AddOrderToConsumedList(string orderId)
	{
		_consumedOrderList.Add(orderId);
	}

	public void SetConsumedOrderDetectFunctionOpen(bool isOpen)
	{
		_isConsumedOrderDetectFunctionOpen = isOpen;
	}

	public bool IsConsumedOrderDetectFunctionOpen()
	{
		return _isConsumedOrderDetectFunctionOpen;
	}

	public void SaveNativeQueryPriceResult(string data)
	{
		_nativeQueryPriceResult = data;
	}

	public string GetNativeQueryPriceResult()
	{
		string nativeQueryPriceResult = _nativeQueryPriceResult;
		_nativeQueryPriceResult = "";
		return nativeQueryPriceResult;
	}

	public void SaveStorefrontCode(string code)
	{
		_storefrontCode = code ?? string.Empty;
	}

	public string GetStorefrontCode()
	{
		return _storefrontCode;
	}

	public void SaveExternalCheckoutUrl(string url)
	{
		_externalCheckoutUrl = url ?? string.Empty;
	}

	public string GetExternalCheckoutUrl()
	{
		return _externalCheckoutUrl;
	}

	public void SaveExternalCheckoutToken(string token)
	{
		_externalCheckoutToken = token ?? string.Empty;
	}

	public string GetExternalCheckoutToken()
	{
		return _externalCheckoutToken;
	}

	public void SaveExternalCheckoutProgram(string program)
	{
		_externalCheckoutProgram = program ?? string.Empty;
	}

	public string GetExternalCheckoutProgram()
	{
		return _externalCheckoutProgram;
	}

	public void SaveExternalCheckoutLastEvent(string eventName)
	{
		_externalCheckoutLastEvent = eventName ?? string.Empty;
	}

	public string GetExternalCheckoutLastEvent()
	{
		return _externalCheckoutLastEvent;
	}
}
