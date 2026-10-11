using System;
using MiniGame.Core;

namespace MiniGame.GGGo.Client;

public class DataUIRender
{
	public abstract class UIRender<T> : IRender, IDisposable where T : class, IDisposable, new()
	{
		public abstract UIRenderType Type { get; }

		public void Recycle()
		{
			GameTempObjectPool<T>.Recycle(this as T);
		}

		public static T Fetch()
		{
			return GameTempObjectPool<T>.Fetch();
		}

		public abstract void Dispose();
	}

	public class UIRefresh : UIRender<UIRefresh>
	{
		public bool IsStart;

		public UIPlayerInfo[] PlayerInfos;

		public ItemType ItemType;

		public override UIRenderType Type => UIRenderType.UIRefresh;

		public override void Dispose()
		{
		}
	}

	public class UIResult : UIRender<UIResult>
	{
		public bool ShowUI;

		public bool IsWin;

		public long battleTimeMills;

		public int SelfPlayerID;

		public EPlayerID WinPlayerID;

		public UIPlayerInfo[] PlayerInfos;

		public string ValidationStr;

		public override UIRenderType Type => UIRenderType.UIResult;

		public override void Dispose()
		{
			ShowUI = false;
			IsWin = false;
			battleTimeMills = 0L;
			SelfPlayerID = 0;
			WinPlayerID = EPlayerID.ID_1P;
			ValidationStr = null;
		}
	}

	public class UIPlayerBind : UIRender<UIPlayerBind>
	{
		public UIGGGoPlayerController Controller;

		public override UIRenderType Type => UIRenderType.UIPlayerBind;

		public override void Dispose()
		{
			Controller = null;
		}
	}

	public class UIHpChange : UIRender<UIHpChange>
	{
		public UIPlayerInfo PlayerInfo;

		public override UIRenderType Type => UIRenderType.UIHpChange;

		public override void Dispose()
		{
		}
	}

	public class UIPlayerHurt : UIRender<UIPlayerHurt>
	{
		public EPlayerID PlayerID;

		public float HurtValue;

		public float ProtectTime;

		public override UIRenderType Type => UIRenderType.UIPlayerHurt;

		public override void Dispose()
		{
			PlayerID = EPlayerID.ID_1P;
			HurtValue = 0f;
			ProtectTime = 0f;
		}
	}

	public class UIMovementChange : UIRender<UIMovementChange>
	{
		public EPlayerID PlayerID;

		public int MoveState;

		public override UIRenderType Type => UIRenderType.UIMovementChange;

		public override void Dispose()
		{
			PlayerID = EPlayerID.ID_1P;
			MoveState = 0;
		}
	}

	public class UIItemType : UIRender<UIItemType>
	{
		public ItemType ItemType;

		public override UIRenderType Type => UIRenderType.UIItemType;

		public override void Dispose()
		{
			ItemType = ItemType.None;
		}
	}

	public class UIPlayerUseItem : UIRender<UIPlayerUseItem>
	{
		public EPlayerID PlayerID;

		public ItemType ItemType;

		public override UIRenderType Type => UIRenderType.UIPlayerUseItem;

		public override void Dispose()
		{
			PlayerID = EPlayerID.ID_1P;
			ItemType = ItemType.None;
		}
	}

	public class UIPlayerUnbind : UIRender<UIPlayerUnbind>
	{
		public UIGGGoPlayerController Controller;

		public override UIRenderType Type => UIRenderType.UIPlayerUnbind;

		public override void Dispose()
		{
			Controller = null;
		}
	}

	public class UIWait : UIRender<UIWait>
	{
		public float WaitTime;

		public override UIRenderType Type => UIRenderType.UIWait;

		public override void Dispose()
		{
			WaitTime = 0f;
		}
	}

	public class UINetWork : UIRender<UINetWork>
	{
		public bool SuccOrFair;

		public override UIRenderType Type => UIRenderType.UINetWork;

		public override void Dispose()
		{
			SuccOrFair = false;
		}
	}
}
