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
	        	//float4 color : COLOR;
	        	
	        };

			
			struct v2f {
	        	fixed4 pos : SV_POSITION;
	        	fixed2 uv : TEXCOORD0;
	   			//fixed4 vertColor : TEXCOORD1;
	   			fixed3 offPos : TEXCOORD2;
	        };
			uniform sampler2D _MainTex;
			//uniform fixed4 _Color;
			
			uniform sampler2D _LightMap;
		    uniform float4 _CamPos;
		    uniform float _CameraSize;
		    //uniform float _CameraSizeX;
		    
		    uniform float4 _AmbientCol;
			//uniform fixed _LightCoff;
			
			uniform sampler2D _LightMask;
			uniform float _LightCoff;
			
			v2f vert(VertIn v) 
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				
				//o.vertColor = v.color; 
				o.offPos = mul(_Object2World, v.vertex).xyz-(_WorldSpaceCameraPos+_CamPos);
				return o;
			}
			
			fixed4 frag(v2f i) : Color {
	        	
	        	fixed4 col =  tex2D(_MainTex, i.uv);
	        	fixed4 retCol;
				
				fixed2 mapUV = (i.offPos.xz+float2(_CameraSize, _CameraSize))/(2*_CameraSize);
				
				retCol.rgb = col.rgb*(_AmbientCol.rgb+tex2D(_LightMap, mapUV).rgb * (1-tex2D(_LightMask, mapUV).a)*_LightCoff );
				retCol.a = col.a;
				return retCol;
			}	

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
