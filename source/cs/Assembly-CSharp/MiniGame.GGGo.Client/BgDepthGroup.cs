using System;
using UnityEngine;

namespace MiniGame.GGGo.Client;

[Serializable]
public class BgDepthGroup
{
	[Tooltip("触发该组的深度阈值（逻辑单位，填正数，abs(Distance) >= threshold 时启用)")]
	public float depthThreshold;

	public BgInfo[] Backgrounds;
}
