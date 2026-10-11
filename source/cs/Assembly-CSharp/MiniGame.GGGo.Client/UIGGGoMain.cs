using System;
using DG.Tweening;
using GameKit.Base;
using MiniGame.Core;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace MiniGame.GGGo.Client;

[ExecuteInEditMode]
public class UIGGGoMain : MonoBehaviour, IBindUI
{
	[SerializeField]
	private Camera UICamera;

	[SerializeField]
	private Transform GameRoot;

	[SerializeField]
	private GameObject GameEnd;

	[SerializeField]
	private GameObject BgMask;

	[SerializeField]
	private RectTransform TopRoot;

	[SerializeField]
	private GameObject HudRoot;

	[SerializeField]
	private TextMeshProUGUIEx DurationText;

	[SerializeField]
	private Button ItemBtn;

	[SerializeField]
	private Image ItemIcon;

	[SerializeField]
	private TextMeshProUGUIEx FinalTIme;

	[SerializeField]
	private UIGGGoPlayerHud[] PlayerHuds;

	[SerializeField]
	private UIGGGoProgress Progress;

	private GameWorld world;

	private GGGoEnvClient envClient;

	private Action<IRender> callback;

	private Vector2 _joystickDirection;

	private GGGoRuntime runtime;

	private GGGoRuntimeRollbackPVP pvpRunTime;

	private EPlayerID SelfPlayerId => envClient?.GetPlayerID() ?? EPlayerID.ID_1P;

	public bool IsRuntime
	{
		get
		{
			if (!(runtime != null))
			{
				return pvpRunTime != null;
			}
			return true;
		}
	}

	private void Start()
	{
		for (int i = 0; i < PlayerHuds.Length; i++)
		{
			PlayerHuds[i].Init((EPlayerID)i);
			PlayerHuds[i].transform.localScale = Vector3.zero;
		}
	}

	private void OnDestroy()
	{
		UnBindUI();
		StopAllCoroutines();
	}

	private void Update()
	{
		if (envClient != null && world?.World != null)
		{
			if (DurationText != null && envClient.GameState == EGameWorldState.Running)
			{
				RefreshDuration(FuncGame.GetPlayingTime(world.World).AsFloat);
			}
			if (_joystickDirection.x < 0f)
			{
				envClient.input[GGGoEnvClient.InputKey.Left] = true;
			}
			else if (_joystickDirection.x > 0f)
			{
				envClient.input[GGGoEnvClient.InputKey.Right] = true;
			}
		}
	}

	public void BindUI(GameWorld world, bool isEdit = false)
	{
		UnBindUI();
		this.world = world;
		if (world.Env is GGGoEnvClient gGGoEnvClient)
		{
			envClient = gGGoEnvClient;
			gGGoEnvClient.UIAction = (Action<IRender>)Delegate.Combine(gGGoEnvClient.UIAction, new Action<IRender>(UISyncEvent));
		}
		BgMask?.SetActive(value: true);
	}

	internal void UnBindUI()
	{
		if (envClient != null)
		{
			GGGoEnvClient gGGoEnvClient = envClient;
			gGGoEnvClient.UIAction = (Action<IRender>)Delegate.Remove(gGGoEnvClient.UIAction, new Action<IRender>(UISyncEvent));
			envClient = null;
		}
	}

