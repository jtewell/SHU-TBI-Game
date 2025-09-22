Shader "Custom/ScreenOutline"
{
    Properties
    {
        _EdgeThickness ("Edge Thickness (px)", Float) = 1.0
        _EdgeIntensity ("Edge Intensity",  Float) = 1.0
        _OutlineColor  ("Outline Color",   Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "Queue"="Overlay" }
        ZWrite Off
        ZTest Always
        Cull Off

        Pass
        {
            Name "ScreenOutline"

            HLSLPROGRAM
            // Use Blit.hlsl's full-screen triangle vertex
            #pragma vertex   Vert
            // Give our fragment a unique name to avoid collisions
            #pragma fragment FragOutline

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            // Mask texture produced by the mask render pass
            TEXTURE2D(_OutlineMaskTex);
            SAMPLER(sampler_OutlineMaskTex);

            // Scene color is provided by Blit.hlsl as _BlitTexture
            // sampler_LinearClamp is also provided by Blit.hlsl

            float  _EdgeThickness;
            float  _EdgeIntensity;
            float4 _OutlineColor;

            // Varyings/Attributes come from Blit.hlsl
            half4 FragOutline (Varyings i) : SV_Target
            {
                const float2 uv    = i.texcoord;
                const float2 texel = _ScreenParams.zw * _EdgeThickness; // 1/width, 1/height

                // Sample scene color provided by the blit pass
                const half4 sceneCol = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);

                // Read mask and neighbors (simple 4-neighbor edge)
                const float mC = SAMPLE_TEXTURE2D(_OutlineMaskTex, sampler_OutlineMaskTex, uv).r;
                const float mL = SAMPLE_TEXTURE2D(_OutlineMaskTex, sampler_OutlineMaskTex, uv + float2(-texel.x, 0)).r;
                const float mR = SAMPLE_TEXTURE2D(_OutlineMaskTex, sampler_OutlineMaskTex, uv + float2( texel.x, 0)).r;
                const float mU = SAMPLE_TEXTURE2D(_OutlineMaskTex, sampler_OutlineMaskTex, uv + float2(0, -texel.y)).r;
                const float mD = SAMPLE_TEXTURE2D(_OutlineMaskTex, sampler_OutlineMaskTex, uv + float2(0,  texel.y)).r;

                float edge = (abs(mC - mL) + abs(mC - mR) + abs(mC - mU) + abs(mC - mD));
                edge = saturate(edge * _EdgeIntensity);

                const half3 outCol = lerp(sceneCol.rgb, _OutlineColor.rgb, edge * _OutlineColor.a);
                return half4(outCol, 1);
            }
            ENDHLSL
        }
    }
}
