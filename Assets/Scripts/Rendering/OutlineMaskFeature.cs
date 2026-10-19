using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

// Per-camera, per-frame data. Never retain a TextureHandle between frames.
public sealed class OutlineMaskFrameData : ContextItem
{
    public TextureHandle texture;
    public override void Reset() => texture = TextureHandle.nullHandle;
}

public class OutlineMaskFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public Material maskMaterial;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    public Settings settings = new Settings();
    private MaskPass _pass;
    private readonly List<Renderer> _collector = new List<Renderer>();
    private readonly HashSet<Renderer> _seen = new HashSet<Renderer>();

    public override void Create() => _pass = new MaskPass();

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.maskMaterial == null || _pass == null)
            return;

        _collector.Clear();
        _seen.Clear();
        var markers = Object.FindObjectsByType<Interactable>(
            FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (var marker in markers)
        {
            if (marker == null || !marker.isActiveAndEnabled || !marker.outlineEnabled)
                continue;

            foreach (var child in marker.GetComponentsInChildren<Renderer>(true))
            {
                if (child != null && child.enabled && child.gameObject.activeInHierarchy && _seen.Add(child))
                    _collector.Add(child);
            }
        }

        _pass.Setup(settings.maskMaterial, settings.renderPassEvent, _collector);
        renderer.EnqueuePass(_pass);
    }

    private sealed class MaskPass : ScriptableRenderPass
    {
        private static readonly int MaskId = Shader.PropertyToID("_OutlineMaskTex");
        private Material _material;
        private Renderer[] _renderers;

        private sealed class PassData
        {
            public Material material;
            public Renderer[] renderers;
        }

        public void Setup(Material material, RenderPassEvent passEvent, List<Renderer> renderers)
        {
            _material = material;
            renderPassEvent = passEvent;
            // Snapshot: recording and execution are separate in Render Graph.
            _renderers = renderers.ToArray();
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if (_material == null)
                return;

            var resources = frameData.Get<UniversalResourceData>();
            var depth = resources.activeDepthTexture;
            if (!depth.IsValid())
                return;

            // Match the actual depth attachment, including dimensions, XR slices
            // and MSAA count. Color and depth attachments must have matching samples.
            var descriptor = renderGraph.GetTextureDesc(depth);
            descriptor.name = "_OutlineMaskRT";
            descriptor.colorFormat = GraphicsFormat.R8_UNorm;
            descriptor.depthBufferBits = DepthBits.None;
            descriptor.bindTextureMS = false;
            descriptor.memoryless = RenderTextureMemoryless.None;
            descriptor.isShadowMap = false;
            descriptor.enableRandomWrite = false;
            descriptor.clearBuffer = true;
            descriptor.clearColor = Color.black;
            descriptor.filterMode = FilterMode.Point;
            var mask = renderGraph.CreateTexture(descriptor);

            frameData.GetOrCreate<OutlineMaskFrameData>().texture = mask;

            using (var builder = renderGraph.AddRasterRenderPass<PassData>("Outline Mask", out var passData))
            {
                passData.material = _material;
                passData.renderers = _renderers;
                builder.SetRenderAttachment(mask, 0, AccessFlags.Write);
                builder.SetRenderAttachmentDepth(depth, AccessFlags.Read);
                builder.SetGlobalTextureAfterPass(mask, MaskId);

                builder.SetRenderFunc(static (PassData data, RasterGraphContext context) =>
                {
                    // An empty selection still clears and publishes a black mask.
                    foreach (var item in data.renderers)
                    {
                        if (item == null || !item.enabled || !item.gameObject.activeInHierarchy)
                            continue;

                        int submeshCount = 0;
                        if (item is SkinnedMeshRenderer skinned && skinned.sharedMesh != null)
                            submeshCount = skinned.sharedMesh.subMeshCount;
                        else if (item is MeshRenderer && item.TryGetComponent<MeshFilter>(out var filter)
                                 && filter.sharedMesh != null)
                            submeshCount = filter.sharedMesh.subMeshCount;
                        else
                            submeshCount = item.sharedMaterials.Length;

                        for (int submesh = 0; submesh < submeshCount; submesh++)
                            context.cmd.DrawRenderer(item, data.material, submesh, 0);
                    }
                });
            }
        }
    }
}
