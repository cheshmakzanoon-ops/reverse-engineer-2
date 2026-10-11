using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VEngine;
using WorldDecorationRenderer;

public class WorldZoneRTRenderer : ScriptableRendererFeature
{
	private class WorldZoneRTRendererPass : ScriptableRenderPass
	{
		private const string ProfilerTag = "TerrainQuadLayerRenderPass";

		private readonly ProfilingSampler _profilingSampler = new ProfilingSampler("TerrainQuadLayerRenderPass");

		private int _width;

		private int _height;

		private int _downScaledWidth;

		private int _downScaledHeight;

		private Vector3 _quadScale;

		private float _terrainQuadScale;

		public RenderTexture mZoneRT;

		private WorldZoneRTRenderer mZoneRTRenderer;

		public void Setup(WorldZoneRTRenderer zoneRTRenderer, RenderTargetIdentifier renderTargetIdentifier, int width, int height)
		{
			_width = width;
			_height = height;
			float num = 0.125f;
			_downScaledWidth = (int)((float)_width * num);
			_downScaledHeight = (int)((float)_height * num);
			mZoneRTRenderer = zoneRTRenderer;
			_quadScale = new Vector3(zoneRTRenderer.mQuadScale, 1f, zoneRTRenderer.mQuadScale);
			_terrainQuadScale = zoneRTRenderer.mTerrainQuadSize;
			if (mZoneRT == null)
			{
				Release();
				RenderTextureDescriptor desc = new RenderTextureDescriptor(_downScaledWidth, _downScaledHeight, RenderTextureFormat.ARGB32);
				desc.colorFormat = RenderTextureFormat.ARGB32;
				desc.depthBufferBits = 0;
				desc.msaaSamples = 1;
				desc.sRGB = false;
				desc.useMipMap = false;
				desc.autoGenerateMips = false;
				mZoneRT = RenderTexture.GetTemporary(desc);
				mZoneRT.name = "ZoneRT";
			}
			if (NeedDrawZoneRT())
			{
				ConfigureTarget(mZoneRT);
				ConfigureClear(ClearFlag.Color, Color.black);
			}
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (mZoneRTRenderer.mDrawMeshInstGraphicDic == null || mZoneRTRenderer.mDrawMeshInstGraphicDic.Count == 0 || mZoneRTRenderer.ZoneMaterial_S6 == null || !SystemInfo.supportsInstancing || !NeedDrawZoneRT())
			{
				return;
			}
			CommandBuffer commandBuffer = CommandBufferPool.Get("Zone RT Render Pass");
			try
			{
				if (NeedCalRenderZoneData())
				{
					if (mZoneRTRenderer.mDrawMeshInstancedBufferData != null)
					{
						mZoneRTRenderer.mDrawMeshInstancedBufferData.BeginFrame();
					}
					CalRenderZoneData();
				}
				DrawZoneRT(commandBuffer);
				commandBuffer.SetGlobalTexture(mZoneRTRenderer._rtZoneNameID, mZoneRT);
				context.ExecuteCommandBuffer(commandBuffer);
			}
			finally
			{
				commandBuffer.Clear();
				CommandBufferPool.Release(commandBuffer);
			}
		}

