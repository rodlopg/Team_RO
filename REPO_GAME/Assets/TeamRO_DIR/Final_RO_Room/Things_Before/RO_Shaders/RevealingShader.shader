//Shady
Shader "Revealing Under Light"
{
	Properties
	{
		MyColor("Color", Color) = (1,1,1,1)
		MyMainTex("Albedo (RGB)", 2D) = "white" {}
		MyGlossiness("Smoothness", Range(0,1)) = 0.5
		MyMetallic("Metallic", Range(0,1)) = 0.0
		MyLightDirection("Light Direction", Vector) = (0,0,1,0)
		MyLightPosition("Light Position", Vector) = (0,0,0,0)
		MyLightAngle("Light Angle", Range(0,180)) = 45
		MyStrengthScalor("Strength", Float) = 50
	}//Properties end
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		CGPROGRAM
#pragma surface SurfaceReveal Standard fullforwardshadows alpha:fade
#pragma target 3.0
		sampler2D MyMainTex;

		struct Input
		{
			float2 UVMainTex;
			float3 worldPos;
		};//Struct end

		half   MyGlossiness;
		half   MyMetallic;
		fixed4 MyColor;
		float4 MyLightPosition;
		float4 MyLightDirection;
		float  MyLightAngle;
		float  MyStrengthScalor;

		void SurfaceReveal(Input input, inout SurfaceOutputStandard R)
		{ 
			float3 Dir      = normalize(MyLightPosition - input.worldPos);
			float  Scale    = dot(Dir, MyLightDirection);
			float  Strength = Scale - cos(MyLightAngle * (3.14 / 360.0));
			Strength        = min(max(Strength * MyStrengthScalor, 0), 1);
			fixed4 RC       = tex2D(MyMainTex, input.UVMainTex) * MyColor;
			R.Albedo        = RC.rgb;
			R.Emission      = RC.rgb * RC.a * Strength;
			R.Metallic      = MyMetallic;
			R.Smoothness    = MyGlossiness;
			R.Alpha         = Strength * RC.a;
		}//SurfaceReveal end
	ENDCG
	}//SubShader end
		FallBack "Diffuse"
}//Shader end