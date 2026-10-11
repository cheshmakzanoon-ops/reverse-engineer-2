using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ExternalCheckoutCoordinatorWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ExternalCheckoutCoordinator);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 9, 0, 0);
		Utils.RegisterFunc(L, -3, "PrepareUsExternalCheckout", _m_PrepareUsExternalCheckout);
		Utils.RegisterFunc(L, -3, "CheckUsExternalCheckoutAvailability", _m_CheckUsExternalCheckoutAvailability);
		Utils.RegisterFunc(L, -3, "LaunchConfiguredUsExternalCheckout", _m_LaunchConfiguredUsExternalCheckout);
		Utils.RegisterFunc(L, -3, "HandleNativeOpened", _m_HandleNativeOpened);
		Utils.RegisterFunc(L, -3, "HandleNativeClosed", _m_HandleNativeClosed);
		Utils.RegisterFunc(L, -3, "HandleNativeFailed", _m_HandleNativeFailed);
		Utils.RegisterFunc(L, -3, "HandleNativeLaunchApproved", _m_HandleNativeLaunchApproved);
		Utils.RegisterFunc(L, -3, "HandleNativeToken", _m_HandleNativeToken);
		Utils.RegisterFunc(L, -3, "HandleNativeAvailability", _m_HandleNativeAvailability);
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
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<Action<string, string>>(L, 2))
			{
				ExternalCheckoutCoordinator o = new ExternalCheckoutCoordinator(objectTranslator.GetDelegate<Action<string, string>>(L, 2));
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ExternalCheckoutCoordinator constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PrepareUsExternalCheckout(IntPtr L)
	{
		try
		{
			ExternalCheckoutCoordinator externalCheckoutCoordinator = (ExternalCheckoutCoordinator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool preferWebView = Lua.lua_toboolean(L, 2);
				bool value = externalCheckoutCoordinator.PrepareUsExternalCheckout(preferWebView);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 1)
			{
				bool value2 = externalCheckoutCoordinator.PrepareUsExternalCheckout();
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ExternalCheckoutCoordinator.PrepareUsExternalCheckout!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckUsExternalCheckoutAvailability(IntPtr L)
	{
		try
		{
			bool value = ((ExternalCheckoutCoordinator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CheckUsExternalCheckoutAvailability();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LaunchConfiguredUsExternalCheckout(IntPtr L)
	{
		try
		{
			ExternalCheckoutCoordinator externalCheckoutCoordinator = (ExternalCheckoutCoordinator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool preferWebView = Lua.lua_toboolean(L, 2);
				bool value = externalCheckoutCoordinator.LaunchConfiguredUsExternalCheckout(preferWebView);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 1)
			{
				bool value2 = externalCheckoutCoordinator.LaunchConfiguredUsExternalCheckout();
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ExternalCheckoutCoordinator.LaunchConfiguredUsExternalCheckout!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleNativeOpened(IntPtr L)
	{
		try
		{
			ExternalCheckoutCoordinator obj = (ExternalCheckoutCoordinator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string data = Lua.lua_tostring(L, 2);
			obj.HandleNativeOpened(data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleNativeClosed(IntPtr L)
	{
		try
		{
			ExternalCheckoutCoordinator obj = (ExternalCheckoutCoordinator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string data = Lua.lua_tostring(L, 2);
			string str = obj.HandleNativeClosed(data);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleNativeFailed(IntPtr L)
	{
		try
		{
			ExternalCheckoutCoordinator obj = (ExternalCheckoutCoordinator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string data = Lua.lua_tostring(L, 2);
			string str = obj.HandleNativeFailed(data);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleNativeLaunchApproved(IntPtr L)
	{
		try
		{
			ExternalCheckoutCoordinator obj = (ExternalCheckoutCoordinator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string data = Lua.lua_tostring(L, 2);
			obj.HandleNativeLaunchApproved(data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleNativeToken(IntPtr L)
	{
		try
		{
			ExternalCheckoutCoordinator obj = (ExternalCheckoutCoordinator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string data = Lua.lua_tostring(L, 2);
			string str = obj.HandleNativeToken(data);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleNativeAvailability(IntPtr L)
	{
		try
		{
			ExternalCheckoutCoordinator obj = (ExternalCheckoutCoordinator)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string data = Lua.lua_tostring(L, 2);
			obj.HandleNativeAvailability(data);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
