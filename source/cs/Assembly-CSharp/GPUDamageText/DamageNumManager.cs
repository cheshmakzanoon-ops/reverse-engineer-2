using System.Collections.Generic;
using System.Reflection;
using FibMatrix.Rendering;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VEngine;

namespace GPUDamageText;

public class DamageNumManager
{
	private static DamageNumManager ms_Instance;

	public const int DEFAULT_RENDER_MAX_NUM = 1023;

	private Texture2D m_FontTexture;

	private Mesh m_DamageNumMesh;

	private DamageNumRes m_Res;

	private MaterialPropertyBlock m_PropertyBlock = new MaterialPropertyBlock();

	private bool m_Inited;

	private bool m_InitCompleted;

	private float2[] m_FontVertices;

	private float2[] m_FontUvs;

	private int m_VertCordXMin = int.MaxValue;

	private int m_VertCordXMax = int.MinValue;

	private int m_VertCordYMin = int.MaxValue;

	private int m_VertCordYMax = int.MinValue;

	private int m_RenderCount;

	private ulong m_ID = 1uL;

	private readonly Dictionary<ulong, int> m_IdMap = new Dictionary<ulong, int>();

	private readonly List<ulong> m_ToRemoveIds = new List<ulong>();

	private readonly Matrix4x4[] m_RenderLocalToWorlds = new Matrix4x4[1023];

	private readonly Matrix4x4[] m_RenderEntityCharVertIndex = new Matrix4x4[1023];

	private readonly Matrix4x4[] m_RenderEntityCharAdv = new Matrix4x4[1023];

	private readonly Vector4[] m_RenderEntityScales = new Vector4[1023];

	private readonly float[] m_RenderEntityAlpha = new float[1023];

	private DamageNumEntity[] m_RenderEntities = new DamageNumEntity[1023];

	private Quaternion m_DefaultQuaternion = Quaternion.Euler(50f, 0f, 0f);

	public static DamageNumManager Instance
	{
		get
		{
			if (ms_Instance == null)
			{
				ms_Instance = new DamageNumManager();
			}
			return ms_Instance;
		}
	}

	public static GPUDmgTextRenderFeature RenderFeature { get; private set; }

	public int GetRenderCount()
	{
		return m_RenderCount;
	}

	public void Init()
	{
		if (!m_Inited)
		{
			m_Inited = true;
			m_InitCompleted = false;
			if (Camera.main != null)
			{
				m_DefaultQuaternion = Camera.main.transform.rotation;
			}
			else
			{
				m_DefaultQuaternion = Quaternion.Euler(50f, 0f, 0f);
			}
			DamageNumResLoader.GetResAsset(OnResLoadComplete);
		}
	}

	private void OnResLoadComplete(DamageNumRes res)
	{
		m_Res = res;
		InitFontVetUv();
		CreateCharVertTexture2D();
		InitMesh();
		InitMaterial();
		m_InitCompleted = true;
	}

	private void InitMaterial()
	{
		if (!(m_Res == null))
		{
			Material fontMaterial = m_Res.FontMaterial;
			if (!(fontMaterial == null))
			{
				fontMaterial.SetVector("_FontVertSpan", new Vector4(m_VertCordXMin, m_VertCordXMax, m_VertCordYMin, m_VertCordYMax));
				fontMaterial.SetTexture("_TextFonts", m_FontTexture);
				fontMaterial.enableInstancing = true;
			}
		}
	}

	private void InitMesh()
	{
		if (!(m_Res == null))
		{
			Font fontRes = m_Res.FontRes;
			if (!(fontRes == null))
			{
				m_DamageNumMesh = FontMeshHelper.CreateMesh(fontRes);
			}
		}
	}

	private void InitFontVetUv()
	{
		char[] chars = FontMeshHelper.Chars;
		int num = chars.Length;
		int num2 = num * 4;
		m_FontVertices = new float2[num2];
		m_FontUvs = new float2[num2];
		if (m_Res == null)
		{
			return;
		}
		Font fontRes = m_Res.FontRes;
		if (fontRes == null)
		{
			return;
		}
		for (int i = 0; i < num; i++)
		{
			char ch = chars[i];
			if (fontRes.GetCharacterInfo(ch, out var info))
			{
				if (info.minX < m_VertCordXMin)
				{
					m_VertCordXMin = info.minX;
				}
				if (info.maxX > m_VertCordXMax)
				{
					m_VertCordXMax = info.maxX;
				}
				if (info.minY < m_VertCordYMin)
				{
					m_VertCordYMin = info.minY;
				}
				if (info.maxY > m_VertCordYMax)
				{
					m_VertCordYMax = info.maxY;
				}
				int num3 = i * 4;
				m_FontVertices[num3] = new float2(info.minX, info.maxY);
				m_FontVertices[num3 + 1] = new float2(info.maxX, info.maxY);
				m_FontVertices[num3 + 2] = new float2(info.minX, info.minY);
				m_FontVertices[num3 + 3] = new float2(info.maxX, info.minY);
				m_FontUvs[num3] = info.uvTopLeft;
				m_FontUvs[num3 + 1] = info.uvTopRight;
				m_FontUvs[num3 + 2] = info.uvBottomLeft;
				m_FontUvs[num3 + 3] = info.uvBottomRight;
			}
		}
	}

