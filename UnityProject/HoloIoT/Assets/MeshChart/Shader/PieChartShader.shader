Shader "Custom/PieChartShader" {
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
		#pragma surface surf Lambert 

		sampler2D _MainTex;

		struct Input {
			float4 color : COLOR;
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = IN.color.rgb;
			o.Alpha = IN.color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
