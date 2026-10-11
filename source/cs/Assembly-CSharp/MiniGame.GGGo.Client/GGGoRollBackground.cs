using UnityEngine;

namespace MiniGame.GGGo.Client;

public class GGGoRollBackground : MonoBehaviour
{
	private const float ZRange = 0.001f;

	[Header("常驻滚动背景（始终轮换，不受深度影响）")]
	public BgInfo[] Backgrounds;

	[Header("按深度分段的滚动背景组（threshold 升序）")]
	public BgDepthGroup[] DepthGroups;

	[Header("按深度按颜色过渡的静态层（threshold 升序）")]
	public BgFadeEntry[] FadeLayers;

	public float unit;

	public float predictTime = 0.1f;

	[Tooltip("距离差超过此值（逻辑单位）时直接强拉，不做插值（默认 10）")]
	public float snapThreshold = 10f;

	private float _runtime;

	private float _distStartPos;

	private float _distTargetPos;

	private int _activeGroupIndex = -1;

	private int _nextGroupIndex = -1;

	private int _blendStage;

	private int _blendCycleRef = int.MinValue;

	public void Init(float dist, float unit)
	{
		this.unit = unit;
		float distance = dist * unit;
		_activeGroupIndex = ResolveDepthGroupIndex(distance);
		_nextGroupIndex = -1;
		_blendStage = 0;
		_blendCycleRef = int.MinValue;
		if (DepthGroups != null)
		{
			for (int i = 0; i < DepthGroups.Length; i++)
			{
				SetGroupActive(i, i == _activeGroupIndex);
			}
		}
		RollBg(distance);
	}

	public void RollDist(float dist, float speed)
	{
		_runtime = 0f;
		float num = (dist + speed * predictTime) * unit;
		float y = base.transform.localPosition.y;
		if (Mathf.Abs(num - y) > snapThreshold * Mathf.Max(0.0001f, unit))
		{
			_distStartPos = num;
			_distTargetPos = num;
		}
		else
		{
			_distStartPos = y;
			_distTargetPos = num;
		}
	}

	public void Update()
	{
		_runtime += Time.deltaTime;
		float num = Mathf.LerpUnclamped(_distStartPos, _distTargetPos, _runtime / predictTime);
		Vector3 localPosition = base.transform.localPosition;
		localPosition.y = num;
		base.transform.localPosition = localPosition;
		RollBg(num);
	}

	private void RollBg(float distance)
	{
		if (Backgrounds != null)
		{
			for (int i = 0; i < Backgrounds.Length; i++)
			{
				UpdateBgPositionsByEffective(Backgrounds[i], distance);
			}
		}
		UpdateDepthGroup(distance);
		UpdateFadeLayers(distance);
	}

	private void UpdateDepthGroup(float distance)
	{
		if (DepthGroups == null || DepthGroups.Length == 0)
		{
			return;
		}
		int num = ResolveDepthGroupIndex(distance);
		if (_blendStage == 0)
		{
			if (num != _activeGroupIndex)
			{
				_nextGroupIndex = num;
				_blendStage = 1;
				_blendCycleRef = CalcCycleIndex(distance, _activeGroupIndex);
				if (_nextGroupIndex >= 0)
				{
					SetGroupActive(_nextGroupIndex, active: true);
				}
			}
			RollGroup(_activeGroupIndex, distance);
		}
		else if (_blendStage == 1)
		{
			if (num != _nextGroupIndex && num != _activeGroupIndex)
			{
				if (_nextGroupIndex >= 0)
				{
					SetGroupActive(_nextGroupIndex, active: false);
				}
				_nextGroupIndex = num;
				if (_nextGroupIndex >= 0)
				{
					SetGroupActive(_nextGroupIndex, active: true);
				}
			}
			int num2 = CalcCycleIndex(distance, _activeGroupIndex);
			if (num2 != _blendCycleRef)
			{
				_blendStage = 2;
				_blendCycleRef = num2;
			}
			RollGroup(_activeGroupIndex, distance);
		}
		else if (_blendStage == 2)
		{
			if (CalcCycleIndex(distance, _activeGroupIndex) != _blendCycleRef)
			{
				SetGroupActive(_activeGroupIndex, active: false);
				_activeGroupIndex = _nextGroupIndex;
				_nextGroupIndex = -1;
				_blendStage = 0;
				_blendCycleRef = int.MinValue;
				RollGroup(_activeGroupIndex, distance);
			}
			else
			{
				RollGroupTopOnly(_activeGroupIndex, distance);
				RollGroupBottomOnly(_nextGroupIndex, distance);
			}
		}
	}

