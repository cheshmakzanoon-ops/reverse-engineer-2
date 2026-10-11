using UnityEngine;

public class LLWorldBuffCityAsync : AsyncMono<LLWorldBuffCityAsync>
{
	public SpriteRenderer icon;

	public TextMeshProEx tmp;

	public void Init(int buffId)
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_status", buffId, "icon");
		icon.LoadSpriteAuto(templateData);
	}

	public void RefreshTxt(string showTxt)
	{
		tmp.text = showTxt;
	}
}
