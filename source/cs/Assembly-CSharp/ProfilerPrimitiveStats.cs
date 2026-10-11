using System.Collections.Generic;
using System.Text;
using GameFramework;
using GameKit.Base;
using UnityEngine;

public class ProfilerPrimitiveStats : SingletonBehaviour<ProfilerPrimitiveStats>
{
	public struct PerObjectStats
	{
		public GameObject GameObject;

		public string Name;

		public int TriangleCount;

		public int MaterialCount;

		public string ObjectType;
	}

	public class ExtremeFrameSnapshot
	{
		public int MaxRenderFrameIndex;

		public int MaxRenderCount;

		public int MaxTriangleFrameIndex;

		public int MaxTriangleCount;

		public List<PerObjectStats> MaxRenderObjects;

		public List<PerObjectStats> MaxTriangleObjects;

		public ExtremeFrameSnapshot()
		{
			MaxRenderObjects = new List<PerObjectStats>();
			MaxTriangleObjects = new List<PerObjectStats>();
		}

		public void Clear()
		{
			MaxRenderFrameIndex = 0;
			MaxRenderCount = 0;
			MaxRenderObjects.Clear();
			MaxTriangleFrameIndex = 0;
			MaxTriangleCount = 0;
			MaxTriangleObjects.Clear();
		}

		public bool IsValid()
		{
			if (MaxRenderCount <= 0)
			{
				return MaxTriangleCount > 0;
			}
			return true;
		}
	}

	public struct PrimitiveStats
	{
		public int StaticMeshCount;

		public int StaticMeshTriangles;

		public int SkinnedMeshCount;

		public int SkinnedMeshTriangles;

		public int ParticleTriangles;

		public int ParticleActiveParCount;

		public int ParticleSystemCount;

		public int ParticleMeshTriangles;

		public int ParticleTrailTriangles;

		public int ParticleNormalTriangles;

		public int ParticleNoiseCount;

		public int ParticleMeshCount;
	}

	public struct PrimitiveStatsStatics
	{
		public PrimitiveStats Min;

		public PrimitiveStats Max;

		public PrimitiveStats Avg;

		public PrimitiveStats Current;
	}

	private static bool kEnableDetailedStats;

	private ExtremeFrameSnapshot _currentPeriodMax = new ExtremeFrameSnapshot();

	private ExtremeFrameSnapshot _historyPeriodMax = new ExtremeFrameSnapshot();

	private PrimitiveStats _stats;

	private StringBuilder _dumpBuilder = new StringBuilder();

	private List<PrimitiveStats> _frameStatsBuffer = new List<PrimitiveStats>();

	private float _statsAccumulateTimer;

	private const float StatsUpdateInterval = 1f;

	private PrimitiveStatsStatics _statics;

	public GameObject Target { get; set; }

	public static void EnableDetailedStats()
	{
		kEnableDetailedStats = true;
	}

	public static void DisableDetailedStats()
	{
		kEnableDetailedStats = false;
	}

	public static bool IsDetailedStatsEnabled()
	{
		return kEnableDetailedStats;
	}

	public ExtremeFrameSnapshot GetCurrentPeriodSnapshot()
	{
		return _currentPeriodMax;
	}

	public ExtremeFrameSnapshot GetHistoryPeriodSnapshot()
	{
		return _historyPeriodMax;
	}

	public void RefreshHistorySnapshot()
	{
		if (kEnableDetailedStats)
		{
			_historyPeriodMax.Clear();
			_historyPeriodMax.MaxRenderFrameIndex = _currentPeriodMax.MaxRenderFrameIndex;
			_historyPeriodMax.MaxRenderCount = _currentPeriodMax.MaxRenderCount;
			_historyPeriodMax.MaxRenderObjects.AddRange(_currentPeriodMax.MaxRenderObjects);
			_historyPeriodMax.MaxTriangleFrameIndex = _currentPeriodMax.MaxTriangleFrameIndex;
			_historyPeriodMax.MaxTriangleCount = _currentPeriodMax.MaxTriangleCount;
			_historyPeriodMax.MaxTriangleObjects.AddRange(_currentPeriodMax.MaxTriangleObjects);
			_currentPeriodMax.Clear();
		}
	}

	public void ClearFrameStatsBuffer()
	{
		_frameStatsBuffer.Clear();
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
		_frameStatsBuffer.Clear();
	}

