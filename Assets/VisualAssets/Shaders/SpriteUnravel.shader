Shader "Unlit/SpriteUnravel"
{
	Properties
	{
		 [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
	}
	SubShader
	{
		Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 objPos : TEXCOORD1;
				UNITY_FOG_COORDS(2)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float2 _Point1;
			float2 _Point2;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.objPos = v.vertex.xy;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//Equation of the line
				// p1: -2,0; -1,1; 0,2
				// p2: 0,-2; 1,-1; 2,0
				float x1 = _Point1.x;
				float y1 = _Point1.y;
				float x2 = _Point2.x;
				float y2 = _Point2.y;

				float m = (y2 - y1)/(x2 - x1);

				float b = y1 - (m * x1);

				float lineXPosition = (i.objPos.y - b)/m;
				
				// return float4 (lineXPosition, 0, 0, 1);
				if(i.objPos.x > lineXPosition){
					discard;
				}
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				if(col.a <= 0.75){
					discard;
				}
				// apply fog
				// UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
