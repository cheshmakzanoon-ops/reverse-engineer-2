using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class PostColorAdjustAni : MonoBehaviour
{
	public float PostExposure;

	public float Contrast = 1f;

	[ColorUsage(true, true)]
	public Color ColorFilter = Color.white.linear;

	public float HueShift = 1f;

	public float Saturation = 1f;

	public Volume volume;

	public VolumeProfile volumeProfile;

	private float originalPostExposure;

	private float originalContrast;

	private Color originalColorFilter;

	private float originalHueShift;

	private float originalSaturation;

	private bool hasSavedOriginalValues;

	private void Start()
	{
		volume = GetComponent<Volume>();
		if (!(volume != null) || !(volume.sharedProfile != null))
		{
			return;
		}
		volumeProfile = volume.sharedProfile;
		if (volumeProfile.TryGet<ColorAdjustments>(out var component))
		{
			if (!hasSavedOriginalValues)
			{
				originalPostExposure = component.postExposure.value;
				originalContrast = component.contrast.value;
				originalColorFilter = component.colorFilter.value;
				originalHueShift = component.hueShift.value;
				originalSaturation = component.saturation.value;
				hasSavedOriginalValues = true;
			}
			PostExposure = component.postExposure.value;
			Contrast = component.contrast.value;
			ColorFilter = component.colorFilter.value;
			HueShift = component.hueShift.value;
			Saturation = component.saturation.value;
		}
	}

	private void Update()
	{
		ColorAdjustments component2;
		if (volumeProfile != null)
		{
			if (volumeProfile.TryGet<ColorAdjustments>(out var component))
			{
				component.postExposure.value = PostExposure;
				component.contrast.value = Contrast;
				component.colorFilter.value = ColorFilter;
				component.hueShift.value = HueShift;
				component.saturation.value = Saturation;
			}
		}
		else if (volume != null && volume.sharedProfile != null && volume.sharedProfile.TryGet<ColorAdjustments>(out component2))
		{
			component2.postExposure.value = PostExposure;
			component2.contrast.value = Contrast;
			component2.colorFilter.value = ColorFilter;
			component2.hueShift.value = HueShift;
			component2.saturation.value = Saturation;
		}
	}

	private void OnDisable()
	{
		RestoreOriginalValues();
	}

	private void OnDestroy()
	{
		RestoreOriginalValues();
	}

	private void RestoreOriginalValues()
	{
		if (!hasSavedOriginalValues)
		{
			return;
		}
		ColorAdjustments component2;
		if (volumeProfile != null)
		{
			if (volumeProfile.TryGet<ColorAdjustments>(out var component))
			{
				component.postExposure.value = originalPostExposure;
				component.contrast.value = originalContrast;
				component.colorFilter.value = originalColorFilter;
				component.hueShift.value = originalHueShift;
				component.saturation.value = originalSaturation;
			}
		}
		else if (volume != null && volume.sharedProfile != null && volume.sharedProfile.TryGet<ColorAdjustments>(out component2))
		{
			component2.postExposure.value = originalPostExposure;
			component2.contrast.value = originalContrast;
			component2.colorFilter.value = originalColorFilter;
			component2.hueShift.value = originalHueShift;
			component2.saturation.value = originalSaturation;
		}
	}
}
