using Unity.Mathematics;
using UnityEngine;

namespace GPUDamageText;

public static class FontMeshHelper
{
	private static string m_TemplateText = "0000000000";

	private const int RENDER_CHAR_LENGTH = 16;

	private const int PER_STYLE_CHAR_LENGTH = 16;

	public const int STYPLE_TYPE_LENGTH = 5;

	private static int[] m_TempIntArr = new int[20];

	public const float TEXT_BASIC_SIZE_FACTOR = 0.0028f;

	public const char CHAR_PHY_ATT_ICON = '!';

	public const char CHAR_MAG_ATT_ICON = '@';

	public const char CHAR_PLANE_ATT_ICON = '#';

	public const char CHAR_DEF_ATT_ICON = '$';

	public const int CHAR_PHY_ATT_ICON_INDEX = 80;

	public const int CHAR_MAG_ATT_ICON_INDEX = 81;

	public const int CHAR_PLANE_ATT_ICON_INDEX = 82;

	public const int CHAR_DEF_ATT_ICON_INDEX = 83;

	public const int STYLE_SUB_SYMBOL_OFFSET = 10;

	public const int STYLE_K_SYMBOL_OFFSET = 11;

	public const int STYLE_M_SYMBOL_OFFSET = 12;

	public const int STYLE_POINT_SYMBOL_OFFSET = 13;

	public const int STYLE_G_SYMBOL_OFFSET = 14;

	public const int STYLE_EX_SYMBOL_OFFSET = 15;

	public static readonly char[] Chars = new char[85]
	{
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
		'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p',
		'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z',
		'x', 'c', 'v', 'b', 'n', 'm', 'Q', 'W', 'E', 'R',
		'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D', 'F',
		'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B',
		'N', 'M', '%', '^', '&', '*', '(', ')', '-', '+',
		'_', '=', '[', ']', '{', '}', ';', ':', ',', '.',
		'!', '@', '#', '$', ' '
	};

	private static readonly int SpaceIndex = Chars.Length - 1;

	private static int MeshVertLen => m_TemplateText.Length * 4;

	private static int MeshIndexLen => m_TemplateText.Length * 6;

	public static Mesh CreateMesh(Font font)
	{
		Mesh mesh = new Mesh();
		Vector3[] vertices = new Vector3[MeshVertLen];
		int[] triangles = new int[MeshIndexLen];
		Vector2[] uv = new Vector2[MeshVertLen];
		int length = m_TemplateText.Length;
		for (int i = 0; i < length; i++)
		{
			SetTriangles(i, ref triangles);
		}
		SetTextUVVet(font, ref vertices, ref uv);
		mesh.SetVertices(vertices);
		mesh.SetTriangles(triangles, 0);
		mesh.SetUVs(0, uv);
		return mesh;
	}

	private static void SetTriangles(int index, ref int[] triangles)
	{
		triangles[6 * index] = 4 * index;
		triangles[6 * index + 1] = 4 * index + 1;
		triangles[6 * index + 2] = 4 * index + 2;
		triangles[6 * index + 3] = 4 * index + 2;
		triangles[6 * index + 4] = 4 * index + 1;
		triangles[6 * index + 5] = 4 * index + 3;
	}

	private static void SetTextUVVet(Font font, ref Vector3[] vertices, ref Vector2[] uv)
	{
		Vector3 vector = default(Vector3);
		int length = m_TemplateText.Length;
		for (int i = 0; i < length; i++)
		{
			char ch = m_TemplateText[i];
			font.GetCharacterInfo(ch, out var info);
			int num = i * 4;
			vertices[num] = vector + new Vector3(info.minX, info.maxY);
			vertices[num + 1] = vector + new Vector3(info.maxX, info.maxY);
			vertices[num + 2] = vector + new Vector3(info.minX, info.minY);
			vertices[num + 3] = vector + new Vector3(info.maxX, info.minY);
			uv[num] = new Vector2(0f, 0f);
			uv[num + 1] = new Vector2(0f, 0f);
			uv[num + 2] = new Vector2(0f, 0f);
			uv[num + 3] = new Vector2(0f, 0f);
			vector += new Vector3(info.advance, 0f);
		}
	}

	public static float SetIndex(Font font, int damageStyle, int damageType, bool showIcon, ulong dmgNum, ref Matrix4x4 index, ref Matrix4x4 adv)
	{
		int pivot = 0;
		if (damageType == 1)
		{
			m_TempIntArr[pivot++] = 15;
		}
		PrintFormat(dmgNum, ref m_TempIntArr, ref pivot);
		int num = damageStyle;
		int num2 = -1;
		if (showIcon)
		{
			switch (damageStyle)
			{
			case 2:
				num2 = 81;
				break;
			case 1:
				num2 = 80;
				break;
			case 3:
				num2 = 83;
				break;
			case 5:
				num2 = 82;
				break;
			}
		}
		if (damageStyle == 5)
		{
			num = 1;
		}
		if (num2 != -1)
		{
			m_TempIntArr[pivot++] = num2;
		}
		int num3 = pivot;
		float num4 = 0f;
		font.GetCharacterInfo(Chars[SpaceIndex], out var info);
		int advance = info.advance;
		for (int i = 0; i < 16; i++)
		{
			float num5 = 0.5f;
			int row = i / 4;
			int column = i % 4;
			if (num3 > i)
			{
				int num6 = num3 - 1 - i;
				int spaceIndex = SpaceIndex;
				spaceIndex = ((i != 0 || num2 == -1) ? (num * 16 + m_TempIntArr[num6]) : m_TempIntArr[num6]);
				index[row, column] = spaceIndex;
				char ch = Chars[spaceIndex];
				font.GetCharacterInfo(ch, out var info2);
				num5 = info2.advance;
			}
			else
			{
				index[row, column] = SpaceIndex;
				num5 = advance;
			}
			adv[row, column] = num4;
			num4 += num5;
		}
		return num4;
	}

	private static void PrintFormat(ulong value, ref int[] bits, ref int pivot)
	{
		ulong num = value / 10;
		ulong num2 = value / 10000;
		ulong num3 = value / 10000000;
		int num4 = -1;
		bool flag = true;
		ulong num5 = value;
		if (num3 >= 100)
		{
			num4 = 14;
			num5 = ((num3 >= 100000) ? 99999 : num3);
		}
		else if (num2 >= 100)
		{
			num4 = 12;
			num5 = num2;
		}
		else if (num >= 100)
		{
			num4 = 11;
			num5 = num;
		}
		else
		{
			flag = false;
		}
		if (num4 != -1)
		{
			bits[pivot++] = num4;
		}
		bool flag2 = false;
		int num6 = 0;
		while (math.floor(num5) > 0f)
		{
			num6++;
			ulong num7 = num5 % 10;
			if (num7 == 0L)
			{
				if (!flag2 && flag)
				{
					num5 /= 10;
					continue;
				}
			}
			else
			{
				flag2 = true;
			}
			bits[pivot++] = (int)num7;
			if (num6 == 2)
			{
				if (flag2 && flag)
				{
					bits[pivot++] = 13;
				}
				flag2 = true;
			}
			num5 /= 10;
		}
		bits[pivot++] = 10;
	}
}
