using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;

public class OutlineMaskFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        // Assign a material that uses the "Hidden/OutlineMask" shader
        public Material maskMaterial;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    class MaskPass : ScriptableRenderPass
    {
        private readonly Material _material;
        private RTHandle _maskRT;

        // Renderers we will explicitly draw (only those under OutlineMarker objects)
        private readonly List<Renderer> _renderers = new List<Renderer>();

        private static readonly int OutlineMaskTexID = Shader.PropertyToID("_OutlineMaskTex");

        public MaskPass(Material mat, RenderPassEvent passEvent)
        {
            _material = mat;
            renderPassEvent = passEvent;

            // Safe default for the first frame
            Shader.SetGlobalTexture(OutlineMaskTexID, Texture2D.blackTexture);
        }

        public void SetRenderers(List<Renderer> renderers)
        {
            _renderers.Clear();
            if (renderers != null) _renderers.AddRange(renderers);
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Create a single-channel color RT that matches the camera
            var desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0;

            // Unity versions differ on which field to use
#if UNITY_2021_2_OR_NEWER
            desc.graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R8_UNorm;
#else
            desc.colorFormat = RenderTextureFormat.R8;
#endif

            RenderingUtils.ReAllocateIfNeeded(ref _maskRT, desc, name: "_OutlineMaskRT");

            // Bind color target (mask) and the camera depth target so ZTest works
            var renderer = renderingData.cameraData.renderer;
            ConfigureTarget(_maskRT, renderer.cameraDepthTargetHandle);

            // Clear only color; keep the current camera depth
            ConfigureClear(ClearFlag.Color, Color.black);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_material == null) return;
            if (_renderers.Count == 0) return;

            var cmd = CommandBufferPool.Get("Outline Mask");

            // Draw each renderer with the mask material and depth test (material uses ZTest LEqual)
            for (int r = 0; r < _renderers.Count; r++)
            {
                var rend = _renderers[r];
                if (rend == null || !rend.enabled) continue;

                // Draw all submeshes
                int submeshCount = 1;
                var mf = rend as MeshRenderer;
                if (mf != null && rend.sharedMaterials != null)
                    submeshCount = rend.sharedMaterials.Length;

                for (int sm = 0; sm < submeshCount; sm++)
                {
                    cmd.DrawRenderer(rend, _material, sm, 0);
                }
            }

            // Expose the mask texture for the composite pass
            cmd.SetGlobalTexture(OutlineMaskTexID, _maskRT);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd) { }

        public void Dispose()
        {
            if (_maskRT != null) _maskRT.Release();
            _maskRT = null;
        }
    }

    public Settings settings = new Settings();
    private MaskPass _pass;
    private readonly List<Renderer> _collector = new List<Renderer>();

    public override void Create()
    {
        _pass = new MaskPass(settings.maskMaterial, settings.renderPassEvent);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.maskMaterial == null) return;

        // Collect all visible/scene renderers under OutlineMarker objects
        _collector.Clear();
        var markers = GameObject.FindObjectsOfType<Interactable>(true);
        for (int i = 0; i < markers.Length; i++)
        {
            var m = markers[i];
            if (m == null) continue;
            if (!m.isActiveAndEnabled) continue;      // component disabled or object inactive
            if (!m.outlineEnabled) continue;          // honor the toggle

            // Gather all child renderers
            var rends = m.GetComponentsInChildren<Renderer>(true);
            for (int r = 0; r < rends.Length; r++)
            {
                var rend = rends[r];
                if (rend != null) _collector.Add(rend);
            }
        }

        _pass.SetRenderers(_collector);
        renderer.EnqueuePass(_pass);
    }

    protected override void Dispose(bool disposing)
    {
        if (_pass != null) _pass.Dispose();
    }
}
