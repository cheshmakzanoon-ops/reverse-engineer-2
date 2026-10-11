using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UIHealthBarControllerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UIHealthBarController);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 0, 0);
		Utils.RegisterFunc(L, -3, "SetHealth", _m_SetHealth);
		Utils.RegisterFunc(L, -3, "SetDividerUnit", _m_SetDividerUnit);
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
				UIHealthBarController o = new UIHealthBarController();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIHealthBarController constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetHealth(IntPtr L)
	{
		try
		{
			UIHealthBarController obj = (UIHealthBarController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float cur = (float)Lua.lua_tonumber(L, 2);
			float max = (float)Lua.lua_tonumber(L, 3);
			obj.SetHealth(cur, max);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDividerUnit(IntPtr L)
	{
		try
		{
			UIHealthBarController obj = (UIHealthBarController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float dividerUnit = (float)Lua.lua_tonumber(L, 2);
			obj.SetDividerUnit(dividerUnit);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
