using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ClearAlphaRendererFeature : ScriptableRendererFeature
{
    class ClearAlphaPass : ScriptableRenderPass
    {
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("ClearAlpha");
            cmd.SetRenderTarget(renderingData.cameraData.renderer.cameraColorTargetHandle);
            cmd.ClearRenderTarget(false, true, new Color(0, 0, 0, 0));
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    ClearAlphaPass pass;

    public override void Create()
    {
        pass = new ClearAlphaPass();
        pass.renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }
}