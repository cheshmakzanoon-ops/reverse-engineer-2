using System;
using UnityEngine;

namespace MiniGame.GGGo.Client;

[Serializable]
public class BgFadeEntry
{
	[Tooltip("开始过渡的深度阈值（逻辑单位，填正数）")]
	public float depthThreshold;

	[Tooltip("过渡区间（逻辑距离单位）：在 [threshold, threshold+fadeDepthRange] 内从 startColor -> endColor")]
	public float fadeDepthRange = 30f;

	[Tooltip("过渡起始颜色")]
	public Color startColor = Color.white;

	[Tooltip("过渡结束颜色")]
	public Color endColor = Color.white;

	[Tooltip("要应用颜色过渡的目标 SpriteRenderer（若为空则该条目仅作占位）")]
	public SpriteRenderer[] targets;
}
