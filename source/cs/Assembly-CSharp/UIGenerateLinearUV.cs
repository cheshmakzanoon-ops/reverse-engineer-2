using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIGenerateLinearUV : BaseMeshEffect
{
	private static readonly List<UIVertex> s_Verts = new List<UIVertex>();

	public override void ModifyMesh(VertexHelper vh)
	{
		if (!IsActive())
		{
			return;
		}
		vh.GetUIVertexStream(s_Verts);
		int count = s_Verts.Count;
		if (count == 0)
		{
			return;
		}
		Vector3 position = s_Verts[0].position;
		Vector3 position2 = s_Verts[0].position;
		for (int i = 1; i < count; i++)
		{
			Vector3 position3 = s_Verts[i].position;
			if (position3.x < position.x)
			{
				position.x = position3.x;
			}
			if (position3.y < position.y)
			{
				position.y = position3.y;
			}
			if (position3.x > position2.x)
			{
				position2.x = position3.x;
			}
			if (position3.y > position2.y)
			{
				position2.y = position3.y;
			}
		}
		float num = position2.x - position.x;
		float num2 = position2.y - position.y;
		if (num <= 0f)
		{
			num = 1f;
		}
		if (num2 <= 0f)
		{
			num2 = 1f;
		}
		for (int j = 0; j < count; j++)
		{
			UIVertex value = s_Verts[j];
			Vector2 uv = new Vector2((value.position.x - position.x) / num, (value.position.y - position.y) / num2);
			value.uv1 = uv;
			s_Verts[j] = value;
		}
		vh.Clear();
		vh.AddUIVertexTriangleStream(s_Verts);
	}
}
