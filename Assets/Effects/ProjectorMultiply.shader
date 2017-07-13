﻿// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Projector/Multiply" {
     Properties{
         _ShadowTex("Cookie", 2D) = "gray" {}
		 _Alpha("Alpha", Range (0.0, 1.0)) = 1.0
     }
     Subshader {
         Tags { "Queue" = "Transparent" }
         Pass {
             ZWrite Off
             ColorMask RGB
             Blend DstColor Zero
             Offset -1, -1
 
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #pragma multi_compile_fog
             #include "UnityCG.cginc"
 
             struct v2f {
                 float4 uvShadow : TEXCOORD0;
                 UNITY_FOG_COORDS(2)
                 float4 pos : SV_POSITION;
             };
 
             float4x4 unity_Projector;
 
             v2f vert(float4 vertex : POSITION)
             {
                 v2f o;
                 o.pos = UnityObjectToClipPos(vertex);
                 o.uvShadow = mul(unity_Projector, vertex);
                 UNITY_TRANSFER_FOG(o,o.pos);
                 return o;
             }
 
             sampler2D _ShadowTex;
			 float _Alpha;

             fixed4 frag(v2f i) : SV_Target
             {
                 fixed4 s = tex2Dproj(_ShadowTex, UNITY_PROJ_COORD(i.uvShadow));
				 fixed4 res = lerp(fixed4(1, 1, 1, 0), s, _Alpha);
 
                 UNITY_APPLY_FOG_COLOR(i.fogCoord, res, fixed4(1,1,1,1));
                 return res;
             }
             ENDCG
         }
     }
 }