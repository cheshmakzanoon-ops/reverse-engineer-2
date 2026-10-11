using System.Runtime.CompilerServices;
using UnityEngine;

namespace RiverGame.Rendering.MaterialPropertyBlockUtilities;

public class IntMaterialProperty : MaterialProperty<int>
{
	public IntMaterialProperty(string propertyName, int value)
		: base(propertyName, value)
	{
	}

	public IntMaterialProperty(int propertyNameId, int propertyInstancingNameId, int value)
		: base(propertyNameId, propertyInstancingNameId, value)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void ApplyToMaterialPropertyBlock(MaterialPropertyBlock mpb, bool useInstancingName = true)
	{
		mpb.SetInt(useInstancingName ? base.propertyInstancingNameId : base.propertyNameId, value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void ApplyToMaterial(Material material)
	{
		material.SetInt(base.propertyNameId, value);
	}
}
