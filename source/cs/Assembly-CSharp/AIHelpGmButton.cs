using System;
using AIHelp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AIHelpGmButton : MonoBehaviour
{
	private const string ZENDESK_ENV_KEY = "ZendeskEnv";

	private const string ENV_DEV = "Dev";

	private const string ENV_TEST = "Test";

	private const string ENV_ONLINE = "Online";

	private const int ZENDESK_ENV_DEV = 0;

	private const int ZENDESK_ENV_TEST = 1;

	private const int ZENDESK_ENV_ONLINE = 2;

	private Button _btnDev;

	private Button _btnTest;

	private Button _btnOnline;

	private Image _imgDevBg;

	private Image _imgTestBg;

	private Image _imgOnlineBg;

	private TextMeshProUGUI _txtDevLabel;

	private TextMeshProUGUI _txtTestLabel;

	private TextMeshProUGUI _txtOnlineLabel;

	private Color _selectedColor = new Color(0.2f, 0.7f, 1f, 1f);

	private Color _deselectedColor = new Color(0.8f, 0.8f, 0.8f, 1f);

	private Color _selectedTextColor = Color.white;

	private Color _deselectedTextColor = Color.black;

	private int _currentEnv;

	public void Initialize(Transform parent)
	{
		if (parent.Find("AIHelpGmButtonPanel") != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GameObject gameObject = new GameObject("AIHelpGmButtonPanel");
		gameObject.transform.SetParent(parent, worldPositionStays: false);
		RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
		rectTransform.anchoredPosition = new Vector2(178f, 3f);
		rectTransform.sizeDelta = new Vector2(400f, 60f);
		HorizontalLayoutGroup horizontalLayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
		horizontalLayoutGroup.spacing = 10f;
		horizontalLayoutGroup.padding = new RectOffset(10, 10, 10, 10);
		horizontalLayoutGroup.childForceExpandWidth = true;
		horizontalLayoutGroup.childForceExpandHeight = true;
		GameObject obj = new GameObject("Label");
		obj.transform.SetParent(gameObject.transform, worldPositionStays: false);
		LayoutElement layoutElement = obj.AddComponent<LayoutElement>();
		layoutElement.preferredWidth = 80f;
		layoutElement.flexibleWidth = 0f;
		TextMeshProUGUI textMeshProUGUI = obj.AddComponent<TextMeshProUGUI>();
		textMeshProUGUI.text = "Zendesk Env:";
		textMeshProUGUI.fontSize = 18f;
		textMeshProUGUI.alignment = TextAlignmentOptions.MidlineLeft;
		_btnDev = CreateButton(gameObject, "Dev", 0);
		_btnTest = CreateButton(gameObject, "Test", 1);
		_btnOnline = CreateButton(gameObject, "Online", 2);
		CreateButton(gameObject, "URL", -1);
		_currentEnv = PlayerPrefs.GetInt("ZendeskEnv", 2);
		RefreshButtonStates();
	}

	private Button CreateButton(GameObject parent, string label, int envType)
	{
		GameObject gameObject = new GameObject("Btn" + label);
		gameObject.transform.SetParent(parent.transform, worldPositionStays: false);
		gameObject.AddComponent<RectTransform>();
		LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();
		layoutElement.preferredWidth = 80f;
		layoutElement.preferredHeight = 40f;
		layoutElement.flexibleWidth = 1f;
		layoutElement.flexibleHeight = 0f;
		Image image = gameObject.AddComponent<Image>();
		image.color = _deselectedColor;
		Button button = gameObject.AddComponent<Button>();
		button.targetGraphic = image;
		ColorBlock colors = button.colors;
		colors.normalColor = _deselectedColor;
		colors.highlightedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
		colors.pressedColor = new Color(0.6f, 0.6f, 0.6f, 1f);
		colors.disabledColor = new Color(0.5f, 0.5f, 0.5f, 1f);
		button.colors = colors;
		GameObject obj = new GameObject("Text");
		obj.transform.SetParent(gameObject.transform, worldPositionStays: false);
		RectTransform rectTransform = obj.AddComponent<RectTransform>();
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.offsetMin = Vector2.zero;
		rectTransform.offsetMax = Vector2.zero;
		TextMeshProUGUI textMeshProUGUI = obj.AddComponent<TextMeshProUGUI>();
		textMeshProUGUI.text = label;
		textMeshProUGUI.fontSize = 20f;
		textMeshProUGUI.alignment = TextAlignmentOptions.Center;
		textMeshProUGUI.color = _deselectedTextColor;
		if (envType == 0)
		{
			_imgDevBg = image;
			_txtDevLabel = textMeshProUGUI;
		}
		else if (envType == 1)
		{
			_imgTestBg = image;
			_txtTestLabel = textMeshProUGUI;
		}
		else if (envType == 2)
		{
			_imgOnlineBg = image;
			_txtOnlineLabel = textMeshProUGUI;
		}
		if (envType == -1)
		{
			button.onClick.AddListener(OnShowUrlButtonClicked);
		}
		else
		{
			button.onClick.AddListener(delegate
			{
				OnEnvironmentButtonClicked(envType);
			});
		}
		return button;
	}

	private void OnShowUrlButtonClicked()
	{
		try
		{
			string url = AIHelpEnvConfig.GetUrl();
			Debug.Log("[AIHelpGmButton] === Zendesk URL ===");
			Debug.Log($"URL Length: {url.Length}");
			int num = 80;
			for (int i = 0; i < url.Length; i += num)
			{
				Debug.Log(url.Substring(i, Math.Min(num, url.Length - i)));
			}
			Debug.Log("[AIHelpGmButton] === URL End ===");
			GUIUtility.systemCopyBuffer = url;
			Debug.Log("[AIHelpGmButton] URL已复制到剪贴板");
		}
		catch (Exception ex)
		{
			Debug.LogError("[AIHelpGmButton] 获取URL失败: " + ex.Message);
		}
	}

	private void OnEnvironmentButtonClicked(int envType)
	{
		_currentEnv = envType;
		PlayerPrefs.SetInt("ZendeskEnv", envType);
		RefreshButtonStates();
		string envName = GetEnvName(envType);
		Debug.Log("[AIHelpGmButton] Zendesk环境已切换为: " + envName);
	}

	private void RefreshButtonStates()
	{
		UpdateButtonState(_btnDev, _imgDevBg, _txtDevLabel, _currentEnv == 0);
		UpdateButtonState(_btnTest, _imgTestBg, _txtTestLabel, _currentEnv == 1);
		UpdateButtonState(_btnOnline, _imgOnlineBg, _txtOnlineLabel, _currentEnv == 2);
	}

	private void UpdateButtonState(Button button, Image bgImage, TextMeshProUGUI textLabel, bool isSelected)
	{
		if (!(button == null) && !(bgImage == null) && !(textLabel == null))
		{
			if (isSelected)
			{
				bgImage.color = _selectedColor;
				textLabel.color = _selectedTextColor;
				ColorBlock colors = button.colors;
				colors.normalColor = _selectedColor;
				button.colors = colors;
			}
			else
			{
				bgImage.color = _deselectedColor;
				textLabel.color = _deselectedTextColor;
				ColorBlock colors2 = button.colors;
				colors2.normalColor = _deselectedColor;
				button.colors = colors2;
			}
		}
	}

	private string GetEnvName(int envType)
	{
		return envType switch
		{
			1 => "Test", 
			2 => "Online", 
			_ => "Dev", 
		};
	}

	public int GetCurrentEnvironment()
	{
		return _currentEnv;
	}

	private void OnDestroy()
	{
		if (_btnDev != null)
		{
			_btnDev.onClick.RemoveAllListeners();
		}
		if (_btnTest != null)
		{
			_btnTest.onClick.RemoveAllListeners();
		}
		if (_btnOnline != null)
		{
			_btnOnline.onClick.RemoveAllListeners();
		}
	}
}
