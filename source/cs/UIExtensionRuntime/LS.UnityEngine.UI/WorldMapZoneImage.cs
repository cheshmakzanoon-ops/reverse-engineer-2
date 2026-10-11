using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
using XLua;

namespace LS.UnityEngine.UI;

[LuaCallCSharp(GenFlag.No)]
public class WorldMapZoneImage : Image
{
	[SerializeField]
	private int m_Rows = 1;

	[SerializeField]
	private int m_Columns = 1;

	[SerializeField]
	private float m_topScale;

	[SerializeField]
	private float m_space;

	[SerializeField]
	private Color[] m_CampColors = new Color[3]
	{
		new Color(0.7f, 0.7f, 0.7f, 1f),
		new Color(0.3529412f, 0.654902f, 33f / 85f, 1f),
		new Color(0.18039216f, 29f / 51f, 59f / 85f, 1f)
	};

	[SerializeField]
	private Color m_DefaultColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	private Color[] m_ZoneColors;

	public int rows
	{
		get
		{
			return m_Rows;
		}
		set
		{
			if (m_Rows != value)
			{
				m_Rows = value;
				SetVerticesDirty();
			}
		}
	}

	public int columns
	{
		get
		{
			return m_Columns;
		}
		set
		{
			if (m_Columns != value)
			{
				m_Columns = value;
				SetVerticesDirty();
			}
		}
	}

	public float topScale
	{
		get
		{
			return m_topScale;
		}
		set
		{
			if (m_topScale != value)
			{
				m_topScale = value;
				SetVerticesDirty();
			}
		}
	}

	public float space
	{
		get
		{
			return m_space;
		}
		set
		{
			if (m_space != value)
			{
				m_space = value;
				SetVerticesDirty();
			}
		}
	}

	public Color[] CampColors
	{
		set
		{
			if (value != null)
			{
				m_CampColors = value;
			}
		}
	}

	private int GetRowStride()
	{
		return m_Columns + (m_Columns - 1);
	}

	private int GetExpectedZoneColorCount()
	{
		return m_Rows * m_Columns + (m_Rows - 1) * (m_Columns - 1);
	}

	private bool HasValidZoneColors()
	{
		if (m_ZoneColors != null)
		{
			return m_ZoneColors.Length == GetExpectedZoneColorCount();
		}
		return false;
	}

	public void SetZoneCampIds(int[] campIds)
	{
		int expectedZoneColorCount = GetExpectedZoneColorCount();
		if (campIds != null && campIds.Length == expectedZoneColorCount)
		{
			if (m_ZoneColors == null || m_ZoneColors.Length != expectedZoneColorCount)
			{
				m_ZoneColors = new Color[expectedZoneColorCount];
			}
			for (int i = 0; i < campIds.Length; i++)
			{
				int campId = campIds[i];
				m_ZoneColors[i] = GetCampColor(campId);
			}
			SetVerticesDirty();
		}
	}

	private Color GetCampColor(int campId)
	{
		if (campId >= 0 && campId < m_CampColors.Length)
		{
			return m_CampColors[campId];
		}
		return m_DefaultColor;
	}

	public void ClearZoneColors()
	{
		m_ZoneColors = null;
		SetVerticesDirty();
	}

	private Color GetBottomZoneColor(int row, int col)
	{
		if (HasValidZoneColors())
		{
			int num = row * GetRowStride() + col;
			return m_ZoneColors[num];
		}
		return m_DefaultColor;
	}

