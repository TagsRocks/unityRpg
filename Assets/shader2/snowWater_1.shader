Shader "torch2/snowWater_5" {

	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Freq_X("wave x freq", float) = 0.07
		_Amplitude_X("wave x amplitude", float) = -0.2

		_Freq_Y("wave  freq", float) = 0.06
		_Amplitude_Y("wave  amplitude", float) = -0.24
		


		_RippleTex ("Base (RGB)", 2D) = "white" {}
		_UVAnimX("UV Anim X", float) = 0
		_UVAnimY("UV Anim Y", float) = 0
		_rotate("rotate", float) = 45


		_UVAnimX1("UV Anim X1", float) = 0
		_UVAnimY1("UV Anim Y1", float) = 0

		_EnvSpec("EnvMap", 2D) = ""
	}

	SubShader {
		Tags { "Queue"="Transparent+5" 
				"IgnoreProjector"="True"
				"RenderType"="Transparent" }
		Pass {
			
			LOD 200
			Blend SrcAlpha OneMinusSrcAlpha 
			
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
	        };
	        
	        uniform fixed4 _Color;
	        uniform sampler2D _MainTex;  
	        uniform fixed _Freq_X;
	        uniform fixed _Amplitude_X; 
	         
	        uniform fixed _Freq_Y;
	        uniform fixed _Amplitude_Y; 
	         
	         
	        v2f vert(VertIn v) 
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				
				float t1 = sin(_Time.y*_Freq_X*6.28)*_Amplitude_X;
				float t2 = sin(_Time.y*_Freq_Y*6.28)*_Amplitude_Y;

				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				o.uv += fixed2(t1, t2);

				return o;
			}
			
			fixed4 frag(v2f i) : Color {
                fixed4 tex =  tex2D(_MainTex, i.uv);
                fixed4 col;
                col.rgb = tex.rgb;
                col.a = tex.a;
				return col;
			}	
	        
	        
	        ENDCG
		}

		Pass{

			LOD 200
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert 
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct VertIn {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f {
				fixed4 pos : SV_POSITION;
				fixed2 uv : TEXCOORD0;
				fixed2 uv1 : TEXCOORD1;
				fixed2 uv2 : TEXCOORD2;
			};

			uniform sampler2D _RippleTex;
			uniform fixed _UVAnimX;
			uniform fixed _UVAnimY;
			uniform fixed _rotate;

			uniform fixed _UVAnimX1;
			uniform fixed _UVAnimY1;

			uniform sampler2D _EnvSpec;

			v2f vert(VertIn v)

			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

				float t1 = _Time.y*_UVAnimX;
				float t2 = _Time.y*_UVAnimY;
				float sinX = sin(_rotate);
				float cosX = cos(_rotate);
				float2x2 rotationMatrix = float2x2(cosX*0.5f + 0.5f, -sinX*0.5f + 0.5f, sinX*0.5f + 0.5f, cosX*0.5f + 0.5f);


				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				o.uv = mul(o.uv, rotationMatrix);
				o.uv += fixed2(t1, t2);


				float t3 = _Time.y*_UVAnimX1;
				float t4 = _Time.y*_UVAnimY1;
				o.uv1 = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				o.uv1 += fixed2(t3, t4);

				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
				float3 r = reflect(-viewDir, v.normal);
				r = mul((float3x3)UNITY_MATRIX_MV, r);
				r.z += 1;
				float m = 2 * length(r);
				o.uv2 = r.xy / m + 0.5;

				return o;
			}

			fixed4 frag(v2f i) : Color{
				fixed4 tex = tex2D(_RippleTex, i.uv);
				fixed4 col;
				col.rgb = tex.rgb;
				col.a = tex.a;

				fixed4 tex1 = tex2D(_RippleTex, i.uv1);
				col.rgb = tex1.rgb + (1-tex1.a) * col.rgb;
				col.a = col.a * tex1.a;

				fixed4 tex2 = tex2D(_EnvSpec, i.uv2);
				col.rgb = tex2.rgb+col.rgb;
				return col;
			}


				ENDCG
		}


		
	} 

	FallBack "Diffuse"
}
