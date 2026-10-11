using System;
using GameFramework;
using UnityEngine;

public static class ExternalCheckoutService
{
	[Serializable]
	private sealed class ExternalCheckoutNativePayload
	{
		public string url;

		public ExternalCheckoutProgram program;

		public bool callerWillLaunchLink;
	}

	private static string _pendingApprovalResolvedUrl;

	private static bool _pendingApprovalUseWebView;

	private static ExternalCheckoutProgram _pendingApprovalProgram;

	private static bool _pendingIosBrowserReturnClose;

	private static string _pendingIosBrowserReturnUrl;

	private static bool _pendingIosBrowserOpened;

	static ExternalCheckoutService()
	{
		Application.focusChanged += HandleApplicationFocusChanged;
	}

	public static bool Open(ExternalCheckoutRequest request)
	{
		return OpenInternal(request);
	}

	public static bool PrepareAndroidBillingApproval(ExternalCheckoutProgram program, bool preferWebView = true, string storefrontCountryCode = "")
	{
		return StartAndroidApprovalFlowInternal(program, preferWebView, storefrontCountryCode);
	}

	public static bool CheckAvailability(ExternalCheckoutProgram program)
	{
		return ProbeAvailabilityInternal(program);
	}

	public static void Close()
	{
		CloseInternal(notifyClosed: true);
	}

	public static bool OpenPreparedUrl(string url)
	{
		return ContinueApprovalFlowWithPreparedUrlInternal(url);
	}

	public static void CancelPendingApproval()
	{
		ResetApprovalFlowState();
	}

	private static bool OpenInternal(ExternalCheckoutRequest request)
	{
		if (request == null)
		{
			Debug.LogWarning("ExternalCheckoutService: request is empty.");
			NotifyGame("onExternalCheckoutFailed", "empty_request");
			return false;
		}
		bool flag = RequiresAndroidBillingApproval(request.program);
		if (string.IsNullOrEmpty(request.url) && !flag)
		{
			Debug.LogWarning("ExternalCheckoutService: request url is empty.");
			NotifyGame("onExternalCheckoutFailed", "empty_request_url");
			return false;
		}
		bool useWebView = ShouldUseWebView(request);
		if (flag)
		{
			if (!string.IsNullOrEmpty(request.url))
			{
				Debug.LogWarning("ExternalCheckoutService: direct open is unsupported for approval-required programs.");
				NotifyGame("onExternalCheckoutFailed", "unsupported_direct_open_for_approval_program");
				return false;
			}
			return BeginAndroidApprovalFlow(null, request.program, useWebView);
		}
		if (!Uri.TryCreate(request.url, UriKind.Absolute, out var result))
		{
			Debug.LogWarning("ExternalCheckoutService: invalid url " + request.url);
			NotifyGame("onExternalCheckoutFailed", request.url);
			return false;
		}
		return OpenResolvedUrl(result.AbsoluteUri, useWebView);
	}

	private static bool StartAndroidApprovalFlowInternal(ExternalCheckoutProgram program, bool preferWebView, string storefrontCountryCode)
	{
		if (!RequiresAndroidBillingApproval(program))
		{
			return false;
		}
		bool useWebView = ShouldUseWebView(new ExternalCheckoutRequest
		{
			storefrontCountryCode = storefrontCountryCode,
			preferWebView = preferWebView,
			allowWebView = true,
			mode = ExternalCheckoutMode.Auto,
			program = program
		});
		return BeginAndroidApprovalFlow(null, program, useWebView);
	}

	private static bool ProbeAvailabilityInternal(ExternalCheckoutProgram program)
	{
		return SendDataToNative("Pay_checkExternalCheckoutAvailability", JsonUtility.ToJson(new ExternalCheckoutNativePayload
		{
			program = program
		}));
	}

