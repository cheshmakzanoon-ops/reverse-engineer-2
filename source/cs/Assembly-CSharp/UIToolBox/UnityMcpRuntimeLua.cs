using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using XLua;
using XLua.LuaDLL;

namespace UIToolBox;

public sealed class UnityMcpRuntimeLua : MonoBehaviour
{
	[Serializable]
	public sealed class LuaEvalResult
	{
		public bool ok;

		public object result;

		public string error;

		public string detail;

		public string[] logs;

		public float elapsedMs;

		public float normalizeElapsedMs;
	}

	private sealed class LogCollector
	{
		private const int MaxLines = 200;

		private const int MaxChars = 32768;

		private readonly List<string> _lines = new List<string>(64);

		private int _chars;

		public void Attach()
		{
			Application.logMessageReceived += OnLog;
		}

		public void Detach()
		{
			Application.logMessageReceived -= OnLog;
		}

		public string[] GetLogs()
		{
			return _lines.ToArray();
		}

		private void OnLog(string condition, string stackTrace, LogType type)
		{
			if (_lines.Count < 200 && _chars < 32768)
			{
				string text = string.Concat("[", type, "] ", Truncate(condition, 512));
				if ((type == LogType.Error || type == LogType.Exception || type == LogType.Assert) && !string.IsNullOrEmpty(stackTrace))
				{
					int num = stackTrace.IndexOf('\n');
					string text2 = ((num > 0) ? stackTrace.Substring(0, num) : stackTrace);
					text = text + " | " + Truncate(text2, 256);
				}
				_lines.Add(text);
				_chars += text.Length;
			}
		}

