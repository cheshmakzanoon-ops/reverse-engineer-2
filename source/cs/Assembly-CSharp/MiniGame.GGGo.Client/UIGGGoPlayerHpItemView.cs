using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGame.GGGo.Client;

public class UIGGGoPlayerHpItemView : MonoBehaviour
{
	public Slider Slider_Self;

	public Slider Slider_Target;

	public Slider Background;

	public RectTransform Go_FengGe;

	private RectTransform RectTransform;

	public float FixedTotalWidth = 100f;

	public float EdgePadding = 4f;

	private bool IsMe;

	private List<RectTransform> FenGeList = new List<RectTransform>();

	private List<Tween> Tweeners = new List<Tween>();

	private Slider SelfSlider
	{
		get
		{
			if (!IsMe)
			{
				return Slider_Target;
			}
			return Slider_Self;
		}
	}

	private Slider TargetSlider
	{
		get
		{
			if (!IsMe)
			{
				return Slider_Self;
			}
			return Slider_Target;
		}
	}

	private void Awake()
	{
		RectTransform = GetComponent<RectTransform>();
	}

	public void Refresh(bool isMe, float curHp, int maxHp)
	{
		if (RectTransform == null)
		{
			return;
		}
		IsMe = isMe;
		SelfSlider.gameObject.SetActive(value: true);
		TargetSlider.gameObject.SetActive(value: false);
		float num = ((FixedTotalWidth > 0f) ? FixedTotalWidth : RectTransform.sizeDelta.x);
		RectTransform.sizeDelta = new Vector2(num, RectTransform.sizeDelta.y);
		for (int i = 0; i < FenGeList.Count; i++)
		{
			FenGeList[i].gameObject.SetActive(value: false);
		}
		for (int j = 0; j < maxHp - 1; j++)
		{
			bool num2 = j < FenGeList.Count;
			RectTransform rectTransform = null;
			if (!num2)
			{
				rectTransform = Object.Instantiate(Go_FengGe, RectTransform);
				FenGeList.Add(rectTransform);
			}
			else
			{
				rectTransform = FenGeList[j];
			}
			rectTransform.gameObject.SetActive(value: true);
			float num3 = Mathf.Max(1f, num - 2f * EdgePadding) / (float)maxHp;
			rectTransform.anchoredPosition = new Vector2(EdgePadding + num3 * (float)(j + 1), rectTransform.anchoredPosition.y);
		}
		ClearTween();
		float endValue = curHp / (float)maxHp;
		Tweeners.Add(SelfSlider.DOValue(endValue, 0.05f));
		Tweeners.Add(Background.DOValue(endValue, 0.2f));
	}

	private void OnDestroy()
	{
		ClearTween();
	}

	private void ClearTween()
	{
		for (int i = 0; i < Tweeners.Count; i++)
		{
			Tweeners[i].Kill();
		}
		Tweeners.Clear();
	}
}
