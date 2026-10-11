using System.Globalization;
using GameFramework;
using GameKit.Base;
using UnityEngine;
using UnityEngine.UI;
using VEngine;

public class UISplashLoadingComponent : MonoBehaviour
{
	public const string KEY_LOGO = "KEY_SPLASH_LOGO";

	public const string KEY_LOADING = "KEY_SPLASH_LOADING";

	public const string ASSET_PATH = "Assets/Main/Loading/Prefabs/UISplashLoading.prefab";

	public RawImage bg;

	public RawImage background;

	public RawImage logoImage;

	public float duration = 1f;

	private bool _hiding;

	private float _elapsed;

	private Asset _loadingAsset;

	private Asset _backgroundAsset;

	private Asset _logoImageAsset;

	private Canvas _canvas;

	private Color _white = new Color(1f, 1f, 1f, 1f);

	private bool _hideNativeSplash;

	private int _gapFrame4HideNativeSplash;

	public bool fadeoutComplete => _elapsed >= duration;

	public static UISplashLoadingComponent Load()
	{
		Asset asset = GameEntry.Resource.LoadAsset("Assets/Main/Loading/Prefabs/UISplashLoading.prefab", typeof(GameObject));
		if (asset != null && !asset.isError)
		{
			GameObject obj = Object.Instantiate(asset.asset as GameObject);
			RectTransform component = obj.GetComponent<RectTransform>();
			component.SetParent(GameEntry.UIContainer, worldPositionStays: false);
			component.localScale = Vector3.one;
			component.offsetMin = Vector3.zero;
			component.offsetMax = Vector3.zero;
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.pivot = new Vector2(0.5f, 0.5f);
			component.SetAsLastSibling();
			UISplashLoadingComponent component2 = obj.GetComponent<UISplashLoadingComponent>();
			component2.LoadPlayerPref();
			component2._loadingAsset = asset;
			return component2;
		}
		return null;
	}

	private void LoadPlayerPref()
	{
		ApplicationLaunch.StepLog("Show Splash Loading");
		_canvas = base.gameObject.GetOrAddComponent<Canvas>();
		_canvas.overrideSorting = true;
		_canvas.sortingOrder = 9999;
		bool flag = false;
		string text = GameEntry.Setting.GetString("KEY_SPLASH_LOADING");
		if (!string.IsNullOrEmpty(text))
		{
			string texturePathByName = GetTexturePathByName(text);
			if (GameEntry.Resource.PrefabAssetsDownloaded(texturePathByName))
			{
				_backgroundAsset = GameEntry.Resource.LoadAsset(texturePathByName, typeof(Texture));
				if (_backgroundAsset != null)
				{
					background.texture = (Texture)_backgroundAsset.asset;
					flag = true;
				}
			}
		}
		if (!flag)
		{
			background.enabled = false;
		}
		bool flag2 = false;
		string text2 = GameEntry.Setting.GetString("KEY_SPLASH_LOGO");
		Log.Info("[UISplashLoadingComponent] get KEY_LOGO: " + text2);
		if (!string.IsNullOrEmpty(text2))
		{
			string[] array = text2.Split(new char[1] { '|' });
			if (array.Length >= 7)
			{
				string text3 = array[0];
				float x = float.Parse(array[1], CultureInfo.InvariantCulture);
				float y = float.Parse(array[2], CultureInfo.InvariantCulture);
				float x2 = float.Parse(array[3], CultureInfo.InvariantCulture);
				float y2 = float.Parse(array[4], CultureInfo.InvariantCulture);
				float x3 = float.Parse(array[5], CultureInfo.InvariantCulture);
				float y3 = float.Parse(array[6], CultureInfo.InvariantCulture);
				RectTransform rectTransform = logoImage.rectTransform;
				rectTransform.anchoredPosition = new Vector2(x, y);
				rectTransform.sizeDelta = new Vector2(x2, y2);
				rectTransform.localScale = new Vector3(x3, y3, 1f);
				string text4 = null;
				text4 = ((text3.IsNullOrEmpty() || (!text3.StartsWith("cfm_logo_S") && !text3.StartsWith("cfm_saiji_logo"))) ? GetTexturePathByName(text3) : ("Assets/Main/Loading/Season/" + text3 + ".png"));
				if (GameEntry.Resource.PrefabAssetsDownloaded(text4))
				{
					_logoImageAsset = GameEntry.Resource.LoadAsset(text4, typeof(Texture));
					if (_logoImageAsset != null)
					{
						logoImage.texture = (Texture)_logoImageAsset.asset;
						flag2 = true;
					}
				}
			}
		}
		if (!flag2)
		{
			logoImage.enabled = false;
		}
		SetFadeoutProgress(0f);
		base.gameObject.SetActive(value: true);
		_elapsed = duration + 1f;
		_hiding = false;
		_gapFrame4HideNativeSplash = Time.frameCount + 2;
		_hideNativeSplash = false;
	}

	public void Hide()
	{
		ApplicationLaunch.StepLog("Hide Splash Loading");
		if (base.gameObject.activeSelf && !_hiding)
		{
			_elapsed = 0f;
			_hiding = true;
		}
	}

	private void Update()
	{
		if (!_hideNativeSplash && Time.frameCount >= _gapFrame4HideNativeSplash)
		{
			GameEntry.Sdk.HideSplash();
			_hideNativeSplash = true;
		}
		if (_hiding)
		{
			_elapsed += Time.deltaTime;
			float num = Mathf.Clamp01(_elapsed / duration);
			float num2 = num * num * num;
			if (num2 < 1f)
			{
				SetFadeoutProgress(num2);
				return;
			}
			_elapsed = duration + 1f;
			_hiding = false;
			base.gameObject.SetActive(value: false);
		}
	}

	private void SetFadeoutProgress(float p)
	{
		_white.a = p;
		bg.color = _white;
		if (background.enabled)
		{
			background.color = _white;
		}
		if (logoImage.enabled)
		{
			logoImage.color = _white;
		}
	}

	private string GetTexturePathByName(string texName)
	{
		return "Assets/Main/SingleSprites/" + texName + ".png";
	}

	public void Release()
	{
		_loadingAsset?.Release();
		_loadingAsset = null;
		_backgroundAsset?.Release();
		_backgroundAsset = null;
		_logoImageAsset?.Release();
		_logoImageAsset = null;
	}
}
