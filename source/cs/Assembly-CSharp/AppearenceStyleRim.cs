using RiverGame.Rendering.MaterialPropertyBlockUtilities;
using UnityEngine;

public class AppearenceStyleRim
{
	public class MaterialPropertyGroupRimRed : MaterialPropertyGroup
	{
		private static int ID = MaterialPropertyGroup.IDAllocator++;

		public override int id => ID;

		public MaterialPropertyGroupRimRed()
		{
			properties = new IMaterialProperty[1]
			{
				new FloatMaterialProperty("_OnHit", 1f)
			};
		}

		public void UpdateMaterialProperty(Color value)
		{
		}
	}

	public class MaterialPropertyGroupRimGold : MaterialPropertyGroup
	{
		private static int ID = MaterialPropertyGroup.IDAllocator++;

		public override int id => ID;

		public MaterialPropertyGroupRimGold()
		{
			Color value = new Color(1f, 0.72156864f, 0f, 0f);
			properties = new IMaterialProperty[4]
			{
				new FloatMaterialProperty("_OnOutlineRim", 1f),
				new FloatMaterialProperty("_OutlineRimPower", 2f),
				new FloatMaterialProperty("_OutlineRimRange", 0.5f),
				new ColorMaterialProperty("_OutlineRimeColor", value)
			};
		}

		public void UpdateMaterialProperty(Color value)
		{
			(properties[3] as ColorMaterialProperty).value = value;
		}
	}

	public class MaterialPropertyGroupRimBorn : MaterialPropertyGroup
	{
		private static int ID = MaterialPropertyGroup.IDAllocator++;

		public override int id => ID;

		public MaterialPropertyGroupRimBorn()
		{
			float p = 0f;
			p = Mathf.Pow(2f, p);
			Color value = new Color(27f / 85f * p, 0.6745098f * p, 1f * p, 0f);
			properties = new IMaterialProperty[4]
			{
				new FloatMaterialProperty("_OnOutlineRim", 1f),
				new FloatMaterialProperty("_OutlineRimPower", 4f),
				new FloatMaterialProperty("_OutlineRimRange", 1.5f),
				new ColorMaterialProperty("_OutlineRimeColor", value)
			};
		}

		public void UpdateMaterialProperty(Color value)
		{
			(properties[3] as ColorMaterialProperty).value = value;
		}
	}

	public class MaterialPropertyGroupRimModelScale : MaterialPropertyGroup
	{
		private static int ID = MaterialPropertyGroup.IDAllocator++;

		public override int id => ID;

		public MaterialPropertyGroupRimModelScale()
		{
			float p = 1.4169f;
			p = Mathf.Pow(2f, p);
			Color value = new Color(0.7490196f * p, 0.24313726f * p, 2f / 51f * p, 0f);
			properties = new IMaterialProperty[4]
			{
				new FloatMaterialProperty("_OnOutlineRim", 1f),
				new FloatMaterialProperty("_OutlineRimPower", 2f),
				new FloatMaterialProperty("_OutlineRimRange", 3f),
				new ColorMaterialProperty("_OutlineRimeColor", value)
			};
		}

		public void UpdateMaterialProperty(Color value)
		{
			(properties[3] as ColorMaterialProperty).value = value;
		}
	}

	public class MaterialPropertyGroupRimShield : MaterialPropertyGroup
	{
		private static int ID = MaterialPropertyGroup.IDAllocator++;

		public override int id => ID;

		public MaterialPropertyGroupRimShield()
		{
			float p = 2.0299f;
			p = Mathf.Pow(2f, p);
			Color value = new Color(0f * p, 0.12156863f * p, 0.7490196f * p, 0f);
			properties = new IMaterialProperty[4]
			{
				new FloatMaterialProperty("_OnOutlineRim", 1f),
				new FloatMaterialProperty("_OutlineRimPower", 5f),
				new FloatMaterialProperty("_OutlineRimRange", 2.97f),
				new ColorMaterialProperty("_OutlineRimeColor", value)
			};
		}

		public void UpdateMaterialProperty(Color value)
		{
			(properties[3] as ColorMaterialProperty).value = value;
		}
	}

