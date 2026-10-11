using UnityEngine;

public class KR_RefundNoticeNode : MonoBehaviour
{
	[Header("Korea Settings")]
	public bool usePosition;

	public Vector2 koreaAnchoredPos;

	public bool useSize;

	public Vector2 koreaSizeDelta;

	private void OnEnable()
	{
		if (GameEntry.Sdk.IsKoreaRegion())
		{
			ApplyLayout();
		}
	}

	public void ApplyLayout()
	{
		RectTransform component = GetComponent<RectTransform>();
		if (!(component == null))
		{
			if (usePosition)
			{
				component.anchoredPosition = koreaAnchoredPos;
			}
			if (useSize)
			{
				component.sizeDelta = koreaSizeDelta;
			}
		}
	}
}