	private static bool ContinueApprovalFlowWithPreparedUrlInternal(string url)
	{
		if (string.IsNullOrEmpty(url))
		{
			Debug.LogWarning("ExternalCheckoutService: prepared url is empty.");
			NotifyGame("onExternalCheckoutFailed", "empty_prepared_url");
			return false;
		}
		if (HasPendingAndroidApprovalFlow())
		{
			_pendingApprovalResolvedUrl = url;
			if (SendDataToNative("Pay_launchExternalCheckout", JsonUtility.ToJson(new ExternalCheckoutNativePayload
			{
				url = url,
				program = _pendingApprovalProgram,
				callerWillLaunchLink = _pendingApprovalUseWebView
			})))
			{
				return true;
			}
			NotifyGame("onExternalCheckoutFailed", "launch_dispatch_failed");
			ResetApprovalFlowState();
			return false;
		}
		return OpenResolvedUrl(url, ShouldUseResolvedUrlWebView());
	}

	private static bool ShouldUseWebView(ExternalCheckoutRequest request)
	{
		if (!request.allowWebView)
		{
			return false;
		}
		if (request.mode == ExternalCheckoutMode.Browser)
		{
			return false;
		}
		if (request.mode == ExternalCheckoutMode.WebView)
		{
			return IsWebViewAllowedForProgram(request.program, request.storefrontCountryCode);
		}
		if (!request.preferWebView)
		{
			return false;
		}
		return IsWebViewAllowedForProgram(request.program, request.storefrontCountryCode);
	}

	private static bool IsWebViewAllowedForProgram(ExternalCheckoutProgram program, string storefrontCountryCode)
	{
		string text = NormalizeCountryCode(storefrontCountryCode);
		if (program == ExternalCheckoutProgram.IosKrExternalPurchase || text == "KR" || text == "KOR")
		{
			return false;
		}
		return true;
	}

	private static bool RequiresAndroidBillingApproval(ExternalCheckoutProgram program)
	{
		if (program != ExternalCheckoutProgram.AndroidUsExternalContent)
		{
			return program == ExternalCheckoutProgram.AndroidJapanExternalPayments;
		}
		return true;
	}

	private static bool OpenResolvedUrl(string url, bool useWebView)
	{
		if (useWebView)
		{
			NotifyGame("onExternalCheckoutOpened", url);
			return true;
		}
		if (!SendDataToNative("Pay_launchExternalCheckout", JsonUtility.ToJson(new ExternalCheckoutNativePayload
		{
			url = url,
			program = ExternalCheckoutProgram.None,
			callerWillLaunchLink = false
		})))
		{
			NotifyGame("onExternalCheckoutFailed", "launch_dispatch_failed");
			return false;
		}
		NotifyGame("onExternalCheckoutOpened", url);
		return true;
	}

	private static void CloseInternal(bool notifyClosed = false)
	{
		ResetIosBrowserReturnTrackingState();
		if (notifyClosed)
		{
			NotifyGame("onExternalCheckoutClosed", string.Empty);
		}
	}

	private static string NormalizeCountryCode(string storefrontCountryCode)
	{
		if (!string.IsNullOrEmpty(storefrontCountryCode))
		{
			return storefrontCountryCode.Trim().ToUpperInvariant();
		}
		return string.Empty;
	}

	private static bool SendDataToNative(string funcName, string data)
	{
		if (GameEntry.Sdk == null)
		{
			Log.Warning("ExternalCheckoutService dropped native call {0} because GameEntry.Sdk is null.", funcName);
			return false;
		}
		GameEntry.Sdk.SendDataToNative(funcName, data ?? string.Empty);
		return true;
	}

	private static void NotifyGame(string funcName, string data)
	{
		if (GameEntry.Sdk != null)
		{
			GameEntry.Sdk.SendDataToGame(funcName, data ?? string.Empty);
		}
		else
		{
			Log.Warning("ExternalCheckoutService dropped callback {0} because GameEntry.Sdk is null.", funcName);
		}
	}

	public static void HandleNativeLaunchApproved(string url)
	{
		CompleteApprovedLaunchInternal(url);
	}

	public static void HandleNativeAvailability(string data)
	{
		AdvanceApprovalFlowAfterAvailabilityInternal(data);
	}

	public static void HandleNativeToken(string token)
	{
		ConsumeApprovalFlowTokenInternal(token);
	}

	public static void HandleNativeFailure(string reason)
	{
		ResetApprovalFlowState();
	}

	private static void HandleApplicationFocusChanged(bool hasFocus)
	{
	}

