using System;
using GameFramework;
using SFSLitJson;

public sealed class ExternalCheckoutCoordinator
{
	private enum ExternalCheckoutResultCode
	{
		Unknown,
		UserCancelled,
		Closed,
		LaunchFailed
	}

	private enum ExternalCheckoutResultCategory
	{
		UserCancelled = 1,
		Failed
	}

	private enum ExternalCheckoutResultSource
	{
		NativeFailed = 1,
		NativeClosed,
		LuaUI,
		LuaFlow
	}

	private readonly Action<string, string> _dispatchPayCallback;

	public ExternalCheckoutCoordinator(Action<string, string> dispatchPayCallback)
	{
		_dispatchPayCallback = dispatchPayCallback;
	}

	private string GetPayStorefrontCode()
	{
		return GameEntry.PayOrderData?.GetStorefrontCode() ?? string.Empty;
	}

	private string GetCurrentUsFlowProgramName()
	{
		if (SDKManager.IS_UNITY_ANDROID())
		{
			return ExternalCheckoutProgram.AndroidUsExternalContent.ToString();
		}
		if (SDKManager.IS_UNITY_IOS())
		{
			return ExternalCheckoutProgram.IosUsExternalLink.ToString();
		}
		return string.Empty;
	}

	public bool PrepareUsExternalCheckout(bool preferWebView = true)
	{
		string currentUsFlowProgramName = GetCurrentUsFlowProgramName();
		if (string.IsNullOrEmpty(currentUsFlowProgramName))
		{
			Log.Info("[ExternalCheckout] PrepareUsExternalCheckout aborted: empty program");
			return false;
		}
		ExternalCheckoutProgram program = ParseExternalCheckoutProgram(currentUsFlowProgramName);
		string payStorefrontCode = GetPayStorefrontCode();
		GameEntry.PayOrderData?.SaveExternalCheckoutProgram(program.ToString());
		if (SDKManager.IS_UNITY_IOS())
		{
			string arg = CreateExternalCheckoutTokenResultJson(string.Empty, CanSkipNativeTokenForProgram(program));
			if (GameEntry.PayOrderData != null)
			{
				GameEntry.PayOrderData.SaveExternalCheckoutToken(string.Empty);
				GameEntry.PayOrderData.SaveExternalCheckoutLastEvent("onExternalCheckoutToken");
			}
			_dispatchPayCallback?.Invoke("onExternalCheckoutToken", arg);
			return true;
		}
		return ExternalCheckoutService.PrepareAndroidBillingApproval(program, preferWebView, payStorefrontCode);
	}

	public bool CheckUsExternalCheckoutAvailability()
	{
		string currentUsFlowProgramName = GetCurrentUsFlowProgramName();
		if (string.IsNullOrEmpty(currentUsFlowProgramName))
		{
			Log.Info("[ExternalCheckout] CheckUsExternalCheckoutAvailability aborted: empty program");
			return false;
		}
		if (SDKManager.IS_UNITY_IOS())
		{
			GameEntry.PayOrderData?.SaveExternalCheckoutLastEvent("onExternalCheckoutAvailability");
			_dispatchPayCallback?.Invoke("onExternalCheckoutAvailability", "OK");
			return true;
		}
		return ExternalCheckoutService.CheckAvailability(ParseExternalCheckoutProgram(currentUsFlowProgramName));
	}

	public bool LaunchConfiguredUsExternalCheckout(bool preferWebView = true)
	{
		return PrepareUsExternalCheckout(preferWebView);
	}

	public void HandleNativeOpened(string data)
	{
		if (GameEntry.PayOrderData != null)
		{
			GameEntry.PayOrderData.SaveExternalCheckoutLastEvent("onExternalCheckoutOpened");
			GameEntry.PayOrderData.SaveExternalCheckoutUrl(data);
		}
	}

	public string HandleNativeClosed(string data)
	{
		GameEntry.PayOrderData?.SaveExternalCheckoutLastEvent("onExternalCheckoutClosed");
		return CreateExternalCheckoutClosedResultJson(data);
	}

