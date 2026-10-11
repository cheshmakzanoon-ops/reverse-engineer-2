using System;
using Spine;
using Spine.Unity;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SpineUnitySkeletonAnimationWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SkeletonAnimation);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 9, 5, 4);
		Utils.RegisterFunc(L, -3, "ClearState", _m_ClearState);
		Utils.RegisterFunc(L, -3, "Initialize", _m_Initialize);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "LateUpdate", _m_LateUpdate);
		Utils.RegisterFunc(L, -3, "OnBecameVisible", _m_OnBecameVisible);
		Utils.RegisterFunc(L, -3, "BeforeApply", _e_BeforeApply);
		Utils.RegisterFunc(L, -3, "UpdateLocal", _e_UpdateLocal);
		Utils.RegisterFunc(L, -3, "UpdateWorld", _e_UpdateWorld);
		Utils.RegisterFunc(L, -3, "UpdateComplete", _e_UpdateComplete);
		Utils.RegisterFunc(L, -2, "AnimationState", _g_get_AnimationState);
		Utils.RegisterFunc(L, -2, "AnimationName", _g_get_AnimationName);
		Utils.RegisterFunc(L, -2, "state", _g_get_state);
		Utils.RegisterFunc(L, -2, "loop", _g_get_loop);
		Utils.RegisterFunc(L, -2, "timeScale", _g_get_timeScale);
		Utils.RegisterFunc(L, -1, "AnimationName", _s_set_AnimationName);
		Utils.RegisterFunc(L, -1, "state", _s_set_state);
		Utils.RegisterFunc(L, -1, "loop", _s_set_loop);
		Utils.RegisterFunc(L, -1, "timeScale", _s_set_timeScale);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterFunc(L, -4, "AddToGameObject", _m_AddToGameObject_xlua_st_);
		Utils.RegisterFunc(L, -4, "NewSkeletonAnimationGameObject", _m_NewSkeletonAnimationGameObject_xlua_st_);
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
				SkeletonAnimation o = new SkeletonAnimation();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonAnimation constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddToGameObject_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<GameObject>(L, 1) && objectTranslator.Assignable<SkeletonDataAsset>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				GameObject gameObject = (GameObject)objectTranslator.GetObject(L, 1, typeof(GameObject));
				SkeletonDataAsset skeletonDataAsset = (SkeletonDataAsset)objectTranslator.GetObject(L, 2, typeof(SkeletonDataAsset));
				bool quiet = Lua.lua_toboolean(L, 3);
				SkeletonAnimation o = SkeletonAnimation.AddToGameObject(gameObject, skeletonDataAsset, quiet);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<GameObject>(L, 1) && objectTranslator.Assignable<SkeletonDataAsset>(L, 2))
			{
				GameObject gameObject2 = (GameObject)objectTranslator.GetObject(L, 1, typeof(GameObject));
				SkeletonDataAsset skeletonDataAsset2 = (SkeletonDataAsset)objectTranslator.GetObject(L, 2, typeof(SkeletonDataAsset));
				SkeletonAnimation o2 = SkeletonAnimation.AddToGameObject(gameObject2, skeletonDataAsset2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonAnimation.AddToGameObject!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NewSkeletonAnimationGameObject_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<SkeletonDataAsset>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				SkeletonDataAsset skeletonDataAsset = (SkeletonDataAsset)objectTranslator.GetObject(L, 1, typeof(SkeletonDataAsset));
				bool quiet = Lua.lua_toboolean(L, 2);
				SkeletonAnimation o = SkeletonAnimation.NewSkeletonAnimationGameObject(skeletonDataAsset, quiet);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<SkeletonDataAsset>(L, 1))
			{
				SkeletonAnimation o2 = SkeletonAnimation.NewSkeletonAnimationGameObject((SkeletonDataAsset)objectTranslator.GetObject(L, 1, typeof(SkeletonDataAsset)));
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonAnimation.NewSkeletonAnimationGameObject!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearState(IntPtr L)
	{
		try
		{
			((SkeletonAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearState();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Initialize(IntPtr L)
	{
		try
		{
			SkeletonAnimation skeletonAnimation = (SkeletonAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				bool overwrite = Lua.lua_toboolean(L, 2);
				bool quiet = Lua.lua_toboolean(L, 3);
				skeletonAnimation.Initialize(overwrite, quiet);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool overwrite2 = Lua.lua_toboolean(L, 2);
				skeletonAnimation.Initialize(overwrite2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonAnimation.Initialize!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			SkeletonAnimation obj = (SkeletonAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaTime = (float)Lua.lua_tonumber(L, 2);
			obj.Update(deltaTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LateUpdate(IntPtr L)
	{
		try
		{
			((SkeletonAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LateUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBecameVisible(IntPtr L)
	{
		try
		{
			((SkeletonAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnBecameVisible();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AnimationState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonAnimation skeletonAnimation = (SkeletonAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonAnimation.AnimationState);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AnimationName(IntPtr L)
	{
		try
		{
			SkeletonAnimation skeletonAnimation = (SkeletonAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, skeletonAnimation.AnimationName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_state(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SkeletonAnimation skeletonAnimation = (SkeletonAnimation)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, skeletonAnimation.state);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loop(IntPtr L)
	{
		try
		{
			SkeletonAnimation skeletonAnimation = (SkeletonAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, skeletonAnimation.loop);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_timeScale(IntPtr L)
	{
		try
		{
			SkeletonAnimation skeletonAnimation = (SkeletonAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, skeletonAnimation.timeScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_AnimationName(IntPtr L)
	{
		try
		{
			((SkeletonAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).AnimationName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_state(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SkeletonAnimation)objectTranslator.FastGetCSObj(L, 1)).state = (Spine.AnimationState)objectTranslator.GetObject(L, 2, typeof(Spine.AnimationState));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loop(IntPtr L)
	{
		try
		{
			((SkeletonAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).loop = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_timeScale(IntPtr L)
	{
		try
		{
			((SkeletonAnimation)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).timeScale = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_BeforeApply(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			SkeletonAnimation skeletonAnimation = (SkeletonAnimation)objectTranslator.FastGetCSObj(L, 1);
			UpdateBonesDelegate updateBonesDelegate = objectTranslator.GetDelegate<UpdateBonesDelegate>(L, 3);
			if (updateBonesDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need Spine.Unity.UpdateBonesDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					skeletonAnimation.BeforeApply += updateBonesDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					skeletonAnimation.BeforeApply -= updateBonesDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonAnimation.BeforeApply!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_UpdateLocal(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			SkeletonAnimation skeletonAnimation = (SkeletonAnimation)objectTranslator.FastGetCSObj(L, 1);
			UpdateBonesDelegate updateBonesDelegate = objectTranslator.GetDelegate<UpdateBonesDelegate>(L, 3);
			if (updateBonesDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need Spine.Unity.UpdateBonesDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					skeletonAnimation.UpdateLocal += updateBonesDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					skeletonAnimation.UpdateLocal -= updateBonesDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonAnimation.UpdateLocal!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_UpdateWorld(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			SkeletonAnimation skeletonAnimation = (SkeletonAnimation)objectTranslator.FastGetCSObj(L, 1);
			UpdateBonesDelegate updateBonesDelegate = objectTranslator.GetDelegate<UpdateBonesDelegate>(L, 3);
			if (updateBonesDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need Spine.Unity.UpdateBonesDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					skeletonAnimation.UpdateWorld += updateBonesDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					skeletonAnimation.UpdateWorld -= updateBonesDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonAnimation.UpdateWorld!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_UpdateComplete(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			SkeletonAnimation skeletonAnimation = (SkeletonAnimation)objectTranslator.FastGetCSObj(L, 1);
			UpdateBonesDelegate updateBonesDelegate = objectTranslator.GetDelegate<UpdateBonesDelegate>(L, 3);
			if (updateBonesDelegate == null)
			{
				return Lua.luaL_error(L, "#3 need Spine.Unity.UpdateBonesDelegate!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					skeletonAnimation.UpdateComplete += updateBonesDelegate;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					skeletonAnimation.UpdateComplete -= updateBonesDelegate;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonAnimation.UpdateComplete!");
		return 0;
	}
}
