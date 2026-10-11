using System.Collections.Generic;
using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;
using MiniGame.Core.Client;
using UnityEngine;

namespace MiniGame.GGGo.Client;

public class MiniBox2DGame
{
	public float Scale;

	private GameUnityRuntime _gameUnityRuntime;

	private readonly List<ComponentRegion> _sortedRegions = new List<ComponentRegion>();

	public EcsWorld World { get; set; }

	public void DebugDraw()
	{
		if (World != null)
		{
			GGGoEnvClient shared = World.GetShared<GGGoEnvClient>();
			DrawColliders(shared);
			DrawTriggers(shared);
			DrawRegions(shared);
			DrawRange(shared);
		}
	}

	private FVector2 ToDrawPos(in FVector2 worldPos)
	{
		return new FVector2(worldPos.X * (FP)Scale, worldPos.Y * (FP)Scale);
	}

	private FVector2 ToDrawSize(in FVector2 worldHalfSize)
	{
		return new FVector2(worldHalfSize.X * (FP)Scale, worldHalfSize.Y * (FP)Scale);
	}

	private static void DrawRectGizmos(in FVector2 worldCenter, in FVector2 halfSize, in UnityEngine.Color color, float z = 0f)
	{
		Gizmos.color = color;
		Gizmos.DrawWireCube(new Vector3((float)worldCenter.X, (float)worldCenter.Y, 0f), new Vector3((float)(halfSize.X * (FP)2), (float)(halfSize.Y * (FP)2), z));
	}

	private static void DrawLabel(in FVector2 worldPos, string text, in UnityEngine.Color color)
	{
	}

	private void DrawColliders(GGGoEnvClient env)
	{
		EcsPool<ComponentPosition> pool = World.GetPool<ComponentPosition>();
		EcsPool<ComponentCollider> pool2 = World.GetPool<ComponentCollider>();
		foreach (int item in World.Filter<ComponentCollider>().End())
		{
			ref ComponentPosition reference = ref pool.Get(item);
			ref ComponentCollider reference2 = ref pool2.Get(item);
			DrawRectGizmos(ToDrawPos(reference.Position + reference2.Offset), ToDrawSize(in reference2.HalfSize), UnityEngine.Color.green);
		}
	}

	private void DrawTriggers(GGGoEnvClient env)
	{
		EcsPool<ComponentPosition> pool = World.GetPool<ComponentPosition>();
		EcsPool<ComponentColliderTrigger> pool2 = World.GetPool<ComponentColliderTrigger>();
		foreach (int item in World.Filter<ComponentColliderTrigger>().End())
		{
			ref ComponentPosition reference = ref pool.Get(item);
			ref ComponentColliderTrigger reference2 = ref pool2.Get(item);
			DrawRectGizmos(ToDrawPos(reference.Position + reference2.Offset), ToDrawSize(in reference2.HalfSize), UnityEngine.Color.yellow);
		}
	}

	private void DrawRange(GGGoEnvClient env)
	{
		if (_gameUnityRuntime == null)
		{
			_gameUnityRuntime = env.Scene.gameObject.GetComponentInParent<GameUnityRuntime>();
		}
		GGGoLevelConfig level = env.Level;
		FP x = level.RangeHorizon.X;
		FP y = level.RangeHorizon.Y;
		FP y2 = env.Distance;
		FP x2 = level.RangeVertical.X + y2;
		FP y3 = level.RangeVertical.Y + y2;
		FVector2 worldPos = new FVector2((x + y) / 2, (x2 + y3) / 2);
		FVector2 worldHalfSize = new FVector2((y - x) / 2, (y3 - x2) / 2);
		DrawRectGizmos(ToDrawPos(in worldPos), ToDrawSize(in worldHalfSize), UnityEngine.Color.red);
	}

	private void DrawRegions(GGGoEnvClient env)
	{
		if (env == null)
		{
			return;
		}
		EcsPool<ComponentRegion> pool = World.GetPool<ComponentRegion>();
		EcsPool<ComponentUniqueID> pool2 = World.GetPool<ComponentUniqueID>();
		EcsFilter ecsFilter = World.Filter<ComponentRegion>().End();
		if (ecsFilter.GetEntitiesCount() <= 0)
		{
			return;
		}
		GGGoLevelConfig level = env.Level;
		FP y = level.RangeHorizon.X;
		FP x = level.RangeHorizon.Y;
		FP x2 = (x - y) / 2;
		FP x3 = (y + x) / 2;
		UnityEngine.Color color = UnityEngine.Color.cyan;
		_sortedRegions.Clear();
		foreach (int item in ecsFilter)
		{
			ref ComponentRegion reference = ref pool.Get(item);
			_sortedRegions.Add(reference);
		}
		_sortedRegions.Sort((ComponentRegion a, ComponentRegion b) => b.StartPos.CompareTo(a.StartPos));
		int num = 0;
		foreach (int item2 in ecsFilter)
		{
			ref ComponentRegion reference2 = ref pool.Get(item2);
			FP x4 = reference2.StartPos;
			FP y2 = reference2.StartPos - reference2.Length;
			FP y3 = (x4 + y2) / 2;
			FP y4 = FP.Abs(x4 - y2) / 2;
			FVector2 worldPos = new FVector2(x3, y3);
			FVector2 worldHalfSize = new FVector2(x2, y4);
			DrawRectGizmos(ToDrawPos(in worldPos), ToDrawSize(in worldHalfSize), in color);
			FVector2 worldPos2 = new FVector2(x, x4);
			string text = $"Region#{num}  startY={x4}  len={reference2.Length}  speed={reference2.Speed}";
			if (pool2.Has(item2))
			{
				ref ComponentUniqueID reference3 = ref pool2.Get(item2);
				text += "\nuid:";
				if (reference3.ID > 0)
				{
					text += $"  id={reference3.ID}";
				}
				if (!string.IsNullOrEmpty(reference3.Name))
				{
					text = text + "  Name=" + reference3.Name;
				}
			}
			DrawLabel(ToDrawPos(in worldPos2), text, in color);
			num++;
		}
	}
}