	private Color GetTopZoneColor(int row, int col)
	{
		if (HasValidZoneColors())
		{
			int num = row * GetRowStride() + m_Columns + col;
			return m_ZoneColors[num];
		}
		return m_DefaultColor;
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
		if (m_Rows <= 0 || m_Columns <= 0)
		{
			return;
		}
		Rect pixelAdjustedRect = GetPixelAdjustedRect();
		float num = pixelAdjustedRect.width / (float)m_Columns;
		float num2 = pixelAdjustedRect.height / (float)m_Rows;
		Vector4 vector = ((base.overrideSprite != null) ? DataUtility.GetOuterUV(base.overrideSprite) : new Vector4(0f, 0f, 1f, 1f));
		float num3 = vector.z - vector.x;
		float num4 = vector.w - vector.y;
		float x = vector.x;
		float y = vector.y;
		float x2 = vector.x + num3;
		float y2 = vector.y + num4;
		float width = pixelAdjustedRect.width;
		float height = pixelAdjustedRect.height;
		float num5 = pixelAdjustedRect.width * 0.5f;
		float num6 = pixelAdjustedRect.height * 0.5f;
		Color white = Color.white;
		Color white2 = Color.white;
		for (int i = 0; i < m_Rows; i++)
		{
			for (int j = 0; j < m_Columns; j++)
			{
				float num7 = pixelAdjustedRect.x + (float)j * num + m_space;
				float num8 = pixelAdjustedRect.y + (float)i * num2 + m_space;
				float num9 = pixelAdjustedRect.x + (float)(j + 1) * num - m_space;
				float num10 = pixelAdjustedRect.y + (float)(i + 1) * num2 - m_space;
				float x3 = (num7 + num5) / width;
				float y3 = (num8 + num6) / height;
				float x4 = (num9 + num5) / width;
				float y4 = (num10 + num6) / height;
				int currentVertCount = vh.currentVertCount;
				white = GetBottomZoneColor(i, j);
				white2 = color * white;
				vh.AddVert(new Vector3(num7, num8), white2, new Vector2(x, y), new Vector2(x3, y3), Vector2.zero, Vector2.zero);
				vh.AddVert(new Vector3(num7, num10), white2, new Vector2(x, y2), new Vector2(x3, y4), Vector2.zero, Vector2.zero);
				vh.AddVert(new Vector3(num9, num10), white2, new Vector2(x2, y2), new Vector2(x4, y4), Vector2.zero, Vector2.zero);
				vh.AddVert(new Vector3(num9, num8), white2, new Vector2(x2, y), new Vector2(x4, y3), Vector2.zero, Vector2.zero);
				vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
				vh.AddTriangle(currentVertCount, currentVertCount + 2, currentVertCount + 3);
			}
		}
		Vector3 vector2 = new Vector3(num - m_space * 2f, num2 - m_space * 2f, 0f) * m_topScale;
		Vector3 vector3 = new Vector3((0f - vector2.x) * 0.5f, (0f - vector2.y) * 0.5f, 0f);
		Vector3 vector4 = new Vector3((0f - vector2.x) * 0.5f, vector2.y * 0.5f, 0f);
		Vector3 vector5 = new Vector3(vector2.x * 0.5f, vector2.y * 0.5f, 0f);
		Vector3 vector6 = new Vector3(vector2.x * 0.5f, (0f - vector2.y) * 0.5f, 0f);
		for (int k = 0; k < m_Rows - 1; k++)
		{
			for (int l = 0; l < m_Columns - 1; l++)
			{
				_ = pixelAdjustedRect.x;
				_ = pixelAdjustedRect.y;
				float x5 = pixelAdjustedRect.x + (float)(l + 1) * num;
				float y5 = pixelAdjustedRect.y + (float)(k + 1) * num2;
				Vector2 uv = Vector2.one * 0.5f;
				Vector3 vector7 = new Vector3(x5, y5, 0f);
				int currentVertCount2 = vh.currentVertCount;
				white = GetTopZoneColor(k, l);
				vh.AddVert(vector7 + vector3, white, new Vector2(x, y), uv, Vector2.zero, Vector2.zero);
				vh.AddVert(vector7 + vector4, white, new Vector2(x, y2), uv, Vector2.zero, Vector2.zero);
				vh.AddVert(vector7 + vector5, white, new Vector2(x2, y2), uv, Vector2.zero, Vector2.zero);
				vh.AddVert(vector7 + vector6, white, new Vector2(x2, y), uv, Vector2.zero, Vector2.zero);
				vh.AddTriangle(currentVertCount2, currentVertCount2 + 1, currentVertCount2 + 2);
				vh.AddTriangle(currentVertCount2, currentVertCount2 + 2, currentVertCount2 + 3);
			}
		}
	}
}
