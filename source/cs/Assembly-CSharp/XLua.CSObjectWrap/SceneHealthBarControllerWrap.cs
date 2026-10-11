using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SceneHealthBarControllerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SceneHealthBarController);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 3, 3);
		Utils.RegisterFunc(L, -3, "SetCurAndMaxHP", _m_SetCurAndMaxHP);
		Utils.RegisterFunc(L, -3, "SetDividerUnit", _m_SetDividerUnit);
		Utils.RegisterFunc(L, -3, "UpdateMaterialProperties", _m_UpdateMaterialProperties);
		Utils.RegisterFunc(L, -2, "lineColor", _g_get_lineColor);
		Utils.RegisterFunc(L, -2, "lineWidth", _g_get_lineWidth);
		Utils.RegisterFunc(L, -2, "useQualityMode", _g_get_useQualityMode);
		Utils.RegisterFunc(L, -1, "lineColor", _s_set_lineColor);
		Utils.RegisterFunc(L, -1, "lineWidth", _s_set_lineWidth);
		Utils.RegisterFunc(L, -1, "useQualityMode", _s_set_useQualityMode);
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
				SceneHealthBarController o = new SceneHealthBarController();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SceneHealthBarController constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCurAndMaxHP(IntPtr L)
	{
		try
		{
			SceneHealthBarController obj = (SceneHealthBarController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float cur = (float)Lua.lua_tonumber(L, 2);
			float max = (float)Lua.lua_tonumber(L, 3);
			obj.SetCurAndMaxHP(cur, max);
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
			SceneHealthBarController obj = (SceneHealthBarController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float dividerUnit = (float)Lua.lua_tonumber(L, 2);
			obj.SetDividerUnit(dividerUnit);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateMaterialProperties(IntPtr L)
	{
		try
		{
			((SceneHealthBarController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateMaterialProperties();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lineColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneHealthBarController sceneHealthBarController = (SceneHealthBarController)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, sceneHealthBarController.lineColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lineWidth(IntPtr L)
	{
		try
		{
			SceneHealthBarController sceneHealthBarController = (SceneHealthBarController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, sceneHealthBarController.lineWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useQualityMode(IntPtr L)
	{
		try
		{
			SceneHealthBarController sceneHealthBarController = (SceneHealthBarController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, sceneHealthBarController.useQualityMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lineColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneHealthBarController sceneHealthBarController = (SceneHealthBarController)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			sceneHealthBarController.lineColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lineWidth(IntPtr L)
	{
		try
		{
			((SceneHealthBarController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lineWidth = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useQualityMode(IntPtr L)
	{
		try
		{
			((SceneHealthBarController)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useQualityMode = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
