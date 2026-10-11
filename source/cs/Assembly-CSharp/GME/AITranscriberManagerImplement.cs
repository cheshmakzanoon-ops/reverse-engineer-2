using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using AOT;

namespace GME;

public class AITranscriberManagerImplement : IAITranscriberManager
{
	private readonly ConcurrentDictionary<string, IAITranscriberListener> _listenerDic = new ConcurrentDictionary<string, IAITranscriberListener>();

	private static readonly ConcurrentDictionary<IntPtr, AITranscriberManagerImplement> _transcriberManagerDic = new ConcurrentDictionary<IntPtr, AITranscriberManagerImplement>();

	private readonly TRTCActionQueue _actionQueue;

	private IntPtr _nativeObj;

	private IntPtr _globalTranscriberListener;

	private static string nativeUtf8PtrToString(IntPtr ptr)
	{
		if (ptr == IntPtr.Zero)
		{
			return null;
		}
		int i;
		for (i = 0; Marshal.ReadByte(ptr, i) != 0; i++)
		{
		}
		byte[] array = new byte[i];
		Marshal.Copy(ptr, array, 0, i);
		return Encoding.UTF8.GetString(array);
	}

	private static IntPtr stringToNativeUtf8Ptr(string inputStr)
	{
		if (inputStr == null)
		{
			return IntPtr.Zero;
		}
		byte[] bytes = Encoding.UTF8.GetBytes(inputStr);
		IntPtr intPtr = Marshal.AllocHGlobal(bytes.Length + 1);
		Marshal.Copy(bytes, 0, intPtr, bytes.Length);
		Marshal.WriteByte(intPtr, bytes.Length, 0);
		return intPtr;
	}

	private static void freeNativePtrIfNonZero(IntPtr p)
	{
		if (p != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(p);
		}
	}

	private static IntPtr convertStringListToIntPtrArray(List<string> stringList)
	{
		if (stringList == null || stringList.Count == 0)
		{
			return IntPtr.Zero;
		}
		int count = stringList.Count;
		IntPtr intPtr = Marshal.AllocHGlobal(count * IntPtr.Size);
		for (int i = 0; i < count; i++)
		{
			IntPtr val = stringToNativeUtf8Ptr(stringList[i]);
			Marshal.WriteIntPtr(intPtr, i * IntPtr.Size, val);
		}
		return intPtr;
	}

	private static void freeStringListIntPtrArray(IntPtr arrayPtr, int count)
	{
		if (arrayPtr != IntPtr.Zero)
		{
			for (int i = 0; i < count; i++)
			{
				freeNativePtrIfNonZero(Marshal.ReadIntPtr(arrayPtr, i * IntPtr.Size));
			}
			Marshal.FreeHGlobal(arrayPtr);
		}
	}

	public static AITranscriberManagerImplement getAITranscriberManagerInstance(IntPtr nativeObj)
	{
		if (_transcriberManagerDic.TryGetValue(nativeObj, out var value))
		{
			return value;
		}
		return null;
	}

	[MonoPInvokeCallback(typeof(AITranscriberManagerNative.UnityAITranscriberOnStarted))]
	public static void UnityAITranscriberOnStarted(IntPtr instance, IntPtr roomId, IntPtr transcriberRobotId)
	{
		getAITranscriberManagerInstance(instance)?.onRealtimeTranscriberStarted(nativeUtf8PtrToString(roomId), nativeUtf8PtrToString(transcriberRobotId));
	}

