using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using VEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SoundComponentSoundGroupWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SoundComponent.SoundGroup);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 20, 24, 18);
		Utils.RegisterFunc(L, -3, "GetAudioSources", _m_GetAudioSources);
		Utils.RegisterFunc(L, -3, "GetAudioURLs", _m_GetAudioURLs);
		Utils.RegisterFunc(L, -3, "IsSoundPlaying", _m_IsSoundPlaying);
		Utils.RegisterFunc(L, -3, "HasSerialId", _m_HasSerialId);
		Utils.RegisterFunc(L, -3, "IsNotExpired", _m_IsNotExpired);
		Utils.RegisterFunc(L, -3, "StopAllAudioSource", _m_StopAllAudioSource);
		Utils.RegisterFunc(L, -3, "PlayAudioSource", _m_PlayAudioSource);
		Utils.RegisterFunc(L, -3, "GetAudioMixerGroupEQ", _m_GetAudioMixerGroupEQ);
		Utils.RegisterFunc(L, -3, "SetAudioMixerGroupEQ", _m_SetAudioMixerGroupEQ);
		Utils.RegisterFunc(L, -3, "GetAudioMixerGroupDbVolume", _m_GetAudioMixerGroupDbVolume);
		Utils.RegisterFunc(L, -3, "SetAudioMixerGroupDbVolume", _m_SetAudioMixerGroupDbVolume);
		Utils.RegisterFunc(L, -3, "PlaySound", _m_PlaySound);
		Utils.RegisterFunc(L, -3, "PlayOneShot", _m_PlayOneShot);
		Utils.RegisterFunc(L, -3, "StopSound", _m_StopSound);
		Utils.RegisterFunc(L, -3, "StopSoundBySerialId", _m_StopSoundBySerialId);
		Utils.RegisterFunc(L, -3, "PauseSound", _m_PauseSound);
		Utils.RegisterFunc(L, -3, "ResumeSound", _m_ResumeSound);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "ChangeVolume", _m_ChangeVolume);
		Utils.RegisterFunc(L, -3, "FadeOutAndPlaySound", _m_FadeOutAndPlaySound);
		Utils.RegisterFunc(L, -2, "MixerGroup", _g_get_MixerGroup);
		Utils.RegisterFunc(L, -2, "IsNewGroup", _g_get_IsNewGroup);
		Utils.RegisterFunc(L, -2, "GlobalSoundVolumeRatio", _g_get_GlobalSoundVolumeRatio);
		Utils.RegisterFunc(L, -2, "LodVolumeRatio", _g_get_LodVolumeRatio);
		Utils.RegisterFunc(L, -2, "GlobalSettingVolumeRatio", _g_get_GlobalSettingVolumeRatio);
		Utils.RegisterFunc(L, -2, "SharedAudioSource", _g_get_SharedAudioSource);
		Utils.RegisterFunc(L, -2, "SoundAsset", _g_get_SoundAsset);
		Utils.RegisterFunc(L, -2, "SerialId", _g_get_SerialId);
		Utils.RegisterFunc(L, -2, "Name", _g_get_Name);
		Utils.RegisterFunc(L, -2, "CurTime", _g_get_CurTime);
		Utils.RegisterFunc(L, -2, "AudioSourceTransform", _g_get_AudioSourceTransform);
		Utils.RegisterFunc(L, -2, "Mute", _g_get_Mute);
		Utils.RegisterFunc(L, -2, "Volume", _g_get_Volume);
		Utils.RegisterFunc(L, -2, "Loop", _g_get_Loop);
		Utils.RegisterFunc(L, -2, "isEff", _g_get_isEff);
		Utils.RegisterFunc(L, -2, "isAmb", _g_get_isAmb);
		Utils.RegisterFunc(L, -2, "lodVolumeCityMin", _g_get_lodVolumeCityMin);
		Utils.RegisterFunc(L, -2, "lodVolumeCityMax", _g_get_lodVolumeCityMax);
		Utils.RegisterFunc(L, -2, "lodEQCityMin", _g_get_lodEQCityMin);
		Utils.RegisterFunc(L, -2, "lodEQCityMax", _g_get_lodEQCityMax);
		Utils.RegisterFunc(L, -2, "lodVolumeWorldMin", _g_get_lodVolumeWorldMin);
		Utils.RegisterFunc(L, -2, "lodVolumeWorldMax", _g_get_lodVolumeWorldMax);
		Utils.RegisterFunc(L, -2, "lodEQWorldMin", _g_get_lodEQWorldMin);
		Utils.RegisterFunc(L, -2, "lodEQWorldMax", _g_get_lodEQWorldMax);
		Utils.RegisterFunc(L, -1, "MixerGroup", _s_set_MixerGroup);
		Utils.RegisterFunc(L, -1, "GlobalSoundVolumeRatio", _s_set_GlobalSoundVolumeRatio);
		Utils.RegisterFunc(L, -1, "LodVolumeRatio", _s_set_LodVolumeRatio);
		Utils.RegisterFunc(L, -1, "GlobalSettingVolumeRatio", _s_set_GlobalSettingVolumeRatio);
		Utils.RegisterFunc(L, -1, "Name", _s_set_Name);
		Utils.RegisterFunc(L, -1, "Mute", _s_set_Mute);
		Utils.RegisterFunc(L, -1, "Volume", _s_set_Volume);
		Utils.RegisterFunc(L, -1, "Loop", _s_set_Loop);
		Utils.RegisterFunc(L, -1, "isEff", _s_set_isEff);
		Utils.RegisterFunc(L, -1, "isAmb", _s_set_isAmb);
		Utils.RegisterFunc(L, -1, "lodVolumeCityMin", _s_set_lodVolumeCityMin);
		Utils.RegisterFunc(L, -1, "lodVolumeCityMax", _s_set_lodVolumeCityMax);
		Utils.RegisterFunc(L, -1, "lodEQCityMin", _s_set_lodEQCityMin);
		Utils.RegisterFunc(L, -1, "lodEQCityMax", _s_set_lodEQCityMax);
		Utils.RegisterFunc(L, -1, "lodVolumeWorldMin", _s_set_lodVolumeWorldMin);
		Utils.RegisterFunc(L, -1, "lodVolumeWorldMax", _s_set_lodVolumeWorldMax);
		Utils.RegisterFunc(L, -1, "lodEQWorldMin", _s_set_lodEQWorldMin);
		Utils.RegisterFunc(L, -1, "lodEQWorldMax", _s_set_lodEQWorldMax);
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
			if (Lua.lua_gettop(L) == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string name = Lua.lua_tostring(L, 2);
				bool useAudioMixer = Lua.lua_toboolean(L, 3);
				SoundComponent.SoundGroup o = new SoundComponent.SoundGroup(name, useAudioMixer);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				SoundComponent.SoundGroup o2 = new SoundComponent.SoundGroup(Lua.lua_tostring(L, 2));
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.SoundGroup constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAudioSources(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<AudioSource> audioSources = ((SoundComponent.SoundGroup)objectTranslator.FastGetCSObj(L, 1)).GetAudioSources();
			objectTranslator.Push(L, audioSources);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAudioURLs(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<string> audioURLs = ((SoundComponent.SoundGroup)objectTranslator.FastGetCSObj(L, 1)).GetAudioURLs();
			objectTranslator.Push(L, audioURLs);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSoundPlaying(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup obj = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string path = Lua.lua_tostring(L, 2);
			bool value = obj.IsSoundPlaying(path);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasSerialId(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup obj = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int serialId = Lua.xlua_tointeger(L, 2);
			bool value = obj.HasSerialId(serialId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsNotExpired(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup obj = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float loadStartTime = (float)Lua.lua_tonumber(L, 2);
			bool value = obj.IsNotExpired(loadStartTime);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopAllAudioSource(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopAllAudioSource();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayAudioSource(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)objectTranslator.FastGetCSObj(L, 1);
			int serialId = Lua.xlua_tointeger(L, 2);
			SoundComponent.PlayAudioParams playAudioParams = (SoundComponent.PlayAudioParams)objectTranslator.GetObject(L, 3, typeof(SoundComponent.PlayAudioParams));
			Asset soundAsset = (Asset)objectTranslator.GetObject(L, 4, typeof(Asset));
			soundGroup.PlayAudioSource(serialId, playAudioParams, soundAsset);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAudioMixerGroupEQ(IntPtr L)
	{
		try
		{
			float audioMixerGroupEQ = ((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetAudioMixerGroupEQ();
			Lua.lua_pushnumber(L, audioMixerGroupEQ);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAudioMixerGroupEQ(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup obj = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float audioMixerGroupEQ = (float)Lua.lua_tonumber(L, 2);
			obj.SetAudioMixerGroupEQ(audioMixerGroupEQ);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAudioMixerGroupDbVolume(IntPtr L)
	{
		try
		{
			float audioMixerGroupDbVolume = ((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetAudioMixerGroupDbVolume();
			Lua.lua_pushnumber(L, audioMixerGroupDbVolume);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAudioMixerGroupDbVolume(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup obj = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float audioMixerGroupDbVolume = (float)Lua.lua_tonumber(L, 2);
			obj.SetAudioMixerGroupDbVolume(audioMixerGroupDbVolume);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlaySound(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 7 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Asset>(L, 3) && objectTranslator.Assignable<SoundComponent.PlaySoundParams>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && objectTranslator.Assignable<Action>(L, 7))
			{
				int serialId = Lua.xlua_tointeger(L, 2);
				Asset soundAsset = (Asset)objectTranslator.GetObject(L, 3, typeof(Asset));
				SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)objectTranslator.GetObject(L, 4, typeof(SoundComponent.PlaySoundParams));
				float startTime = (float)Lua.lua_tonumber(L, 5);
				float dspTime = (float)Lua.lua_tonumber(L, 6);
				Action onFinishCallback = objectTranslator.GetDelegate<Action>(L, 7);
				soundGroup.PlaySound(serialId, soundAsset, playSoundParams, startTime, dspTime, onFinishCallback);
				return 0;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Asset>(L, 3) && objectTranslator.Assignable<SoundComponent.PlaySoundParams>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				int serialId2 = Lua.xlua_tointeger(L, 2);
				Asset soundAsset2 = (Asset)objectTranslator.GetObject(L, 3, typeof(Asset));
				SoundComponent.PlaySoundParams playSoundParams2 = (SoundComponent.PlaySoundParams)objectTranslator.GetObject(L, 4, typeof(SoundComponent.PlaySoundParams));
				float startTime2 = (float)Lua.lua_tonumber(L, 5);
				float dspTime2 = (float)Lua.lua_tonumber(L, 6);
				soundGroup.PlaySound(serialId2, soundAsset2, playSoundParams2, startTime2, dspTime2);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Asset>(L, 3) && objectTranslator.Assignable<SoundComponent.PlaySoundParams>(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int serialId3 = Lua.xlua_tointeger(L, 2);
				Asset soundAsset3 = (Asset)objectTranslator.GetObject(L, 3, typeof(Asset));
				SoundComponent.PlaySoundParams playSoundParams3 = (SoundComponent.PlaySoundParams)objectTranslator.GetObject(L, 4, typeof(SoundComponent.PlaySoundParams));
				float startTime3 = (float)Lua.lua_tonumber(L, 5);
				soundGroup.PlaySound(serialId3, soundAsset3, playSoundParams3, startTime3);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Asset>(L, 3) && objectTranslator.Assignable<SoundComponent.PlaySoundParams>(L, 4))
			{
				int serialId4 = Lua.xlua_tointeger(L, 2);
				Asset soundAsset4 = (Asset)objectTranslator.GetObject(L, 3, typeof(Asset));
				SoundComponent.PlaySoundParams playSoundParams4 = (SoundComponent.PlaySoundParams)objectTranslator.GetObject(L, 4, typeof(SoundComponent.PlaySoundParams));
				soundGroup.PlaySound(serialId4, soundAsset4, playSoundParams4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.SoundGroup.PlaySound!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PlayOneShot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Asset>(L, 3) && objectTranslator.Assignable<SoundComponent.PlaySoundParams>(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				int serialId = Lua.xlua_tointeger(L, 2);
				Asset soundAsset = (Asset)objectTranslator.GetObject(L, 3, typeof(Asset));
				SoundComponent.PlaySoundParams playSoundParams = (SoundComponent.PlaySoundParams)objectTranslator.GetObject(L, 4, typeof(SoundComponent.PlaySoundParams));
				bool cacheAsset = Lua.lua_toboolean(L, 5);
				soundGroup.PlayOneShot(serialId, soundAsset, playSoundParams, cacheAsset);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Asset>(L, 3) && objectTranslator.Assignable<SoundComponent.PlaySoundParams>(L, 4))
			{
				int serialId2 = Lua.xlua_tointeger(L, 2);
				Asset soundAsset2 = (Asset)objectTranslator.GetObject(L, 3, typeof(Asset));
				SoundComponent.PlaySoundParams playSoundParams2 = (SoundComponent.PlaySoundParams)objectTranslator.GetObject(L, 4, typeof(SoundComponent.PlaySoundParams));
				soundGroup.PlayOneShot(serialId2, soundAsset2, playSoundParams2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SoundComponent.SoundGroup.PlayOneShot!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopSound(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopSound();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopSoundBySerialId(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup obj = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int serialId = Lua.xlua_tointeger(L, 2);
			bool value = obj.StopSoundBySerialId(serialId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PauseSound(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).PauseSound();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResumeSound(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResumeSound();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdate(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup obj = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float elapseSeconds = (float)Lua.lua_tonumber(L, 2);
			obj.OnUpdate(elapseSeconds);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeVolume(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup obj = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float to = (float)Lua.lua_tonumber(L, 2);
			float time = (float)Lua.lua_tonumber(L, 3);
			obj.ChangeVolume(to, time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FadeOutAndPlaySound(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup obj = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float time = (float)Lua.lua_tonumber(L, 2);
			obj.FadeOutAndPlaySound(time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_MixerGroup(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, soundGroup.MixerGroup);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsNewGroup(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, soundGroup.IsNewGroup);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GlobalSoundVolumeRatio(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, soundGroup.GlobalSoundVolumeRatio);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LodVolumeRatio(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, soundGroup.LodVolumeRatio);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GlobalSettingVolumeRatio(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, soundGroup.GlobalSettingVolumeRatio);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SharedAudioSource(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, soundGroup.SharedAudioSource);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SoundAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, soundGroup.SoundAsset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SerialId(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, soundGroup.SerialId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Name(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, soundGroup.Name);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurTime(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, soundGroup.CurTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AudioSourceTransform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, soundGroup.AudioSourceTransform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Mute(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, soundGroup.Mute);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Volume(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, soundGroup.Volume);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Loop(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, soundGroup.Loop);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isEff(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, soundGroup.isEff);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isAmb(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, soundGroup.isAmb);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lodVolumeCityMin(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, soundGroup.lodVolumeCityMin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lodVolumeCityMax(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, soundGroup.lodVolumeCityMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lodEQCityMin(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, soundGroup.lodEQCityMin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lodEQCityMax(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, soundGroup.lodEQCityMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lodVolumeWorldMin(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, soundGroup.lodVolumeWorldMin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lodVolumeWorldMax(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, soundGroup.lodVolumeWorldMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lodEQWorldMin(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, soundGroup.lodEQWorldMin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lodEQWorldMax(IntPtr L)
	{
		try
		{
			SoundComponent.SoundGroup soundGroup = (SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, soundGroup.lodEQWorldMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_MixerGroup(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SoundComponent.SoundGroup)objectTranslator.FastGetCSObj(L, 1)).MixerGroup = (AudioMixerGroup)objectTranslator.GetObject(L, 2, typeof(AudioMixerGroup));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GlobalSoundVolumeRatio(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GlobalSoundVolumeRatio = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_LodVolumeRatio(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LodVolumeRatio = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GlobalSettingVolumeRatio(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GlobalSettingVolumeRatio = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Name(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Name = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Mute(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Mute = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Volume(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Volume = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Loop(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Loop = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isEff(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isEff = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isAmb(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isAmb = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lodVolumeCityMin(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lodVolumeCityMin = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lodVolumeCityMax(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lodVolumeCityMax = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lodEQCityMin(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lodEQCityMin = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lodEQCityMax(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lodEQCityMax = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lodVolumeWorldMin(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lodVolumeWorldMin = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lodVolumeWorldMax(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lodVolumeWorldMax = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lodEQWorldMin(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lodEQWorldMin = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lodEQWorldMax(IntPtr L)
	{
		try
		{
			((SoundComponent.SoundGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lodEQWorldMax = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
