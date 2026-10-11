using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class VoiceChatManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(VoiceChatManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 37, 3, 0);
		Utils.RegisterFunc(L, -3, "AddEnterRoomCompletedListener", _m_AddEnterRoomCompletedListener);
		Utils.RegisterFunc(L, -3, "RemoveEnterRoomCompletedListener", _m_RemoveEnterRoomCompletedListener);
		Utils.RegisterFunc(L, -3, "AddExitRoomCompletedListener", _m_AddExitRoomCompletedListener);
		Utils.RegisterFunc(L, -3, "RemoveExitRoomCompletedListener", _m_RemoveExitRoomCompletedListener);
		Utils.RegisterFunc(L, -3, "AddRoomDisconnectedListener", _m_AddRoomDisconnectedListener);
		Utils.RegisterFunc(L, -3, "RemoveRoomDisconnectedListener", _m_RemoveRoomDisconnectedListener);
		Utils.RegisterFunc(L, -3, "AddUserUpdatedListener", _m_AddUserUpdatedListener);
		Utils.RegisterFunc(L, -3, "RemoveUserUpdatedListener", _m_RemoveUserUpdatedListener);
		Utils.RegisterFunc(L, -3, "AddCustomEventReceivedListener", _m_AddCustomEventReceivedListener);
		Utils.RegisterFunc(L, -3, "RemoveCustomEventReceivedListener", _m_RemoveCustomEventReceivedListener);
		Utils.RegisterFunc(L, -3, "SetService", _m_SetService);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "InitWithParams", _m_InitWithParams);
		Utils.RegisterFunc(L, -3, "Uninit", _m_Uninit);
		Utils.RegisterFunc(L, -3, "ResetForReload", _m_ResetForReload);
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
		Utils.RegisterFunc(L, -3, "EnterRoomCompleted", _e_EnterRoomCompleted);
		Utils.RegisterFunc(L, -3, "ExitRoomCompleted", _e_ExitRoomCompleted);
		Utils.RegisterFunc(L, -3, "RoomDisconnected", _e_RoomDisconnected);
		Utils.RegisterFunc(L, -3, "UserUpdated", _e_UserUpdated);
		Utils.RegisterFunc(L, -3, "CustomEventReceived", _e_CustomEventReceived);
		Utils.RegisterFunc(L, -2, "IsInitialized", _g_get_IsInitialized);
		Utils.RegisterFunc(L, -2, "IsRoomOpPending", _g_get_IsRoomOpPending);
		Utils.RegisterFunc(L, -2, "Service", _g_get_Service);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 1, 0);
		Utils.RegisterFunc(L, -4, "OnPermissionCallback", _m_OnPermissionCallback_xlua_st_);
		Utils.RegisterFunc(L, -2, "Instance", _g_get_Instance);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "VoiceChatManager does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddEnterRoomCompletedListener(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatEnterRoomResult> handler = objectTranslator.GetDelegate<Action<VoiceChatEnterRoomResult>>(L, 2);
			voiceChatManager.AddEnterRoomCompletedListener(handler);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveEnterRoomCompletedListener(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatEnterRoomResult> handler = objectTranslator.GetDelegate<Action<VoiceChatEnterRoomResult>>(L, 2);
			voiceChatManager.RemoveEnterRoomCompletedListener(handler);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddExitRoomCompletedListener(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatExitRoomResult> handler = objectTranslator.GetDelegate<Action<VoiceChatExitRoomResult>>(L, 2);
			voiceChatManager.AddExitRoomCompletedListener(handler);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveExitRoomCompletedListener(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatExitRoomResult> handler = objectTranslator.GetDelegate<Action<VoiceChatExitRoomResult>>(L, 2);
			voiceChatManager.RemoveExitRoomCompletedListener(handler);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddRoomDisconnectedListener(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatDisconnectResult> handler = objectTranslator.GetDelegate<Action<VoiceChatDisconnectResult>>(L, 2);
			voiceChatManager.AddRoomDisconnectedListener(handler);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveRoomDisconnectedListener(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatDisconnectResult> handler = objectTranslator.GetDelegate<Action<VoiceChatDisconnectResult>>(L, 2);
			voiceChatManager.RemoveRoomDisconnectedListener(handler);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddUserUpdatedListener(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatUserUpdate> handler = objectTranslator.GetDelegate<Action<VoiceChatUserUpdate>>(L, 2);
			voiceChatManager.AddUserUpdatedListener(handler);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveUserUpdatedListener(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatUserUpdate> handler = objectTranslator.GetDelegate<Action<VoiceChatUserUpdate>>(L, 2);
			voiceChatManager.RemoveUserUpdatedListener(handler);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddCustomEventReceivedListener(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatCustomEvent> handler = objectTranslator.GetDelegate<Action<VoiceChatCustomEvent>>(L, 2);
			voiceChatManager.AddCustomEventReceivedListener(handler);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveCustomEventReceivedListener(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatCustomEvent> handler = objectTranslator.GetDelegate<Action<VoiceChatCustomEvent>>(L, 2);
			voiceChatManager.RemoveCustomEventReceivedListener(handler);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetService(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			IVoiceChatService service = (IVoiceChatService)objectTranslator.GetObject(L, 2, typeof(IVoiceChatService));
			voiceChatManager.SetService(service);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			VoiceChatInitOptions options = (VoiceChatInitOptions)objectTranslator.GetObject(L, 2, typeof(VoiceChatInitOptions));
			int value = voiceChatManager.Init(options);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitWithParams(IntPtr L)
	{
		try
		{
			VoiceChatManager obj = (VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string sdkAppId = Lua.lua_tostring(L, 2);
			string openId = Lua.lua_tostring(L, 3);
			int appScene = Lua.xlua_tointeger(L, 4);
			bool enableSpeakerOnEnter = Lua.lua_toboolean(L, 5);
			bool enableMicOnEnter = Lua.lua_toboolean(L, 6);
			int value = obj.InitWithParams(sdkAppId, openId, appScene, enableSpeakerOnEnter, enableMicOnEnter);
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
			int value = ((VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Uninit();
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetForReload(IntPtr L)
	{
		try
		{
			((VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetForReload();
			return 0;
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
			int value = ((VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Poll();
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
			VoiceChatManager voiceChatManager = (VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING))
			{
				string voiceRoomId = Lua.lua_tostring(L, 2);
				int roomType = Lua.xlua_tointeger(L, 3);
				string userSig = Lua.lua_tostring(L, 4);
				int value = voiceChatManager.EnterRoom(voiceRoomId, roomType, userSig);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && (Lua.lua_isnil(L, 5) || Lua.lua_type(L, 5) == LuaTypes.LUA_TSTRING))
			{
				string chatRoomId = Lua.lua_tostring(L, 2);
				string voiceRoomId2 = Lua.lua_tostring(L, 3);
				int roomType2 = Lua.xlua_tointeger(L, 4);
				string userSig2 = Lua.lua_tostring(L, 5);
				int value2 = voiceChatManager.EnterRoom(chatRoomId, voiceRoomId2, roomType2, userSig2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to VoiceChatManager.EnterRoom!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ExitRoom(IntPtr L)
	{
		try
		{
			VoiceChatManager voiceChatManager = (VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				int value2 = voiceChatManager.ExitRoom();
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool uninitService = Lua.lua_toboolean(L, 2);
					int value = voiceChatManager.ExitRoom(uninitService);
					Lua.xlua_pushinteger(L, value);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to VoiceChatManager.ExitRoom!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsRoomEntered(IntPtr L)
	{
		try
		{
			bool value = ((VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsRoomEntered();
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
			VoiceChatManager obj = (VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_OnPermissionCallback_xlua_st_(IntPtr L)
	{
		try
		{
			VoiceChatManager.OnPermissionCallback();
			return 0;
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
			bool value = ((VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMicEnabled();
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
			int micListCount = ((VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMicListCount();
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
			VoiceChatManager obj = (VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			bool value = ((VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsSpeakerEnabled();
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
			int speakerListCount = ((VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSpeakerListCount();
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
			VoiceChatManager obj = (VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			VoiceChatManager obj = (VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			VoiceChatManager obj = (VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			VoiceChatManager obj = (VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			int value = ((VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CheckMicPermission();
			Lua.xlua_pushinteger(L, value);
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
			string sdkVersion = ((VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSdkVersion();
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
			VoiceChatManager obj = (VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _g_get_Instance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, VoiceChatManager.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsInitialized(IntPtr L)
	{
		try
		{
			VoiceChatManager voiceChatManager = (VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, voiceChatManager.IsInitialized);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsRoomOpPending(IntPtr L)
	{
		try
		{
			VoiceChatManager voiceChatManager = (VoiceChatManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, voiceChatManager.IsRoomOpPending);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Service(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, voiceChatManager.Service);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_EnterRoomCompleted(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatEnterRoomResult> action = objectTranslator.GetDelegate<Action<VoiceChatEnterRoomResult>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<VoiceChatEnterRoomResult>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					voiceChatManager.EnterRoomCompleted += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					voiceChatManager.EnterRoomCompleted -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to VoiceChatManager.EnterRoomCompleted!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_ExitRoomCompleted(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatExitRoomResult> action = objectTranslator.GetDelegate<Action<VoiceChatExitRoomResult>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<VoiceChatExitRoomResult>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					voiceChatManager.ExitRoomCompleted += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					voiceChatManager.ExitRoomCompleted -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to VoiceChatManager.ExitRoomCompleted!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_RoomDisconnected(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatDisconnectResult> action = objectTranslator.GetDelegate<Action<VoiceChatDisconnectResult>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<VoiceChatDisconnectResult>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					voiceChatManager.RoomDisconnected += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					voiceChatManager.RoomDisconnected -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to VoiceChatManager.RoomDisconnected!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_UserUpdated(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatUserUpdate> action = objectTranslator.GetDelegate<Action<VoiceChatUserUpdate>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<VoiceChatUserUpdate>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					voiceChatManager.UserUpdated += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					voiceChatManager.UserUpdated -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to VoiceChatManager.UserUpdated!");
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_CustomEventReceived(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			VoiceChatManager voiceChatManager = (VoiceChatManager)objectTranslator.FastGetCSObj(L, 1);
			Action<VoiceChatCustomEvent> action = objectTranslator.GetDelegate<Action<VoiceChatCustomEvent>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<VoiceChatCustomEvent>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					voiceChatManager.CustomEventReceived += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					voiceChatManager.CustomEventReceived -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to VoiceChatManager.CustomEventReceived!");
		return 0;
	}
}
