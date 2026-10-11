using UnityEngine;

public class LLWorldOccupyAsync : AsyncMono<LLWorldOccupyAsync>
{
	public SpriteRenderer Bg;

	public TextMeshProEx tmp;

	public void Refresh(bool isEnemy, string showTxt)
	{
		Bg.LoadSpriteAuto(isEnemy ? "Assets/Main/Sprites/LodIcon/mjc_wzz_jijianshitubg_hong.png" : "Assets/Main/Sprites/LodIcon/mjc_wzz_jijianshitubg_lan.png");
		tmp.text = showTxt;
	}
}
