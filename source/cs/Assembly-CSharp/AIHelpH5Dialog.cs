using System;
using AIHelp;
using UnityEngine;
using UnityEngine.UI;

public class AIHelpH5Dialog : UIPrivacyBaseView
{
	private static AIHelpH5Dialog _instance;

	private GameObject _rootGameObject;

	private bool inClose;

	private TextMeshProUGUIEx _textTitle;

	private Button _btnBack;

	private Button _btnMessage;

	private TextMeshProUGUIEx _textMessage;

	private GameObject _compContainer;

	public static AIHelpH5Dialog Instance => _instance ?? (_instance = new AIHelpH5Dialog());

	public override string GetLoadAssetPath()
	{
		return "Assets/Main/Prefabs/UI/UIZendesk/UIZendesk.prefab";
	}

	public override void OnCreate(GameObject go)
	{
		_rootGameObject = go;
		ComponentDefine();
		DataDefine();
	}

	public override void OnDestroy()
	{
		DataDestroy();
		ComponentDestroy();
		_instance = null;
	}

	private T FindComponent<T>(string path) where T : Component
	{
		Transform transform = (_rootGameObject ? _rootGameObject.transform.Find(path) : null);
		if (!transform)
		{
			return null;
		}
		return transform.GetComponent<T>();
	}

	private void ComponentDefine()
	{
		_textTitle = FindComponent<TextMeshProUGUIEx>("Root/TopBar/TextTitle");
		_btnBack = FindComponent<Button>("Root/BottomBar/BtnBack");
		_btnMessage = FindComponent<Button>("Root/BottomBar/BtnMessage");
		_textMessage = FindComponent<TextMeshProUGUIEx>("Root/BottomBar/BtnMessage/TextMessage");
		_compContainer = _rootGameObject.transform.Find("Root/Middle/Container").gameObject;
		_rootGameObject.transform.Find("Root/BottomBar/BtnMessage/dot").gameObject.SetActive(value: false);
		if (_btnBack != null)
		{
			_btnBack.onClick.AddListener(OnBtnBackClick);
		}
		if (_btnMessage != null)
		{
			_btnMessage.onClick.AddListener(OnBtnMessageClick);
		}
		_btnMessage.gameObject.SetActive(value: false);
		if (CommonUtils.IsDebug())
		{
			InitializeGMButton();
		}
	}

	private void InitializeGMButton()
	{
		try
		{
			Transform transform = _rootGameObject.transform.Find("Root/TopBar");
			if (transform != null)
			{
				transform.gameObject.AddComponent<AIHelpGmButton>().Initialize(transform);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("[AIHelpH5Dialog] 初始化GM按钮失败: " + ex.Message);
		}
	}

	private void ComponentDestroy()
	{
		AIHelpGmButton aIHelpGmButton = _rootGameObject?.GetComponentInChildren<AIHelpGmButton>();
		if (aIHelpGmButton != null)
		{
			UnityEngine.Object.Destroy(aIHelpGmButton.gameObject);
		}
		_textTitle = null;
		_btnBack = null;
		_btnMessage = null;
		_textMessage = null;
		_compContainer = null;
	}

	private void DataDefine()
	{
		if (_textTitle != null)
		{
			_textTitle.text = GameEntry.Localization.GetString("gm_title_helpcenter");
		}
		if (_textMessage != null)
		{
			_textMessage.text = GameEntry.Localization.GetString("gm_btn_contactus");
		}
		inClose = false;
		string url = AIHelpEnvConfig.GetUrl();
		ZendeskSupportView.Show(_compContainer, url, delegate
		{
			CloseSelf();
		});
	}

	private void DataDestroy()
	{
		ZendeskSupportView.Close();
	}

	private void OnBtnBackClick()
	{
		CloseSelf();
	}

	private void OnBtnMessageClick()
	{
		ZendeskSupportView.ShowMessaging();
		CloseSelf();
	}

	private void CloseSelf()
	{
		if (!inClose)
		{
			inClose = true;
			AIHelpProxy.CloseAiHelpUnreadCount();
			ClosePrivacyView();
		}
	}
}
