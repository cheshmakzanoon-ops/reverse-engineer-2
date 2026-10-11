using GPUDamageText;
using Unity.Mathematics;
using UnityEngine;

public class Test : MonoBehaviour
{
	[SerializeField]
	private Material material;

	public Font m_font;

	public Texture2D m_FontTexture;

	private Mesh mesh;

	private float2[] fontVertices;

	private float2[] fontUvs;

	private MaterialPropertyBlock propertyBlock;

	private const int MAX_RENDER_NUM = 1023;

	private Matrix4x4[] m_RenderLocalToWords = new Matrix4x4[1023];

	private Matrix4x4[] m_RenderEntityCharVertIndex = new Matrix4x4[1023];

	private Matrix4x4[] m_RenderEntityCharAdv = new Matrix4x4[1023];

	private float[] m_RenderEntityScales = new float[1023];

	private float m_VertNormalizeV;

	private int m_VertCordXMin = int.MaxValue;

	private int m_VertCordXMax = int.MinValue;

	private int m_VertCordYMin = int.MaxValue;

	private int m_VertCordYMax = int.MinValue;

	private int renderCount;

	private void Start()
	{
		Init();
	}

	private void Init()
	{
		propertyBlock = new MaterialPropertyBlock();
		InitFontTexture();
		InitMesh();
		InitMaterial();
	}

	private void InitFontTexture()
	{
		InitFontVetUv();
		CreateCharVertTexture2D();
	}

	private void InitFontVetUv()
	{
		char[] chars = FontMeshHelper.Chars;
		int num = chars.Length;
		int num2 = num * 4;
		fontVertices = new float2[num2];
		fontUvs = new float2[num2];
		for (int i = 0; i < num; i++)
		{
			char ch = chars[i];
			if (m_font.GetCharacterInfo(ch, out var info))
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
				fontVertices[num3] = new float2(info.minX, info.maxY);
				fontVertices[num3 + 1] = new float2(info.maxX, info.maxY);
				fontVertices[num3 + 2] = new float2(info.minX, info.minY);
				fontVertices[num3 + 3] = new float2(info.maxX, info.minY);
				fontUvs[num3] = info.uvTopLeft;
				fontUvs[num3 + 1] = info.uvTopRight;
				fontUvs[num3 + 2] = info.uvBottomLeft;
				fontUvs[num3 + 3] = info.uvBottomRight;
			}
		}
	}

	private void CreateCharVertTexture2D()
	{
		int num = fontVertices.Length;
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
			float2 @float = fontVertices[i];
			float r = (@float.x - (float)m_VertCordXMin) / (float)num3;
			float g = (@float.y - (float)m_VertCordYMin) / (float)num4;
			float2 float2 = fontUvs[i];
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

	private void InitMesh()
	{
		mesh = FontMeshHelper.CreateMesh(m_font);
	}

	private void InitMaterial()
	{
		material.SetVector("_FontVertSpan", new Vector4(m_VertCordXMin, m_VertCordXMax, m_VertCordYMin, m_VertCordYMax));
		material.SetTexture("_TextFonts", m_FontTexture);
		material.enableInstancing = true;
	}

	private void Render()
	{
		renderCount = 1;
		TestRandomGeneratorText(renderCount);
		propertyBlock.SetMatrixArray("charIds", m_RenderEntityCharVertIndex);
		propertyBlock.SetMatrixArray("charAdv", m_RenderEntityCharAdv);
		propertyBlock.SetFloatArray("charScale", m_RenderEntityScales);
		Graphics.DrawMeshInstanced(mesh, 0, material, m_RenderLocalToWords, renderCount, propertyBlock);
	}

	private void TestRandomGeneratorText(int num)
	{
		for (int i = 0; i < num; i++)
		{
			m_RenderLocalToWords[i] = Matrix4x4.TRS(new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f), 0f), Quaternion.identity, Vector3.one);
			int damageStyle = UnityEngine.Random.Range(0, 6);
			int num2 = UnityEngine.Random.Range(0, 999999999);
			float num3 = UnityEngine.Random.Range(0.8f, 1.5f) * 0.0028f;
			int damageType = UnityEngine.Random.Range(0, 2);
			FontMeshHelper.SetIndex(m_font, damageStyle, damageType, showIcon: true, (ulong)num2, ref m_RenderEntityCharVertIndex[i], ref m_RenderEntityCharAdv[i]);
			m_RenderEntityScales[i] = num3;
		}
	}

	private void Update()
	{
		Render();
	}

	private void OnDestroy()
	{
		propertyBlock?.Clear();
		Object.Destroy(m_FontTexture);
	}
}