	private static void BeginIosBrowserReturnTracking(string url)
	{
		_pendingIosBrowserReturnClose = !string.IsNullOrEmpty(url);
		_pendingIosBrowserReturnUrl = url;
		_pendingIosBrowserOpened = false;
	}

	private static void HandleIosBrowserFocusChanged(bool hasFocus)
	{
		if (!_pendingIosBrowserReturnClose)
		{
			return;
		}
		if (!hasFocus)
		{
			if (!_pendingIosBrowserOpened)
			{
				_pendingIosBrowserOpened = true;
				NotifyGame("onExternalCheckoutOpened", _pendingIosBrowserReturnUrl ?? string.Empty);
			}
		}
		else if (_pendingIosBrowserOpened)
		{
			ResetIosBrowserReturnTrackingState();
			NotifyGame("onExternalCheckoutClosed", string.Empty);
		}
	}

	private static void CompleteApprovedLaunchInternal(string url)
	{
		string text = (string.IsNullOrEmpty(url) ? _pendingApprovalResolvedUrl : url);
		bool pendingApprovalUseWebView = _pendingApprovalUseWebView;
		_pendingApprovalResolvedUrl = null;
		_pendingApprovalUseWebView = false;
		_pendingApprovalProgram = ExternalCheckoutProgram.None;
		if (string.IsNullOrEmpty(text))
		{
			NotifyGame("onExternalCheckoutFailed", "missing_approved_url");
		}
		else
		{
			OpenResolvedUrl(text, pendingApprovalUseWebView);
		}
	}

	private static bool BeginAndroidApprovalFlow(string url, ExternalCheckoutProgram program, bool useWebView)
	{
		_pendingApprovalResolvedUrl = url;
		_pendingApprovalUseWebView = useWebView;
		_pendingApprovalProgram = program;
		if (SendDataToNative("Pay_checkExternalCheckoutAvailability", JsonUtility.ToJson(new ExternalCheckoutNativePayload
		{
			program = program
		})))
		{
			return true;
		}
		ResetApprovalFlowState();
		NotifyGame("onExternalCheckoutFailed", "availability_dispatch_failed");
		return false;
	}

	private static void AdvanceApprovalFlowAfterAvailabilityInternal(string data)
	{
		if (HasPendingAndroidApprovalFlow())
		{
			if (!string.Equals(data, "OK", StringComparison.OrdinalIgnoreCase))
			{
				NotifyGame("onExternalCheckoutFailed", "availability:" + data);
				ResetApprovalFlowState();
			}
			else if (!SendDataToNative("Pay_prepareExternalCheckoutToken", JsonUtility.ToJson(new ExternalCheckoutNativePayload
			{
				program = _pendingApprovalProgram
			})))
			{
				NotifyGame("onExternalCheckoutFailed", "token_dispatch_failed");
				ResetApprovalFlowState();
			}
		}
	}

	private static void ConsumeApprovalFlowTokenInternal(string token)
	{
		if (HasPendingAndroidApprovalFlow() && !string.IsNullOrEmpty(_pendingApprovalResolvedUrl))
		{
			NotifyGame("onExternalCheckoutFailed", "unexpected_direct_open_approval_state");
			ResetApprovalFlowState();
		}
	}

	private static void ResetApprovalFlowState()
	{
		_pendingApprovalResolvedUrl = null;
		_pendingApprovalUseWebView = false;
		_pendingApprovalProgram = ExternalCheckoutProgram.None;
		ResetIosBrowserReturnTrackingState();
	}

	private static void ResetIosBrowserReturnTrackingState()
	{
		_pendingIosBrowserReturnClose = false;
		_pendingIosBrowserReturnUrl = null;
		_pendingIosBrowserOpened = false;
	}

	private static bool HasPendingAndroidApprovalFlow()
	{
		if (_pendingApprovalProgram != ExternalCheckoutProgram.AndroidUsExternalContent)
		{
			return _pendingApprovalProgram == ExternalCheckoutProgram.AndroidJapanExternalPayments;
		}
		return true;
	}

	private static bool ShouldUseResolvedUrlWebView()
	{
		if (!HasPendingAndroidApprovalFlow())
		{
			return _pendingApprovalUseWebView;
		}
		return false;
	}
}