		public void CalRenderZoneData()
		{
			if (mZoneRTRenderer.mZoneGraphicDataDic == null || mZoneRTRenderer.mZoneGraphicDataDic.Count == 0 || SceneManager.World == null)
			{
				return;
			}
			List<WorldZone> clippedZones = SceneManager.World.GetClippedZones();
			if (clippedZones == null || clippedZones.Count == 0)
			{
				return;
			}
			bool flag = false;
			foreach (WorldZone item in clippedZones)
			{
				Vector2 vector = new Vector2(item.data.X * 2 + 2, item.data.Y * 2 + 2);
				int zoneAtlasIndex_S = item.GetZoneAtlasIndex_S6();
				if (zoneAtlasIndex_S < 0 || zoneAtlasIndex_S >= mZoneRTRenderer.mZoneGraphicDataDic.Count)
				{
					continue;
				}
				ZoneGraphicData zoneGraphicData = mZoneRTRenderer.mZoneGraphicDataDic[zoneAtlasIndex_S];
				Vector2 meshOffset = zoneGraphicData.meshOffset;
				Vector3 pos = new Vector3(vector.x + meshOffset.x, 0f, vector.y + meshOffset.y);
				Quaternion quaternion = zoneGraphicData.quaternion;
				DrawMeshInstGraphic g = mZoneRTRenderer.mDrawMeshInstGraphicDic[zoneGraphicData.zoneMeshType];
				DrawMeshInstancedBatch orCreateBatch = mZoneRTRenderer.mDrawMeshInstancedBufferData.GetOrCreateBatch(in zoneGraphicData.drawKey, g);
				float z = 0f;
				float w = 0f;
				if (item.GetMapIndex_S6() != 4)
				{
					if (item.mOccupiedCampId == -1)
					{
						item.mOccupiedCampId = item.GetCampId_S6();
					}
					float deltaTime = Time.deltaTime;
					if (item.mOccupiedCampId == 1)
					{
						if (item.mOccupiedTransitionTime_Swap > 0f)
						{
							item.mOccupiedTransitionTime_Swap -= deltaTime;
							item.mOccupiedTransitionTime_Forest += deltaTime;
							z = 1f - (OccupiedChangeTime - item.mOccupiedTransitionTime_Swap) / OccupiedChangeTime;
							w = 1f - (OccupiedChangeTime - item.mOccupiedTransitionTime_Forest) / OccupiedChangeTime;
							flag = true;
						}
						else
						{
							z = 0f;
							w = 1f;
						}
						if (item.mOccupiedTransitionTime_Swap < 0f)
						{
							item.mOccupiedTransitionTime_Swap = 0f;
						}
						if (item.mOccupiedTransitionTime_Forest > OccupiedChangeTime)
						{
							item.mOccupiedTransitionTime_Forest = OccupiedChangeTime;
						}
					}
					else if (item.mOccupiedCampId == 2)
					{
						if (item.mOccupiedTransitionTime_Forest > 0f)
						{
							item.mOccupiedTransitionTime_Forest -= deltaTime;
							item.mOccupiedTransitionTime_Swap += deltaTime;
							z = 1f - (OccupiedChangeTime - item.mOccupiedTransitionTime_Swap) / OccupiedChangeTime;
							w = 1f - (OccupiedChangeTime - item.mOccupiedTransitionTime_Forest) / OccupiedChangeTime;
							flag = true;
						}
						else
						{
							z = 1f;
							w = 0f;
						}
						if (item.mOccupiedTransitionTime_Forest < 0f)
						{
							item.mOccupiedTransitionTime_Forest = 0f;
						}
						if (item.mOccupiedTransitionTime_Swap > OccupiedChangeTime)
						{
							item.mOccupiedTransitionTime_Swap = OccupiedChangeTime;
						}
					}
				}
				orCreateBatch.AddBuffer(v4: new Vector4(1f, 1f, z, w), matrix: Matrix4x4.TRS(pos, quaternion, _quadScale));
			}
			Dictionary<Vector2Int, GameObject> currentTerrainObjects = mZoneRTRenderer.mWorldStaticManager.GetCurrentTerrainObjects();
			DrawMeshInstGraphic g2 = mZoneRTRenderer.mDrawMeshInstGraphicDic[1001];
			DrawMeshInstancedBatch orCreateBatch2 = mZoneRTRenderer.mDrawMeshInstancedBufferData.GetOrCreateBatch(in mZoneRTRenderer.mTerrainDrawKey, g2);
			foreach (KeyValuePair<Vector2Int, GameObject> item2 in currentTerrainObjects)
			{
				if (item2.Key.x < 0 || item2.Key.y < 0 || item2.Key.x > 5 || item2.Key.y > 5)
				{
					Transform transform = item2.Value.GetComponentInChildren<MeshRenderer>().transform;
					if (transform != null)
					{
						Vector3 position = transform.position;
						Quaternion rotation = transform.rotation;
						Vector3 s = transform.lossyScale * _terrainQuadScale;
						orCreateBatch2.AddBuffer(v4: new Vector4(1f, 1f, 1f, 0f), matrix: Matrix4x4.TRS(position, rotation, s));
					}
				}
			}
			if (!flag)
			{
				mZoneRTRenderer.mWorldStaticManager.zoneMapViewChange = false;
			}
		}