	private void UISyncEvent(IRender render)
	{
		if (render == null)
		{
			return;
		}
		if (!(render is DataUIRender.UIWait uIWait))
		{
			if (!(render is DataUIRender.UIRefresh uIRefresh))
			{
				UIGGGoPlayerHud[] playerHuds;
				if (!(render is DataUIRender.UIResult uIResult))
				{
					if (!(render is DataUIRender.UINetWork uINetWork))
					{
						if (!(render is DataUIRender.UIPlayerBind uIPlayerBind))
						{
							if (!(render is DataUIRender.UIPlayerUnbind uIPlayerUnbind))
							{
								if (!(render is DataUIRender.UIMovementChange uIMovementChange))
								{
									if (!(render is DataUIRender.UIHpChange uIHpChange))
									{
										if (!(render is DataUIRender.UIItemType uIItemType))
										{
											if (!(render is DataUIRender.UIPlayerUseItem uIPlayerUseItem))
											{
												if (!(render is DataUIRender.UIPlayerHurt uIPlayerHurt))
												{
													return;
												}
												DataUIRender.UIPlayerHurt uIPlayerHurt2 = uIPlayerHurt;
												playerHuds = PlayerHuds;
												foreach (UIGGGoPlayerHud uIGGGoPlayerHud in playerHuds)
												{
													if (uIGGGoPlayerHud.PlayerID == uIPlayerHurt2.PlayerID)
													{
														try
														{
															uIGGGoPlayerHud.PlayerController?.Hurt(uIPlayerHurt2.HurtValue, uIPlayerHurt2.ProtectTime);
															break;
														}
														catch (Exception)
														{
															break;
														}
													}
												}
												return;
											}
											DataUIRender.UIPlayerUseItem uIPlayerUseItem2 = uIPlayerUseItem;
											playerHuds = PlayerHuds;
											foreach (UIGGGoPlayerHud uIGGGoPlayerHud2 in playerHuds)
											{
												if (uIGGGoPlayerHud2.PlayerID == uIPlayerUseItem2.PlayerID)
												{
													uIGGGoPlayerHud2.PlayerController?.UseItem(uIPlayerUseItem2.ItemType);
													break;
												}
											}
										}
										else
										{
											DataUIRender.UIItemType uIItemType2 = uIItemType;
											RefreshItem(uIItemType2.ItemType);
										}
									}
									else
									{
										DataUIRender.UIHpChange uIHpChange2 = uIHpChange;
										RefreshHp(uIHpChange2.PlayerInfo);
									}
								}
								else
								{
									DataUIRender.UIMovementChange uIMovementChange2 = uIMovementChange;
									RefreshMovement(uIMovementChange2.PlayerID, uIMovementChange2.MoveState);
								}
								return;
							}
							DataUIRender.UIPlayerUnbind uIPlayerUnbind2 = uIPlayerUnbind;
							playerHuds = PlayerHuds;
							foreach (UIGGGoPlayerHud uIGGGoPlayerHud3 in playerHuds)
							{
								if (uIGGGoPlayerHud3.PlayerController == uIPlayerUnbind2.Controller)
								{
									uIGGGoPlayerHud3.UnbindController();
									break;
								}
							}
							return;
						}
						DataUIRender.UIPlayerBind uIPlayerBind2 = uIPlayerBind;
						Progress?.Init(world);
						playerHuds = PlayerHuds;
						foreach (UIGGGoPlayerHud uIGGGoPlayerHud4 in playerHuds)
						{
							if (uIGGGoPlayerHud4.PlayerID == uIPlayerBind2.Controller.PlayerID)
							{
								uIGGGoPlayerHud4.BindController(uIPlayerBind2, UICamera);
								break;
							}
						}
					}
					else
					{
						DataUIRender.UINetWork obj = uINetWork;
						callback?.Invoke(obj);
					}
					return;
				}
				DataUIRender.UIResult uIResult2 = uIResult;
				bool isWin = uIResult2.IsWin;
				EPlayerID winPlayerID = uIResult2.WinPlayerID;
				if (HudRoot != null)
				{
					HudRoot.SetActive(value: false);
				}
				float num = (float)uIResult2.battleTimeMills * 0.001f;
				RefreshDuration(num);
				if (uIResult2.ShowUI)
				{
					if (IsRuntime)
					{
						GameEnd.SetActive(value: false);
						callback?.Invoke(uIResult2);
						return;
					}
					GameEnd.SetActive(value: true);
					GameEnd.transform.Find("Win").gameObject.SetActive(isWin);
					GameEnd.transform.Find("Lose").gameObject.SetActive(!isWin);
					FinalTIme?.SetText($"Final Time: {num:F3}s");
					return;
				}
				playerHuds = PlayerHuds;
				foreach (UIGGGoPlayerHud uIGGGoPlayerHud5 in playerHuds)
				{
					if (uIGGGoPlayerHud5.PlayerID == winPlayerID)
					{
						uIGGGoPlayerHud5.PlayerController?.Win();
						continue;
					}
					UIPlayerInfo uIPlayerInfo = default(UIPlayerInfo);
					UIPlayerInfo[] playerInfos = uIResult2.PlayerInfos;
					for (int j = 0; j < playerInfos.Length; j++)
					{
						UIPlayerInfo uIPlayerInfo2 = playerInfos[j];
						if (uIPlayerInfo2.PlayerID == uIGGGoPlayerHud5.PlayerID)
						{
							uIPlayerInfo = uIPlayerInfo2;
							break;
						}
					}
					if (uIPlayerInfo.CurHp > 0f)
					{
						uIGGGoPlayerHud5.PlayerController?.Die();
					}
				}
			}
			else
			{
				DataUIRender.UIRefresh uIRefresh2 = uIRefresh;
				GameEnd.SetActive(value: false);
				RefreshUI(uIRefresh2);
				if (uIRefresh2.IsStart)
				{
					OnGameStart();
				}
				callback?.Invoke(uIRefresh2);
			}
		}
		else
		{
			DataUIRender.UIWait obj2 = uIWait;
			OnGameInit();
			callback?.Invoke(obj2);
		}
	}

