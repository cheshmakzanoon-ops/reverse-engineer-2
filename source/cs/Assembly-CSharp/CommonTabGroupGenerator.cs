using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommonTabGroupGenerator : MonoBehaviour
{
	public enum GenerateMode
	{
		AutoGenerate,
		ManualPlacement
	}

	public enum ToggleSourceType
	{
		Prefab,
		TemplateNode
	}

	[Serializable]
	public class TabChangedEvent : UnityEvent<int>
	{
	}

	[Header("配置")]
	[SerializeField]
	private GenerateMode generateMode;

	[SerializeField]
	private ToggleSourceType toggleSourceType;

	[SerializeField]
	private GameObject togglePrefab;

	[SerializeField]
	private GameObject toggleTemplateNode;

	[SerializeField]
	private Transform toggleContainer;

	[SerializeField]
	private int defaultSelectedIndex;

	[SerializeField]
	private bool allowDeselect;

	[Header("只读状态")]
	[SerializeField]
	private int currentSelectedIndex = -1;

	[SerializeField]
	private List<CommonToggleItem> toggleItems = new List<CommonToggleItem>();

	private ScrollRect scrollRect;

	private bool autoScrollEnabled = true;

	private const float k_ScrollDuration = 0.3f;

	[Header("事件")]
	public TabChangedEvent onTabChanged = new TabChangedEvent();

	public int CurrentSelectedIndex => currentSelectedIndex;

	public int ToggleCount => toggleItems.Count;

	private void Awake()
	{
		if (onTabChanged == null)
		{
			onTabChanged = new TabChangedEvent();
		}
		if (toggleContainer == null)
		{
			toggleContainer = base.transform;
		}
		if (generateMode == GenerateMode.AutoGenerate && toggleSourceType == ToggleSourceType.TemplateNode && toggleTemplateNode != null)
		{
			toggleTemplateNode.SetActive(value: false);
		}
		if (generateMode == GenerateMode.ManualPlacement)
		{
			RegisterManualToggles();
		}
		scrollRect = GetComponent<ScrollRect>();
	}

	public void GenerateTabs(string[] tabNames, string[] icons = null, int selectIndex = -1)
	{
		if (generateMode == GenerateMode.ManualPlacement)
		{
			Debug.LogWarning("CommonTabGroupGenerator: 当前处于手动放置模式，无法调用GenerateTabs()，会在Awake时自动注册！请切换到自动生成模式或使用RegisterManualToggles()");
			return;
		}
		ClearTabs();
		if (tabNames == null || tabNames.Length == 0)
		{
			Debug.LogWarning("CommonTabGroupGenerator: Tab names array is empty!");
			return;
		}
		if (toggleSourceType == ToggleSourceType.Prefab && togglePrefab == null)
		{
			Debug.LogError("CommonTabGroupGenerator: Toggle prefab is not assigned!");
			return;
		}
		if (toggleSourceType == ToggleSourceType.TemplateNode && toggleTemplateNode == null)
		{
			Debug.LogError("CommonTabGroupGenerator: Toggle template node is not assigned!");
			return;
		}
		for (int i = 0; i < tabNames.Length; i++)
		{
			CreateToggle(i, tabNames[i], (icons != null && i < icons.Length) ? icons[i] : null);
		}
		int num = ((selectIndex >= 0) ? selectIndex : defaultSelectedIndex);
		if (num >= 0 && num < toggleItems.Count)
		{
			SelectTab(num, immediate: true);
		}
		if (autoScrollEnabled && scrollRect != null)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(toggleContainer as RectTransform);
			ScrollToTab(num);
		}
	}

	private void GenerateTabs(int count, string namePrefix = "Tab")
	{
		if (count <= 0)
		{
			Debug.LogWarning("Tab count must be greater than 0!");
			return;
		}
		string[] array = new string[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = $"{namePrefix} {i + 1}";
		}
		GenerateTabs(array);
	}

	private void RegisterManualToggles(bool autoInitialize = true)
	{
		if (toggleContainer == null)
		{
			Debug.LogError("CommonTabGroupGenerator: toggleContainer is null!");
			return;
		}
		toggleItems.Clear();
		currentSelectedIndex = -1;
		int childCount = toggleContainer.childCount;
		if (childCount == 0)
		{
			Debug.LogWarning("CommonTabGroupGenerator: 在toggleContainer子节点中未找到任何CommonToggleItem组件！");
			return;
		}
		int num = 0;
		for (int i = 0; i < childCount; i++)
		{
			CommonToggleItem component = toggleContainer.GetChild(i).GetComponent<CommonToggleItem>();
			if (component != null && component.gameObject.activeSelf)
			{
				if (autoInitialize)
				{
					component.InitializeManual(this, num);
				}
				toggleItems.Add(component);
				num++;
			}
		}
		if (num == 0)
		{
			Debug.LogWarning("CommonTabGroupGenerator: 在toggleContainer子节点中未找到任何CommonToggleItem组件！");
		}
		else if (defaultSelectedIndex >= 0 && defaultSelectedIndex < toggleItems.Count)
		{
			SelectTab(defaultSelectedIndex, immediate: true);
		}
	}

	private void CreateToggle(int index, string text, string iconPath = null)
	{
		if (toggleContainer == null)
		{
			Debug.LogError("CommonTabGroupGenerator: toggleContainer is null!");
			return;
		}
		GameObject gameObject = null;
		if (toggleSourceType == ToggleSourceType.Prefab)
		{
			gameObject = UnityEngine.Object.Instantiate(togglePrefab, toggleContainer);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate(toggleTemplateNode, toggleContainer);
			gameObject.SetActive(value: true);
		}
		gameObject.name = $"Toggle_{index}_{text}";
		CommonToggleItem commonToggleItem = gameObject.GetComponent<CommonToggleItem>();
		if (commonToggleItem == null)
		{
			commonToggleItem = gameObject.AddComponent<CommonToggleItem>();
		}
		commonToggleItem.gameObject.SetActive(value: true);
		commonToggleItem.Initialize(this, index, text, iconPath);
		toggleItems.Add(commonToggleItem);
	}

	public void ClearTabs()
	{
		foreach (CommonToggleItem toggleItem in toggleItems)
		{
			if (toggleItem != null && toggleItem.gameObject != null)
			{
				toggleItem.transform.SetParent(null);
				UnityEngine.Object.Destroy(toggleItem.gameObject);
			}
		}
		toggleItems.Clear();
		currentSelectedIndex = -1;
	}

	public void OnToggleClicked(CommonToggleItem clickedToggle)
	{
		int index = clickedToggle.Index;
		if (index == currentSelectedIndex)
		{
			if (allowDeselect)
			{
				clickedToggle.SetSelected(selected: false);
				currentSelectedIndex = -1;
				onTabChanged?.Invoke(-1);
			}
			return;
		}
		if (currentSelectedIndex >= 0 && currentSelectedIndex < toggleItems.Count)
		{
			toggleItems[currentSelectedIndex].SetSelected(selected: false);
		}
		clickedToggle.SetSelected(selected: true);
		currentSelectedIndex = index;
		onTabChanged?.Invoke(currentSelectedIndex);
		if (autoScrollEnabled && scrollRect != null)
		{
			ScrollToTab(index);
		}
	}

	public void SelectTab(int index, bool immediate = false)
	{
		if (index < 0 || index >= toggleItems.Count)
		{
			Debug.LogWarning($"Invalid tab index: {index}");
			return;
		}
		if (currentSelectedIndex >= 0 && currentSelectedIndex < toggleItems.Count)
		{
			toggleItems[currentSelectedIndex].SetSelected(selected: false, immediate);
		}
		toggleItems[index].SetSelected(selected: true, immediate);
		currentSelectedIndex = index;
		if (!immediate)
		{
			onTabChanged?.Invoke(currentSelectedIndex);
		}
		if (!immediate && autoScrollEnabled && scrollRect != null)
		{
			ScrollToTab(index);
		}
	}

	private CommonToggleItem GetToggle(int index)
	{
		if (index >= 0 && index < toggleItems.Count)
		{
			return toggleItems[index];
		}
		return null;
	}

	public void UpdateTabText(int index, string text)
	{
		CommonToggleItem toggle = GetToggle(index);
		if (toggle != null)
		{
			toggle.UpdateText(text);
		}
	}

	public void UpdateTabIcon(int index, string iconPath)
	{
		CommonToggleItem toggle = GetToggle(index);
		if (toggle != null)
		{
			toggle.UpdateIcon(iconPath);
		}
	}

	public void SetTabInteractable(int index, bool interactable)
	{
		CommonToggleItem toggle = GetToggle(index);
		if (toggle != null)
		{
			toggle.SetInteractable(interactable);
		}
	}

	public void SetTabRedPoint(int index, int redPointCount)
	{
		CommonToggleItem toggle = GetToggle(index);
		if (toggle != null)
		{
			toggle.SetRedPoint(redPointCount);
		}
	}

	public List<CommonToggleItem> GetAllToggles()
	{
		return new List<CommonToggleItem>(toggleItems);
	}

	public int GetCurrentSelectIndex()
	{
		return CurrentSelectedIndex;
	}

	public void SetAutoScrollEnabled(bool enabled)
	{
		autoScrollEnabled = enabled;
	}

	public void ScrollToTab(int index)
	{
		if (toggleItems == null || toggleItems.Count == 0)
		{
			Log.Error("CommonTabGroupGenerator: 没有可滚动的toggle");
			return;
		}
		if (index < 0 || index >= toggleItems.Count)
		{
			Log.Error($"CommonTabGroupGenerator: 无效的tab索引 {index}，有效范围 [0, {toggleItems.Count - 1}]");
			return;
		}
		if (scrollRect == null)
		{
			Log.Info("CommonTabGroupGenerator: 未检测到 ScrollRect 组件，无法滚动");
			return;
		}
		if (scrollRect.content == null)
		{
			Log.Error("CommonTabGroupGenerator: ScrollRect.content 未设置，请将Content设置为toggleContainer");
			return;
		}
		bool flag = scrollRect.horizontal && !scrollRect.vertical;
		bool flag2 = scrollRect.vertical && !scrollRect.horizontal;
		if (!flag && !flag2)
		{
			Log.Error("CommonTabGroupGenerator: ScrollRect必须明确设置为水平或垂直布局之一，不能同时为true或同时为false");
			return;
		}
		scrollRect.StopMovement();
		StopAllCoroutines();
		StartCoroutine(ScrollToTabCoroutine(index, flag));
	}

	private IEnumerator ScrollToTabCoroutine(int index, bool isHorizontal)
	{
		RectTransform component = toggleItems[index].GetComponent<RectTransform>();
		if (component == null)
		{
			yield break;
		}
		RectTransform contentRect = scrollRect.content;
		if (IsTargetVisible(component, contentRect, isHorizontal, out var need))
		{
			yield break;
		}
		float currentAnchoredPos = (isHorizontal ? contentRect.anchoredPosition.x : contentRect.anchoredPosition.y);
		float targetAnchoredPos = currentAnchoredPos + need;
		float elapsed = 0f;
		while (elapsed < 0.3f)
		{
			elapsed += Time.unscaledDeltaTime;
			float t = Mathf.Clamp01(elapsed / 0.3f);
			float t2 = EaseOutCubic(t);
			float num = Mathf.Lerp(currentAnchoredPos, targetAnchoredPos, t2);
			if (isHorizontal)
			{
				contentRect.anchoredPosition = new Vector2(num, contentRect.anchoredPosition.y);
			}
			else
			{
				contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, num);
			}
			yield return null;
		}
		if (isHorizontal)
		{
			contentRect.anchoredPosition = new Vector2(targetAnchoredPos, contentRect.anchoredPosition.y);
		}
		else
		{
			contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, targetAnchoredPos);
		}
	}

	private bool IsTargetVisible(RectTransform target, RectTransform contentRect, bool isHorizontal, out float need)
	{
		need = 0f;
		if (scrollRect.viewport == null)
		{
			return false;
		}
		RectTransform viewport = scrollRect.viewport;
		float num = (isHorizontal ? (contentRect.anchoredPosition.x + contentRect.rect.xMin) : (contentRect.anchoredPosition.y + contentRect.rect.yMin));
		float num2 = (isHorizontal ? viewport.anchoredPosition.x : viewport.anchoredPosition.y);
		float num3 = (isHorizontal ? viewport.rect.width : viewport.rect.height);
		float num4 = num2 + num3;
		float num5 = num + (isHorizontal ? target.anchoredPosition.x : target.anchoredPosition.y) + (isHorizontal ? target.rect.xMin : target.rect.yMin);
		float num6 = (isHorizontal ? target.rect.width : target.rect.height);
		float num7 = num5 + num6;
		if (num5 < num2)
		{
			need = num2 - num5;
			return false;
		}
		if (num7 > num4)
		{
			need = num4 - num7;
			return false;
		}
		return true;
	}

	private float EaseOutCubic(float t)
	{
		return 1f - Mathf.Pow(1f - t, 3f);
	}
}
