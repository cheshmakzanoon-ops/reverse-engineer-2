using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof(MaskableGraphic))]
public class DynamicSkinImage : MonoBehaviour
{
	[Serializable]
	public class SkinInfo
	{
		public SeasonType key;

		public bool invert;

		public bool includePreview;

		public string imgPath;

		public Color color = Color.white;

		public Rect rect = Rect.zero;
	}

	[SerializeField]
	public SkinInfo[] m_skins;

	[SerializeField]
	public bool enableIt = true;

	private Image _image;

	private RawImage _rawImage;

	private void CacheGraphic()
	{
		if (_image == null)
		{
			_image = GetComponent<Image>();
		}
		if (!(_image != null) && _rawImage == null)
		{
			_rawImage = GetComponent<RawImage>();
		}
	}

	public void Awake()
	{
		if (enableIt)
		{
			SceneSkinMeta baseSkinMeta = SceneSkinManager.Instance.GetBaseSkinMeta();
			if (baseSkinMeta != null)
			{
				SwitchSkin(baseSkinMeta.GetMapType(), baseSkinMeta.GetPreviewType());
			}
		}
	}

	private void SwitchSkin(SeasonType type, SeasonType PreviewType)
	{
		if (m_skins == null || m_skins.Length == 0)
		{
			return;
		}
		if (PreviewType != SeasonType.Nothing)
		{
			SkinInfo[] skins = m_skins;
			foreach (SkinInfo skinInfo in skins)
			{
				if (skinInfo != null && !skinInfo.invert && skinInfo.includePreview && skinInfo.key == PreviewType)
				{
					ApplySkin(skinInfo);
					return;
				}
			}
		}
		SwitchSkin(type);
	}

	public void SwitchSkin(SeasonType type, bool async = false)
	{
		if (m_skins == null || m_skins.Length == 0)
		{
			return;
		}
		SkinInfo[] skins = m_skins;
		foreach (SkinInfo skinInfo in skins)
		{
			if (skinInfo != null && !skinInfo.invert && skinInfo.key == type)
			{
				ApplySkin(skinInfo, async);
				return;
			}
		}
		skins = m_skins;
		foreach (SkinInfo skinInfo2 in skins)
		{
			if (skinInfo2 != null && skinInfo2.invert && skinInfo2.key != type)
			{
				ApplySkin(skinInfo2, async);
				break;
			}
		}
	}

	public void ApplySkin(SkinInfo skin, bool async = false)
	{
		if (skin != null && !skin.rect.Equals(Rect.zero))
		{
			RectTransform component = GetComponent<RectTransform>();
			if (component != null)
			{
				component.Set_localPosition(skin.rect.x, skin.rect.y, 0f);
				component.Set_sizeDelta(skin.rect.width, skin.rect.height);
			}
		}
		CacheGraphic();
		if (skin == null || skin.imgPath.IsNullOrEmpty())
		{
			if (_image != null)
			{
				_image.sprite = null;
				if (skin != null)
				{
					_image.color = skin.color;
				}
			}
			else if (_rawImage != null)
			{
				_rawImage.texture = null;
				if (skin != null)
				{
					_rawImage.color = skin.color;
				}
			}
		}
		else if (_image != null)
		{
			if (async)
			{
				_image.LoadSpriteAsync(skin.imgPath);
			}
			else
			{
				_image.LoadSprite(skin.imgPath);
			}
			_image.color = skin.color;
		}
		else if (_rawImage != null)
		{
			if (async)
			{
				_rawImage.LoadSpriteAsync(skin.imgPath);
			}
			else
			{
				_rawImage.LoadSprite(skin.imgPath);
			}
			_rawImage.color = skin.color;
		}
	}
}