	public string HandleNativeFailed(string data)
	{
		Log.Info("[ExternalCheckout] onExternalCheckoutFailed data=" + data);
		ExternalCheckoutService.HandleNativeFailure(data);
		GameEntry.PayOrderData?.SaveExternalCheckoutLastEvent("onExternalCheckoutFailed");
		return CreateExternalCheckoutFailureResultJson(data);
	}

	public void HandleNativeLaunchApproved(string data)
	{
		ExternalCheckoutService.HandleNativeLaunchApproved(data);
	}

	public string HandleNativeToken(string data)
	{
		ExternalCheckoutProgram program = ParseExternalCheckoutProgram(GameEntry.PayOrderData?.GetExternalCheckoutProgram());
		string text = CreateExternalCheckoutTokenResultJson(data, CanSkipNativeTokenForProgram(program));
		string externalCheckoutTokenFromResult = GetExternalCheckoutTokenFromResult(text);
		ExternalCheckoutService.HandleNativeToken(externalCheckoutTokenFromResult);
		if (GameEntry.PayOrderData != null)
		{
			GameEntry.PayOrderData.SaveExternalCheckoutToken(externalCheckoutTokenFromResult);
			GameEntry.PayOrderData.SaveExternalCheckoutLastEvent("onExternalCheckoutToken");
		}
		return text;
	}

	public void HandleNativeAvailability(string data)
	{
		ExternalCheckoutService.HandleNativeAvailability(data);
		GameEntry.PayOrderData?.SaveExternalCheckoutLastEvent("onExternalCheckoutAvailability");
	}

	private static ExternalCheckoutProgram ParseExternalCheckoutProgram(string program)
	{
		if (string.IsNullOrEmpty(program))
		{
			return ExternalCheckoutProgram.None;
		}
		if (Enum.TryParse<ExternalCheckoutProgram>(program, ignoreCase: true, out var result))
		{
			return result;
		}
		return ExternalCheckoutProgram.None;
	}

	private static bool CanSkipNativeTokenForProgram(ExternalCheckoutProgram program)
	{
		return program == ExternalCheckoutProgram.IosUsExternalLink;
	}

	private static string CreateExternalCheckoutTokenResultJson(string token, bool canSkipTokenCheck)
	{
		return new JsonData
		{
			["token"] = token ?? string.Empty,
			["canSkipTokenCheck"] = canSkipTokenCheck
		}.ToJson();
	}

	private static string GetExternalCheckoutTokenFromResult(string resultJson)
	{
		if (string.IsNullOrEmpty(resultJson))
		{
			return string.Empty;
		}
		try
		{
			JsonData jsonData = JsonMapper.ToObject(resultJson);
			if (jsonData != null && jsonData.IsObject && jsonData.Keys.Contains("token"))
			{
				return jsonData["token"]?.ToString() ?? string.Empty;
			}
		}
		catch
		{
		}
		return resultJson;
	}

	private static string CreateExternalCheckoutFailureResultJson(string rawReason)
	{
		string text = rawReason?.Trim() ?? string.Empty;
		string text2 = TryParseNativeExternalCheckoutResult(text);
		if (text2 != null)
		{
			return text2;
		}
		ExternalCheckoutResultCode num = ParseLegacyExternalCheckoutFailureCode(text);
		return CreateExternalCheckoutResultJson(num, (num == ExternalCheckoutResultCode.UserCancelled) ? ExternalCheckoutResultCategory.UserCancelled : ExternalCheckoutResultCategory.Failed, text, ExternalCheckoutResultSource.NativeFailed);
	}

	private static string CreateExternalCheckoutClosedResultJson(string rawReason)
	{
		string text = rawReason?.Trim() ?? string.Empty;
		string text2 = TryParseNativeExternalCheckoutResult(text);
		if (text2 != null)
		{
			return text2;
		}
		return CreateExternalCheckoutResultJson(ExternalCheckoutResultCode.Closed, ExternalCheckoutResultCategory.UserCancelled, text, ExternalCheckoutResultSource.NativeClosed);
	}

