using GameFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonToggleItem : MonoBehaviour
{
	[Header("组件引用 (自动索引)")]
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private Button button;

	[SerializeField]
	private GameObject disableGroup;

	[SerializeField]
	private GameObject enableGroup;

	[SerializeField]
	private TextMeshProUGUI disableText;

	[SerializeField]
	private TextMeshProUGUI enableText;

	[SerializeField]
	private Image disableIcon;

	[SerializeField]
	private Image enableIcon;

	[SerializeField]
	private GameObject redPoint;

	[SerializeField]
	private TextMeshProUGUI redPointText;

	[Header("只读状态")]
	[SerializeField]
	private bool isSelected;

	[SerializeField]
	private int index = -1;

	private CommonTabGroupGenerator parentGroup;

	public int Index => index;

	public bool IsSelected => isSelected;

	private void Awake()
	{
		AutoIndexComponents();
		if (button != null)
		{
			button.onClick.AddListener(OnClick);
		}
	}

	private void AutoIndexComponents()
	{
		if (animator == null)
		{
			animator = GetComponent<Animator>();
		}
		if (button == null)
		{
			button = GetComponent<Button>();
		}
		Transform transform = base.transform.Find("Content");
		if (transform == null)
		{
			Debug.LogWarning("CommonToggleItem: 未找到Content节点 on " + base.gameObject.name);
			return;
		}
		Transform transform2 = transform.Find("DisableGroup");
		if (transform2 != null)
		{
			disableGroup = transform2.gameObject;
			Transform transform3 = transform2.Find("DisableText");
			if (transform3 != null)
			{
				disableText = transform3.GetComponent<TextMeshProUGUI>();
			}
			Transform transform4 = transform2.Find("DisableIcon");
			if (transform4 != null)
			{
				disableIcon = transform4.GetComponent<Image>();
			}
		}
		Transform transform5 = transform.Find("EnableGroup");
		if (transform5 != null)
		{
			enableGroup = transform5.gameObject;
			Transform transform6 = transform5.Find("EnableText");
			if (transform6 != null)
			{
				enableText = transform6.GetComponent<TextMeshProUGUI>();
			}
			Transform transform7 = transform5.Find("EnableIcon");
			if (transform7 != null)
			{
				enableIcon = transform7.GetComponent<Image>();
			}
		}
		Transform transform8 = transform.Find("CommonRedPoint");
		if (transform8 != null)
		{
			redPoint = transform8.gameObject;
			Transform transform9 = transform8.Find("Num");
			if (transform9 != null)
			{
				redPointText = transform9.GetComponent<TextMeshProUGUI>();
			}
		}
	}

	public void Initialize(CommonTabGroupGenerator group, int tabIndex, string text, string iconPath = null)
	{
		parentGroup = group;
		index = tabIndex;
		if (disableText == null)
		{
			Log.Error("CommonToggleItem: 未找到DisableText on " + base.gameObject.name);
		}
		if (enableText == null)
		{
			Log.Error("CommonToggleItem: 未找到EnableText on " + base.gameObject.name);
		}
		SetTextSafely(disableText, text);
		SetTextSafely(enableText, text);
		if (iconPath != null)
		{
			if (disableIcon != null)
			{
				disableIcon.LoadSpriteAsync(iconPath);
				disableIcon.gameObject.SetActive(value: true);
			}
			if (enableIcon != null)
			{
				enableIcon.LoadSpriteAsync(iconPath);
				enableIcon.gameObject.SetActive(value: true);
			}
		}
		else
		{
			if (disableIcon != null)
			{
				disableIcon.gameObject.SetActive(value: false);
			}
			if (enableIcon != null)
			{
				enableIcon.gameObject.SetActive(value: false);
			}
		}
		SetSelected(selected: false, immediate: true);
		if (redPoint != null)
		{
			redPoint.SetActive(value: false);
		}
	}

	public void InitializeManual(CommonTabGroupGenerator group, int tabIndex)
	{
		parentGroup = group;
		index = tabIndex;
		SetSelected(selected: false, immediate: true);
	}

	private void OnClick()
	{
		parentGroup?.OnToggleClicked(this);
	}

	public void SetSelected(bool selected, bool immediate = false)
	{
		if (isSelected == selected && !immediate)
		{
			return;
		}
		isSelected = selected;
		if (animator != null)
		{
			if (immediate)
			{
				animator.Play(selected ? "IsEnable" : "IsDisable", 0, 1f);
				if (disableGroup != null)
				{
					disableGroup.SetActive(!selected);
				}
				if (enableGroup != null)
				{
					enableGroup.SetActive(selected);
				}
			}
			else
			{
				animator.Play(selected ? "ToEnable" : "ToDisable");
			}
		}
		else
		{
			if (disableGroup != null)
			{
				disableGroup.SetActive(!selected);
			}
			if (enableGroup != null)
			{
				enableGroup.SetActive(selected);
			}
		}
	}

	public void UpdateText(string text)
	{
		SetTextSafely(disableText, text);
		SetTextSafely(enableText, text);
	}

	private void SetTextSafely(TextMeshProUGUI textComponent, string text)
	{
		if (!(textComponent == null))
		{
			textComponent.SetText(text);
		}
	}

	public void UpdateIcon(string iconPath)
	{
		if (iconPath != null)
		{
			if (disableIcon != null)
			{
				disableIcon.LoadSpriteAsync(iconPath);
				disableIcon.gameObject.SetActive(value: true);
			}
			if (enableIcon != null)
			{
				enableIcon.LoadSpriteAsync(iconPath);
				enableIcon.gameObject.SetActive(value: true);
			}
		}
		else
		{
			if (disableIcon != null)
			{
				disableIcon.gameObject.SetActive(value: false);
			}
			if (enableIcon != null)
			{
				enableIcon.gameObject.SetActive(value: false);
			}
		}
	}

	public void SetInteractable(bool interactable)
	{
		if (button != null)
		{
			button.interactable = interactable;
		}
	}

	public void SetRedPoint(int redPointCount)
	{
		if (redPoint != null)
		{
			redPoint.SetActive(redPointCount > 0);
		}
		if (redPointText != null)
		{
			redPointText.SetText(redPointCount.ToString());
		}
	}
}
