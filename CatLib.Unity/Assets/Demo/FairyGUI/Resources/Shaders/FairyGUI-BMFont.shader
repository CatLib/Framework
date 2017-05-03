// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "FairyGUI/BMFont"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}

		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		_BlendSrcFactor ("Blend SrcFactor", Float) = 5
		_BlendDstFactor ("Blend DstFactor", Float) = 10
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend [_BlendSrcFactor] [_BlendDstFactor]
		ColorMask [_ColorMask]

		Pass
		{
			CGPROGRAM
				#pragma multi_compile NOT_GRAYED GRAYED
				#pragma multi_compile NOT_CLIPPED CLIPPED SOFT_CLIPPED
				#pragma vertex vert
				#pragma fragment frag
				#pragma exclude_renderers d3d9 opengl flash

				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					fixed2 flags : TEXCOORD1;

					#ifdef CLIPPED
					half2 clipPos : TEXCOORD2;
					#endif

					#ifdef SOFT_CLIPPED
					half2 clipPos : TEXCOORD2;
					#endif
				};

				sampler2D _MainTex;

				#ifdef CLIPPED
				float4 _ClipBox = float4(-2, -2, 0, 0);
				#endif

				#ifdef SOFT_CLIPPED
				float4 _ClipBox = float4(-2, -2, 0, 0);
				half4 _ClipSoftness = half4(0, 0, 0, 0);
				#endif

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color;

					float2 texcoord = v.texcoord;
					o.flags.x = floor(texcoord.x/10);
					texcoord.x = texcoord.x - o.flags.x*10;
						
					#ifdef GRAYED
					if(texcoord.y >1)
					{
						texcoord.y = texcoord.y - 10;
						o.flags.y = 1;
					}
					else
						o.flags.y = 0;
					#else
						o.flags.y = 0;
					#endif
					
					o.texcoord = texcoord;

					#ifdef CLIPPED
					o.clipPos = mul(unity_ObjectToWorld, v.vertex).xy * _ClipBox.zw + _ClipBox.xy;
					#endif

					#ifdef SOFT_CLIPPED
					o.clipPos = mul(unity_ObjectToWorld, v.vertex).xy * _ClipBox.zw + _ClipBox.xy;
					#endif

					return o;
				}

				fixed4 frag (v2f i) : SV_Target
				{
					fixed4 col = i.color;
					fixed4 tcol = tex2D(_MainTex, i.texcoord);
					col.a *= tcol[i.flags.x];

					#ifdef GRAYED
					if(i.flags.y==1)
					{
						fixed grey = dot(col.rgb, fixed3(0.299, 0.587, 0.114));
						col.rgb = fixed3(grey, grey, grey);  
					}
					else
						col.rgb = fixed3(0.8, 0.8, 0.8);
					#endif

					#ifdef SOFT_CLIPPED
					half2 factor = half2(0,0);
					if(i.clipPos.x<0)
						factor.x = (1.0-abs(i.clipPos.x)) * _ClipSoftness.x;
					else
						factor.x = (1.0-i.clipPos.x) * _ClipSoftness.z;
					if(i.clipPos.y<0)
						factor.y = (1.0-abs(i.clipPos.y)) * _ClipSoftness.w;
					else
						factor.y = (1.0-i.clipPos.y) * _ClipSoftness.y;
					col.a *= clamp(min(factor.x, factor.y), 0.0, 1.0);
					#endif

					#ifdef CLIPPED
					half2 factor = abs(i.clipPos);
					col.a *= step(max(factor.x, factor.y), 1);
					#endif

					return col;
				}
			ENDCG
		}
	}

	Fallback "FairyGUI/Text"
}