	private static string TryParseNativeExternalCheckoutResult(string rawData)
	{
		if (string.IsNullOrEmpty(rawData))
		{
			return null;
		}
		try
		{
			JsonData jsonData = JsonMapper.ToObject(rawData);
			if (jsonData == null || !jsonData.IsObject || !jsonData.Keys.Contains("code"))
			{
				return null;
			}
			ExternalCheckoutResultCode code = ParseExternalCheckoutResultCode(jsonData["code"]?.ToString());
			ExternalCheckoutResultCategory category = ParseExternalCheckoutResultCategory((!jsonData.Keys.Contains("category")) ? null : jsonData["category"]?.ToString(), code);
			string message = ((!jsonData.Keys.Contains("message")) ? rawData : jsonData["message"]?.ToString());
			ExternalCheckoutResultSource source = ParseExternalCheckoutResultSource((!jsonData.Keys.Contains("source")) ? null : jsonData["source"]?.ToString(), ExternalCheckoutResultSource.NativeFailed);
			return CreateExternalCheckoutResultJson(code, category, message, source);
		}
		catch
		{
			return null;
		}
	}

	private static ExternalCheckoutResultCode ParseLegacyExternalCheckoutFailureCode(string rawReason)
	{
		if (string.IsNullOrEmpty(rawReason))
		{
			return ExternalCheckoutResultCode.LaunchFailed;
		}
		switch (rawReason.ToLowerInvariant())
		{
		case "launch:1":
		case "user_canceled":
		case "user_cancelled":
		case "user canceled":
		case "user cancelled":
			return ExternalCheckoutResultCode.UserCancelled;
		default:
			return ExternalCheckoutResultCode.LaunchFailed;
		}
	}

	private static string CreateExternalCheckoutResultJson(ExternalCheckoutResultCode code, ExternalCheckoutResultCategory category, string message, ExternalCheckoutResultSource source)
	{
		return new JsonData
		{
			["code"] = (int)code,
			["category"] = (int)category,
			["message"] = message ?? string.Empty,
			["source"] = (int)source
		}.ToJson();
	}

	private static ExternalCheckoutResultCode ParseExternalCheckoutResultCode(string code)
	{
		if (int.TryParse(code, out var result) && Enum.IsDefined(typeof(ExternalCheckoutResultCode), result))
		{
			return (ExternalCheckoutResultCode)result;
		}
		return (code ?? string.Empty).Trim().ToLowerInvariant() switch
		{
			"user_cancelled" => ExternalCheckoutResultCode.UserCancelled, 
			"closed" => ExternalCheckoutResultCode.Closed, 
			"launch_failed" => ExternalCheckoutResultCode.LaunchFailed, 
			_ => ExternalCheckoutResultCode.Unknown, 
		};
	}

	private static ExternalCheckoutResultCategory ParseExternalCheckoutResultCategory(string category, ExternalCheckoutResultCode code)
	{
		if (int.TryParse(category, out var result) && Enum.IsDefined(typeof(ExternalCheckoutResultCategory), result))
		{
			return (ExternalCheckoutResultCategory)result;
		}
		string text = (category ?? string.Empty).Trim().ToLowerInvariant();
		if (!(text == "user_cancelled"))
		{
			if (text == "failed")
			{
				return ExternalCheckoutResultCategory.Failed;
			}
			if (code != ExternalCheckoutResultCode.UserCancelled && code != ExternalCheckoutResultCode.Closed)
			{
				return ExternalCheckoutResultCategory.Failed;
			}
			return ExternalCheckoutResultCategory.UserCancelled;
		}
		return ExternalCheckoutResultCategory.UserCancelled;
	}

	private static ExternalCheckoutResultSource ParseExternalCheckoutResultSource(string source, ExternalCheckoutResultSource defaultSource)
	{
		if (int.TryParse(source, out var result) && Enum.IsDefined(typeof(ExternalCheckoutResultSource), result))
		{
			return (ExternalCheckoutResultSource)result;
		}
		string text = (source ?? string.Empty).Trim().ToLowerInvariant();
		if (!(text == "native_closed"))
		{
			if (text == "native_failed")
			{
				return ExternalCheckoutResultSource.NativeFailed;
			}
			return defaultSource;
		}
		return ExternalCheckoutResultSource.NativeClosed;
	}
}
