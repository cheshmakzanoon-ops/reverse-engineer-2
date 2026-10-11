using System.Numerics;
using MapLineSegmentToIsometricArc;
using TMPro;
using UnityEngine;

namespace Scripts.Utilities;

[ExecuteInEditMode]
[RequireComponent(typeof(TextMeshProUGUIEx))]
public class TextProOnACircleCurveNew : MonoBehaviour
{
	[SerializeField]
	[Min(1f)]
	private float radius = 1000f;

	private TextMeshProUGUIEx m_TextComponent;

	private string _lastText;

	private float _lastRadius = -1f;

	private UnityEngine.Vector2 _lastRectSize = UnityEngine.Vector2.negativeInfinity;

	private bool _dirty = true;

	private bool _isInternalMeshUpdate;

	private void Awake()
	{
		TryGetComponent<TextMeshProUGUIEx>(out m_TextComponent);
	}

	private void OnEnable()
	{
		if (m_TextComponent == null)
		{
			TryGetComponent<TextMeshProUGUIEx>(out m_TextComponent);
		}
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
		_dirty = true;
		UpdateCircle();
	}

	private void OnDisable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
		if (m_TextComponent != null)
		{
			m_TextComponent.ForceMeshUpdate();
			m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		}
		_lastText = null;
		_lastRadius = -1f;
		_lastRectSize = UnityEngine.Vector2.negativeInfinity;
		_dirty = true;
	}

	private void OnTextChanged(Object obj)
	{
		if (!(obj != m_TextComponent) && !_isInternalMeshUpdate)
		{
			UpdateCircle(respondToTmpMeshRebuild: true);
		}
	}

	private void OnRectTransformDimensionsChange()
	{
		_dirty = true;
	}

	private void OnValidate()
	{
		radius = Mathf.Max(1f, radius);
		_dirty = true;
	}

	private void LateUpdate()
	{
		if (!(m_TextComponent == null))
		{
			UnityEngine.Vector2 size = m_TextComponent.rectTransform.rect.size;
			if (_dirty || _lastText != m_TextComponent.text || !Mathf.Approximately(_lastRadius, radius) || _lastRectSize != size)
			{
				UpdateCircle();
			}
		}
	}

	protected void UpdateCircle(bool respondToTmpMeshRebuild = false)
	{
		if (m_TextComponent == null)
		{
			return;
		}
		string text = m_TextComponent.text;
		UnityEngine.Vector2 size = m_TextComponent.rectTransform.rect.size;
		if (!respondToTmpMeshRebuild && !_dirty && text == _lastText && Mathf.Approximately(_lastRadius, radius) && _lastRectSize == size)
		{
			return;
		}
		_lastText = text;
		_lastRadius = radius;
		_lastRectSize = size;
		_dirty = false;
		if (!respondToTmpMeshRebuild)
		{
			_isInternalMeshUpdate = true;
			try
			{
				m_TextComponent.ForceMeshUpdate();
			}
			finally
			{
				_isInternalMeshUpdate = false;
			}
		}
		TMP_TextInfo textInfo = m_TextComponent.textInfo;
		int characterCount = textInfo.characterCount;
		if (characterCount == 0)
		{
			return;
		}
		float x = m_TextComponent.bounds.min.x;
		float x2 = m_TextComponent.bounds.max.x;
		if (Mathf.Approximately(x, x2))
		{
			return;
		}
		float x3 = (x + x2) * 0.5f;
		float y = m_TextComponent.rectTransform.rect.center.y;
		System.Numerics.Vector2 vector = new System.Numerics.Vector2(x3, y - radius);
		Line2CirArcTransformator line2CirArcTransformator = new Line2CirArcTransformator(new System.Numerics.Vector2(x, y), new System.Numerics.Vector2(x2, y), vector, new System.Numerics.Vector2(x3, y));
		for (int i = 0; i < characterCount; i++)
		{
			if (textInfo.characterInfo[i].isVisible)
			{
				int vertexIndex = textInfo.characterInfo[i].vertexIndex;
				int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
				UnityEngine.Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
				UnityEngine.Vector3 vector2 = new UnityEngine.Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, y);
				vertices[vertexIndex] += -vector2;
				vertices[vertexIndex + 1] += -vector2;
				vertices[vertexIndex + 2] += -vector2;
				vertices[vertexIndex + 3] += -vector2;
				System.Numerics.Vector2 vector3 = line2CirArcTransformator.MapLinePoint(new System.Numerics.Vector2(vector2.x, vector2.y));
				System.Numerics.Vector2 vector4 = vector3 - vector;
				float num = Mathf.Atan2(vector4.Y, vector4.X);
				UnityEngine.Matrix4x4 matrix4x = UnityEngine.Matrix4x4.TRS(new UnityEngine.Vector3(vector3.X, vector3.Y, 0f), UnityEngine.Quaternion.AngleAxis(num * 57.29578f - 90f, UnityEngine.Vector3.forward), UnityEngine.Vector3.one);
				vertices[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex]);
				vertices[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 1]);
				vertices[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 2]);
				vertices[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 3]);
			}
		}
		m_TextComponent.UpdateVertexData();
	}
}