	[MonoPInvokeCallback(typeof(AITranscriberManagerNative.UnityAITranscriberOnMessageReceived))]
	public static void UnityAITranscriberOnMessageReceived(IntPtr instance, IntPtr roomId, IntPtr messagePtr)
	{
		AITranscriberManagerImplement aITranscriberManagerInstance = getAITranscriberManagerInstance(instance);
		if (aITranscriberManagerInstance == null || !(messagePtr != IntPtr.Zero))
		{
			return;
		}
		NativeTranscriberMessage nativeTranscriberMessage = Marshal.PtrToStructure<NativeTranscriberMessage>(messagePtr);
		TranscriberMessage message = new TranscriberMessage
		{
			segmentId = nativeUtf8PtrToString(nativeTranscriberMessage.segment_id),
			speakerUserId = nativeUtf8PtrToString(nativeTranscriberMessage.speaker_user_id),
			sourceText = nativeUtf8PtrToString(nativeTranscriberMessage.source_text),
			translationTexts = new Dictionary<string, string>(),
			timestamp = nativeTranscriberMessage.timestamp,
			isCompleted = (nativeTranscriberMessage.is_completed != 0)
		};
		int translation_texts_count = nativeTranscriberMessage.translation_texts_count;
		if (translation_texts_count > 0)
		{
			IntPtr translation_texts_keys = nativeTranscriberMessage.translation_texts_keys;
			IntPtr translation_texts_values = nativeTranscriberMessage.translation_texts_values;
			if (translation_texts_keys != IntPtr.Zero && translation_texts_values != IntPtr.Zero)
			{
				for (int i = 0; i < translation_texts_count; i++)
				{
					IntPtr ptr = Marshal.ReadIntPtr(translation_texts_keys, i * IntPtr.Size);
					IntPtr ptr2 = Marshal.ReadIntPtr(translation_texts_values, i * IntPtr.Size);
					string text = nativeUtf8PtrToString(ptr);
					string text2 = nativeUtf8PtrToString(ptr2);
					if (text != null && text2 != null)
					{
						message.translationTexts[text] = text2;
					}
				}
			}
		}
		aITranscriberManagerInstance.onReceiveTranscriberMessage(nativeUtf8PtrToString(roomId), message);
	}

	[MonoPInvokeCallback(typeof(AITranscriberManagerNative.UnityAITranscriberOnStopped))]
	public static void UnityAITranscriberOnStopped(IntPtr instance, IntPtr roomId, IntPtr transcriberRobotId, int reason)
	{
		getAITranscriberManagerInstance(instance)?.onRealtimeTranscriberStopped(nativeUtf8PtrToString(roomId), nativeUtf8PtrToString(transcriberRobotId), reason);
	}

	[MonoPInvokeCallback(typeof(AITranscriberManagerNative.UnityAITranscriberOnError))]
	public static void UnityAITranscriberOnError(IntPtr instance, IntPtr roomId, IntPtr transcriberRobotId, int error, IntPtr errorInfo)
	{
		getAITranscriberManagerInstance(instance)?.onRealtimeTranscriberError(nativeUtf8PtrToString(roomId), nativeUtf8PtrToString(transcriberRobotId), error, nativeUtf8PtrToString(errorInfo));
	}

	private void onRealtimeTranscriberStarted(string roomId, string transcriberRobotId)
	{
		List<IAITranscriberListener> listenersCopy = new List<IAITranscriberListener>(_listenerDic.Values);
		_actionQueue?.Enqueue(delegate
		{
			foreach (IAITranscriberListener item in listenersCopy)
			{
				item.onRealtimeTranscriberStarted(roomId, transcriberRobotId);
			}
		}, canDrop: true);
	}

	private void onReceiveTranscriberMessage(string roomId, TranscriberMessage message)
	{
		List<IAITranscriberListener> listenersCopy = new List<IAITranscriberListener>(_listenerDic.Values);
		_actionQueue?.Enqueue(delegate
		{
			foreach (IAITranscriberListener item in listenersCopy)
			{
				item.onReceiveTranscriberMessage(roomId, message);
			}
		}, canDrop: true);
	}

	private void onRealtimeTranscriberStopped(string roomId, string transcriberRobotId, int reason)
	{
		List<IAITranscriberListener> listenersCopy = new List<IAITranscriberListener>(_listenerDic.Values);
		_actionQueue?.Enqueue(delegate
		{
			foreach (IAITranscriberListener item in listenersCopy)
			{
				item.onRealtimeTranscriberStopped(roomId, transcriberRobotId, reason);
			}
		}, canDrop: true);
	}

	private void onRealtimeTranscriberError(string roomId, string transcriberRobotId, int error, string errorInfo)
	{
		List<IAITranscriberListener> listenersCopy = new List<IAITranscriberListener>(_listenerDic.Values);
		_actionQueue?.Enqueue(delegate
		{
			foreach (IAITranscriberListener item in listenersCopy)
			{
				item.onRealtimeTranscriberError(roomId, transcriberRobotId, error, errorInfo);
			}
		}, canDrop: true);
	}

