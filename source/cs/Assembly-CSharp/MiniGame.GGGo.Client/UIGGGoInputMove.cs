using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MiniGame.GGGo.Client;

public class UIGGGoInputMove : MonoBehaviour
{
	private enum Side
	{
		None,
		Left,
		Right
	}

	[Header("区域（可为空，空则默认屏幕左右半区）")]
	public RectTransform leftArea;

	public RectTransform rightArea;

	[Header("高亮按钮")]
	public Image leftBtn;

	public Image rightBtn;

	[Header("高亮颜色")]
	public Color normalColor = Color.white;

	public Color highlightColor = new Color(1f, 1f, 1f, 0.6f);

	[Header("可在面板接入的事件（在 Inspector 中绑定方法）")]
	public UnityEvent onLeft;

	public UnityEvent onRight;

	private Side currentSide;

	private void Update()
	{
		currentSide = Side.None;
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
			{
				currentSide = GetSide(touch.position);
			}
		}
		else if (Application.isEditor && Input.GetMouseButton(0))
		{
			currentSide = GetSide(Input.mousePosition);
		}
		if (currentSide == Side.Left)
		{
			onLeft?.Invoke();
		}
		else if (currentSide == Side.Right)
		{
			onRight?.Invoke();
		}
		UpdateHighlight();
	}

	private Side GetSide(Vector2 screenPos)
	{
		if (screenPos.x < (float)Screen.width * 0.5f)
		{
			return Side.Left;
		}
		if (screenPos.x >= (float)Screen.width * 0.5f)
		{
			return Side.Right;
		}
		return Side.None;
	}

	private void UpdateHighlight()
	{
		if (leftBtn != null)
		{
			leftBtn.color = ((currentSide == Side.Left) ? highlightColor : normalColor);
		}
		if (rightBtn != null)
		{
			rightBtn.color = ((currentSide == Side.Right) ? highlightColor : normalColor);
		}
	}
}