	private void Update()
	{
		_stats = CollectPrimitiveStats(Target);
		_frameStatsBuffer.Add(_stats);
		UpdateCurrentDetails();
		_statsAccumulateTimer += Time.unscaledDeltaTime;
		if (_statsAccumulateTimer >= 1f)
		{
			_statsAccumulateTimer = 0f;
			UpdateStatics();
			RefreshHistorySnapshot();
			_frameStatsBuffer.Clear();
		}
	}

	private void UpdateCurrentDetails()
	{
		int num = _stats.StaticMeshCount + _stats.SkinnedMeshCount + _stats.ParticleSystemCount;
		int num2 = _stats.StaticMeshTriangles + _stats.SkinnedMeshTriangles + _stats.ParticleTriangles;
		bool flag = num > _currentPeriodMax.MaxRenderCount;
		bool flag2 = num2 > _currentPeriodMax.MaxTriangleCount;
		if (flag || flag2)
		{
			Dictionary<GameObject, PerObjectStats> dictionary = new Dictionary<GameObject, PerObjectStats>();
			CollectDetailedStats(Target, dictionary);
			List<PerObjectStats> list = new List<PerObjectStats>(dictionary.Values);
			list.Sort((PerObjectStats a, PerObjectStats b) => b.TriangleCount.CompareTo(a.TriangleCount));
			if (flag)
			{
				_currentPeriodMax.MaxRenderFrameIndex = Time.frameCount;
				_currentPeriodMax.MaxRenderCount = num;
				_currentPeriodMax.MaxRenderObjects = list;
			}
			if (flag2)
			{
				_currentPeriodMax.MaxTriangleFrameIndex = Time.frameCount;
				_currentPeriodMax.MaxTriangleCount = num2;
				_currentPeriodMax.MaxTriangleObjects = list;
			}
		}
	}

