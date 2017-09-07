Shader "Custom/CobwebChartSlimShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Blend SrcAlpha OneMinusSrcAlpha
        Tags {
            "Queue" = "Transparent"
            "RenderType" = "Opaque"
        }
		LOD 200
		
		Cull Off
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;

		struct Input {
			float4 color : COLOR;
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = IN.color.rgb * min((1.0 - IN.uv_MainTex.y) * 2.5 + 0.5, 1.0);
			o.Alpha = IN.color.a * IN.uv_MainTex.y;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
