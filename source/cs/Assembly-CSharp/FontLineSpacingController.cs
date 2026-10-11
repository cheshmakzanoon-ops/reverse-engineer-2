using System;
using System.Collections.Generic;
using GameFramework.Localization;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class FontLineSpacingController : MonoBehaviour
{
	[Serializable]
	public class LanguageLineSpacing
	{
		public Language language;

		public float lineSpacing;
	}

	[Header("多语言行间距配置")]
	public List<LanguageLineSpacing> configs = new List<LanguageLineSpacing>();

	private TextMeshProUGUI _tmp;

	private float _lastAppliedValue = float.MinValue;

	private static readonly Dictionary<Language, float> DefaultLineSpacingMap = new Dictionary<Language, float>
	{
		{
			Language.English,
			-20f
		},
		{
			Language.Korean,
			20f
		}
	};

	private static readonly Language[] DefaultLanguages = new Language[2]
	{
		Language.English,
		Language.Korean
	};

	private void Awake()
	{
		_tmp = GetComponent<TextMeshProUGUI>();
		ApplyLineSpacing();
	}

	private void ApplyLineSpacing()
	{
		if (!(_tmp == null))
		{
			Language language = GameEntry.Localization.Language;
			if (TryGetLineSpacing(language, out var spacing) && !Mathf.Approximately(_lastAppliedValue, spacing))
			{
				_tmp.lineSpacing = spacing;
				_tmp.SetAllDirty();
				_lastAppliedValue = spacing;
			}
		}
	}

	private bool TryGetLineSpacing(Language language, out float spacing)
	{
		for (int i = 0; i < configs.Count; i++)
		{
			if (configs[i].language == language)
			{
				spacing = configs[i].lineSpacing;
				return true;
			}
		}
		spacing = 0f;
		return false;
	}
}
