Shader "Custom/UV_TMP_SDF"
{
    Properties
    {
        _MainTex ("Font Atlas (SDF)", 2D) = "white" {}
        _FaceColor ("Face Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.05
        _Softness ("Softness", Range(0, 0.1)) = 0.01
        _ClipRect ("Clip Rect", Vector) = (-32767, -32767, 32767, 32767)

        // UV Reveal Props
        _LightDir("Light Direction", Vector) = (0,0,1,0)
        _LightPos("Light Position", Vector) = (0,0,0,0)
        _LightAngle("Light Angle", Range(0,180)) = 45
        _StrengthScale("Strength", Float) = 50
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _FaceColor;
            fixed4 _OutlineColor;
            float _OutlineWidth;
            float _Softness;
            float4 _ClipRect;

            // UV Reveal Props
            float4 _LightDir;
            float4 _LightPos;
            float _LightAngle;
            float _StrengthScale;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Clipping
                if (i.worldPos.x < _ClipRect.x || i.worldPos.y < _ClipRect.y || 
                    i.worldPos.x > _ClipRect.z || i.worldPos.y > _ClipRect.w)
                    discard;

                float distance = tex2D(_MainTex, i.uv).a;
                float width = fwidth(distance);
                float outlineEdge = _OutlineWidth + _Softness;

                float alpha = smoothstep(0.5 - _Softness - _OutlineWidth, 0.5 - _Softness, distance);
                float outline = smoothstep(0.5 - outlineEdge - width, 0.5 - outlineEdge, distance);

                fixed3 finalColor = lerp(_OutlineColor.rgb, _FaceColor.rgb, alpha);
                float finalAlpha = lerp(_OutlineColor.a, _FaceColor.a, alpha) * max(outline, alpha);

                // --- UV Reveal Logic ---
                float3 L = normalize(_LightPos.xyz - i.worldPos);
                float3 D = normalize(_LightDir.xyz);

                float outerAngleRad = radians(_LightAngle * 0.5);
                float innerAngleRad = outerAngleRad * 0.8;

                float cosTheta = dot(L, D);
                float outerCos = cos(outerAngleRad);
                float innerCos = cos(innerAngleRad);
                float strength = saturate((cosTheta - outerCos) / (innerCos - outerCos));

                finalAlpha *= strength;

                return fixed4(finalColor, finalAlpha);
            }
            ENDCG
        }
    }
}