		public void DrawZoneRT(CommandBuffer cmd)
		{
			List<int> activeBatchIndices = mZoneRTRenderer.mDrawMeshInstancedBufferData.activeBatchIndices;
			List<DrawMeshInstancedBatch> batches = mZoneRTRenderer.mDrawMeshInstancedBufferData.batches;
			for (int i = 0; i < activeBatchIndices.Count; i++)
			{
				int num = activeBatchIndices[i];
				if (num < 0 || num >= batches.Count)
				{
					continue;
				}
				DrawMeshInstancedBatch drawMeshInstancedBatch = batches[num];
				if (drawMeshInstancedBatch == null)
				{
					continue;
				}
				DrawMeshInstGraphic graphicsInfo = drawMeshInstancedBatch.graphicsInfo;
				if (graphicsInfo.material == null || graphicsInfo.mesh == null)
				{
					continue;
				}
				for (int j = 0; j < drawMeshInstancedBatch.matrices.Count; j++)
				{
					int num2 = drawMeshInstancedBatch.counts[j];
					if (num2 > 0)
					{
						drawMeshInstancedBatch.SetPropertyBufferVector4(j);
						cmd.DrawMeshInstanced(graphicsInfo.mesh, 0, graphicsInfo.material, 0, drawMeshInstancedBatch.matrices[j], num2, drawMeshInstancedBatch.propertyBlock);
					}
				}
			}
		}

		public void Release()
		{
			if (mZoneRT != null)
			{
				RenderTexture.ReleaseTemporary(mZoneRT);
			}
		}

		private bool NeedDrawZoneRT()
		{
			if (SceneManager.World.GetLodLevel() >= 6)
			{
				return false;
			}
			return true;
		}

		private bool NeedCalRenderZoneData()
		{
			if (mZoneRTRenderer.mWorldStaticManager.zoneMapViewChange)
			{
				return true;
			}
			return false;
		}
	}

	public struct zoneGraphicInfo
	{
		public float MeshOffSetX;

		public float MeshOffSetY;

		public float ST_X;

		public float ST_Y;
	}

	public struct ZoneGraphicData
	{
		public Mesh zoneMesh;

		public Quaternion quaternion;

		public Vector2 meshOffset;

		public int zoneMeshType;

		public DrawKey drawKey;
	}

	public static float OccupiedChangeTime = 1f;

	public const int TerrainMeshType = 1001;

	private WorldZoneRTRendererPass m_ScriptablePass;

	public Material ZoneMaterial_S6;

	public Material TerrainMaterial_S6;

	public Mesh TerrainMesh;

	public WorldStaticManager mWorldStaticManager;

	public Dictionary<int, ZoneGraphicData> mZoneGraphicDataDic;

	public float mZoneAtlasSTScale;

	public float mTerrainQuadSize = 1f;

	private DrawMeshInstancedBufferData mDrawMeshInstancedBufferData;

	private DrawMeshInstGraphicCache mDrawMeshGraphicCache;

	private Dictionary<int, DrawMeshInstGraphic> mDrawMeshInstGraphicDic;

	private DrawKey mTerrainDrawKey;

	private Asset mConfigRequest;

	private string zoneMaterialPath = "";

	private int _rtZoneNameID;

	public float mQuadScale = 1f;

	public override void Create()
	{
		m_ScriptablePass = new WorldZoneRTRendererPass();
		m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
	}

