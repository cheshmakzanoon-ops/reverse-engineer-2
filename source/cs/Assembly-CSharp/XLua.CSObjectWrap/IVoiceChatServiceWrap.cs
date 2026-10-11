using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class IVoiceChatServiceWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(IVoiceChatService);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 25, 0, 0);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "Uninit", _m_Uninit);
		Utils.RegisterFunc(L, -3, "Poll", _m_Poll);
		Utils.RegisterFunc(L, -3, "EnterRoom", _m_EnterRoom);
		Utils.RegisterFunc(L, -3, "ExitRoom", _m_ExitRoom);
		Utils.RegisterFunc(L, -3, "IsRoomEntered", _m_IsRoomEntered);
		Utils.RegisterFunc(L, -3, "SetMicEnabled", _m_SetMicEnabled);
		Utils.RegisterFunc(L, -3, "IsMicEnabled", _m_IsMicEnabled);
		Utils.RegisterFunc(L, -3, "GetMicListCount", _m_GetMicListCount);
		Utils.RegisterFunc(L, -3, "SetSpeakerEnabled", _m_SetSpeakerEnabled);
		Utils.RegisterFunc(L, -3, "IsSpeakerEnabled", _m_IsSpeakerEnabled);
		Utils.RegisterFunc(L, -3, "GetSpeakerListCount", _m_GetSpeakerListCount);
		Utils.RegisterFunc(L, -3, "SetSpeakerVolumeByUserId", _m_SetSpeakerVolumeByUserId);
		Utils.RegisterFunc(L, -3, "StartPlayMusic", _m_StartPlayMusic);
		Utils.RegisterFunc(L, -3, "StopPlayMusic", _m_StopPlayMusic);
		Utils.RegisterFunc(L, -3, "IsMusicPlayEnd", _m_IsMusicPlayEnd);
		Utils.RegisterFunc(L, -3, "CheckMicPermission", _m_CheckMicPermission);
		Utils.RegisterFunc(L, -3, "GetSdkVersion", _m_GetSdkVersion);
		Utils.RegisterFunc(L, -3, "OnApplicationFocus", _m_OnApplicationFocus);
		Utils.RegisterFunc(L, -3, "GenAuthBuffer", _m_GenAuthBuffer);
		Utils.RegisterFunc(L, -3, "EnterRoomCompleted", _e_EnterRoomCompleted);
		Utils.RegisterFunc(L, -3, "ExitRoomCompleted", _e_ExitRoomCompleted);
		Utils.RegisterFunc(L, -3, "RoomDisconnected", _e_RoomDisconnected);
		Utils.RegisterFunc(L, -3, "UserUpdated", _e_UserUpdated);
		Utils.RegisterFunc(L, -3, "CustomEventReceived", _e_CustomEventReceived);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "IVoiceChatService does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			IVoiceChatService voiceChatService = (IVoiceChatService)objectTranslator.FastGetCSObj(L, 1);
			VoiceChatInitOptions options = (VoiceChatInitOptions)objectTranslator.GetObject(L, 2, typeof(VoiceChatInitOptions));
			int value = voiceChatService.Init(options);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Uninit(IntPtr L)
	{
		try
		{
			int value = ((IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Uninit();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Poll(IntPtr L)
	{
		try
		{
			int value = ((IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Poll();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnterRoom(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			IVoiceChatService voiceChatService = (IVoiceChatService)objectTranslator.FastGetCSObj(L, 1);
			string roomId = Lua.lua_tostring(L, 2);
			objectTranslator.Get(L, 3, out VoiceChatRoomType val);
			string userSig = Lua.lua_tostring(L, 4);
			int value = voiceChatService.EnterRoom(roomId, val, userSig);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ExitRoom(IntPtr L)
	{
		try
		{
			int value = ((IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ExitRoom();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsRoomEntered(IntPtr L)
	{
		try
		{
			bool value = ((IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsRoomEntered();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMicEnabled(IntPtr L)
	{
		try
		{
			IVoiceChatService obj = (IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool micEnabled = Lua.lua_toboolean(L, 2);
			int value = obj.SetMicEnabled(micEnabled);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMicEnabled(IntPtr L)
	{
		try
		{
			bool value = ((IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMicEnabled();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMicListCount(IntPtr L)
	{
		try
		{
			int micListCount = ((IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMicListCount();
			Lua.xlua_pushinteger(L, micListCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSpeakerEnabled(IntPtr L)
	{
		try
		{
			IVoiceChatService obj = (IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool speakerEnabled = Lua.lua_toboolean(L, 2);
			int value = obj.SetSpeakerEnabled(speakerEnabled);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSpeakerEnabled(IntPtr L)
	{
		try
		{
			bool value = ((IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsSpeakerEnabled();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSpeakerListCount(IntPtr L)
	{
		try
		{
			int speakerListCount = ((IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSpeakerListCount();
			Lua.xlua_pushinteger(L, speakerListCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSpeakerVolumeByUserId(IntPtr L)
	{
		try
		{
			IVoiceChatService obj = (IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string userId = Lua.lua_tostring(L, 2);
			int volume = Lua.xlua_tointeger(L, 3);
			int value = obj.SetSpeakerVolumeByUserId(userId, volume);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartPlayMusic(IntPtr L)
	{
		try
		{
			IVoiceChatService obj = (IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int soundId = Lua.xlua_tointeger(L, 2);
			string filePath = Lua.lua_tostring(L, 3);
			int loopCount = Lua.xlua_tointeger(L, 4);
			int value = obj.StartPlayMusic(soundId, filePath, loopCount);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopPlayMusic(IntPtr L)
	{
		try
		{
			IVoiceChatService obj = (IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int soundId = Lua.xlua_tointeger(L, 2);
			int value = obj.StopPlayMusic(soundId);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMusicPlayEnd(IntPtr L)
	{
		try
		{
			IVoiceChatService obj = (IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int soundId = Lua.xlua_tointeger(L, 2);
			int value = obj.IsMusicPlayEnd(soundId);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckMicPermission(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatMicPermission val = ((IVoiceChatService)objectTranslator.FastGetCSObj(L, 1)).CheckMicPermission();
			objectTranslator.PushVoiceChatMicPermission(L, val);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSdkVersion(IntPtr L)
	{
		try
		{
			string sdkVersion = ((IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSdkVersion();
			Lua.lua_pushstring(L, sdkVersion);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnApplicationFocus(IntPtr L)
	{
		try
		{
			IVoiceChatService obj = (IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool hasFocus = Lua.lua_toboolean(L, 2);
			obj.OnApplicationFocus(hasFocus);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GenAuthBuffer(IntPtr L)
	{
		try
		{
			IVoiceChatService obj = (IVoiceChatService)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string roomId = Lua.lua_tostring(L, 2);
			string openId = Lua.lua_tostring(L, 3);
			string str = obj.GenAuthBuffer(roomId, openId);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_EnterRoomCompleted(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			IVoiceChatService voiceChatService = (IVoiceChatService)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatEnterRoomResult, bool, bool> action = objectTranslator.GetDelegate<Action<VoiceChatEnterRoomResult, bool, bool>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<VoiceChatEnterRoomResult, bool, bool>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					voiceChatService.EnterRoomCompleted += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					voiceChatService.EnterRoomCompleted -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to IVoiceChatService.EnterRoomCompleted!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_ExitRoomCompleted(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			IVoiceChatService voiceChatService = (IVoiceChatService)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatExitRoomResult> action = objectTranslator.GetDelegate<Action<VoiceChatExitRoomResult>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<VoiceChatExitRoomResult>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					voiceChatService.ExitRoomCompleted += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					voiceChatService.ExitRoomCompleted -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to IVoiceChatService.ExitRoomCompleted!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_RoomDisconnected(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			IVoiceChatService voiceChatService = (IVoiceChatService)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatDisconnectResult> action = objectTranslator.GetDelegate<Action<VoiceChatDisconnectResult>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<VoiceChatDisconnectResult>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					voiceChatService.RoomDisconnected += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					voiceChatService.RoomDisconnected -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to IVoiceChatService.RoomDisconnected!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_UserUpdated(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			IVoiceChatService voiceChatService = (IVoiceChatService)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatUserUpdate> action = objectTranslator.GetDelegate<Action<VoiceChatUserUpdate>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<VoiceChatUserUpdate>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					voiceChatService.UserUpdated += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					voiceChatService.UserUpdated -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to IVoiceChatService.UserUpdated!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_CustomEventReceived(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			IVoiceChatService voiceChatService = (IVoiceChatService)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatCustomEvent> action = objectTranslator.GetDelegate<Action<VoiceChatCustomEvent>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<VoiceChatCustomEvent>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					voiceChatService.CustomEventReceived += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					voiceChatService.CustomEventReceived -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to IVoiceChatService.CustomEventReceived!");
		return 0;
	}
}