	private void RollGroup(int groupIndex, float distance)
	{
		if (groupIndex < 0 || DepthGroups == null || groupIndex >= DepthGroups.Length)
		{
			return;
		}
		BgInfo[] backgrounds = DepthGroups[groupIndex].Backgrounds;
		if (backgrounds != null)
		{
			for (int i = 0; i < backgrounds.Length; i++)
			{
				UpdateBgPositionsByEffective(backgrounds[i], distance);
			}
		}
	}

	private void RollGroupTopOnly(int groupIndex, float distance)
	{
		if (groupIndex < 0 || DepthGroups == null || groupIndex >= DepthGroups.Length)
		{
			return;
		}
		BgInfo[] backgrounds = DepthGroups[groupIndex].Backgrounds;
		if (backgrounds == null)
		{
			return;
		}
		foreach (BgInfo bgInfo in backgrounds)
		{
			if (bgInfo != null)
			{
				float num = ((bgInfo.height <= 0f) ? 1f : bgInfo.height);
				float num2 = ((bgInfo.parallax > 0f) ? bgInfo.parallax : 1f);
				Transform transform = ((Mathf.FloorToInt(distance * num2 / num) % 2 == 0) ? bgInfo.bgTransform : bgInfo.bgTransform2);
				UpdateBgPositionsByEffective(bgInfo, distance);
				if (transform != null)
				{
					Vector3 localPosition = transform.localPosition;
					localPosition.y = 99999f;
					transform.localPosition = localPosition;
				}
			}
		}
	}

	private void RollGroupBottomOnly(int groupIndex, float distance)
	{
		if (groupIndex < 0 || DepthGroups == null || groupIndex >= DepthGroups.Length)
		{
			return;
		}
		BgInfo[] backgrounds = DepthGroups[groupIndex].Backgrounds;
		if (backgrounds == null)
		{
			return;
		}
		foreach (BgInfo bgInfo in backgrounds)
		{
			if (bgInfo != null)
			{
				float num = ((bgInfo.height <= 0f) ? 1f : bgInfo.height);
				float num2 = ((bgInfo.parallax > 0f) ? bgInfo.parallax : 1f);
				Transform transform = ((Mathf.FloorToInt(distance * num2 / num) % 2 == 0) ? bgInfo.bgTransform2 : bgInfo.bgTransform);
				UpdateBgPositionsByEffective(bgInfo, distance);
				if (transform != null)
				{
					Vector3 localPosition = transform.localPosition;
					localPosition.y = 99999f;
					transform.localPosition = localPosition;
				}
			}
		}
	}

	private int CalcCycleIndex(float distance, int groupIndex)
	{
		if (groupIndex < 0 || DepthGroups == null || groupIndex >= DepthGroups.Length)
		{
			return 0;
		}
		BgInfo[] backgrounds = DepthGroups[groupIndex].Backgrounds;
		if (backgrounds == null || backgrounds.Length == 0)
		{
			return 0;
		}
		BgInfo bgInfo = backgrounds[0];
		float num = ((bgInfo.height <= 0f) ? 1f : bgInfo.height);
		float num2 = ((bgInfo.parallax > 0f) ? bgInfo.parallax : 1f);
		return Mathf.FloorToInt(distance * num2 / num);
	}

	private int ResolveDepthGroupIndex(float distance)
	{
		if (DepthGroups == null || DepthGroups.Length == 0)
		{
			return -1;
		}
		float num = Mathf.Abs(distance / Mathf.Max(0.0001f, unit));
		int result = -1;
		for (int i = 0; i < DepthGroups.Length; i++)
		{
			if (num >= DepthGroups[i].depthThreshold && DepthGroups[i].Backgrounds != null && DepthGroups[i].Backgrounds.Length != 0)
			{
				result = i;
			}
		}
		return result;
	}

	private void SetGroupActive(int index, bool active)
	{
		if (DepthGroups == null || index < 0 || index >= DepthGroups.Length)
		{
			return;
		}
		BgInfo[] backgrounds = DepthGroups[index].Backgrounds;
		if (backgrounds == null)
		{
			return;
		}
		foreach (BgInfo bgInfo in backgrounds)
		{
			if (bgInfo != null)
			{
				bgInfo.bgTransform?.gameObject.SetActive(active);
				bgInfo.bgTransform2?.gameObject.SetActive(active);
			}
		}
	}

