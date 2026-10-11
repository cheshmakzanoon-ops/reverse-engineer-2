using UnityEngine;

public class CommonTabGroupGeneratorExample : MonoBehaviour
{
	[Header("组件引用")]
	[SerializeField]
	private CommonTabGroupGenerator tabGroup;

	[Header("自动生成模式配置")]
	[SerializeField]
	private string[] testTabNames = new string[4] { "首页", "背包", "商店", "设置" };

	private void Start()
	{
		if (tabGroup == null)
		{
			tabGroup = GetComponent<CommonTabGroupGenerator>();
			if (tabGroup == null)
			{
				Debug.LogError("CommonTabGroupGeneratorExample: 未找到CommonTabGroupGenerator组件！");
				return;
			}
		}
		tabGroup.onTabChanged.AddListener(OnTabChanged);
		GenerateExampleTabs();
	}

	private void GenerateExampleTabs()
	{
		if (testTabNames == null || testTabNames.Length == 0)
		{
			Debug.LogWarning("CommonTabGroupGeneratorExample: testTabNames为空，跳过生成");
		}
		else
		{
			tabGroup.GenerateTabs(testTabNames);
		}
	}

	private void OnTabChanged(int index)
	{
		Debug.Log(string.Format("切换到第 {0} 个页签: {1}", index, (index >= 0 && index < testTabNames.Length) ? testTabNames[index] : "无"));
		switch (index)
		{
		case 0:
			ShowHomePage();
			break;
		case 1:
			ShowBagPage();
			break;
		case 2:
			ShowShopPage();
			break;
		case 3:
			ShowSettingsPage();
			break;
		}
	}

	private void ShowHomePage()
	{
		Debug.Log("显示首页");
	}

	private void ShowBagPage()
	{
		Debug.Log("显示背包页");
	}

	private void ShowShopPage()
	{
		Debug.Log("显示商店页");
	}

	private void ShowSettingsPage()
	{
		Debug.Log("显示设置页");
	}
}
