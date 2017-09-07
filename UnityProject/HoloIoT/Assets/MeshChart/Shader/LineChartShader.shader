Shader "Custom/LineChartShader" {
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
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			float bl = 1.5 * (1.0 - abs((IN.uv_MainTex.y - 0.5) * 2.0)); 
			o.Albedo = IN.color.rgb + float3(1.0, 1.0, 1.0) * max(bl-1.0, 0.0);//float3(0.0, 1.0, 0.0);
			o.Alpha = min(bl, 1.0) * IN.color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
