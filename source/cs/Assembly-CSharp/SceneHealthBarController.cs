using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class SceneHealthBarController : MonoBehaviour
{
	private SpriteRenderer m_SpriteRenderer;

	private MaterialPropertyBlock m_PropBlock;

	[FormerlySerializedAs("currentMaxHP")]
	[Header("Health Settings (Read Only in Inspector)")]
	[SerializeField]
	private float maxHP = 1000f;

	[SerializeField]
	private float currentHP = 777f;

	[SerializeField]
	private float dividerUnit = 100f;

	[Header("Visual Settings")]
	public Color lineColor = Color.black;

	[Range(0.1f, 10f)]
	public float lineWidth = 1f;

	public bool useQualityMode = true;

	private static readonly int MaxHPID = Shader.PropertyToID("_MaxHP");

	private static readonly int UnitHPID = Shader.PropertyToID("_UnitHP");

	private static readonly int FillAmountID = Shader.PropertyToID("_FillAmount");

	private static readonly int LineColorID = Shader.PropertyToID("_LineColor");

	private static readonly int LineWidthID = Shader.PropertyToID("_LineWidth");

	private static readonly int UseScreenSpaceID = Shader.PropertyToID("_UseScreenSpaceSize");

	private void Awake()
	{
		Init();
		UpdateMaterialProperties();
	}

	private void Init()
	{
		if (m_SpriteRenderer == null)
		{
			m_SpriteRenderer = GetComponent<SpriteRenderer>();
		}
		if (m_PropBlock == null)
		{
			m_PropBlock = new MaterialPropertyBlock();
		}
	}

	public void SetCurAndMaxHP(float cur, float max)
	{
		currentHP = cur;
		maxHP = max;
		UpdateMaterialProperties();
	}

	public void SetDividerUnit(float unit)
	{
		dividerUnit = unit;
		UpdateMaterialProperties();
	}

	private void OnValidate()
	{
		UpdateMaterialProperties();
	}

	public void UpdateMaterialProperties()
	{
		Init();
		float value = ((maxHP > 0f) ? Mathf.Clamp01(currentHP / maxHP) : 0f);
		m_SpriteRenderer.GetPropertyBlock(m_PropBlock);
		m_PropBlock.SetColor("_RendererColor", m_SpriteRenderer.color);
		m_PropBlock.SetFloat(MaxHPID, maxHP);
		m_PropBlock.SetFloat(UnitHPID, dividerUnit);
		m_PropBlock.SetFloat(FillAmountID, value);
		m_PropBlock.SetColor(LineColorID, lineColor);
		m_PropBlock.SetFloat(LineWidthID, lineWidth);
		m_PropBlock.SetFloat(UseScreenSpaceID, useQualityMode ? 1f : 0f);
		m_SpriteRenderer.SetPropertyBlock(m_PropBlock);
	}
}