	private void CreateCharVertTexture2D()
	{
		int num = m_FontVertices.Length;
		int n = Mathf.CeilToInt(Mathf.Sqrt(num));
		int num2 = (int)NextPowerOfTwo((uint)n);
		m_FontTexture = new Texture2D(num2, num2, TextureFormat.RGBAHalf, mipChain: false)
		{
			filterMode = FilterMode.Point,
			wrapMode = TextureWrapMode.Clamp
		};
		Color[] array = new Color[num2 * num2];
		int num3 = m_VertCordXMax - m_VertCordXMin;
		int num4 = m_VertCordYMax - m_VertCordYMin;
		for (int i = 0; i < num; i++)
		{
			float2 @float = m_FontVertices[i];
			float r = (@float.x - (float)m_VertCordXMin) / (float)num3;
			float g = (@float.y - (float)m_VertCordYMin) / (float)num4;
			float2 float2 = m_FontUvs[i];
			array[i] = new Color(r, g, float2.x, float2.y);
		}
		m_FontTexture.SetPixels(0, 0, num2, num2, array);
		m_FontTexture.Apply();
	}

	private uint NextPowerOfTwo(uint n)
	{
		if (n <= 1)
		{
			return 1u;
		}
		n--;
		n |= n >> 1;
		n |= n >> 2;
		n |= n >> 4;
		n |= n >> 8;
		n |= n >> 16;
		return n + 1;
	}

	public void Update()
	{
		if (!m_InitCompleted || m_Res == null || m_Res.FontMaterial == null || m_DamageNumMesh == null)
		{
			return;
		}
		UpdateEntity();
		if (m_RenderCount > 0)
		{
			Render();
			if (RenderFeature == null)
			{
				InitRenderFeature();
			}
			else
			{
				RenderFeature.SetActive(active: true);
			}
		}
		else if (RenderFeature != null)
		{
			RenderFeature.SetActive(active: false);
		}
	}

