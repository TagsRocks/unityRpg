Shader "Custom/waterWaveShader" {
	Properties {
		_Color ("Ambient", Color) = (0.588, 0.588, 0.588, 1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		//_UVAnimX ("UV Anim X", float) = 0
		//_UVAnimY ("UV Anim Y", float) = 0
		_Base_X("wave base scale X", float) = 1
		_Freq_X("wave x freq", float) = 0.07
		_Phase_X("wave x phase", float) = 0
		_Amplitude_X("wave x amplitude", float) = -0.2
		
		_Base_Y("wave base scale ", float) = 1
		_Freq_Y("wave  freq", float) = 0.06
		_Phase_Y("wave  phase", float) = 0
		_Amplitude_Y("wave  amplitude", float) = -0.24
	
	
		_AnimTex("Animation Texture Pass", 2D) = "white" {}	
		_Base_X1("wave base scale X", float) = 1
		_Freq_X1("wave x freq", float) = 0.02
		_Phase_X1("wave x phase", float) = 0
		_Amplitude_X1("wave x amplitude", float) = 0.05
		
		_Base_Y1("wave base scale ", float) = 1
		_Freq_Y1("wave  freq", float) = 0.02
		_Phase_Y1("wave  phase", float) = 0
		_Amplitude_Y1("wave  amplitude", float) = 0.05
	}
	
	
	SubShader {
		Tags { "Queue"="Transparent" 
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
	        	float4 color : COLOR;
	        };
	        
	        struct v2f {
	        	fixed4 pos : SV_POSITION;
	        	fixed2 uv : TEXCOORD0;
	   			fixed4 vertColor : TEXCOORD1;
	        };
	        
	        uniform fixed4 _Color;
	        uniform sampler2D _MainTex;  
	        //uniform fixed _UVAnimX;
	        //uniform fixed _UVAnimY;
	        uniform fixed _Base_X;
	        uniform fixed _Freq_X;
	        uniform fixed _Phase_X;
	        uniform fixed _Amplitude_X; 
	         
	        uniform fixed _Base_Y;
	        uniform fixed _Freq_Y;
	        uniform fixed _Phase_Y;
	        uniform fixed _Amplitude_Y; 
	         
	         
	        v2f vert(VertIn v) 
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				
				//float t1 = _Time.y*_UVAnimX;
				float t1 = sin(_Time.y*_Freq_X*6.28)*_Amplitude_X;
				float t2 = sin(_Time.y*_Freq_Y*6.28)*_Amplitude_Y;
				//float t2 = _Time.y*_UVAnimY;
	
				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				o.uv += fixed2(t1, t2);
				
				o.vertColor = v.color; 
				return o;
			}
			
			fixed4 frag(v2f i) : Color {
                fixed4 tex =  tex2D(_MainTex, i.uv);
                fixed4 col;
                col.rgb = tex.rgb*(i.vertColor+_Color);
                col.a = tex.a;
				return col;
			}	
	        
	        
	        ENDCG
		}
		
		
		Pass {
			LOD 200
			blend one one
			zwrite off
			lighting off
			
			CGPROGRAM
			#pragma vertex vert 
	        #pragma fragment frag
	        #include "UnityCG.cginc"
	        
	        struct VertIn {
	        	float4 vertex : POSITION;
	        	float4 texcoord : TEXCOORD0;
	        	float4 color : COLOR;
	        };
	        
	        struct v2f {
	        	fixed4 pos : SV_POSITION;
	        	fixed2 uv : TEXCOORD0;
	   			fixed4 vertColor : TEXCOORD1;
	        };
	        
	        uniform fixed4 _Color;
	        uniform sampler2D _AnimTex;  
	        //uniform fixed _UVAnimX;
	        //uniform fixed _UVAnimY;
	        uniform fixed _Base_X1;
	        uniform fixed _Freq_X1;
	        uniform fixed _Phase_X1;
	        uniform fixed _Amplitude_X1; 
	         
	        uniform fixed _Base_Y1;
	        uniform fixed _Freq_Y1;
	        uniform fixed _Phase_Y1;
	        uniform fixed _Amplitude_Y1; 
	         
	         
	        v2f vert(VertIn v) 
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				
				//float t1 = _Time.y*_UVAnimX;
				float t1 = sin(_Time.y*_Freq_X1*6.28)*_Amplitude_X1;
				float t2 = sin(_Time.y*_Freq_Y1*6.28)*_Amplitude_Y1;
				//float t2 = _Time.y*_UVAnimY;
	
				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				o.uv += fixed2(t1, t2);
				
				o.vertColor = v.color; 
				return o;
			}
			
			fixed4 frag(v2f i) : Color {
                fixed4 tex =  tex2D(_AnimTex, i.uv);
                fixed4 col;
                col.rgb = tex.rgb*(i.vertColor+_Color);
                col.a = tex.a;
				return col;
			}	
	        
	        ENDCG
		}
		
	} 
	FallBack "Diffuse"
}
