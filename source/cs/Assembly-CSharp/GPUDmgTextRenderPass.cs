using GPUDamageText;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GPUDmgTextRenderPass : ScriptableRenderPass
{
	public GPUDmgTextRenderPass()
	{
		base.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		ref CameraData cameraData = ref renderingData.cameraData;
		if (cameraData.camera.cameraType == CameraType.Game && cameraData.renderType == CameraRenderType.Base && DamageNumManager.Instance.GetRenderCount() > 0)
		{
			CommandBuffer commandBuffer = CommandBufferPool.Get();
			commandBuffer.name = "GPUDmgTextRenderPass";
			commandBuffer.DrawMeshInstanced(DamageNumManager.Instance.GetRenderMesh(), 0, DamageNumManager.Instance.GetRenderMaterial(), 0, DamageNumManager.Instance.GetRenderLocalToWorlds(), Mathf.Min(DamageNumManager.Instance.GetRenderCount(), 1023), DamageNumManager.Instance.GetRenderMaterialPropertyBlock());
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}
	}
}
