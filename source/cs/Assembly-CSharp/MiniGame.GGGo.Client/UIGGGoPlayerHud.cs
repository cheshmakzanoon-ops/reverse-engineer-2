using UnityEngine;

namespace MiniGame.GGGo.Client;

public class UIGGGoPlayerHud : MonoBehaviour
{
	public UIGGGoPlayerHpItemView hpView;

	private Camera UICamera;

	private RectTransform rectTransform;

	public UIGGGoPlayerController PlayerController { get; private set; }

	private Camera GameCamera
	{
		get
		{
			if (!(PlayerController != null))
			{
				return null;
			}
			return PlayerController.Camera;
		}
	}

	public EPlayerID PlayerID { get; private set; }

	public void Init(EPlayerID playerId)
	{
		PlayerID = playerId;
		rectTransform = GetComponent<RectTransform>();
		base.gameObject.SetActive(value: false);
	}

	public void Refresh(UIPlayerInfo playerInfo, bool isMe)
	{
		if (playerInfo.PlayerID == PlayerID)
		{
			hpView.Refresh(isMe, playerInfo.CurHp, playerInfo.MaxHp);
			if (PlayerController != null && playerInfo.CurHp <= 0f)
			{
				PlayerController.Die();
			}
		}
	}

	private void Update()
	{
		if (PlayerController != null && GameCamera != null && UICamera != null)
		{
			Vector3 vector = GameCamera.WorldToScreenPoint(PlayerController.UIPos.position);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent.GetComponent<RectTransform>(), vector, UICamera, out var localPoint);
			rectTransform.anchoredPosition = localPoint;
		}
	}

	public void BindController(DataUIRender.UIPlayerBind uiPlayerBind, Camera uiCamera)
	{
		PlayerController = uiPlayerBind.Controller;
		UICamera = uiCamera;
		base.gameObject.SetActive(value: true);
	}

	public void UnbindController()
	{
		PlayerController = null;
		base.gameObject.SetActive(value: false);
	}
}
