using System;
using MiniGame.Biubiu.Client;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MiniGameBiubiuClientFuncUdpLatencySafeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(FuncUdpLatencySafe);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterFunc(L, -4, "PingAll", _m_PingAll_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGameLiftServerPingValues", _m_GetGameLiftServerPingValues_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "MiniGame.Biubiu.Client.FuncUdpLatencySafe does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PingAll_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int[] serverIds = (int[])objectTranslator.GetObject(L, 1, typeof(int[]));
			Action<string> complete = objectTranslator.GetDelegate<Action<string>>(L, 2);
			FuncUdpLatencySafe.PingAll(serverIds, complete);
			return 0;
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
			int[] serverIds = (int[])objectTranslator.GetObject(L, 1, typeof(int[]));
			Action<string> complete = objectTranslator.GetDelegate<Action<string>>(L, 2);
			FuncUdpLatencySafe.GetGameLiftServerPingValues(serverIds, complete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