	private void InitRenderFeature()
	{
		foreach (ScriptableRendererFeature rendererFeatures in RenderQualitySetting.ScriptableRenderer.GetRendererFeaturesList())
		{
			if (rendererFeatures is GPUDmgTextRenderFeature)
			{
				RenderFeature = rendererFeatures as GPUDmgTextRenderFeature;
				break;
			}
		}
		if (RenderFeature == null)
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
			ScriptableRendererData[] obj = (ScriptableRendererData[])universalRenderPipelineAsset.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(universalRenderPipelineAsset);
			ScriptableRendererData scriptableRendererData = ((obj != null) ? obj[0] : null);
			if (scriptableRendererData != null)
			{
				GPUDmgTextRenderFeature gPUDmgTextRenderFeature = ScriptableObject.CreateInstance<GPUDmgTextRenderFeature>();
				if (gPUDmgTextRenderFeature != null)
				{
					gPUDmgTextRenderFeature.name = "GPUDmgTextRenderFeature[Dynamic]";
					scriptableRendererData.rendererFeatures.Insert(0, gPUDmgTextRenderFeature);
					RenderFeature = gPUDmgTextRenderFeature;
				}
				scriptableRendererData.SetDirty();
			}
		}
		if (RenderFeature == null)
		{
			VEngine.Logger.E("[GPUDmgTextRenderFeature]RenderFeature is null");
		}
		else if ((bool)RenderFeature)
		{
			RenderFeature.SetActive(active: true);
		}
	}

	public ulong AddDamageNum(int style, int damageType, ulong damageNum, bool showIcon, float posX, float poxY, float posZ, int animStyle, float scale, float time)
	{
		if (!m_InitCompleted)
		{
			return 0uL;
		}
		if (m_Res == null)
		{
			return 0uL;
		}
		Font fontRes = m_Res.FontRes;
		if (fontRes == null)
		{
			return 0uL;
		}
		if (m_RenderCount >= 1023)
		{
			return 0uL;
		}
		ulong num = m_ID++;
		int num2 = m_RenderCount++;
		m_IdMap[num] = num2;
		float num3 = FontMeshHelper.SetIndex(fontRes, style, damageType, showIcon, damageNum, ref m_RenderEntityCharVertIndex[num2], ref m_RenderEntityCharAdv[num2]) / 2f;
		float totalTime = ((time == 0f) ? m_Res.FontCurves[animStyle].Length : time);
		float num4 = scale * 0.0028f;
		Vector3 vector = new Vector3(posX - num3 * num4, poxY, posZ);
		m_RenderEntities[num2] = new DamageNumEntity
		{
			Id = num,
			StartPos = vector,
			StartScale = num4,
			Time = 0f,
			AnimStyle = animStyle,
			TotalTime = totalTime
		};
		m_RenderLocalToWorlds[num2] = Matrix4x4.TRS(vector, m_DefaultQuaternion, Vector3.one);
		m_RenderEntityScales[num2] = new float4(num4, num4, num4, num4);
		m_RenderEntityAlpha[num2] = 0f;
		return num;
	}

	public int GetAliveCount()
	{
		return m_RenderCount;
	}

	public Mesh GetRenderMesh()
	{
		return m_DamageNumMesh;
	}

	public Material GetRenderMaterial()
	{
		return m_Res.FontMaterial;
	}

	public MaterialPropertyBlock GetRenderMaterialPropertyBlock()
	{
		return m_PropertyBlock;
	}

	public Matrix4x4[] GetRenderLocalToWorlds()
	{
		return m_RenderLocalToWorlds;
	}

	private void UpdateEntity()
	{
		m_ToRemoveIds.Clear();
		for (int i = 0; i < m_RenderCount; i++)
		{
			ref DamageNumEntity reference = ref m_RenderEntities[i];
			if (reference.Time >= reference.TotalTime)
			{
				m_ToRemoveIds.Add(reference.Id);
				continue;
			}
			reference.Time += Time.deltaTime;
			float num = reference.Time / reference.TotalTime;
			AnimationData obj = m_Res.FontCurves[reference.AnimStyle];
			float length = obj.Length;
			float time = num * length;
			float startScale = reference.StartScale;
			float num2 = obj.Evaluate(4, time);
			float num3 = obj.Evaluate(5, time);
			float num4 = obj.Evaluate(6, time);
			float x = startScale * num2;
			float y = startScale * num3;
			float z = startScale * num4;
			ref Vector4 reference2 = ref m_RenderEntityScales[i];
			reference2.x = x;
			reference2.y = y;
			reference2.z = z;
			float num5 = obj.Evaluate(1, time);
			num5 += reference.StartPos.x;
			float num6 = obj.Evaluate(2, time);
			num6 += reference.StartPos.y;
			float num7 = obj.Evaluate(3, time);
			num7 += reference.StartPos.z;
			ref Matrix4x4 reference3 = ref m_RenderLocalToWorlds[i];
			reference3.m03 = num5;
			reference3.m13 = num6;
			reference3.m23 = num7;
			float num8 = obj.Evaluate(0, time);
			m_RenderEntityAlpha[i] = num8;
		}
		foreach (ulong toRemoveId in m_ToRemoveIds)
		{
			int num9 = m_IdMap[toRemoveId];
			int num10 = m_RenderCount - 1;
			if (num9 < num10)
			{
				DamageNumEntity damageNumEntity = m_RenderEntities[num10];
				m_RenderEntities[num9] = damageNumEntity;
				m_RenderLocalToWorlds[num9] = m_RenderLocalToWorlds[num10];
				m_RenderEntityScales[num9] = m_RenderEntityScales[num10];
				m_RenderEntityAlpha[num9] = m_RenderEntityAlpha[num10];
				m_RenderEntityCharVertIndex[num9] = m_RenderEntityCharVertIndex[num10];
				m_RenderEntityCharAdv[num9] = m_RenderEntityCharAdv[num10];
				m_IdMap[damageNumEntity.Id] = num9;
			}
			m_RenderCount--;
			m_IdMap.Remove(toRemoveId);
		}
	}

	private void Render()
	{
		m_PropertyBlock.SetMatrixArray("charIds", m_RenderEntityCharVertIndex);
		m_PropertyBlock.SetMatrixArray("charAdv", m_RenderEntityCharAdv);
		m_PropertyBlock.SetVectorArray("charScale", m_RenderEntityScales);
		m_PropertyBlock.SetFloatArray("charAlpha", m_RenderEntityAlpha);
	}

	public void Clear()
	{
		m_PropertyBlock?.Clear();
		m_RenderCount = 0;
		m_ID = 1uL;
		m_IdMap.Clear();
		m_ToRemoveIds.Clear();
		if ((bool)RenderFeature)
		{
			RenderFeature.SetActive(active: false);
		}
	}

	public void Release()
	{
		Clear();
		m_Inited = false;
		m_InitCompleted = false;
		m_Res = null;
		RenderFeature = null;
		if (m_DamageNumMesh != null)
		{
			Object.Destroy(m_DamageNumMesh);
		}
		m_DamageNumMesh = null;
		if (m_FontTexture != null)
		{
			Object.Destroy(m_FontTexture);
		}
		m_FontTexture = null;
		DamageNumResLoader.Release();
	}
}
