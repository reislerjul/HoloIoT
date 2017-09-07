Shader "Custom/BarChartShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
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
			o.Albedo = IN.color.rgb * max(0.0, (0.4 - abs(IN.uv_MainTex.x - 0.5)) * 5.0);//float3(0.0, 1.0, 0.0);
			o.Alpha = IN.color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
