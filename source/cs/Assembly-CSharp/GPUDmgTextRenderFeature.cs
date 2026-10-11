using GPUDamageText;
using UnityEngine.Rendering.Universal;

public class GPUDmgTextRenderFeature : ScriptableRendererFeature
{
	private GPUDmgTextRenderPass _customPass;

	public override void Create()
	{
		_customPass = new GPUDmgTextRenderPass();
		_customPass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (DamageNumManager.Instance.GetRenderCount() > 0)
		{
			renderer.EnqueuePass(_customPass);
		}
	}
}
