using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class OutlineSettings
    {
        public Material outlineMaterial;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    class OutlinePass : ScriptableRenderPass
    {
        private readonly Material _material;
        private RTHandle _tempColor;

        public OutlinePass(Material material, RenderPassEvent passEvent)
        {
            _material = material;
            renderPassEvent = passEvent;

            // Just in case the mask feature didn’t set a texture yet
            Shader.SetGlobalTexture("_OutlineMaskTex", Texture2D.blackTexture);
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Allocate a temp color RT matching the camera descriptor
            var desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0; // color only
            RenderingUtils.ReAllocateIfNeeded(ref _tempColor, desc, name: "_OutlineTempColor");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            // Only run on the final resolve of the base camera. This avoids null targets during stacks/overlays.
            if (!renderingData.cameraData.resolveFinalTarget)
                return;

            if (_material == null)
                return;

            // Get the camera color target
            var renderer = renderingData.cameraData.renderer;
            var src = renderer.cameraColorTargetHandle;

            // If the source isn't valid, skip (prevents Blitter from binding a null texture)
            if (src == null || src.rt == null)
                return;

            var cmd = CommandBufferPool.Get("Screen Outline");

            // 1) Copy camera color -> temp (also binds it internally for blit)
            Blitter.BlitCameraTexture(cmd, src, _tempColor);

            // 2) Run the outline material, sampling _BlitTexture (the temp) and writing back to camera color
            Blitter.BlitCameraTexture(cmd, _tempColor, src, _material, 0);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd) { }

        public void Dispose()
        {
            _tempColor?.Release();
        }
    }

    public OutlineSettings settings = new OutlineSettings();
    private OutlinePass _pass;

    public override void Create()
    {
        _pass = new OutlinePass(settings.outlineMaterial, settings.renderPassEvent);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.outlineMaterial == null)
            return;

        renderer.EnqueuePass(_pass);
    }

    protected override void Dispose(bool disposing)
    {
        _pass?.Dispose();
    }
}
