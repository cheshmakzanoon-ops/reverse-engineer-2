using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class FunTapPlayerInfoView
{
	public static string BaseUrl = "https://cp.hhgame.vn/user/profile/{userid}/update?os={os}";

	public static string AuthName = "Authorization";

	public static string AuthBearer = "Bearer jFfH3z3lhjNnHHLJGkYqCXhDyF0HeweOL9dl4pXUzsg9jrnxMq8QNyAWj3UW";

	private static WebViewObject webViewObject;

	private static GameObject webViewGameObject;

	private static Action<bool> whenDone;

	private static string funtapUserId = "testuser";

	private static bool httpError = false;

	private static string finishMessage = "https://cp.hhgame.vn/user/profile/notice?os={os}";

	private static string finishTitle = "<title>Command</title>";

	private static string finishFlag = "class=\"msg-success\"";

	private static Action<bool> alreadyFinish;

	private static readonly char[] Base58Alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz".ToCharArray();

	private static string GetFinishMessage()
	{
		string text = "android";
		text = "android";
		return finishMessage.Replace("{os}", text);
	}

	public static void CheckWebView(string userId, Action<bool> finish, MonoBehaviour launcher)
	{
		if (webViewObject != null)
		{
			Stop(isDone: false);
		}
		funtapUserId = userId;
		alreadyFinish = finish;
		launcher.StartCoroutine(WebRequestCheck());
	}

	private static IEnumerator WebRequestCheck()
	{
		string url = GetUrl();
		Debug.LogWarning("[Webview] Url: " + url);
		UnityWebRequest request = UnityWebRequest.Get(url);
		request.SetRequestHeader(AuthName, AuthBearer);
		yield return request.SendWebRequest();
		if (request.isNetworkError || request.isHttpError)
		{
			Debug.LogError("[WebRequestCheck] Error: " + request.error);
			yield return new WaitForSeconds(1f);
			yield return WebRequestCheck();
			yield break;
		}
		Action<bool> action = alreadyFinish;
		alreadyFinish = null;
		string text = request.downloadHandler.text;
		Debug.LogWarning("[WebRequestCheck] Response: " + text);
		bool obj = text.Contains(finishTitle) && text.Contains(finishFlag);
		action(obj);
	}

	private static IEnumerator WebviewStart()
	{
		webViewGameObject = new GameObject("WebViewObject");
		webViewObject = webViewGameObject.AddComponent<WebViewObject>();
		string Url = GetUrl();
		Debug.LogWarning("[Webview] Url: " + Url);
		webViewObject.Init(delegate(string msg)
		{
			Debug.LogWarning($"[Webview]CallFromJS[{msg}]");
		}, delegate(string msg)
		{
			Debug.LogWarning($"[Webview]CallOnError[{msg}]");
			httpError = true;
			Stop(isDone: false);
		}, delegate(string msg)
		{
			Debug.LogWarning($"[Webview]CallOnHttpError[{msg}]");
			httpError = true;
			Stop(isDone: false);
		}, delegate(string msg)
		{
			Debug.LogWarning($"[Webview]CallOnLoaded[{msg}]");
		}, delegate(string msg)
		{
			Debug.LogWarning($"[Webview]CallOnStarted[{msg}]");
			if (msg.Contains(GetFinishMessage()))
			{
				Stop(!httpError);
			}
		}, delegate(string msg)
		{
			Debug.LogWarning($"[Webview]CallOnHooked[{msg}]");
		}, delegate(string msg)
		{
			Debug.LogWarning($"[Webview]CallOnCookies[{msg}]");
		});
		while (!webViewObject.IsInitialized())
		{
			yield return null;
		}
		webViewObject.SetMargins(0, 0, 0, 0);
		webViewObject.SetTextZoom(100);
		webViewObject.SetMixedContentMode(0);
		webViewObject.SetVisibility(v: true);
		webViewObject.SetInteractionEnabled(enabled: true);
		webViewObject.AddCustomHeader(AuthName, AuthBearer);
		if (Url.StartsWith("http"))
		{
			webViewObject.LoadURL(Url.Replace(" ", "%20"));
			yield break;
		}
		string[] array = new string[3] { ".jpg", ".js", ".html" };
		string[] array2 = array;
		foreach (string ext in array2)
		{
			string path = Url.Replace(".html", ext);
			string text = Path.Combine(Application.streamingAssetsPath, path);
			string dst = Path.Combine(Application.temporaryCachePath, path);
			byte[] bytes;
			if (text.Contains("://"))
			{
				UnityWebRequest unityWebRequest = UnityWebRequest.Get(text);
				yield return unityWebRequest.SendWebRequest();
				bytes = unityWebRequest.downloadHandler.data;
			}
			else
			{
				bytes = File.ReadAllBytes(text);
			}
			File.WriteAllBytes(dst, bytes);
			if (ext == ".html")
			{
				webViewObject.LoadURL("file://" + dst.Replace(" ", "%20"));
				break;
			}
		}
	}

	private static void Stop(bool isDone)
	{
		if (webViewObject != null)
		{
			webViewObject.SetVisibility(v: false);
			webViewObject.SetInteractionEnabled(enabled: false);
			UnityEngine.Object.Destroy(webViewGameObject);
			webViewObject = null;
			webViewGameObject = null;
			if (whenDone != null)
			{
				whenDone(isDone);
			}
			whenDone = null;
			funtapUserId = "testuser";
			httpError = false;
		}
	}

	private static string GetUrl()
	{
		string text = "android";
		text = "android";
		return BaseUrl.Replace("{userid}", funtapUserId).Replace("{os}", text);
	}

	public static void ShowWebView(string userId, Action<bool> done, MonoBehaviour launcher)
	{
		if (webViewObject != null)
		{
			Stop(isDone: false);
		}
		funtapUserId = userId;
		whenDone = done;
		launcher.StartCoroutine(WebviewStart());
	}

	public static string Encrypt(string deviceId)
	{
		SHA1 sHA = SHA1.Create();
		byte[] bytes = Encoding.UTF8.GetBytes(deviceId);
		return EncodeBase58(sHA.ComputeHash(bytes));
	}

	public static string EncodeBase58(byte[] input)
	{
		StringBuilder stringBuilder = new StringBuilder();
		BigInteger bigInteger = input.Aggregate(0, (BigInteger current, byte t) => current * 256 + t);
		while (bigInteger > 0L)
		{
			int num = (int)(bigInteger % 58);
			bigInteger /= (BigInteger)58;
			stringBuilder.Insert(0, Base58Alphabet[num]);
		}
		for (int num2 = 0; num2 < input.Length && input[num2] == 0; num2++)
		{
			stringBuilder.Insert(0, Base58Alphabet[0]);
		}
		return stringBuilder.ToString();
	}

	public static byte[] DecodeBase58(string input)
	{
		BigInteger bigInteger = input.Aggregate(0, (BigInteger current, char t) => current * 58 + Array.IndexOf(Base58Alphabet, t));
		byte[] first = new byte[input.TakeWhile((char c) => c == Base58Alphabet[0]).Count()];
		byte[] second = bigInteger.ToByteArray().Reverse().SkipWhile((byte b) => b == 0)
			.ToArray();
		return first.Concat(second).ToArray();
	}

	public static string EncodeBase58FromHex(string hex)
	{
		if (string.IsNullOrEmpty(hex))
		{
			throw new ArgumentException("Hex string cannot be null or empty", "hex");
		}
		if (hex.Length % 2 != 0)
		{
			throw new ArgumentException("Hex string must have an even length", "hex");
		}
		byte[] array = new byte[hex.Length / 2];
		for (int i = 0; i < hex.Length; i += 2)
		{
			array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
		}
		return EncodeBase58(array);
	}

	public static string DecodeBase58ToHex(string input)
	{
		return BitConverter.ToString(DecodeBase58(input)).Replace("-", "").ToLower();
	}
}
