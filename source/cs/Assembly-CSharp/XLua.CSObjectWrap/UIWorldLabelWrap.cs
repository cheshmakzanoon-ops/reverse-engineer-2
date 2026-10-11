using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UIWorldLabelWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UIWorldLabel);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 10, 0, 0);
		Utils.RegisterFunc(L, -3, "SetLevel", _m_SetLevel);
		Utils.RegisterFunc(L, -3, "SetTip", _m_SetTip);
		Utils.RegisterFunc(L, -3, "SetName", _m_SetName);
		Utils.RegisterFunc(L, -3, "SetNameMaterial", _m_SetNameMaterial);
		Utils.RegisterFunc(L, -3, "SetNameColor", _m_SetNameColor);
		Utils.RegisterFunc(L, -3, "ShowNameTitle", _m_ShowNameTitle);
		Utils.RegisterFunc(L, -3, "ShowFlag", _m_ShowFlag);
		Utils.RegisterFunc(L, -3, "SetFlag", _m_SetFlag);
		Utils.RegisterFunc(L, -3, "SetNameBgSkin", _m_SetNameBgSkin);
		Utils.RegisterFunc(L, -3, "SetFireworkQuickMode", _m_SetFireworkQuickMode);
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
				UIWorldLabel o = new UIWorldLabel();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIWorldLabel constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLevel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIWorldLabel uIWorldLabel = (UIWorldLabel)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int level = Lua.xlua_tointeger(L, 2);
				uIWorldLabel.SetLevel(level);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool level2 = Lua.lua_toboolean(L, 2);
				uIWorldLabel.SetLevel(level2);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string level3 = Lua.lua_tostring(L, 2);
				uIWorldLabel.SetLevel(level3);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<GameDefines.CityLabelColorType>(L, 3))
			{
				int l = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out GameDefines.CityLabelColorType val);
				uIWorldLabel.SetLevel(l, val);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIWorldLabel.SetLevel!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTip(IntPtr L)
	{
		try
		{
			UIWorldLabel obj = (UIWorldLabel)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string tip = Lua.lua_tostring(L, 2);
			obj.SetTip(tip);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetName(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIWorldLabel uIWorldLabel = (UIWorldLabel)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string name = Lua.lua_tostring(L, 2);
				uIWorldLabel.SetName(name);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<GameDefines.CityLabelColorType>(L, 3))
			{
				string n = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out GameDefines.CityLabelColorType val);
				uIWorldLabel.SetName(n, val);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIWorldLabel.SetName!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetNameMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIWorldLabel uIWorldLabel = (UIWorldLabel)objectTranslator.FastGetCSObj(L, 1);
			Material nameMaterial = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
			uIWorldLabel.SetNameMaterial(nameMaterial);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetNameColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIWorldLabel uIWorldLabel = (UIWorldLabel)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out GameDefines.CityLabelColorType val);
			uIWorldLabel.SetNameColor(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowNameTitle(IntPtr L)
	{
		try
		{
			UIWorldLabel obj = (UIWorldLabel)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool s = Lua.lua_toboolean(L, 2);
			obj.ShowNameTitle(s);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowFlag(IntPtr L)
	{
		try
		{
			UIWorldLabel obj = (UIWorldLabel)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool s = Lua.lua_toboolean(L, 2);
			obj.ShowFlag(s);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFlag(IntPtr L)
	{
		try
		{
			UIWorldLabel obj = (UIWorldLabel)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string flag = Lua.lua_tostring(L, 2);
			obj.SetFlag(flag);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetNameBgSkin(IntPtr L)
	{
		try
		{
			UIWorldLabel uIWorldLabel = (UIWorldLabel)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int nameBgSkin = Lua.xlua_tointeger(L, 2);
				uIWorldLabel.SetNameBgSkin(nameBgSkin);
				return 0;
			}
			if (num == 1)
			{
				uIWorldLabel.SetNameBgSkin();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIWorldLabel.SetNameBgSkin!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFireworkQuickMode(IntPtr L)
	{
		try
		{
			UIWorldLabel obj = (UIWorldLabel)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isInFireworkQuickMode = Lua.lua_toboolean(L, 2);
			string playerUid = Lua.lua_tostring(L, 3);
			obj.SetFireworkQuickMode(isInFireworkQuickMode, playerUid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