		private static string Truncate(string text, int maxLen)
		{
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}
			if (text.Length <= maxLen)
			{
				return text;
			}
			return text.Substring(0, maxLen) + "...";
		}
	}

	private static UnityMcpRuntimeLua _instance;

	private static readonly object RecursiveLuaTableSentinel = new object();

	public static UnityMcpRuntimeLua GetOrCreate()
	{
		if (_instance != null)
		{
			return _instance;
		}
		_instance = UnityEngine.Object.FindObjectOfType<UnityMcpRuntimeLua>();
		if (_instance != null)
		{
			return _instance;
		}
		GameObject obj = new GameObject("__UnityMcpRuntimeLua__")
		{
			hideFlags = HideFlags.DontSave
		};
		UnityEngine.Object.DontDestroyOnLoad(obj);
		_instance = obj.AddComponent<UnityMcpRuntimeLua>();
		return _instance;
	}

	public Task<LuaEvalResult> EvalAsync(string code, string resultExpr, bool captureLogs)
	{
		if (string.IsNullOrEmpty(code))
		{
			throw new ArgumentException("code is required", "code");
		}
		TaskCompletionSource<LuaEvalResult> taskCompletionSource = new TaskCompletionSource<LuaEvalResult>();
		StartCoroutine(EvalRoutine(code, resultExpr, captureLogs, taskCompletionSource));
		return taskCompletionSource.Task;
	}

	private static IEnumerator EvalRoutine(string code, string resultExpr, bool captureLogs, TaskCompletionSource<LuaEvalResult> tcs)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		LogCollector logCollector = (captureLogs ? new LogCollector() : null);
		object[] raw = null;
		Exception ex = null;
		LuaTable luaTable = null;
		try
		{
			logCollector?.Attach();
			LuaEnv luaEnv = GameEntry.Lua?.Env;
			if (luaEnv == null)
			{
				throw new Exception("GameEntry.Lua.Env is null.");
			}
			luaTable = CreateEvalEnvironment(luaEnv);
			string source = BuildEvalSource(code, resultExpr);
			try
			{
				raw = ExecuteInSafeMode(luaEnv, source, "runtime_lua_eval", luaTable);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
		}
		catch (Exception ex3)
		{
			ex = ex3;
		}
		finally
		{
			logCollector?.Detach();
		}
		float elapsedMs = (Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f;
		string[] logs = ((logCollector != null) ? logCollector.GetLogs() : new string[0]);
		if (ex != null)
		{
			DisposeLuaWrapperIfNeeded(luaTable);
			tcs.TrySetResult(new LuaEvalResult
			{
				ok = false,
				result = null,
				error = "lua_exception",
				detail = ex.ToString(),
				logs = logs,
				elapsedMs = elapsedMs,
				normalizeElapsedMs = 0f
			});
			yield break;
		}
		float realtimeSinceStartup2 = Time.realtimeSinceStartup;
		object result = null;
		Exception ex4 = null;
		try
		{
			result = NormalizeLuaResults(raw);
		}
		catch (Exception ex5)
		{
			ex4 = ex5;
		}
		finally
		{
			DisposeTopLevelLuaResults(raw);
		}
		DisposeLuaWrapperIfNeeded(luaTable);
		float normalizeElapsedMs = (Time.realtimeSinceStartup - realtimeSinceStartup2) * 1000f;
		elapsedMs = (Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f;
		if (ex4 != null)
		{
			tcs.TrySetResult(new LuaEvalResult
			{
				ok = false,
				result = null,
				error = "normalize_exception",
				detail = ex4.ToString(),
				logs = logs,
				elapsedMs = elapsedMs,
				normalizeElapsedMs = normalizeElapsedMs
			});
		}
		else
		{
			tcs.TrySetResult(new LuaEvalResult
			{
				ok = true,
				result = result,
				error = null,
				detail = null,
				logs = logs,
				elapsedMs = elapsedMs,
				normalizeElapsedMs = normalizeElapsedMs
			});
		}
	}

	private static LuaTable CreateEvalEnvironment(LuaEnv env)
	{
		object[] array = env.DoString("local __aps_global_env = _G or _ENV\nlocal __aps_user_debug = setmetatable({}, {\n    __index = function(_, key)\n        if key == 'sethook' then\n            return function()\n                error('debug.sethook is disabled in runtime_lua_eval', 0)\n            end\n        end\n        return debug and debug[key]\n    end,\n    __newindex = function(tbl, key, value)\n        rawset(tbl, key, value)\n    end,\n    __metatable = 'runtime_lua_eval:debug'\n})\nlocal __aps_user_coroutine = setmetatable({}, {\n    __index = function(_, key)\n        if key == 'create' or key == 'wrap' or key == 'resume' then\n            return function()\n                error('coroutine.' .. key .. ' is disabled in runtime_lua_eval', 0)\n            end\n        end\n        return coroutine and coroutine[key]\n    end,\n    __newindex = function(tbl, key, value)\n        rawset(tbl, key, value)\n    end,\n    __metatable = 'runtime_lua_eval:coroutine'\n})\nlocal __aps_user_env\n__aps_user_env = setmetatable({\n    debug = __aps_user_debug,\n    coroutine = __aps_user_coroutine,\n}, {\n    __index = function(_, key)\n        if key == '_G' or key == '_ENV' then return __aps_user_env end\n        return __aps_global_env[key]\n    end,\n    __newindex = function(tbl, key, value)\n        rawset(tbl, key, value)\n    end,\n    __metatable = 'runtime_lua_eval:env'\n})\nrawset(__aps_user_env, '_G', __aps_user_env)\nrawset(__aps_user_env, '_ENV', __aps_user_env)\nreturn __aps_user_env\n", "runtime_lua_eval_env");
		if (array == null || array.Length == 0 || !(array[0] is LuaTable result))
		{
			DisposeTopLevelLuaResults(array);
			throw new Exception("Failed to create runtime_lua_eval environment.");
		}
		return result;
	}

	private static string BuildEvalSource(string code, string resultExpr)
	{
		if (!string.IsNullOrEmpty(resultExpr))
		{
			return (code ?? string.Empty) + "\nreturn (" + resultExpr + ")\n";
		}
		return code ?? string.Empty;
	}

	private static object[] ExecuteInSafeMode(LuaEnv env, string source, string chunkName, LuaTable evalEnv)
	{
		return env.DoString(source, chunkName, evalEnv);
	}

	private static object NormalizeLuaResults(object[] raw)
	{
		if (raw == null || raw.Length == 0)
		{
			return null;
		}
		Dictionary<LuaTable, object> luaTableCache = new Dictionary<LuaTable, object>();
		if (raw.Length == 1)
		{
			return NormalizeValue(raw[0], 0, luaTableCache);
		}
		List<object> list = new List<object>(Math.Min(raw.Length, 32));
		int num = Math.Min(raw.Length, 32);
		for (int i = 0; i < num; i++)
		{
			list.Add(NormalizeValue(raw[i], 0, luaTableCache));
		}
		if (raw.Length > num)
		{
			list.Add("... (" + (raw.Length - num) + " more)");
		}
		return list;
	}

	private static object NormalizeValue(object value, int depth, Dictionary<LuaTable, object> luaTableCache)
	{
		if (value == null)
		{
			return null;
		}
		if (value is string || value is bool || value is char || value is byte || value is sbyte || value is short || value is ushort || value is int || value is uint || value is long || value is ulong || value is float || value is double || value is decimal)
		{
			return value;
		}
		if (value is Enum obj)
		{
			return obj.ToString();
		}
		if (value is LuaTable luaTable)
		{
			if (luaTableCache != null && luaTableCache.TryGetValue(luaTable, out var value2))
			{
				if (value2 != RecursiveLuaTableSentinel)
				{
					return value2;
				}
				return "(recursive table)";
			}
			if (luaTableCache != null)
			{
				luaTableCache[luaTable] = RecursiveLuaTableSentinel;
			}
			object obj2 = NormalizeAndDisposeLuaTable(luaTable, depth, luaTableCache);
			if (luaTableCache != null)
			{
				luaTableCache[luaTable] = obj2;
			}
			return obj2;
		}
		if (value is LuaBase luaBase)
		{
			return NormalizeAndDisposeLuaBase(luaBase);
		}
		if (value is UnityEngine.Object unityObj)
		{
			return NormalizeUnityObject(unityObj);
		}
		if (depth >= 3)
		{
			return SafeToString(value);
		}
		if (value is IDictionary dictionary)
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>(StringComparer.Ordinal);
			int num = 0;
			foreach (DictionaryEntry item in dictionary)
			{
				if (num++ >= 20)
				{
					dictionary2["__truncated"] = true;
					break;
				}
				string text = ((item.Key == null) ? "null" : item.Key.ToString());
				if (string.IsNullOrEmpty(text))
				{
					text = "__null_key";
				}
				dictionary2[text] = NormalizeValue(item.Value, depth + 1, luaTableCache);
			}
			return dictionary2;
		}
		if (value is IEnumerable enumerable)
		{
			List<object> list = new List<object>();
			int num2 = 0;
			foreach (object item2 in enumerable)
			{
				if (num2++ >= 20)
				{
					list.Add("... (truncated)");
					break;
				}
				list.Add(NormalizeValue(item2, depth + 1, luaTableCache));
			}
			return list;
		}
		return SafeToString(value);
	}

	private static object NormalizeAndDisposeLuaTable(LuaTable luaTable, int depth, Dictionary<LuaTable, object> luaTableCache)
	{
		if (luaTable == null)
		{
			return null;
		}
		try
		{
			return NormalizeLuaTable(luaTable, depth, luaTableCache);
		}
		finally
		{
			try
			{
				luaTable.Dispose();
			}
			catch
			{
			}
		}
	}

	private static object NormalizeLuaTable(LuaTable luaTable, int depth, Dictionary<LuaTable, object> luaTableCache)
	{
		if (luaTable == null)
		{
			return null;
		}
		if (depth >= 3)
		{
			return SafeToString(luaTable);
		}
		bool truncated;
		List<KeyValuePair<object, object>> list = EnumerateLuaTableEntries(luaTable, 21, out truncated);
		if (list.Count == 0)
		{
			return new Dictionary<string, object>(StringComparer.Ordinal);
		}
		int num = 0;
		try
		{
			num = Math.Max(0, luaTable.Length);
		}
		catch
		{
			num = 0;
		}
		if (num > 0 && LooksLikeArraySample(list))
		{
			int num2 = Math.Min(num, 20);
			List<object> list2 = new List<object>(num2 + ((num > 20) ? 1 : 0));
			for (int i = 1; i <= num2; i++)
			{
				luaTable.Get<int, object>(i, out var value);
				list2.Add(NormalizeValue(value, depth + 1, luaTableCache));
			}
			if (num > 20)
			{
				list2.Add("... (truncated)");
			}
			return list2;
		}
		return NormalizeLuaKeyedEntries(list, depth, luaTableCache, truncated);
	}

	private static object NormalizeLuaKeyedEntries(List<KeyValuePair<object, object>> entries, int depth, Dictionary<LuaTable, object> luaTableCache, bool truncated)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.Ordinal);
		List<object> list = new List<object>();
		List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		bool flag = false;
		int num = Math.Min(entries.Count, 20);
		for (int i = 0; i < num; i++)
		{
			object key = entries[i].Key;
			string text = ((key == null) ? "null" : key.ToString());
			if (string.IsNullOrEmpty(text))
			{
				text = "__null_key";
			}
			object value = NormalizeValue(entries[i].Value, depth + 1, luaTableCache);
			Dictionary<string, object> item = new Dictionary<string, object>(StringComparer.Ordinal)
			{
				["key"] = NormalizeValue(key, depth + 1, luaTableCache),
				["keyText"] = text,
				["value"] = value
			};
			if (!hashSet.Add(text))
			{
				flag = true;
			}
			if (!flag)
			{
				list2.Add(item);
				dictionary[text] = value;
				continue;
			}
			if (list.Count == 0)
			{
				list.AddRange(list2);
			}
			list.Add(item);
		}
		if (!flag)
		{
			if (truncated || entries.Count > 20)
			{
				dictionary["__truncated"] = true;
			}
			return dictionary;
		}
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>(StringComparer.Ordinal) { ["__entries"] = list };
		if (truncated || entries.Count > 20)
		{
			dictionary2["__truncated"] = true;
		}
		return dictionary2;
	}

	private static bool LooksLikeArraySample(List<KeyValuePair<object, object>> entries)
	{
		if (entries == null || entries.Count == 0)
		{
			return false;
		}
		HashSet<int> hashSet = new HashSet<int>();
		int num = 0;
		for (int i = 0; i < entries.Count; i++)
		{
			if (!TryConvertLuaArrayIndex(entries[i].Key, out var index) || index <= 0)
			{
				return false;
			}
			if (!hashSet.Add(index))
			{
				return false;
			}
			if (index > num)
			{
				num = index;
			}
		}
		return num == hashSet.Count;
	}

	private static List<KeyValuePair<object, object>> EnumerateLuaTableEntries(LuaTable luaTable, int maxEntries, out bool truncated)
	{
		truncated = false;
		List<KeyValuePair<object, object>> entries = new List<KeyValuePair<object, object>>();
		if (luaTable == null)
		{
			return entries;
		}
		if (!TryEnumerateLuaTableEntriesFast(luaTable, maxEntries, entries, ref truncated))
		{
			bool fallbackTruncated = false;
			luaTable.ForEach(delegate(object key, object entryValue)
			{
				if (entries.Count >= maxEntries)
				{
					fallbackTruncated = true;
				}
				else
				{
					entries.Add(new KeyValuePair<object, object>(key, entryValue));
				}
			});
			truncated = fallbackTruncated;
		}
		if (entries.Count > maxEntries)
		{
			entries.RemoveRange(maxEntries, entries.Count - maxEntries);
		}
		if (entries.Count > 20)
		{
			truncated = true;
			entries.RemoveRange(20, entries.Count - 20);
		}
		return entries;
	}

	private static bool TryEnumerateLuaTableEntriesFast(LuaTable luaTable, int maxEntries, List<KeyValuePair<object, object>> entries, ref bool truncated)
	{
		try
		{
			Type baseType = typeof(LuaTable).BaseType;
			FieldInfo fieldInfo = ((baseType != null) ? baseType.GetField("luaReference", BindingFlags.Instance | BindingFlags.NonPublic) : null);
			FieldInfo fieldInfo2 = ((baseType != null) ? baseType.GetField("luaEnv", BindingFlags.Instance | BindingFlags.NonPublic) : null);
			if (fieldInfo == null || fieldInfo2 == null)
			{
				return false;
			}
			object value = fieldInfo2.GetValue(luaTable);
			if (value == null)
			{
				return false;
			}
			Type type = value.GetType();
			PropertyInfo property = type.GetProperty("L", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			FieldInfo field = type.GetField("translator", BindingFlags.Instance | BindingFlags.NonPublic);
			PropertyInfo property2 = type.GetProperty("luaEnvLock", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property == null || field == null || property2 == null)
			{
				return false;
			}
			object value2 = field.GetValue(value);
			object value3 = property2.GetValue(value, null);
			if (value2 == null || value3 == null)
			{
				return false;
			}
			MethodInfo method = value2.GetType().GetMethod("GetObject", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[2]
			{
				typeof(IntPtr),
				typeof(int)
			}, null);
			if (method == null)
			{
				return false;
			}
			lock (value3)
			{
				IntPtr intPtr = (IntPtr)property.GetValue(value, null);
				int newTop = Lua.lua_gettop(intPtr);
				try
				{
					Lua.lua_getref(intPtr, (int)fieldInfo.GetValue(luaTable));
					Lua.lua_pushnil(intPtr);
					while (Lua.lua_next(intPtr, -2) != 0)
					{
						if (entries.Count >= maxEntries)
						{
							truncated = true;
							Lua.lua_pop(intPtr, 1);
							break;
						}
						object key = method.Invoke(value2, new object[2] { intPtr, -2 });
						object value4 = method.Invoke(value2, new object[2] { intPtr, -1 });
						entries.Add(new KeyValuePair<object, object>(key, value4));
						Lua.lua_pop(intPtr, 1);
					}
				}
				finally
				{
					Lua.lua_settop(intPtr, newTop);
				}
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	private static object NormalizeAndDisposeLuaBase(LuaBase luaBase)
	{
		if (luaBase == null)
		{
			return null;
		}
		try
		{
			return SafeToString(luaBase);
		}
		finally
		{
			try
			{
				luaBase.Dispose();
			}
			catch
			{
			}
		}
	}

	private static object NormalizeUnityObject(UnityEngine.Object unityObj)
	{
		if ((object)unityObj == null)
		{
			return new
			{
				type = "Object",
				name = "(destroyed)",
				instanceId = 0,
				destroyed = true
			};
		}
		string type = "Object";
		int instanceId = 0;
		try
		{
			type = unityObj.GetType().Name;
		}
		catch
		{
		}
		try
		{
			instanceId = unityObj.GetInstanceID();
		}
		catch
		{
		}
		if (unityObj == null)
		{
			return new
			{
				type = type,
				name = "(destroyed)",
				instanceId = instanceId,
				destroyed = true
			};
		}
		try
		{
			return new
			{
				type = type,
				name = unityObj.name,
				instanceId = instanceId,
				destroyed = false
			};
		}
		catch (MissingReferenceException)
		{
			return new
			{
				type = type,
				name = "(destroyed)",
				instanceId = instanceId,
				destroyed = true
			};
		}
	}

	private static string SafeToString(object value)
	{
		if (value == null)
		{
			return null;
		}
		try
		{
			return value.ToString();
		}
		catch (MissingReferenceException)
		{
			if (value is UnityEngine.Object obj)
			{
				string text = "Object";
				try
				{
					text = obj.GetType().Name;
				}
				catch
				{
				}
				return "(destroyed " + text + ")";
			}
			return "(destroyed object)";
		}
		catch
		{
			return "(" + value.GetType().Name + ")";
		}
	}

	private static void DisposeTopLevelLuaResults(object[] raw)
	{
		if (raw == null)
		{
			return;
		}
		for (int i = 0; i < raw.Length; i++)
		{
			if (raw[i] is LuaBase luaBase)
			{
				try
				{
					luaBase.Dispose();
				}
				catch
				{
				}
			}
		}
	}

	private static void DisposeLuaWrapperIfNeeded(object value)
	{
		if (!(value is LuaBase luaBase))
		{
			return;
		}
		try
		{
			luaBase.Dispose();
		}
		catch
		{
		}
	}

	private static bool TryConvertLuaArrayIndex(object key, out int index)
	{
		if (key != null)
		{
			if (key is int num)
			{
				int num2 = num;
				index = num2;
				return true;
			}
			if (key is long num3)
			{
				long num4 = num3;
				if (num4 <= int.MaxValue && num4 >= int.MinValue)
				{
					index = (int)num4;
					return true;
				}
			}
			if (key is uint num5)
			{
				uint num6 = num5;
				if (num6 <= int.MaxValue)
				{
					index = (int)num6;
					return true;
				}
			}
			if (key is double num7)
			{
				double num8 = num7;
				if (Math.Abs(num8 % 1.0) < 1E-06 && num8 <= 2147483647.0 && num8 >= -2147483648.0)
				{
					index = (int)num8;
					return true;
				}
			}
			if (key is float num9)
			{
				float num10 = num9;
				if (Math.Abs(num10 % 1f) < 1E-06f && num10 <= 2.1474836E+09f && num10 >= -2.1474836E+09f)
				{
					index = (int)num10;
					return true;
				}
			}
		}
		index = 0;
		return false;
	}
}