	public AITranscriberManagerImplement(IntPtr nativeObj, TRTCActionQueue actionQueue)
	{
		_nativeObj = nativeObj;
		_actionQueue = actionQueue;
		_globalTranscriberListener = AITranscriberManagerNative.tx_ai_transcriber_manager_create_transcriber_listener(nativeObj, UnityAITranscriberOnStarted, UnityAITranscriberOnMessageReceived, UnityAITranscriberOnStopped, UnityAITranscriberOnError);
		AITranscriberManagerNative.tx_ai_transcriber_manager_add_transcriber_listener(nativeObj, _globalTranscriberListener);
		_transcriberManagerDic.TryAdd(nativeObj, this);
	}

	~AITranscriberManagerImplement()
	{
		AITranscriberManagerNative.tx_ai_transcriber_manager_destroy_transcriber_listener(_globalTranscriberListener);
		_globalTranscriberListener = IntPtr.Zero;
	}

	public void Destroy()
	{
		AITranscriberManagerNative.tx_ai_transcriber_manager_remove_transcriber_listener(_nativeObj, _globalTranscriberListener);
		_listenerDic.Clear();
		_transcriberManagerDic.TryRemove(_nativeObj, out var _);
		_nativeObj = IntPtr.Zero;
	}

	public override void startRealtimeTranscriber(TranscriberParams transcriberParams)
	{
		NativeTranscriberParams transcriberParams2 = default(NativeTranscriberParams);
		IntPtr arrayPtr = IntPtr.Zero;
		IntPtr arrayPtr2 = IntPtr.Zero;
		try
		{
			transcriberParams2.transcriberRobotId = stringToNativeUtf8Ptr(transcriberParams.transcriberRobotId);
			transcriberParams2.sourceLanguage = stringToNativeUtf8Ptr(transcriberParams.sourceLanguage);
			arrayPtr = (transcriberParams2.userIds = convertStringListToIntPtrArray(transcriberParams.userIdsToTranscribe));
			transcriberParams2.userIdsCount = transcriberParams.userIdsToTranscribe?.Count ?? 0;
			arrayPtr2 = (transcriberParams2.translationLanguages = convertStringListToIntPtrArray(transcriberParams.translationLanguages));
			transcriberParams2.translationLanguagesCount = transcriberParams.translationLanguages?.Count ?? 0;
			AITranscriberManagerNative.tx_ai_transcriber_manager_start_realtime_transcriber(_nativeObj, ref transcriberParams2);
		}
		finally
		{
			freeNativePtrIfNonZero(transcriberParams2.transcriberRobotId);
			freeNativePtrIfNonZero(transcriberParams2.sourceLanguage);
			freeStringListIntPtrArray(arrayPtr, transcriberParams.userIdsToTranscribe?.Count ?? 0);
			freeStringListIntPtrArray(arrayPtr2, transcriberParams.translationLanguages?.Count ?? 0);
		}
	}

	public override void stopRealtimeTranscriber(string transcriberRobotId)
	{
		AITranscriberManagerNative.tx_ai_transcriber_manager_stop_realtime_transcriber(_nativeObj, stringToNativeUtf8Ptr(transcriberRobotId));
	}

	public override void pauseReceivingMessage()
	{
		AITranscriberManagerNative.tx_ai_transcriber_manager_pause_receiving_message(_nativeObj);
	}

	public override void resumeReceivingMessage()
	{
		AITranscriberManagerNative.tx_ai_transcriber_manager_resume_receiving_message(_nativeObj);
	}

	public override void addListener(IAITranscriberListener listener)
	{
		if (listener != null)
		{
			string key = listener.GetHashCode().ToString();
			_listenerDic.TryAdd(key, listener);
		}
	}

	public override void removeListener(IAITranscriberListener listener)
	{
		if (listener != null)
		{
			string key = listener.GetHashCode().ToString();
			_listenerDic.TryRemove(key, out var _);
		}
	}
}