	public class MaterialPropertyGroupGray : MaterialPropertyGroup
	{
		private static int ID = MaterialPropertyGroup.IDAllocator++;

		public override int id => ID;

		public MaterialPropertyGroupGray()
		{
			properties = new IMaterialProperty[1]
			{
				new FloatMaterialProperty("_OnGray", 1f)
			};
		}

		public void UpdateMaterialProperty(Color value)
		{
		}
	}

	public class MaterialPropertyGroupGlassCrack : MaterialPropertyGroup
	{
		private static int ID = MaterialPropertyGroup.IDAllocator++;

		public override int id => ID;

		public MaterialPropertyGroupGlassCrack()
		{
			properties = new IMaterialProperty[1]
			{
				new FloatMaterialProperty("_SetFrame", 0f)
			};
		}

		public void UpdateMaterialProperty(float value)
		{
			(properties[0] as FloatMaterialProperty).value = value;
		}
	}

	public class MaterialPropertyGroupWaveIntensity : MaterialPropertyGroup
	{
		private static int ID = MaterialPropertyGroup.IDAllocator++;

		public override int id => ID;

		public MaterialPropertyGroupWaveIntensity()
		{
			properties = new IMaterialProperty[1]
			{
				new FloatMaterialProperty("_WaveIntensity", 0f)
			};
		}

		public void UpdateMaterialProperty(float value)
		{
			(properties[0] as FloatMaterialProperty).value = value;
		}
	}

	public class MaterialPropertyGroupFrozen : MaterialPropertyGroup
	{
		private static int ID = MaterialPropertyGroup.IDAllocator++;

		public override int id => ID;

		public MaterialPropertyGroupFrozen()
		{
			properties = new IMaterialProperty[1]
			{
				new FloatMaterialProperty("_OnFrozen", 1f)
			};
		}

		public void UpdateMaterialProperty(int value)
		{
			(properties[0] as FloatMaterialProperty).value = value;
		}
	}

	private static MaterialPropertyGroupRimRed s_Red;

	private static MaterialPropertyGroupRimGold s_Gold;

	private static MaterialPropertyGroupRimBorn s_Born;

	private static MaterialPropertyGroupRimModelScale s_ModelScale;

	private static MaterialPropertyGroupRimShield s_Shield;

	private static MaterialPropertyGroupGray s_Gray;

	private static MaterialPropertyGroupGlassCrack s_GlassCrack;

	private static MaterialPropertyGroupWaveIntensity s_WaveIntensity;

	private static MaterialPropertyGroupFrozen s_Frozen;

	public static MaterialPropertyGroupRimRed Red => s_Red ?? (s_Red = new MaterialPropertyGroupRimRed());

	public static MaterialPropertyGroupRimGold Gold => s_Gold ?? (s_Gold = new MaterialPropertyGroupRimGold());

	public static MaterialPropertyGroupRimBorn Born => s_Born ?? (s_Born = new MaterialPropertyGroupRimBorn());

	public static MaterialPropertyGroupRimModelScale ModelScale => s_ModelScale ?? (s_ModelScale = new MaterialPropertyGroupRimModelScale());

	public static MaterialPropertyGroupRimShield Shield => s_Shield ?? (s_Shield = new MaterialPropertyGroupRimShield());

	public static MaterialPropertyGroupGray Gray => s_Gray ?? (s_Gray = new MaterialPropertyGroupGray());

	public static MaterialPropertyGroupGlassCrack GlassCrack => s_GlassCrack ?? (s_GlassCrack = new MaterialPropertyGroupGlassCrack());

	public static MaterialPropertyGroupWaveIntensity WaveIntensity => s_WaveIntensity ?? (s_WaveIntensity = new MaterialPropertyGroupWaveIntensity());

	public static MaterialPropertyGroupFrozen Frozen => s_Frozen ?? (s_Frozen = new MaterialPropertyGroupFrozen());

	public static void Dispose()
	{
		s_Red = null;
		s_Gold = null;
		s_Born = null;
		s_ModelScale = null;
	}
}
