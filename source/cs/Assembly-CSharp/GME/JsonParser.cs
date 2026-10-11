using System;
using UnityEngine;

namespace GME;

public class JsonParser
{
	public static T FromJson<T>(string json) where T : class
	{
		try
		{
			return JsonUtility.FromJson<T>(json);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			string message = "GMECSharpEventCallback.OnEventCallBack Parse data to json failed, data=" + json;
			TMGContextNative.GMEUnity_WriteLog(1, message);
			return null;
		}
	}
}
