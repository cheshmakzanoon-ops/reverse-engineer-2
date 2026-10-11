using System;
using MiniGame.Core;
using MiniGame.GGGo.Client;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MiniGameGGGoClientUIGGGoMainWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UIGGGoMain);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 14, 1, 0);
		Utils.RegisterFunc(L, -3, "BindUI", _m_BindUI);
		Utils.RegisterFunc(L, -3, "OnClickLeftBtn", _m_OnClickLeftBtn);
		Utils.RegisterFunc(L, -3, "OnClickRightBtn", _m_OnClickRightBtn);
		Utils.RegisterFunc(L, -3, "OnClickItemBtn", _m_OnClickItemBtn);
		Utils.RegisterFunc(L, -3, "OnJoystickMove", _m_OnJoystickMove);
		Utils.RegisterFunc(L, -3, "OnJoystickEnd", _m_OnJoystickEnd);
		Utils.RegisterFunc(L, -3, "BindCallback", _m_BindCallback);
		Utils.RegisterFunc(L, -3, "UnBindCallback", _m_UnBindCallback);
		Utils.RegisterFunc(L, -3, "StartGame", _m_StartGame);
		Utils.RegisterFunc(L, -3, "EndGame", _m_EndGame);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -3, "IsDone", _m_IsDone);
		Utils.RegisterFunc(L, -3, "ConnectGameLift", _m_ConnectGameLift);
		Utils.RegisterFunc(L, -3, "StartGameByPvpConnect", _m_StartGameByPvpConnect);
		Utils.RegisterFunc(L, -2, "IsRuntime", _g_get_IsRuntime);
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
				UIGGGoMain o = new UIGGGoMain();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MiniGame.GGGo.Client.UIGGGoMain constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BindUI(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGGGoMain uIGGGoMain = (UIGGGoMain)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<GameWorld>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				GameWorld world = (GameWorld)objectTranslator.GetObject(L, 2, typeof(GameWorld));
				bool isEdit = Lua.lua_toboolean(L, 3);
				uIGGGoMain.BindUI(world, isEdit);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<GameWorld>(L, 2))
			{
				GameWorld world2 = (GameWorld)objectTranslator.GetObject(L, 2, typeof(GameWorld));
				uIGGGoMain.BindUI(world2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MiniGame.GGGo.Client.UIGGGoMain.BindUI!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnClickLeftBtn(IntPtr L)
	{
		try
		{
			((UIGGGoMain)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnClickLeftBtn();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnClickRightBtn(IntPtr L)
	{
		try
		{
			((UIGGGoMain)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnClickRightBtn();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnClickItemBtn(IntPtr L)
	{
		try
		{
			((UIGGGoMain)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnClickItemBtn();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnJoystickMove(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGGGoMain uIGGGoMain = (UIGGGoMain)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			uIGGGoMain.OnJoystickMove(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnJoystickEnd(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGGGoMain uIGGGoMain = (UIGGGoMain)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			uIGGGoMain.OnJoystickEnd(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BindCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGGGoMain uIGGGoMain = (UIGGGoMain)objectTranslator.FastGetCSObj(L, 1);
			Action<IRender> handle = objectTranslator.GetDelegate<Action<IRender>>(L, 2);
			uIGGGoMain.BindCallback(handle);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnBindCallback(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGGGoMain uIGGGoMain = (UIGGGoMain)objectTranslator.FastGetCSObj(L, 1);
			Action<IRender> handle = objectTranslator.GetDelegate<Action<IRender>>(L, 2);
			uIGGGoMain.UnBindCallback(handle);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartGame(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGGGoMain uIGGGoMain = (UIGGGoMain)objectTranslator.FastGetCSObj(L, 1);
			string levelPath = Lua.lua_tostring(L, 2);
			Transform gameRoot = (Transform)objectTranslator.GetObject(L, 3, typeof(Transform));
			uIGGGoMain.StartGame(levelPath, gameRoot);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EndGame(IntPtr L)
	{
		try
		{
			((UIGGGoMain)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EndGame();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Dispose(IntPtr L)
	{
		try
		{
			((UIGGGoMain)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDone(IntPtr L)
	{
		try
		{
			bool value = ((UIGGGoMain)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsDone();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ConnectGameLift(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UIGGGoMain uIGGGoMain = (UIGGGoMain)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 9 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TTABLE) && objectTranslator.Assignable<Action>(L, 5) && objectTranslator.Assignable<Action>(L, 6) && objectTranslator.Assignable<Action>(L, 7) && objectTranslator.Assignable<Action>(L, 8) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 9))
			{
				int serverId = Lua.xlua_tointeger(L, 2);
				string levelPath = Lua.lua_tostring(L, 3);
				LuaTable data = (LuaTable)objectTranslator.GetObject(L, 4, typeof(LuaTable));
				Action onDisconnected = objectTranslator.GetDelegate<Action>(L, 5);
				Action onConnected = objectTranslator.GetDelegate<Action>(L, 6);
				Action onTryReconnect = objectTranslator.GetDelegate<Action>(L, 7);
				Action onReconnected = objectTranslator.GetDelegate<Action>(L, 8);
				bool useHybridNetwork = Lua.lua_toboolean(L, 9);
				uIGGGoMain.ConnectGameLift(serverId, levelPath, data, onDisconnected, onConnected, onTryReconnect, onReconnected, useHybridNetwork);
				return 0;
			}
			if (num == 8 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TTABLE) && objectTranslator.Assignable<Action>(L, 5) && objectTranslator.Assignable<Action>(L, 6) && objectTranslator.Assignable<Action>(L, 7) && objectTranslator.Assignable<Action>(L, 8))
			{
				int serverId2 = Lua.xlua_tointeger(L, 2);
				string levelPath2 = Lua.lua_tostring(L, 3);
				LuaTable data2 = (LuaTable)objectTranslator.GetObject(L, 4, typeof(LuaTable));
				Action onDisconnected2 = objectTranslator.GetDelegate<Action>(L, 5);
				Action onConnected2 = objectTranslator.GetDelegate<Action>(L, 6);
				Action onTryReconnect2 = objectTranslator.GetDelegate<Action>(L, 7);
				Action onReconnected2 = objectTranslator.GetDelegate<Action>(L, 8);
				uIGGGoMain.ConnectGameLift(serverId2, levelPath2, data2, onDisconnected2, onConnected2, onTryReconnect2, onReconnected2);
				return 0;
			}
			if (num == 7 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TTABLE) && objectTranslator.Assignable<Action>(L, 5) && objectTranslator.Assignable<Action>(L, 6) && objectTranslator.Assignable<Action>(L, 7))
			{
				int serverId3 = Lua.xlua_tointeger(L, 2);
				string levelPath3 = Lua.lua_tostring(L, 3);
				LuaTable data3 = (LuaTable)objectTranslator.GetObject(L, 4, typeof(LuaTable));
				Action onDisconnected3 = objectTranslator.GetDelegate<Action>(L, 5);
				Action onConnected3 = objectTranslator.GetDelegate<Action>(L, 6);
				Action onTryReconnect3 = objectTranslator.GetDelegate<Action>(L, 7);
				uIGGGoMain.ConnectGameLift(serverId3, levelPath3, data3, onDisconnected3, onConnected3, onTryReconnect3);
				return 0;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TTABLE) && objectTranslator.Assignable<Action>(L, 5) && objectTranslator.Assignable<Action>(L, 6))
			{
				int serverId4 = Lua.xlua_tointeger(L, 2);
				string levelPath4 = Lua.lua_tostring(L, 3);
				LuaTable data4 = (LuaTable)objectTranslator.GetObject(L, 4, typeof(LuaTable));
				Action onDisconnected4 = objectTranslator.GetDelegate<Action>(L, 5);
				Action onConnected4 = objectTranslator.GetDelegate<Action>(L, 6);
				uIGGGoMain.ConnectGameLift(serverId4, levelPath4, data4, onDisconnected4, onConnected4);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TTABLE) && objectTranslator.Assignable<Action>(L, 5))
			{
				int serverId5 = Lua.xlua_tointeger(L, 2);
				string levelPath5 = Lua.lua_tostring(L, 3);
				LuaTable data5 = (LuaTable)objectTranslator.GetObject(L, 4, typeof(LuaTable));
				Action onDisconnected5 = objectTranslator.GetDelegate<Action>(L, 5);
				uIGGGoMain.ConnectGameLift(serverId5, levelPath5, data5, onDisconnected5);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TTABLE))
			{
				int serverId6 = Lua.xlua_tointeger(L, 2);
				string levelPath6 = Lua.lua_tostring(L, 3);
				LuaTable data6 = (LuaTable)objectTranslator.GetObject(L, 4, typeof(LuaTable));
				uIGGGoMain.ConnectGameLift(serverId6, levelPath6, data6);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to MiniGame.GGGo.Client.UIGGGoMain.ConnectGameLift!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartGameByPvpConnect(IntPtr L)
	{
		try
		{
			((UIGGGoMain)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StartGameByPvpConnect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsRuntime(IntPtr L)
	{
		try
		{
			UIGGGoMain uIGGGoMain = (UIGGGoMain)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, uIGGGoMain.IsRuntime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
