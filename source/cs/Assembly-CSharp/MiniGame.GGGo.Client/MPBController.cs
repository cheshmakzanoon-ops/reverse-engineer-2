using UnityEngine;

namespace MiniGame.GGGo.Client;

public class MPBController
{
	private MaterialPropertyBlock _mpb;

	private Renderer _renderer;

	private static readonly int ColorID = Shader.PropertyToID("_MainTexColor");

	private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");

	private bool _hasEmission;

	public bool? ForceEmission { get; set; }

	public MPBController(Renderer renderer)
	{
		_renderer = renderer;
		_mpb = new MaterialPropertyBlock();
		Material sharedMaterial = renderer.sharedMaterial;
		if (sharedMaterial != null)
		{
			_hasEmission = sharedMaterial.IsKeywordEnabled("_EMISSION") || sharedMaterial.HasProperty("_EmissionColor");
		}
	}

	public void SetColor(Color color)
	{
		_renderer.GetPropertyBlock(_mpb);
		if (ForceEmission.HasValue ? ForceEmission.Value : _hasEmission)
		{
			_mpb.SetColor(EmissionColorID, color);
		}
		else
		{
			_mpb.SetColor(ColorID, color);
		}
		_renderer.SetPropertyBlock(_mpb);
	}

	public void SetBothColors(Color color)
	{
		_renderer.GetPropertyBlock(_mpb);
		_mpb.SetColor(ColorID, color);
		_mpb.SetColor(EmissionColorID, color);
		_renderer.SetPropertyBlock(_mpb);
	}

	public void Clear()
	{
		_mpb.Clear();
		_renderer.SetPropertyBlock(_mpb);
	}
}
