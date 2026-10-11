using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using GameFramework;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Util;
using UnityEngine;

public class MessageFactory
{
	private static MessageFactory _instance;

	private static Dictionary<Type, BaseMessage> mHandlers;

	public float asyncDelayTime;

	private string _asyncDelayCmd;

	public HashSet<string> asyncDelayCmdSet;

	private static readonly Dictionary<string, Type> supportMessageTypes = new Dictionary<string, Type>
	{
		{
			"bind.gaid",
			typeof(UserBindGaidMessage)
		},
		{
			"build.main.city",
			typeof(BuildMainCityMessage)
		},
		{
			"user.modify.nickName.google",
			typeof(ChangePFdisplayName)
		},
		{
			"check.device.change",
			typeof(CheckDeviceChangeMessage)
		},
		{
			"cross.world.mv",
			typeof(CrossWorldMoveMessage)
		},
		{
			"change.user.parseid",
			typeof(FcmTokenMessage)
		},
		{
			"get.al.points",
			typeof(GetALPointsMessage)
		},
		{
			"get.server.list",
			typeof(GetServerListMessage)
		},
		{
			"world.get.new",
			typeof(GetViewLevelWorldInfoMessage)
		},
		{
			"praise.receive",
			typeof(FSTaskCommand)
		},
		{
			"push.popup.5star",
			typeof(Popup5starPush)
		},
		{
			"hot.spot.base.info",
			typeof(HotSpotBaseInfoMessage)
		},
		{
			"hot.spot.may.effect.me",
			typeof(HotSpotMayEffectMeMessage)
		},
		{
			"init.before",
			typeof(InitBeforeMessage)
		},
		{
			"init.after",
			typeof(InitAfterMessage)
		},
		{
			"init.error",
			typeof(InitErrorMessage)
		},
		{
			"init",
			typeof(InitMessage)
		},
		{
			"login.ext",
			typeof(LoginExtMessage)
		},
		{
			"login.init",
			typeof(LoginInitCommand)
		},
		{
			"login",
			typeof(LoginMessage)
		},
		{
			"login.push.shumei.exception.level",
			typeof(LoginPushShumeiExceptionLevelMessage)
		},
		{
			"logout",
			typeof(LogoutMessage)
		},
		{
			"push.aiHelp.status",
			typeof(PushAIHelpConversationStatus)
		},
		{
			"push.blood.queen.gunner.attack",
			typeof(PushBloodQueenGunnerAttackMessage)
		},
		{
			"push.hot.spot.event.del",
			typeof(PushHotSpotEventDelMessage)
		},
		{
			"push.hot.spot.patch",
			typeof(PushHotSpotPatchMessage)
		},
		{
			"push.lua.env",
			typeof(PushLuaEnvMessage)
		},
		{
			"push.monster.invasion.boss.progress",
			typeof(PushMonsterInvasionBossProgressMessage)
		},
		{
			"push.monster.invasion.summon",
			typeof(PushMonsterInvasionSummon)
		},
		{
			"push.world.point.update",
			typeof(PushPointInfoUpdate)
		},
		{
			"push.lw.alliance.alert.info.create",
			typeof(PushAllianceAlertInfoCreateMessage)
		},
		{
			"push.lw.alliance.alert.info.remove",
			typeof(PushAllianceAlertInfoRemoveMessage)
		},
		{
			"push.record",
			typeof(PushRecordMessage)
		},
		{
			"push.sandworm.delete",
			typeof(PushSandWormDeleteMessage)
		},
		{
			"push.sandworm.update",
			typeof(PushSandWormUpdateMessage)
		},
		{
			"push.thermal.conductor.info",
			typeof(PushThermalConductorInfoMessage)
		},
		{
			"push.update.light.data",
			typeof(PushUpdateLightDataMessage)
		},
		{
			"push.update.world.assistance.Info",
			typeof(PushUpdateWorldAssistanceInfoMessage)
		},
		{
			"push.user.off",
			typeof(PushUserOffMessage)
		},
		{
			"push.wolf.status.change",
			typeof(PushWolfStatusChangeMessage)
		},
		{
			"push.update.green.points",
			typeof(PushWorldAreaGreenUpdate)
		},
		{
			"push.battle.finish",
			typeof(PushWorldBattleFinishMessage)
		},
		{
			"push.battle.round.info",
			typeof(PushWorldBattleUpdateMessage)
		},
		{
			"push.world.desert.update",
			typeof(PushWorldDesertUpdate)
		},
		{
			"push.world.get.block",
			typeof(PushWorldGetBlock)
		},
		{
			"push.world.kill.skin",
			typeof(PushWorldKillSkinMessage)
		},
		{
			"push.world.land.update",
			typeof(PushWorldLandUpdate)
		},
		{
			"push.world.march.new",
			typeof(PushWorldMarchMessage)
		},
		{
			"world.march.full.info.view",
			typeof(GetFullPushWorldMarchMessage)
		},
		{
			"push.world.march.del",
			typeof(PushWorldMarchDelMessage)
		},
		{
			"push.city.be.move",
			typeof(PushWorldBeMoveMessage)
		},
		{
			"push.world.user.crash",
			typeof(PushWorldUserCrashMessage)
		},
		{
			"push.world.march.world.get.new",
			typeof(PushWorldMarchWorldGet)
		},
		{
			"push.world.obj.state.change",
			typeof(PushWorldObjStateChange)
		},
		{
			"push.world.trigger.del",
			typeof(PushWorldTriggerDelMessage)
		},
		{
			"push.world.trigger.update",
			typeof(PushWorldTriggerUpdateMessage)
		},
		{
			"season.get.zone.train.activity.info",
			typeof(SeasonGetZoneTrainActivityInfoMessage)
		},
		{
			"season.hunter.refresh.shadow.info",
			typeof(SeasonHunterRefreshShadowInfoMessage)
		},
		{
			"shumei.request",
			typeof(ShumeiSendDeviceIdMessage)
		},
		{
			"thermal.conductor.info",
			typeof(ThermalConductorInfoMessage)
		},
		{
			"user.clean.post",
			typeof(UserCleanPostMessage)
		},
		{
			"user.tower.change.listen",
			typeof(UserTowerChangeListenMessage)
		},
		{
			"world.get.block",
			typeof(WorldGetBlockMessage)
		},
		{
			"user.leave.world",
			typeof(WorldLeaveCrossServerMessage)
		},
		{
			"world.march.formation.new",
			typeof(WorldMarchFormationMessage)
		},
		{
			"train.send",
			typeof(WorldMarchTrainSendMessage)
		},
		{
			"train.batch.send",
			typeof(WorldMarchTrainListSendMessage)
		},
		{
			"world.march.change",
			typeof(WorldMarchFormationChangeMessage)
		},
		{
			"world.march.speed.up",
			typeof(WorldMarchFormationRapidMessage)
		},
		{
			"world.get.march.infos",
			typeof(WorldGetRectMarchInfosMessage)
		},
		{
			"create.zendesk.token",
			typeof(ZendeskJWTTokenMessage)
		}
	};