	private void UpdateStatics()
	{
		int count = _frameStatsBuffer.Count;
		PrimitiveStats min = _frameStatsBuffer[0];
		PrimitiveStats max = _frameStatsBuffer[0];
		PrimitiveStats primitiveStats = default(PrimitiveStats);
		for (int i = 0; i < count; i++)
		{
			PrimitiveStats primitiveStats2 = _frameStatsBuffer[i];
			min.StaticMeshCount = Mathf.Min(min.StaticMeshCount, primitiveStats2.StaticMeshCount);
			max.StaticMeshCount = Mathf.Max(max.StaticMeshCount, primitiveStats2.StaticMeshCount);
			primitiveStats.StaticMeshCount += primitiveStats2.StaticMeshCount;
			min.StaticMeshTriangles = Mathf.Min(min.StaticMeshTriangles, primitiveStats2.StaticMeshTriangles);
			max.StaticMeshTriangles = Mathf.Max(max.StaticMeshTriangles, primitiveStats2.StaticMeshTriangles);
			primitiveStats.StaticMeshTriangles += primitiveStats2.StaticMeshTriangles;
			min.SkinnedMeshCount = Mathf.Min(min.SkinnedMeshCount, primitiveStats2.SkinnedMeshCount);
			max.SkinnedMeshCount = Mathf.Max(max.SkinnedMeshCount, primitiveStats2.SkinnedMeshCount);
			primitiveStats.SkinnedMeshCount += primitiveStats2.SkinnedMeshCount;
			min.SkinnedMeshTriangles = Mathf.Min(min.SkinnedMeshTriangles, primitiveStats2.SkinnedMeshTriangles);
			max.SkinnedMeshTriangles = Mathf.Max(max.SkinnedMeshTriangles, primitiveStats2.SkinnedMeshTriangles);
			primitiveStats.SkinnedMeshTriangles += primitiveStats2.SkinnedMeshTriangles;
			min.ParticleTriangles = Mathf.Min(min.ParticleTriangles, primitiveStats2.ParticleTriangles);
			max.ParticleTriangles = Mathf.Max(max.ParticleTriangles, primitiveStats2.ParticleTriangles);
			primitiveStats.ParticleTriangles += primitiveStats2.ParticleTriangles;
			min.ParticleActiveParCount = Mathf.Min(min.ParticleActiveParCount, primitiveStats2.ParticleActiveParCount);
			max.ParticleActiveParCount = Mathf.Max(max.ParticleActiveParCount, primitiveStats2.ParticleActiveParCount);
			primitiveStats.ParticleActiveParCount += primitiveStats2.ParticleActiveParCount;
			min.ParticleSystemCount = Mathf.Min(min.ParticleSystemCount, primitiveStats2.ParticleSystemCount);
			max.ParticleSystemCount = Mathf.Max(max.ParticleSystemCount, primitiveStats2.ParticleSystemCount);
			primitiveStats.ParticleSystemCount += primitiveStats2.ParticleSystemCount;
			min.ParticleMeshTriangles = Mathf.Min(min.ParticleMeshTriangles, primitiveStats2.ParticleMeshTriangles);
			max.ParticleMeshTriangles = Mathf.Max(max.ParticleMeshTriangles, primitiveStats2.ParticleMeshTriangles);
			primitiveStats.ParticleMeshTriangles += primitiveStats2.ParticleMeshTriangles;
			min.ParticleTrailTriangles = Mathf.Min(min.ParticleTrailTriangles, primitiveStats2.ParticleTrailTriangles);
			max.ParticleTrailTriangles = Mathf.Max(max.ParticleTrailTriangles, primitiveStats2.ParticleTrailTriangles);
			primitiveStats.ParticleTrailTriangles += primitiveStats2.ParticleTrailTriangles;
			min.ParticleNormalTriangles = Mathf.Min(min.ParticleNormalTriangles, primitiveStats2.ParticleNormalTriangles);
			max.ParticleNormalTriangles = Mathf.Max(max.ParticleNormalTriangles, primitiveStats2.ParticleNormalTriangles);
			primitiveStats.ParticleNormalTriangles += primitiveStats2.ParticleNormalTriangles;
			min.ParticleNoiseCount = Mathf.Min(min.ParticleNoiseCount, primitiveStats2.ParticleNoiseCount);
			max.ParticleNoiseCount = Mathf.Max(max.ParticleNoiseCount, primitiveStats2.ParticleNoiseCount);
			primitiveStats.ParticleNoiseCount += primitiveStats2.ParticleNoiseCount;
			min.ParticleMeshCount = Mathf.Min(min.ParticleMeshCount, primitiveStats2.ParticleMeshCount);
			max.ParticleMeshCount = Mathf.Max(max.ParticleMeshCount, primitiveStats2.ParticleMeshCount);
			primitiveStats.ParticleMeshCount += primitiveStats2.ParticleMeshCount;
		}
		PrimitiveStats avg = new PrimitiveStats
		{
			StaticMeshCount = primitiveStats.StaticMeshCount / count,
			StaticMeshTriangles = primitiveStats.StaticMeshTriangles / count,
			SkinnedMeshCount = primitiveStats.SkinnedMeshCount / count,
			SkinnedMeshTriangles = primitiveStats.SkinnedMeshTriangles / count,
			ParticleTriangles = primitiveStats.ParticleTriangles / count,
			ParticleActiveParCount = primitiveStats.ParticleActiveParCount / count,
			ParticleSystemCount = primitiveStats.ParticleSystemCount / count,
			ParticleMeshTriangles = primitiveStats.ParticleMeshTriangles / count,
			ParticleTrailTriangles = primitiveStats.ParticleTrailTriangles / count,
			ParticleNormalTriangles = primitiveStats.ParticleNormalTriangles / count,
			ParticleNoiseCount = primitiveStats.ParticleNoiseCount / count,
			ParticleMeshCount = primitiveStats.ParticleMeshCount / count
		};
		_statics = new PrimitiveStatsStatics
		{
			Min = min,
			Max = max,
			Avg = avg,
			Current = _frameStatsBuffer[_frameStatsBuffer.Count - 1]
		};
	}

