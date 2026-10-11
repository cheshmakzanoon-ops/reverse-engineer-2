using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PayOrderDataManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PayOrderDataManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 16, 0, 0);
		Utils.RegisterFunc(L, -3, "IsOrderConsumed", _m_IsOrderConsumed);
		Utils.RegisterFunc(L, -3, "AddOrderToConsumedList", _m_AddOrderToConsumedList);
		Utils.RegisterFunc(L, -3, "SetConsumedOrderDetectFunctionOpen", _m_SetConsumedOrderDetectFunctionOpen);
		Utils.RegisterFunc(L, -3, "IsConsumedOrderDetectFunctionOpen", _m_IsConsumedOrderDetectFunctionOpen);
		Utils.RegisterFunc(L, -3, "SaveNativeQueryPriceResult", _m_SaveNativeQueryPriceResult);
		Utils.RegisterFunc(L, -3, "GetNativeQueryPriceResult", _m_GetNativeQueryPriceResult);
		Utils.RegisterFunc(L, -3, "SaveStorefrontCode", _m_SaveStorefrontCode);
		Utils.RegisterFunc(L, -3, "GetStorefrontCode", _m_GetStorefrontCode);
		Utils.RegisterFunc(L, -3, "SaveExternalCheckoutUrl", _m_SaveExternalCheckoutUrl);
		Utils.RegisterFunc(L, -3, "GetExternalCheckoutUrl", _m_GetExternalCheckoutUrl);
		Utils.RegisterFunc(L, -3, "SaveExternalCheckoutToken", _m_SaveExternalCheckoutToken);
		Utils.RegisterFunc(L, -3, "GetExternalCheckoutToken", _m_GetExternalCheckoutToken);
		Utils.RegisterFunc(L, -3, "SaveExternalCheckoutProgram", _m_SaveExternalCheckoutProgram);
		Utils.RegisterFunc(L, -3, "GetExternalCheckoutProgram", _m_GetExternalCheckoutProgram);
		Utils.RegisterFunc(L, -3, "SaveExternalCheckoutLastEvent", _m_SaveExternalCheckoutLastEvent);
		Utils.RegisterFunc(L, -3, "GetExternalCheckoutLastEvent", _m_GetExternalCheckoutLastEvent);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 1)
			{
				PayOrderDataManager o = new PayOrderDataManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PayOrderDataManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsOrderConsumed(IntPtr L)
	{
		try
		{
			PayOrderDataManager obj = (PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string orderId = Lua.lua_tostring(L, 2);
			bool value = obj.IsOrderConsumed(orderId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddOrderToConsumedList(IntPtr L)
	{
		try
		{
			PayOrderDataManager obj = (PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string orderId = Lua.lua_tostring(L, 2);
			obj.AddOrderToConsumedList(orderId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetConsumedOrderDetectFunctionOpen(IntPtr L)
	{
		try
		{
			PayOrderDataManager obj = (PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool consumedOrderDetectFunctionOpen = Lua.lua_toboolean(L, 2);
			obj.SetConsumedOrderDetectFunctionOpen(consumedOrderDetectFunctionOpen);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsConsumedOrderDetectFunctionOpen(IntPtr L)
	{
		try
		{
			bool value = ((PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsConsumedOrderDetectFunctionOpen();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SaveNativeQueryPriceResult(IntPtr L)
	{
		try
		{
			PayOrderDataManager obj = (PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string data = Lua.lua_tostring(L, 2);
			obj.SaveNativeQueryPriceResult(data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetNativeQueryPriceResult(IntPtr L)
	{
		try
		{
			string nativeQueryPriceResult = ((PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetNativeQueryPriceResult();
			Lua.lua_pushstring(L, nativeQueryPriceResult);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SaveStorefrontCode(IntPtr L)
	{
		try
		{
			PayOrderDataManager obj = (PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string code = Lua.lua_tostring(L, 2);
			obj.SaveStorefrontCode(code);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetStorefrontCode(IntPtr L)
	{
		try
		{
			string storefrontCode = ((PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetStorefrontCode();
			Lua.lua_pushstring(L, storefrontCode);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SaveExternalCheckoutUrl(IntPtr L)
	{
		try
		{
			PayOrderDataManager obj = (PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string url = Lua.lua_tostring(L, 2);
			obj.SaveExternalCheckoutUrl(url);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetExternalCheckoutUrl(IntPtr L)
	{
		try
		{
			string externalCheckoutUrl = ((PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetExternalCheckoutUrl();
			Lua.lua_pushstring(L, externalCheckoutUrl);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SaveExternalCheckoutToken(IntPtr L)
	{
		try
		{
			PayOrderDataManager obj = (PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string token = Lua.lua_tostring(L, 2);
			obj.SaveExternalCheckoutToken(token);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetExternalCheckoutToken(IntPtr L)
	{
		try
		{
			string externalCheckoutToken = ((PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetExternalCheckoutToken();
			Lua.lua_pushstring(L, externalCheckoutToken);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SaveExternalCheckoutProgram(IntPtr L)
	{
		try
		{
			PayOrderDataManager obj = (PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string program = Lua.lua_tostring(L, 2);
			obj.SaveExternalCheckoutProgram(program);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetExternalCheckoutProgram(IntPtr L)
	{
		try
		{
			string externalCheckoutProgram = ((PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetExternalCheckoutProgram();
			Lua.lua_pushstring(L, externalCheckoutProgram);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SaveExternalCheckoutLastEvent(IntPtr L)
	{
		try
		{
			PayOrderDataManager obj = (PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string eventName = Lua.lua_tostring(L, 2);
			obj.SaveExternalCheckoutLastEvent(eventName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetExternalCheckoutLastEvent(IntPtr L)
	{
		try
		{
			string externalCheckoutLastEvent = ((PayOrderDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetExternalCheckoutLastEvent();
			Lua.lua_pushstring(L, externalCheckoutLastEvent);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