	public static MessageFactory Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new MessageFactory();
			}
			return _instance;
		}
	}

	public string asyncDelayCmd
	{
		get
		{
			return _asyncDelayCmd;
		}
		set
		{
			_asyncDelayCmd = value;
			if (string.IsNullOrWhiteSpace(_asyncDelayCmd))
			{
				asyncDelayCmdSet = null;
				return;
			}
			asyncDelayCmdSet = new HashSet<string>(_asyncDelayCmd.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries));
		}
	}

	public void DebugDispatchMessage(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
		{
			Log.Error("[DebugDispatchMessage] 输入内容为空");
			return;
		}
		try
		{
			Match match = new Regex(".*?<?(?<cmd>[a-zA-Z0-9\\._]+)>?\\s*\\|\\s*(?:</color>\\s*)?(?<json>[\\{\\[].*[\\}\\]])").Match(input);
			if (!match.Success)
			{
				Log.Error("[DebugDispatchMessage] 格式解析失败，请确保包含 <cmd> | {json}");
				return;
			}
			string value = match.Groups["cmd"].Value;
			string value2 = match.Groups["json"].Value;
			Log.Warning("[DebugDispatchMessage] 正在模拟推送消息 - CMD: " + value);
			if (!(SFSObject.NewFromJsonData(value2) is SFSObject so))
			{
				Log.Error("[DebugDispatchMessage] SFSObject.NewFromJsonData 转换失败");
				return;
			}
			FixSFSObjectByteArray(value, so);
			UIUtils.ShowTipsDirect("模拟推送: " + value);
			DispatchResponse(value, so);
		}
		catch (Exception ex)
		{
			Log.Error("[DebugDispatchMessage] 发生异常: " + ex.Message);
			Log.Error(ex.StackTrace);
			UIUtils.ShowTipsDirect("解析失败，请检查格式或控制台输出");
		}
	}

	private void FixSFSObjectByteArray(string cmd, SFSObject so)
	{
		if (cmd == "push.world.point.update")
		{
			if (!so.ContainsKey("points"))
			{
				return;
			}
			ISFSArray sFSArray = so.GetSFSArray("points");
			SFSArray sFSArray2 = new SFSArray();
			for (int i = 0; i < sFSArray.Count; i++)
			{
				ISFSObject sFSObject = sFSArray.GetSFSObject(i);
				if (sFSObject != null && sFSObject.ContainsKey("Bytes"))
				{
					ISFSArray sFSArray3 = sFSObject.GetSFSArray("Bytes");
					byte[] array = new byte[sFSArray3.Count];
					for (int j = 0; j < sFSArray3.Count; j++)
					{
						array[j] = (byte)sFSArray3.GetInt(j);
					}
					sFSArray2.AddByteArray(new ByteArray(array));
				}
				else
				{
					sFSArray2.AddSFSObject(sFSObject);
				}
			}
			so.PutSFSArray("points", sFSArray2);
		}
		else
		{
			if (!(cmd == "push.world.march.new") || !so.ContainsKey("isProto") || !so.GetBool("isProto") || !so.ContainsKey("_proto"))
			{
				return;
			}
			ISFSObject sFSObject2 = so.GetSFSObject("_proto");
			if (sFSObject2 != null && sFSObject2.ContainsKey("Bytes"))
			{
				ISFSArray sFSArray4 = sFSObject2.GetSFSArray("Bytes");
				byte[] array2 = new byte[sFSArray4.Count];
				for (int k = 0; k < sFSArray4.Count; k++)
				{
					array2[k] = (byte)sFSArray4.GetInt(k);
				}
				so.PutByteArray("_proto", new ByteArray(array2));
			}
		}
	}

	public void InitMessageHandlers()
	{
		mHandlers = new Dictionary<Type, BaseMessage>();
	}

	public void DispatchResponse(BaseEvent e)
	{
		string cmd = (string)e.Params["cmd"];
		SFSObject so = e.Params["params"] as SFSObject;
		DispatchResponse(cmd, so);
	}

	public static BaseMessage GetMessageByCmd(string cmd)
	{
		if (supportMessageTypes.TryGetValue(cmd, out var value))
		{
			return GetMessage(value);
		}
		return null;
	}

	public static T GetMessage<T>() where T : BaseMessage
	{
		Type typeFromHandle = typeof(T);
		if (mHandlers == null)
		{
			Log.Error("Message Factory Error, Call GetMessage When not init");
			return null;
		}
		if (mHandlers.TryGetValue(typeFromHandle, out var value))
		{
			return (T)value;
		}
		return (T)GetMessage(typeFromHandle);
	}

	public static BaseMessage GetMessage(Type type)
	{
		if (mHandlers == null)
		{
			Log.Error("Message Factory Error, Call GetMessage When not init");
			return null;
		}
		if (!mHandlers.TryGetValue(type, out var value))
		{
			value = type.Assembly.CreateInstance(type.FullName) as BaseMessage;
			if (value != null)
			{
				mHandlers[type] = value;
			}
		}
		return value;
	}

	public static bool ContainsMessage(Type type)
	{
		if (mHandlers == null)
		{
			Log.Error("Message Factory Error, Call ContainsMessage When not init");
			return false;
		}
		return mHandlers.ContainsKey(type);
	}

	public void DispatchResponse(string cmd, SFSObject so)
	{
		WorldScene.RecordBattlefieldPush(cmd);
		if (GMSwitch.IsGM && GMSwitch.GetBool("DebugLogProtocolMsg"))
		{
			if (cmd.Equals("world.get.new"))
			{
				Log.Warning($"[Msg][Receive]<color=green>extension res <{cmd}> |</color>");
			}
			else
			{
				string arg = so.ToJson();
				Log.Warning($"[Msg][Receive]<color=green>extension res <{cmd}> |</color> {arg}");
			}
		}
		try
		{
			if (so.ContainsKey("_id"))
			{
				int fuid = so.GetInt("_id");
				int serverTime = so.TryGetInt("_time");
				GameEntry.Network.getFutureManager().onServerMsgCome(fuid, serverTime);
			}
			if (CommonUtils.IsDebug())
			{
				if (asyncDelayTime > 0f && (asyncDelayCmdSet == null || asyncDelayCmdSet.Count == 0 || asyncDelayCmdSet.Contains(cmd)))
				{
					YieldUtils.DelayActionWithOutContext(delegate
					{
						BaseMessage messageByCmd3 = GetMessageByCmd(cmd);
						if (messageByCmd3 != null)
						{
							messageByCmd3.Handle(so);
						}
						else
						{
							GameEntry.Lua.DispatchResponse(cmd, so.ToLuaTable(GameEntry.Lua.Env));
						}
					}, asyncDelayTime);
				}
				else
				{
					BaseMessage messageByCmd = GetMessageByCmd(cmd);
					if (messageByCmd != null)
					{
						messageByCmd.Handle(so);
					}
					else
					{
						GameEntry.Lua.DispatchResponse(cmd, so.ToLuaTable(GameEntry.Lua.Env));
					}
				}
			}
			else
			{
				BaseMessage messageByCmd2 = GetMessageByCmd(cmd);
				if (messageByCmd2 != null)
				{
					messageByCmd2.Handle(so);
				}
				else
				{
					GameEntry.Lua.DispatchResponse(cmd, so.ToLuaTable(GameEntry.Lua.Env));
				}
			}
		}
		catch (Exception arg2)
		{
			Log.Error("process msg {0} error, {1}", cmd, arg2);
		}
	}

	public bool OnLogin(BaseEvent e)
	{
		SFSObject sFSObject = e.Params["data"] as SFSObject;
		if (sFSObject == null)
		{
			sFSObject = e.Params["errorMessage"] as SFSObject;
		}
		if (sFSObject == null)
		{
			Log.Error("Login failed");
			return false;
		}
		GetMessageByCmd("login")?.Handle(sFSObject);
		return true;
	}

	private void CheckMessageRegistConfig()
	{
		Type[] array = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => from t in a.GetTypes()
			where t.BaseType == typeof(BaseMessage)
			select t).ToArray();
		foreach (Type type in array)
		{
			object obj = type.Assembly.CreateInstance(type.FullName);
			if (obj is BaseMessage baseMessage && !(obj is LoginCrossServerMessage) && !(obj is WorldCrossServerMessage))
			{
				string msgId = baseMessage.GetMsgId();
				if (!supportMessageTypes.ContainsKey(msgId))
				{
					Debug.LogError("MsgId为" + msgId + "的" + type.Name + "未注册，请点击\"Tools/Constant代码生成/生成C#BaseMessage注册文件\"进行注册");
				}
				else if (supportMessageTypes[msgId].Name != type.Name)
				{
					Debug.LogError("有MsgId同为" + msgId + "的" + supportMessageTypes[msgId].Name + "和" + type.Name + "，请修改，然后点击\"Tools/Constant代码生成/生成C#BaseMessage注册文件\"刷新注册文件");
				}
			}
		}
	}
}
