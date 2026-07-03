// Palette gradient skybox — kills slop tell #1 (default Unity sky) in one
// material. Assign to Lighting > Environment > Skybox Material; set the three
// colors from the game's named palette and match RenderSettings.fogColor to
// horizonColor so world and sky fuse at the horizon (framework/06).
Shader "Friendslop/GradientSkybox"
{
    Properties
    {
        _TopColor    ("Sky Top",    Color) = (0.35, 0.55, 0.85, 1)
        _HorizonColor("Horizon",    Color) = (0.85, 0.80, 0.70, 1)
        _BottomColor ("Below",      Color) = (0.45, 0.42, 0.40, 1)
        _HorizonSharpness ("Horizon Sharpness", Range(0.5, 8)) = 2.5
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            half4 _TopColor, _HorizonColor, _BottomColor;
            half _HorizonSharpness;

            struct Attributes { float4 positionOS : POSITION; };
            struct Varyings  { float4 positionCS : SV_POSITION; float3 viewDir : TEXCOORD0; };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.viewDir = v.positionOS.xyz;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                half y = normalize(i.viewDir).y;
                half up = pow(saturate(y), 1.0 / _HorizonSharpness);
                half down = pow(saturate(-y), 1.0 / _HorizonSharpness);
                half4 col = lerp(_HorizonColor, _TopColor, up);
                col = lerp(col, _BottomColor, down);
                return half4(col.rgb, 1);
            }
            ENDHLSL
        }
    }
}