	public void InitZoneRTRenderer(WorldStaticManager worldStaticManager)
	{
		_rtZoneNameID = Shader.PropertyToID("_IPControlTex");
		mWorldStaticManager = worldStaticManager;
		mDrawMeshGraphicCache = new DrawMeshInstGraphicCache();
		mDrawMeshInstancedBufferData = new DrawMeshInstancedBufferData();
		mDrawMeshInstGraphicDic = new Dictionary<int, DrawMeshInstGraphic>();
		mConfigRequest = GameEntry.Resource.LoadAssetAsync("Assets/Main/SeasonRes/S6/Scenes/Zone/S6ZoneAtlasDatabase.asset", typeof(ZoneAtlasDatabase));
		Asset asset = mConfigRequest;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, new Action<Asset>(OnConfigLoadComplete));
	}

	public void InitZonesGraphicData(ZoneAtlasDatabase config)
	{
		Dictionary<int, ZoneGraphicData> dictionary = new Dictionary<int, ZoneGraphicData>();
		if (config == null || config.zoneDatas == null || config.zoneDatas.Length == 0 || mDrawMeshInstGraphicDic == null || mDrawMeshGraphicCache == null)
		{
			return;
		}
		mQuadScale = config.tileScale;
		mZoneAtlasSTScale = config.textureScale;
		mTerrainQuadSize = config.terrainMeshSize;
		ZoneTypeData[] zoneDatas = config.zoneDatas;
		foreach (ZoneTypeData zoneTypeData in zoneDatas)
		{
			if (zoneTypeData != null)
			{
				int zoneType = zoneTypeData.zoneType;
				ZoneGraphicData value = new ZoneGraphicData
				{
					zoneMesh = zoneTypeData.mesh,
					quaternion = Quaternion.Euler(zoneTypeData.eulerAngles),
					meshOffset = zoneTypeData.meshOffset,
					zoneMeshType = zoneTypeData.zoneMeshType,
					drawKey = new DrawKey(zoneTypeData.mesh, ZoneMaterial_S6, 1)
				};
				dictionary.Add(zoneType, value);
				if (!mDrawMeshInstGraphicDic.ContainsKey(value.zoneMeshType))
				{
					DrawMeshInstGraphic orCreate = mDrawMeshGraphicCache.GetOrCreate(in value.drawKey, value.zoneMesh, ZoneMaterial_S6, 1);
					mDrawMeshInstGraphicDic.Add(value.zoneMeshType, orCreate);
				}
			}
		}
		mTerrainDrawKey = new DrawKey(TerrainMesh, TerrainMaterial_S6, 1);
		DrawMeshInstGraphic orCreate2 = mDrawMeshGraphicCache.GetOrCreate(in mTerrainDrawKey, TerrainMesh, TerrainMaterial_S6, 1);
		mDrawMeshInstGraphicDic.Add(1001, orCreate2);
		mZoneGraphicDataDic = dictionary;
	}

	public void OnConfigLoadComplete(Asset request)
	{
		if (request.isDone)
		{
			ZoneAtlasDatabase zoneAtlasDatabase = request.asset as ZoneAtlasDatabase;
			if (zoneAtlasDatabase != null)
			{
				ZoneMaterial_S6 = zoneAtlasDatabase.ZoneRTMaterial;
				TerrainMaterial_S6 = zoneAtlasDatabase.terrainRTMaterial;
				OccupiedChangeTime = zoneAtlasDatabase.zoneCampChangeTime;
				TerrainMesh = CreateQuadXZ(1f);
				InitZonesGraphicData(zoneAtlasDatabase);
			}
		}
	}

	public static Mesh CreateQuadXZ(float size)
	{
		Mesh mesh = new Mesh();
		mesh.name = $"Quad_{size}";
		float num = size * 0.5f;
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(0f - num, 0f - num, 0f),
			new Vector3(num, 0f - num, 0f),
			new Vector3(0f - num, num, 0f),
			new Vector3(num, num, 0f)
		};
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		int[] triangles = new int[6] { 0, 2, 1, 2, 3, 1 };
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderingData.cameraData.renderType == CameraRenderType.Base)
		{
			RenderTargetIdentifier cameraColorTarget = renderer.cameraColorTarget;
			RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
			int width = cameraTargetDescriptor.width;
			int height = cameraTargetDescriptor.height;
			m_ScriptablePass.Setup(this, cameraColorTarget, width, height);
			renderer.EnqueuePass(m_ScriptablePass);
		}
	}

	public void AttachToRenderer(Camera camera)
	{
		UniversalAdditionalCameraData component = camera.GetComponent<UniversalAdditionalCameraData>();
		if (!component.scriptableRenderer.rendererFeatures.Contains(this))
		{
			component.scriptableRenderer.rendererFeatures.Add(this);
		}
	}

	public void DetachFromRenderer(Camera camera)
	{
		UniversalAdditionalCameraData component = camera.GetComponent<UniversalAdditionalCameraData>();
		if (component.scriptableRenderer.rendererFeatures.Contains(this))
		{
			component.scriptableRenderer.rendererFeatures.Remove(this);
			m_ScriptablePass.Release();
		}
		if (mConfigRequest != null)
		{
			mConfigRequest.Release();
			mConfigRequest = null;
		}
		if (mDrawMeshInstGraphicDic != null)
		{
			mDrawMeshInstGraphicDic.Clear();
		}
		mDrawMeshInstGraphicDic = null;
		mDrawMeshInstancedBufferData = null;
		mDrawMeshGraphicCache = null;
		mWorldStaticManager = null;
	}
}