	private void RefreshUI(DataUIRender.UIRefresh uiRefresh)
	{
		if (uiRefresh.PlayerInfos != null)
		{
			UIPlayerInfo[] playerInfos = uiRefresh.PlayerInfos;
			foreach (UIPlayerInfo playerInfo in playerInfos)
			{
				RefreshHp(playerInfo);
			}
			RefreshItem(uiRefresh.ItemType);
		}
	}

	private void RefreshHp(UIPlayerInfo playerInfo)
	{
		UIGGGoPlayerHud[] playerHuds = PlayerHuds;
		foreach (UIGGGoPlayerHud uIGGGoPlayerHud in playerHuds)
		{
			if (uIGGGoPlayerHud.PlayerID == playerInfo.PlayerID)
			{
				uIGGGoPlayerHud.Refresh(playerInfo, playerInfo.PlayerID == SelfPlayerId);
			}
		}
	}

	private void RefreshMovement(EPlayerID playerId, int moveState)
	{
		UIGGGoPlayerHud[] playerHuds = PlayerHuds;
		foreach (UIGGGoPlayerHud uIGGGoPlayerHud in playerHuds)
		{
			if (uIGGGoPlayerHud.PlayerID == playerId)
			{
				uIGGGoPlayerHud.PlayerController?.Move(moveState);
				break;
			}
		}
	}

	private void RefreshItem(ItemType itemType)
	{
		if (itemType == ItemType.None)
		{
			ItemBtn.gameObject.SetActive(value: false);
		}
		else
		{
			ItemBtn.gameObject.SetActive(value: true);
		}
	}

	private void RefreshDuration(float duration)
	{
		if (DurationText == null)
		{
			return;
		}
		string text;
		if (GameEntry.Localization != null)
		{
			text = $"{duration:F3}";
			text = GameEntry.Localization.GetString("130076", text);
		}
		else
		{
			text = $"{duration:F3}s";
		}
		try
		{
			DurationText.SetText(text);
		}
		catch (Exception)
		{
			DurationText.text = text;
		}
	}

	private void OnGameInit()
	{
		if (TopRoot != null)
		{
			TopRoot.localPosition = new Vector3(TopRoot.localPosition.x, 200f, TopRoot.localPosition.z);
		}
		if (HudRoot != null)
		{
			HudRoot.SetActive(value: true);
		}
		UIGGGoPlayerHud[] playerHuds = PlayerHuds;
		for (int i = 0; i < playerHuds.Length; i++)
		{
			playerHuds[i].transform.localScale = Vector3.zero;
		}
		if (ItemBtn != null)
		{
			ItemBtn.gameObject.SetActive(value: false);
		}
	}

	private void OnGameStart()
	{
		if (TopRoot != null)
		{
			TopRoot.DOLocalMove(new Vector3(TopRoot.localPosition.x, 0f, TopRoot.localPosition.z), 0.5f).SetEase(Ease.InFlash);
		}
		UIGGGoPlayerHud[] playerHuds = PlayerHuds;
		foreach (UIGGGoPlayerHud uIGGGoPlayerHud in playerHuds)
		{
			if (uIGGGoPlayerHud.PlayerController != null)
			{
				uIGGGoPlayerHud.transform.localScale = Vector3.one;
			}
		}
	}

	public void OnClickLeftBtn()
	{
		if (envClient != null)
		{
			envClient.input[GGGoEnvClient.InputKey.Left] = true;
		}
	}

	public void OnClickRightBtn()
	{
		if (envClient != null)
		{
			envClient.input[GGGoEnvClient.InputKey.Right] = true;
		}
	}

