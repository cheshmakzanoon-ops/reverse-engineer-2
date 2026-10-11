using System.Collections;
using GameKit.Base;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGame.GGGo.Client;

public class UIGGGoHpItem : MonoBehaviour
{
	public Image HpBg;

	public Image HpFill;

	private CanvasGroup _fillCg;

	private void Awake()
	{
		if (HpFill != null)
		{
			_fillCg = HpFill.gameObject.GetOrAddComponent<CanvasGroup>();
		}
	}

	public void SetFilledImmediate(bool filled)
	{
		if (!(HpFill == null))
		{
			HpFill.gameObject.SetActive(filled);
			if (_fillCg != null)
			{
				_fillCg.alpha = (filled ? 1f : 0f);
			}
			HpFill.transform.localScale = (filled ? Vector3.one : (Vector3.one * 0.5f));
		}
	}

	public IEnumerator PlayShow()
	{
		if (!(HpFill == null))
		{
			HpFill.gameObject.SetActive(value: true);
			if (_fillCg != null)
			{
				_fillCg.alpha = 1f;
			}
			Transform target = HpFill.transform;
			Vector3 from = Vector3.one * 0.8f;
			Vector3 mid = Vector3.one * 1.15f;
			Vector3 to = Vector3.one;
			target.localScale = from;
			float t = 0f;
			float d1 = 0.12f;
			while (t < d1)
			{
				target.localScale = Vector3.Lerp(from, mid, t / d1);
				t += Time.deltaTime;
				yield return null;
			}
			t = 0f;
			float d2 = 0.18f;
			while (t < d2)
			{
				target.localScale = Vector3.Lerp(mid, to, t / d2);
				t += Time.deltaTime;
				yield return null;
			}
			target.localScale = to;
		}
	}

	public IEnumerator PlayHide()
	{
		if (HpFill == null)
		{
			yield break;
		}
		Transform target = HpFill.transform;
		Vector3 start = Vector3.one;
		Vector3 peak = Vector3.one * 1.15f;
		Vector3 end = Vector3.one * 0.5f;
		float t = 0f;
		float d1 = 0.12f;
		while (t < d1)
		{
			target.localScale = Vector3.Lerp(start, peak, t / d1);
			t += Time.deltaTime;
			yield return null;
		}
		t = 0f;
		float d2 = 0.25f;
		float startAlpha = ((_fillCg != null) ? _fillCg.alpha : 1f);
		while (t < d2)
		{
			target.localScale = Vector3.Lerp(peak, end, t / d2);
			if (_fillCg != null)
			{
				_fillCg.alpha = Mathf.Lerp(startAlpha, 0f, t / d2);
			}
			t += Time.deltaTime;
			yield return null;
		}
		target.localScale = end;
		if (_fillCg != null)
		{
			_fillCg.alpha = 0f;
		}
		HpFill.gameObject.SetActive(value: false);
	}
}