	public static int GetMeshTriangleCount(Mesh mesh, Material[] materials, int layer)
	{
		int num = 0;
		if (mesh != null)
		{
			int num2 = 1;
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				num += (int)mesh.GetIndexCount(i) * num2 / 3;
			}
		}
		return num;
	}

	public PrimitiveStats CollectPrimitiveStats(GameObject target)
	{
		PrimitiveStats stats = default(PrimitiveStats);
		Renderer[] renderers = ((target == null) ? Object.FindObjectsOfType<Renderer>() : target.GetComponentsInChildren<Renderer>());
		CollectStaticMeshStats(renderers, ref stats);
		CollectSkinnedMeshStats(renderers, ref stats);
		CollectParticleSystemStats((target == null) ? Object.FindObjectsOfType<ParticleSystem>() : target.GetComponentsInChildren<ParticleSystem>(), ref stats);
		return stats;
	}

	private static void CollectStaticMeshStats(Renderer[] renderers, ref PrimitiveStats stats)
	{
		foreach (Renderer renderer in renderers)
		{
			if (IsVisibleInMainCamera(renderer) && renderer.TryGetComponent<MeshFilter>(out var component) && !(component.sharedMesh == null))
			{
				Mesh sharedMesh = component.sharedMesh;
				Material[] sharedMaterials = renderer.sharedMaterials;
				int meshTriangleCount = GetMeshTriangleCount(sharedMesh, sharedMaterials, renderer.gameObject.layer);
				stats.StaticMeshTriangles += meshTriangleCount;
				stats.StaticMeshCount += sharedMesh.subMeshCount;
			}
		}
	}

	private static void CollectSkinnedMeshStats(Renderer[] renderers, ref PrimitiveStats stats)
	{
		foreach (Renderer renderer in renderers)
		{
			if (IsVisibleInMainCamera(renderer) && renderer.TryGetComponent<SkinnedMeshRenderer>(out var component) && !(component.sharedMesh == null))
			{
				Mesh sharedMesh = component.sharedMesh;
				Material[] sharedMaterials = component.sharedMaterials;
				int meshTriangleCount = GetMeshTriangleCount(sharedMesh, sharedMaterials, renderer.gameObject.layer);
				stats.SkinnedMeshTriangles += meshTriangleCount;
				stats.SkinnedMeshCount += sharedMesh.subMeshCount;
			}
		}
	}

	private static void CollectParticleSystemStats(ParticleSystem[] particleSystems, ref PrimitiveStats stats)
	{
		foreach (ParticleSystem particleSystem in particleSystems)
		{
			if (particleSystem.gameObject.activeInHierarchy && particleSystem.TryGetComponent<ParticleSystemRenderer>(out var component) && IsVisibleInMainCamera(component))
			{
				int particleCount = particleSystem.particleCount;
				stats.ParticleSystemCount++;
				stats.ParticleActiveParCount += particleCount;
				if (particleSystem.noise.enabled)
				{
					stats.ParticleNoiseCount++;
				}
				int num = 0;
				ParticleSystem.TrailModule trails = particleSystem.trails;
				if (trails.enabled)
				{
					float constant = trails.lifetime.constant;
					int num2 = Mathf.FloorToInt(particleSystem.main.startSpeed.constant * constant * 0.5f);
					num = particleCount * num2 * 2;
					stats.ParticleTriangles += num;
					stats.ParticleTrailTriangles += num;
				}
				else if (component.renderMode == ParticleSystemRenderMode.Mesh && component.mesh != null)
				{
					Material[] sharedMaterials = component.sharedMaterials;
					num = GetMeshTriangleCount(component.mesh, sharedMaterials, component.gameObject.layer) * particleCount;
					stats.ParticleTriangles += num;
					stats.ParticleMeshTriangles += num;
					stats.ParticleMeshCount++;
				}
				else
				{
					num = 2 * particleCount;
					stats.ParticleTriangles += num;
					stats.ParticleNormalTriangles += num;
				}
			}
		}
	}

	private void CollectDetailedStats(GameObject target, Dictionary<GameObject, PerObjectStats> outObjectStats)
	{
		Renderer[] array = ((target == null) ? Object.FindObjectsOfType<Renderer>() : target.GetComponentsInChildren<Renderer>());
		Renderer[] array2 = array;
		foreach (Renderer renderer in array2)
		{
			if (IsVisibleInMainCamera(renderer) && renderer.TryGetComponent<MeshFilter>(out var component) && !(component.sharedMesh == null))
			{
				Mesh sharedMesh = component.sharedMesh;
				Material[] sharedMaterials = renderer.sharedMaterials;
				int meshTriangleCount = GetMeshTriangleCount(sharedMesh, sharedMaterials, renderer.gameObject.layer);
				if (meshTriangleCount > 0)
				{
					outObjectStats[renderer.gameObject] = new PerObjectStats
					{
						GameObject = renderer.gameObject,
						Name = renderer.gameObject.name,
						TriangleCount = meshTriangleCount,
						MaterialCount = ((sharedMaterials != null) ? sharedMaterials.Length : 0),
						ObjectType = "StaticMesh"
					};
				}
			}
		}
		array2 = array;
		foreach (Renderer renderer2 in array2)
		{
			if (IsVisibleInMainCamera(renderer2) && renderer2.TryGetComponent<SkinnedMeshRenderer>(out var component2) && !(component2.sharedMesh == null))
			{
				Mesh sharedMesh2 = component2.sharedMesh;
				Material[] sharedMaterials2 = component2.sharedMaterials;
				int meshTriangleCount2 = GetMeshTriangleCount(sharedMesh2, sharedMaterials2, renderer2.gameObject.layer);
				if (meshTriangleCount2 > 0)
				{
					outObjectStats[renderer2.gameObject] = new PerObjectStats
					{
						GameObject = renderer2.gameObject,
						Name = renderer2.gameObject.name,
						TriangleCount = meshTriangleCount2,
						MaterialCount = ((sharedMaterials2 != null) ? sharedMaterials2.Length : 0),
						ObjectType = "SkinnedMesh"
					};
				}
			}
		}
		ParticleSystem[] array3 = ((target == null) ? Object.FindObjectsOfType<ParticleSystem>() : target.GetComponentsInChildren<ParticleSystem>());
		foreach (ParticleSystem particleSystem in array3)
		{
			if (particleSystem.gameObject.activeInHierarchy && particleSystem.TryGetComponent<ParticleSystemRenderer>(out var component3) && IsVisibleInMainCamera(component3))
			{
				int particleCount = particleSystem.particleCount;
				int num = 0;
				ParticleSystem.TrailModule trails = particleSystem.trails;
				if (trails.enabled)
				{
					float constant = trails.lifetime.constant;
					int num2 = Mathf.FloorToInt(particleSystem.main.startSpeed.constant * constant * 0.5f);
					num = particleCount * num2 * 2;
				}
				else if (component3.renderMode == ParticleSystemRenderMode.Mesh && component3.mesh != null)
				{
					Material[] sharedMaterials3 = component3.sharedMaterials;
					num = GetMeshTriangleCount(component3.mesh, sharedMaterials3, component3.gameObject.layer) * particleCount;
				}
				else
				{
					num = 2 * particleCount;
				}
				if (num > 0)
				{
					GameObject key = particleSystem.gameObject;
					PerObjectStats value = new PerObjectStats
					{
						GameObject = particleSystem.gameObject,
						Name = particleSystem.gameObject.name,
						TriangleCount = num
					};
					Material[] sharedMaterials4 = component3.sharedMaterials;
					value.MaterialCount = ((sharedMaterials4 != null) ? sharedMaterials4.Length : 0);
					value.ObjectType = "ParticleSystem";
					outObjectStats[key] = value;
				}
			}
		}
	}

	private static bool IsVisibleInMainCamera(Renderer renderer)
	{
		if (renderer == null || !renderer.isVisible || !renderer.gameObject.activeInHierarchy || renderer.sharedMaterials == null || !renderer.enabled)
		{
			return false;
		}
		Camera main = Camera.main;
		if (main != null && !GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(main), renderer.bounds))
		{
			return false;
		}
		return true;
	}

	public PrimitiveStats GetStats()
	{
		return _stats;
	}

	public static PrimitiveStats CollectPrimitiveInfo(GameObject target)
	{
		PrimitiveStats stats = default(PrimitiveStats);
		Renderer[] renderers = ((target == null) ? Object.FindObjectsOfType<Renderer>() : target.GetComponentsInChildren<Renderer>());
		CollectStaticMeshStats(renderers, ref stats);
		CollectSkinnedMeshStats(renderers, ref stats);
		CollectParticleSystemStats((target == null) ? Object.FindObjectsOfType<ParticleSystem>() : target.GetComponentsInChildren<ParticleSystem>(), ref stats);
		return stats;
	}

	public PrimitiveStatsStatics GetStatsStatics()
	{
		return _statics;
	}

	public string DumpToString()
	{
		PrimitiveStatsStatics statsStatics = GetStatsStatics();
		_dumpBuilder.Clear();
		int current = statsStatics.Current.StaticMeshTriangles + statsStatics.Current.SkinnedMeshTriangles + statsStatics.Current.ParticleTriangles;
		int min = statsStatics.Min.StaticMeshTriangles + statsStatics.Min.SkinnedMeshTriangles + statsStatics.Min.ParticleTriangles;
		int max = statsStatics.Max.StaticMeshTriangles + statsStatics.Max.SkinnedMeshTriangles + statsStatics.Max.ParticleTriangles;
		int avg = statsStatics.Avg.StaticMeshTriangles + statsStatics.Avg.SkinnedMeshTriangles + statsStatics.Avg.ParticleTriangles;
		_dumpBuilder.AppendLine("Primitive Statistics (Current / Min / Max / Avg):");
		AppendStatLine("   Total Triangles", current, min, max, avg);
		_dumpBuilder.AppendLine();
		AppendStatLine("Static Meshes", statsStatics.Current.StaticMeshCount, statsStatics.Min.StaticMeshCount, statsStatics.Max.StaticMeshCount, statsStatics.Avg.StaticMeshCount);
		AppendStatLine("   Static Triangles", statsStatics.Current.StaticMeshTriangles, statsStatics.Min.StaticMeshTriangles, statsStatics.Max.StaticMeshTriangles, statsStatics.Avg.StaticMeshTriangles);
		AppendStatLine("Skinned Meshes", statsStatics.Current.SkinnedMeshCount, statsStatics.Min.SkinnedMeshCount, statsStatics.Max.SkinnedMeshCount, statsStatics.Avg.SkinnedMeshCount);
		AppendStatLine("   Skinned Triangles", statsStatics.Current.SkinnedMeshTriangles, statsStatics.Min.SkinnedMeshTriangles, statsStatics.Max.SkinnedMeshTriangles, statsStatics.Avg.SkinnedMeshTriangles);
		_dumpBuilder.AppendLine("Particles:");
		AppendStatLine("   Systems Count", statsStatics.Current.ParticleSystemCount, statsStatics.Min.ParticleSystemCount, statsStatics.Max.ParticleSystemCount, statsStatics.Avg.ParticleSystemCount);
		AppendStatLine("   Active Par Count", statsStatics.Current.ParticleActiveParCount, statsStatics.Min.ParticleActiveParCount, statsStatics.Max.ParticleActiveParCount, statsStatics.Avg.ParticleActiveParCount);
		AppendStatLine("   Noise Count", statsStatics.Current.ParticleNoiseCount, statsStatics.Min.ParticleNoiseCount, statsStatics.Max.ParticleNoiseCount, statsStatics.Avg.ParticleNoiseCount);
		AppendStatLine("   Mesh Count", statsStatics.Current.ParticleMeshCount, statsStatics.Min.ParticleMeshCount, statsStatics.Max.ParticleMeshCount, statsStatics.Avg.ParticleMeshCount);
		AppendStatLine("   Triangles Count", statsStatics.Current.ParticleTriangles, statsStatics.Min.ParticleTriangles, statsStatics.Max.ParticleTriangles, statsStatics.Avg.ParticleTriangles);
		AppendStatLine("       Normal", statsStatics.Current.ParticleNormalTriangles, statsStatics.Min.ParticleNormalTriangles, statsStatics.Max.ParticleNormalTriangles, statsStatics.Avg.ParticleNormalTriangles);
		AppendStatLine("       Mesh", statsStatics.Current.ParticleMeshTriangles, statsStatics.Min.ParticleMeshTriangles, statsStatics.Max.ParticleMeshTriangles, statsStatics.Avg.ParticleMeshTriangles);
		AppendStatLine("       Trail", statsStatics.Current.ParticleTrailTriangles, statsStatics.Min.ParticleTrailTriangles, statsStatics.Max.ParticleTrailTriangles, statsStatics.Avg.ParticleTrailTriangles);
		return _dumpBuilder.ToString();
		void AppendStatLine(string label, int num, int num2, int num3, int num4)
		{
			_dumpBuilder.AppendFormat("{0}: {1} (min: {2}, max: {3}, avg: {4})\n", label, num, num2, num3, num4);
		}
	}

	public void DumpToConsole()
	{
		Log.Info(DumpToString());
	}
}
