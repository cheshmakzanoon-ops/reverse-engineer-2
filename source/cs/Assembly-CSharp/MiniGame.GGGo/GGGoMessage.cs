using System;
using System.Collections.Concurrent;
using System.Text;
using Newtonsoft.Json;

namespace MiniGame.GGGo;

public static class GGGoMessage
{
	private static bool _inited = false;

	private static ConcurrentDictionary<Type, int> _typeToOp = new ConcurrentDictionary<Type, int>();

	private static ConcurrentDictionary<int, Type> _opToType = new ConcurrentDictionary<int, Type>();

	private static JsonSerializerSettings _messageSerializerSettings = new JsonSerializerSettings
	{
		TypeNameHandling = TypeNameHandling.None,
		NullValueHandling = NullValueHandling.Ignore,
		DefaultValueHandling = DefaultValueHandling.Ignore
	};

	private static void Init()
	{
		if (!_inited)
		{
			_inited = true;
			RegisterTypeOp(typeof(GGGoMsgCheckValidation));
			RegisterTypeOp(typeof(GGGoMsgCheckValidationResp));
			RegisterTypeOp(typeof(GGGoMsgFrameSyncResp));
			RegisterTypeOp(typeof(GGGoMsgFrameSyncResp.CmdState));
			RegisterTypeOp(typeof(GGGoMsgReconnectResp));
			RegisterTypeOp(typeof(GGGoMsgFrameAdjust));
			RegisterTypeOp(typeof(GGGoMsgFrameAdjustResp));
			RegisterTypeOp(typeof(GGGoMsgStart));
			RegisterTypeOp(typeof(GGGoMsgStartResp));
			RegisterTypeOp(typeof(GGGoMsgEndResp));
			RegisterTypeOp(typeof(GGGoMsgEnter));
			RegisterTypeOp(typeof(GGGoMsgEnterResp));
			RegisterTypeOp(typeof(GGGoMsgLeave));
			RegisterTypeOp(typeof(GGGoMsgLeaveResp));
		}
	}

	public static bool IsOpCode(Type type, int op)
	{
		if (op == 0)
		{
			return false;
		}
		return type.IsAssignableFrom(GetMessageType(op));
	}

	public static bool IsOpCode<T>(int op)
	{
		return IsOpCode(typeof(T), op);
	}

	public static int GetMessageOpCode(Type type)
	{
		if (_typeToOp.TryGetValue(type, out var value))
		{
			return value;
		}
		throw new ArgumentException($"Unknown message type: {type}");
	}

	public static Type GetMessageType(int op)
	{
		if (_opToType.TryGetValue(op, out var value))
		{
			return value;
		}
		throw new ArgumentException($"Unknown message type: {op}");
	}

	private static void RegisterTypeOp(Type type)
	{
		int opCode = GetOpCode(type);
		_typeToOp.TryAdd(type, opCode);
		_opToType.TryAdd(opCode, type);
	}

	private static int GetOpCode(Type type)
	{
		return GetOpCode(type.FullName);
	}

	private static int GetOpCode(string str)
	{
		return GetOpCode(Encoding.UTF8.GetBytes(str));
	}

	private static int GetOpCode(byte[] buf)
	{
		uint num = 2166136261u;
		num = 2166136261u;
		for (int i = 0; i < buf.Length; i++)
		{
			num = (buf[i] ^ num) * 16777619;
		}
		return (int)num;
	}

	public static bool PackMessage(object message, out int opCode, out object data)
	{
		Init();
		if (message == null)
		{
			opCode = 0;
			data = null;
		}
		else
		{
			Type type = message.GetType();
			opCode = GetMessageOpCode(type);
			data = JsonConvert.SerializeObject(message, _messageSerializerSettings);
		}
		return true;
	}

	public static bool UnpackMessage(int opCode, object message, out object data)
	{
		Init();
		if (message != null)
		{
			Type messageType = GetMessageType(opCode);
			data = JsonConvert.DeserializeObject(message as string, messageType, _messageSerializerSettings);
		}
		else
		{
			data = null;
		}
		return true;
	}
}
