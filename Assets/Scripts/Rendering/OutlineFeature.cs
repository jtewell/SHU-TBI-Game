using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class OutlineFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class OutlineSettings
    {
        public Material outlineMaterial;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    public OutlineSettings settings = new OutlineSettings();
    private OutlinePass _pass;

    public override void Create() => _pass = new OutlinePass();

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.outlineMaterial == null || _pass == null)
            return;

        _pass.Setup(settings.outlineMaterial, settings.renderPassEvent);
        renderer.EnqueuePass(_pass);
    }

    private sealed class OutlinePass : ScriptableRenderPass
    {
        private static readonly int MaskId = Shader.PropertyToID("_OutlineMaskTex");
        private Material _material;

        private sealed class PassData
        {
            public TextureHandle source;
            public Material material;
        }

        public OutlinePass()
        {
            // A fullscreen effect must sample a texture, not the screen backbuffer.
            requiresIntermediateTexture = true;
        }

        public void Setup(Material material, RenderPassEvent passEvent)
        {
            _material = material;
            renderPassEvent = passEvent;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var cameraData = frameData.Get<UniversalCameraData>();
            if (_material == null || !cameraData.resolveFinalTarget)
                return;

            var resources = frameData.Get<UniversalResourceData>();
            if (resources.isActiveTargetBackBuffer)
                return;

            var maskData = frameData.GetOrCreate<OutlineMaskFrameData>();
            if (!maskData.texture.IsValid())
                return;

            var source = resources.activeColorTexture;
            if (!source.IsValid())
                return;

            var descriptor = renderGraph.GetTextureDesc(source);
            descriptor.name = "_OutlineCompositeColor";
            descriptor.depthBufferBits = DepthBits.None;
            descriptor.msaaSamples = MSAASamples.None;
            descriptor.bindTextureMS = false;
            descriptor.memoryless = RenderTextureMemoryless.None;
            descriptor.clearBuffer = false;
            var destination = renderGraph.CreateTexture(descriptor);

            using (var builder = renderGraph.AddRasterRenderPass<PassData>("Screen Outline", out var passData))
            {
                passData.source = source;
                passData.material = _material;
                builder.UseTexture(source, AccessFlags.Read);
                // Declare the dependency on the global published by OutlineMaskFeature.
                builder.UseGlobalTexture(MaskId, AccessFlags.Read);
                builder.SetRenderAttachment(destination, 0, AccessFlags.Write);
                builder.SetRenderFunc(static (PassData data, RasterGraphContext context) =>
                {
                    // Same material inputs as the original: _BlitTexture and _OutlineMaskTex.
                    Blitter.BlitTexture(context.cmd, data.source,
                        new Vector4(1f, 1f, 0f, 0f), data.material, 0);
                });
            }

            // Subsequent URP passes use the composited image. No in-place read/write blit.
            resources.cameraColor = destination;
        }
    }
}
