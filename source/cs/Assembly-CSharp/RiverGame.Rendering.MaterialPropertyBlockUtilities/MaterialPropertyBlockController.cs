using System.Collections.Generic;
using UnityEngine;

namespace RiverGame.Rendering.MaterialPropertyBlockUtilities;

public class MaterialPropertyBlockController
{
	private ulong tag;

	private static Dictionary<ulong, MaterialPropertyBlock> Tag2MPB = new Dictionary<ulong, MaterialPropertyBlock>();

	private List<MaterialPropertyGroup> materialPropertyGroups { get; } = new List<MaterialPropertyGroup>();

	public bool Valid => tag != 0;

	public MaterialPropertyBlock Add(MaterialPropertyGroup properties, bool useInstancingName = true)
	{
		if (properties == null)
		{
			return null;
		}
		if (!materialPropertyGroups.Contains(properties))
		{
			materialPropertyGroups.Add(properties);
		}
		tag |= (ulong)(1L << properties.id);
		return RefreshMaterialPropertyBlock(useInstancingName);
	}

	public MaterialPropertyBlock Remove(MaterialPropertyGroup properties, bool useInstancingName = true)
	{
		materialPropertyGroups.Remove(properties);
		tag = 0uL;
		foreach (MaterialPropertyGroup materialPropertyGroup in materialPropertyGroups)
		{
			tag |= (ulong)(1L << materialPropertyGroup.id);
		}
		return RefreshMaterialPropertyBlock(useInstancingName);
	}

	public MaterialPropertyBlock Clear(bool useInstancingName = true)
	{
		materialPropertyGroups.Clear();
		tag = 0uL;
		return RefreshMaterialPropertyBlock(useInstancingName);
	}

	public MaterialPropertyBlock RefreshMaterialPropertyBlock(bool useInstancingName = true)
	{
		if (!Tag2MPB.TryGetValue(tag, out var value))
		{
			value = new MaterialPropertyBlock();
			Tag2MPB.Add(tag, value);
		}
		value.Clear();
		foreach (MaterialPropertyGroup materialPropertyGroup in materialPropertyGroups)
		{
			materialPropertyGroup.ApplyToMaterialPropertyBlock(value, useInstancingName);
		}
		return value;
	}
}