	public void OnClickItemBtn()
	{
		if (envClient != null)
		{
			envClient.input[GGGoEnvClient.InputKey.Skill] = true;
		}
	}

	public void OnJoystickMove(Vector2 direction)
	{
		_joystickDirection = direction;
	}

	public void OnJoystickEnd(Vector2 direction)
	{
		_joystickDirection = Vector2.zero;
	}

	public void BindCallback(Action<IRender> handle)
	{
		callback = (Action<IRender>)Delegate.Combine(callback, handle);
	}

	public void UnBindCallback(Action<IRender> handle)
	{
		callback = (Action<IRender>)Delegate.Remove(callback, handle);
	}

	public void StartGame(string levelPath, Transform gameRoot)
	{
		runtime = base.gameObject.GetOrAddComponent<GGGoRuntime>();
		runtime.LevelRootGO = GameRoot;
		runtime.StartGame(levelPath);
		BindUI(runtime.Game);
		if (UICamera == null && GameEntry.UICamera != null)
		{
			UICamera = GameEntry.UICamera;
		}
	}

	public void EndGame()
	{
		if (runtime != null)
		{
			runtime.ExitGame();
		}
		if (pvpRunTime != null)
		{
			pvpRunTime.ExitGame();
		}
	}

	public void Dispose()
	{
		if (runtime != null)
		{
			runtime.ExitGame();
		}
		if (pvpRunTime != null)
		{
			pvpRunTime.ExitGame();
		}
	}

	public bool IsDone()
	{
		if (!IsRuntime)
		{
			return false;
		}
		if (envClient == null || world == null)
		{
			return false;
		}
		return envClient.GameState >= EGameWorldState.Preparing;
	}

	private void OnDisable()
	{
		if (runtime != null)
		{
			runtime.ExitGame();
			runtime = null;
		}
		if (pvpRunTime != null)
		{
			pvpRunTime.ExitGame();
			pvpRunTime = null;
		}
	}

	public void ConnectGameLift(int serverId, string levelPath, LuaTable data, Action onDisconnected = null, Action onConnected = null, Action onTryReconnect = null, Action onReconnected = null, bool useHybridNetwork = false)
	{
		pvpRunTime = base.gameObject.GetOrAddComponent<GGGoRuntimeRollbackPVP>();
		string uid = data.Get<string>("uid");
		string uuid = data.Get<string>("uuid");
		string text = data.Get<string>("ip");
		string text2 = data.Get<string>("port");
		string sid = data.Get<string>("sid");
		data.Get<string>("playname");
		string playerSessionId = data.Get<string>("sessionidplayer");
		string roomSessionId = data.Get<string>("sessionidgame");
		int index = data.Get<int>("p");
		string logicVersion = data.Get<string>("logicversion");
		string resVersion = data.Get<string>("resversion");
		data.Get<bool>("reconnect");
		data.Get<float>("time");
		pvpRunTime.OnServerConnected = onConnected;
		pvpRunTime.OnServerDisconnected = onDisconnected;
		pvpRunTime.OnServerTryReconnect = onTryReconnect;
		pvpRunTime.OnServerReconnected = onReconnected;
		pvpRunTime.useHybridNetwork = useHybridNetwork;
		pvpRunTime.SetUpVersion(logicVersion, resVersion);
		pvpRunTime.StartGame(GameRoot, levelPath, text + ":" + text2, uid, sid, uuid, playerSessionId, roomSessionId, index);
		BindUI(pvpRunTime.Game);
		if (UICamera == null && GameEntry.UICamera != null)
		{
			UICamera = GameEntry.UICamera;
		}
		ImageUIPing imageUIPing = base.transform.Find("Head1")?.GetComponent<ImageUIPing>();
		if (imageUIPing != null)
		{
			imageUIPing.gameObject.SetActive(value: true);
			imageUIPing.SetRuntimeRtt(pvpRunTime, envClient, isSelf: true);
		}
		ImageUIPing imageUIPing2 = base.transform.Find("Head2")?.GetComponent<ImageUIPing>();
		if (imageUIPing2 != null)
		{
			imageUIPing2.gameObject.SetActive(value: true);
			imageUIPing2.SetRuntimeRtt(pvpRunTime, envClient, isSelf: false);
		}
	}

	public void StartGameByPvpConnect()
	{
	}
}
