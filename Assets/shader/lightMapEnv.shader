Shader "Custom/lightMapEnv" {
	//floor blank
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
	
		Pass {	
			LOD 200
			Lighting Off
			
			
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
	   			fixed3 worldPos : TEXCOORD3;
	        };

			uniform sampler2D _MainTex;
			uniform sampler2D _LightMap;
			uniform sampler2D _SpecMap;
			uniform float _SpecCoff;
			uniform float _SpecSize;

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
				o.worldPos = mul(_Object2World, v.vertex).xyz;
				return o;
			}
			
			fixed4 frag(v2f i) : Color {
	        	fixed4 col =  tex2D(_MainTex, i.uv);
	        	fixed4 retCol;
				
				fixed2 mapUV = (i.offPos.xz+float2(_CameraSize, _CameraSize))/(2*_CameraSize);
				
				retCol.rgb = col.rgb*(_AmbientCol.rgb+tex2D(_LightMap, mapUV).rgb * (1-tex2D(_LightMask, mapUV).a)*_LightCoff );
				retCol.a = col.a;

				fixed2 specUV = i.worldPos.xz * fixed2(_SpecSize, _SpecSize);
				fixed4 specColor = tex2D(_SpecMap, specUV);
				retCol.rgb += specColor.rgb*_SpecCoff;
				return retCol;
			}	

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