	private void UpdateFadeLayers(float distance)
	{
		if (FadeLayers == null || FadeLayers.Length == 0)
		{
			return;
		}
		float num = Mathf.Abs(distance / Mathf.Max(0.0001f, unit));
		int num2 = FadeLayers.Length;
		for (int i = 0; i < num2; i++)
		{
			BgFadeEntry bgFadeEntry = FadeLayers[i];
			if (bgFadeEntry.targets == null || bgFadeEntry.targets.Length == 0)
			{
				continue;
			}
			for (int j = 0; j < bgFadeEntry.targets.Length; j++)
			{
				SpriteRenderer spriteRenderer = bgFadeEntry.targets[j];
				if (spriteRenderer == null)
				{
					continue;
				}
				bool flag = false;
				for (int k = 0; k < i; k++)
				{
					if (flag)
					{
						break;
					}
					BgFadeEntry bgFadeEntry2 = FadeLayers[k];
					if (bgFadeEntry2.targets == null)
					{
						continue;
					}
					for (int l = 0; l < bgFadeEntry2.targets.Length; l++)
					{
						if ((object)bgFadeEntry2.targets[l] == spriteRenderer)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					continue;
				}
				Color color = bgFadeEntry.startColor;
				for (int m = i; m < num2; m++)
				{
					BgFadeEntry bgFadeEntry3 = FadeLayers[m];
					bool flag2 = false;
					if (bgFadeEntry3.targets != null)
					{
						for (int n = 0; n < bgFadeEntry3.targets.Length; n++)
						{
							if ((object)bgFadeEntry3.targets[n] == spriteRenderer)
							{
								flag2 = true;
								break;
							}
						}
					}
					if (flag2)
					{
						float num3 = Mathf.Max(0.0001f, bgFadeEntry3.fadeDepthRange);
						float t = Mathf.Clamp01((num - bgFadeEntry3.depthThreshold) / num3);
						color = Color.Lerp(color, bgFadeEntry3.endColor, t);
					}
				}
				spriteRenderer.color = color;
			}
		}
	}

	public void RollBgManual(float currentY)
	{
		_distStartPos = currentY;
		_distTargetPos = currentY;
		_runtime = 0f;
		RollBg(currentY);
	}

	public void UpdateBgPositions(BgInfo bg, float distance)
	{
		float height = bg.height;
		if (!(height <= 0f))
		{
			int num = Mathf.FloorToInt(distance / height);
			float num2 = (float)num * height;
			Transform bgTransform = bg.bgTransform;
			Transform bgTransform2 = bg.bgTransform2;
			if (num % 2 == 0)
			{
				float num3 = 0f - num2;
				float num4 = num3 - height;
				bgTransform.localPosition = new Vector3(bgTransform.localPosition.x, 0f - num3 + bg.offset, -0.001f);
				bgTransform2.localPosition = new Vector3(bgTransform2.localPosition.x, 0f - num4 + bg.offset, -0.002f);
			}
			else
			{
				float num5 = 0f - num2;
				float num6 = num5 - height;
				bgTransform2.localPosition = new Vector3(bgTransform2.localPosition.x, 0f - num5 + bg.offset, -0.001f);
				bgTransform.localPosition = new Vector3(bgTransform.localPosition.x, 0f - num6 + bg.offset, -0.002f);
			}
		}
	}

	public void UpdateBgPositionsByEffective(BgInfo bg, float distance)
	{
		float height = bg.height;
		if (height <= 0f)
		{
			return;
		}
		if (bg.parallax <= 0f || Mathf.Approximately(bg.parallax, 1f))
		{
			UpdateBgPositions(bg, distance);
			return;
		}
		float num = distance * bg.parallax;
		float num2 = distance - num;
		int num3 = Mathf.FloorToInt(num / height);
		float num4 = (float)num3 * height;
		Transform bgTransform = bg.bgTransform;
		Transform bgTransform2 = bg.bgTransform2;
		if (num3 % 2 == 0)
		{
			float num5 = 0f - num4;
			float num6 = num5 - height;
			bgTransform.localPosition = new Vector3(bgTransform.localPosition.x, 0f - num5 + num2 + bg.offset, -0.001f);
			bgTransform2.localPosition = new Vector3(bgTransform2.localPosition.x, 0f - num6 + num2 + bg.offset, 0f);
		}
		else
		{
			float num7 = 0f - num4;
			float num8 = num7 - height;
			bgTransform2.localPosition = new Vector3(bgTransform2.localPosition.x, 0f - num7 + num2 + bg.offset, -0.001f);
			bgTransform.localPosition = new Vector3(bgTransform.localPosition.x, 0f - num8 + num2 + bg.offset, 0f);
		}
	}
}
