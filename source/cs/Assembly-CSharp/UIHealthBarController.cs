using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class UIHealthBarController : MonoBehaviour
{
	private Graphic m_Graphic;

	private static readonly int MaxHPID = Shader.PropertyToID("_MaxHP");

	private static readonly int UnitHPID = Shader.PropertyToID("_UnitHP");

	private static readonly int FillAmountID = Shader.PropertyToID("_FillAmount");

	[SerializeField]
	private float maxHP = 1000f;

	[SerializeField]
	private float currentHP;

	[SerializeField]
	private float dividerUnit = 100f;

	private void Awake()
	{
		m_Graphic = GetComponent<Graphic>();
	}

	public void SetHealth(float cur, float max)
	{
		maxHP = max;
		currentHP = cur;
		UpdateProperties();
	}

	public void SetDividerUnit(float unit)
	{
		dividerUnit = unit;
		UpdateProperties();
	}

	private void OnValidate()
	{
		UpdateProperties();
	}

	private void UpdateProperties()
	{
		if (m_Graphic == null)
		{
			m_Graphic = GetComponent<Graphic>();
		}
		if (!(m_Graphic == null) && !(m_Graphic.canvasRenderer == null))
		{
			float value = ((maxHP > 0f) ? Mathf.Clamp01(currentHP / maxHP) : 0f);
			Material material = m_Graphic.material;
			if (material != null)
			{
				material.SetFloat(MaxHPID, maxHP);
				material.SetFloat(UnitHPID, dividerUnit);
				material.SetFloat(FillAmountID, value);
			}
		}
	}
}
