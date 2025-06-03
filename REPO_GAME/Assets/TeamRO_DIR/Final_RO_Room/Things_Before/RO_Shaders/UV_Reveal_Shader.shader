Shader "Custom/UV_Reveal_Shader"
{
    Properties
    {
        _MyColor("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Smoothness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _LightDir("Light Direction", Vector) = (0,0,1,0)
        _LightPos("Light Position", Vector) = (0,0,0,0)
        _LightAngle("Light Angle", Range(0,180)) = 45
        _StrengthScale("Strength", Float) = 50
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MyColor;
            float _Smoothness;
            float _Metallic;
            float4 _LightDir;
            float4 _LightPos;
            float _LightAngle;
            float _StrengthScale;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 lightDir = normalize(_LightPos.xyz - IN.worldPos);
                // Convert angles from degrees to radians
                float outerAngleRad = radians(_LightAngle * 0.5); // spotAngle is full cone, we need half-angle
                float innerAngleRad = outerAngleRad * 0.8;        // 80% of outer for smoother falloff

                float3 L = normalize(_LightPos.xyz - IN.worldPos);
                float3 D = normalize(_LightDir.xyz);

                // Compute the angle between light direction and point
                float cosTheta = dot(L, D);

                // Thresholds
                float outerCos = cos(outerAngleRad);
                float innerCos = cos(innerAngleRad);

                // Smooth step between inner and outer angle
                float strength = saturate((cosTheta - outerCos) / (innerCos - outerCos));


                float4 baseColor = tex2D(_MainTex, IN.uv) * _MyColor;
                float3 finalColor = baseColor.rgb * strength;
                return float4(finalColor, baseColor.a * strength);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
