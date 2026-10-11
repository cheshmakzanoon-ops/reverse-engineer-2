using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniGame.Biubiu.Client;
using MiniGame.Core;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MiniGameBiubiuClientFuncUdpLatencyWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(FuncUdpLatency);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 0, 0);
		Utils.RegisterFunc(L, -4, "PingAll", _m_PingAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetPingResults", _m_GetPingResults_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGameLiftServerPingValues", _m_GetGameLiftServerPingValues_xlua_st_);
		Utils.RegisterFunc(L, -4, "MeasureLatencyAsync", _m_MeasureLatencyAsync_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "MiniGame.Biubiu.Client.FuncUdpLatency does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PingAll_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int[] serverIds = (int[])objectTranslator.GetObject(L, 1, typeof(int[]));
			Action<string> complete = objectTranslator.GetDelegate<Action<string>>(L, 2);
			List<UtilsPing.PingRequest> o = FuncUdpLatency.PingAll(serverIds, complete);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPingResults_xlua_st_(IntPtr L)
	{
		try
		{
			string pingResults = FuncUdpLatency.GetPingResults((List<UtilsPing.PingRequest>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(List<UtilsPing.PingRequest>)));
			Lua.lua_pushstring(L, pingResults);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGameLiftServerPingValues_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<int[]>(L, 1) && objectTranslator.Assignable<Action<string>>(L, 2) && objectTranslator.Assignable<ConcurrentDictionary<string, double>>(L, 3))
			{
				int[] serverIds = (int[])objectTranslator.GetObject(L, 1, typeof(int[]));
				Action<string> complete = objectTranslator.GetDelegate<Action<string>>(L, 2);
				ConcurrentDictionary<string, double> pingResult = (ConcurrentDictionary<string, double>)objectTranslator.GetObject(L, 3, typeof(ConcurrentDictionary<string, double>));
				FuncUdpLatency.GetGameLiftServerPingValues(serverIds, complete, pingResult);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<int[]>(L, 1) && objectTranslator.Assignable<Action<string>>(L, 2))
			{
				int[] serverIds2 = (int[])objectTranslator.GetObject(L, 1, typeof(int[]));
				Action<string> complete2 = objectTranslator.GetDelegate<Action<string>>(L, 2);
				FuncUdpLatency.GetGameLiftServerPingValues(serverIds2, complete2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MiniGame.Biubiu.Client.FuncUdpLatency.GetGameLiftServerPingValues!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MeasureLatencyAsync_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 7 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<ConcurrentDictionary<string, double>>(L, 6) && (Lua.lua_isnil(L, 7) || Lua.lua_type(L, 7) == LuaTypes.LUA_TSTRING))
			{
				string domain = Lua.lua_tostring(L, 1);
				int port = Lua.xlua_tointeger(L, 2);
				string message = Lua.lua_tostring(L, 3);
				int numPings = Lua.xlua_tointeger(L, 4);
				int timeoutMs = Lua.xlua_tointeger(L, 5);
				ConcurrentDictionary<string, double> pingResult = (ConcurrentDictionary<string, double>)objectTranslator.GetObject(L, 6, typeof(ConcurrentDictionary<string, double>));
				string region = Lua.lua_tostring(L, 7);
				Task<double> o = FuncUdpLatency.MeasureLatencyAsync(domain, port, message, numPings, timeoutMs, pingResult, region);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 6 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<ConcurrentDictionary<string, double>>(L, 6))
			{
				string domain2 = Lua.lua_tostring(L, 1);
				int port2 = Lua.xlua_tointeger(L, 2);
				string message2 = Lua.lua_tostring(L, 3);
				int numPings2 = Lua.xlua_tointeger(L, 4);
				int timeoutMs2 = Lua.xlua_tointeger(L, 5);
				ConcurrentDictionary<string, double> pingResult2 = (ConcurrentDictionary<string, double>)objectTranslator.GetObject(L, 6, typeof(ConcurrentDictionary<string, double>));
				Task<double> o2 = FuncUdpLatency.MeasureLatencyAsync(domain2, port2, message2, numPings2, timeoutMs2, pingResult2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 5 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string domain3 = Lua.lua_tostring(L, 1);
				int port3 = Lua.xlua_tointeger(L, 2);
				string message3 = Lua.lua_tostring(L, 3);
				int numPings3 = Lua.xlua_tointeger(L, 4);
				int timeoutMs3 = Lua.xlua_tointeger(L, 5);
				Task<double> o3 = FuncUdpLatency.MeasureLatencyAsync(domain3, port3, message3, numPings3, timeoutMs3);
				objectTranslator.Push(L, o3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MiniGame.Biubiu.Client.FuncUdpLatency.MeasureLatencyAsync!");
	}
}
