using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof(MaskableGraphic))]
public class DynamicImage : MonoBehaviour
{
	[SerializeField]
	public bool enableIt = true;

	[SerializeField]
	public string imgPath;

	[SerializeField]
	public bool setNativeSizeAfterLoad;

	private Image _image;

	private RawImage _rawImage;

	private void Start()
	{
		if (enableIt)
		{
			Load();
		}
	}

	public void Load(bool async = true)
	{
		CacheGraphic();
		if (!(_image == null) || !(_rawImage == null))
		{
			if (string.IsNullOrEmpty(imgPath))
			{
				ClearGraphic();
			}
			else
			{
				LoadInRuntime(async);
			}
		}
	}

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

	private MaskableGraphic ClearGraphic()
	{
		if (_image != null)
		{
			_image.sprite = null;
			return _image;
		}
		if (_rawImage != null)
		{
			_rawImage.texture = null;
			return _rawImage;
		}
		return null;
	}

	private void LoadInEditor()
	{
	}

	private void LoadInRuntime(bool async)
	{
		if (_image != null)
		{
			if (async)
			{
				if (setNativeSizeAfterLoad)
				{
					_image.LoadSpriteAsync(imgPath, delegate
					{
						if (_image != null)
						{
							_image.SetNativeSize();
						}
					});
				}
				else
				{
					_image.LoadSpriteAsync(imgPath);
				}
			}
			else if (setNativeSizeAfterLoad)
			{
				_image.LoadSprite(imgPath);
				_image.SetNativeSize();
			}
			else
			{
				_image.LoadSprite(imgPath);
			}
		}
		else
		{
			if (_rawImage == null)
			{
				return;
			}
			if (async)
			{
				if (setNativeSizeAfterLoad)
				{
					_rawImage.LoadSpriteAsync(imgPath, delegate
					{
						if (_rawImage != null)
						{
							_rawImage.SetNativeSize();
						}
					});
				}
				else
				{
					_rawImage.LoadSpriteAsync(imgPath);
				}
			}
			else if (setNativeSizeAfterLoad)
			{
				_rawImage.LoadSprite(imgPath);
				_rawImage.SetNativeSize();
			}
			else
			{
				_rawImage.LoadSprite(imgPath);
			}
		}
	}
}
