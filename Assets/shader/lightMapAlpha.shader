﻿Shader "Custom/lightMapAlpha" {
	//rubble
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType" = "Transparent" }
		Pass {
			Name "BASE"
			LOD 200
			Blend SrcAlpha OneMinusSrcAlpha
			zwrite off
			Lighting off
			
			CGPROGRAM
			#pragma vertex vert 
		    #pragma fragment frag
		    #include "UnityCG.cginc"
		        
	        struct VertIn {
	        	float4 vertex : POSITION;
	        	float4 texcoord : TEXCOORD0;
	   
	        };

			
			struct v2f {
	        	fixed4 pos : SV_POSITION;
	        	fixed2 uv : TEXCOORD0;
	        	fixed3 offPos : TEXCOORD2;
	        	
	        };
			uniform sampler2D _MainTex;
			
			uniform sampler2D _LightMap;
		    uniform float4 _CamPos;
		    uniform float _CameraSize;
		    uniform float4 _AmbientCol;
		     
		    uniform sampler2D _LightMask;
		    uniform float _LightCoff;
		    
			v2f vert(VertIn v) 
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				
				o.offPos = mul(_Object2World, v.vertex).xyz-(_WorldSpaceCameraPos+_CamPos);
				return o;
			}
			
			fixed4 frag(v2f i) : Color {
	        	fixed4 col =  tex2D(_MainTex, i.uv);
	        	fixed4 retCol;
	        	//col.rgb = tex.rgb;
	        	//retCol.rgb = col.rgb*((_AmbientCol).rgb+tex2D(_LightMap, (i.offPos.xz+float2(_CameraSize, _CameraSize))/(2*_CameraSize)).rgb*2 );
	        	fixed2 mapUV = (i.offPos.xz+float2(_CameraSize, _CameraSize))/(2*_CameraSize);
	        	
	        	//retCol.rgb = tex2D(_LightMap, (i.offPos.xz+float2(_CameraSize, _CameraSize))/(2*_CameraSize)).rgb;
	        	retCol.rgb = col.rgb*(_AmbientCol.rgb+tex2D(_LightMap, mapUV).rgb * (1-tex2D(_LightMask, mapUV).a)*_LightCoff	);
	        	
	        	retCol.a = col.a;
				return retCol;
			}	
			

			ENDCG
			
		}
	} 
	FallBack "Diffuse"
}
