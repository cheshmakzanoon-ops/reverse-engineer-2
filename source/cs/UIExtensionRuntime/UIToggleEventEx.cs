using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteAlways]
[DisallowMultipleComponent]
[RequireComponent(typeof(Toggle))]
public class UIToggleEventEx : MonoBehaviour
{
	[Serializable]
	public class UnityEventBool : UnityEvent<bool>
	{
	}

	public UnityEventBool onValueChangedInvert;

	public UnityEventBool onValueChangedToOn;

	public UnityEventBool onValueChangedToOff;

	private void OnEnable()
	{
		Toggle component = base.transform.GetComponent<Toggle>();
		if (component != null)
		{
			component.onValueChanged.AddListener(OnValueChange);
		}
	}

	private void OnDisable()
	{
		Toggle component = base.transform.GetComponent<Toggle>();
		if (component != null)
		{
			component.onValueChanged.RemoveListener(OnValueChange);
		}
	}

	private void OnValueChange(bool on)
	{
		onValueChangedInvert.Invoke(!on);
		onValueChangedToOn.Invoke(on);
		onValueChangedToOff.Invoke(!on);
	}
}
