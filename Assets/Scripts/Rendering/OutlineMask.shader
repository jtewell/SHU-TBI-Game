Shader "Hidden/OutlineMask"
{
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" }
        Pass
        {
            ZWrite Off 
            ZTest LEqual 
            Cull Back
            Blend One Zero
            ColorMask R
            Offset -1, -1

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes { float4 positionOS : POSITION; };
            struct Varyings  { float4 positionHCS: SV_POSITION; };

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                return o;
            }

            half4 frag () : SV_Target
            {
                // White = outlined, Black = background (RenderPass clears to black)
                return half4(1,1,1,1);
            }
            ENDHLSL
        }
    }
}
