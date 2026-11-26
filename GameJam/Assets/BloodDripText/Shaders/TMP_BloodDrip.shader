// ============================================================
//  TMP 血滴文字 Shader (Blood Drip Text)
//  基于 TextMeshPro Mobile Distance Field 改造
//  用法：TextMeshPro 的 Font Asset 材质换成这个 Shader
//  参数：Drip Color / Drip Length / Drip Density / Drip Width / Drip Speed
// ============================================================
Shader "TextMeshPro/Blood Drip Text"
{
	Properties
	{
		[HDR]_FaceColor		("Face Color", Color) = (0.55,0.02,0.02,1)
		_FaceDilate			("Face Dilate", Range(-1,1)) = 0

		[HDR]_OutlineColor	("Outline Color", Color) = (0,0,0,1)
		_OutlineWidth		("Outline Thickness", Range(0,1)) = 0
		_OutlineSoftness	("Outline Softness", Range(0,1)) = 0

		_WeightNormal		("Weight Normal", float) = 0
		_WeightBold			("Weight Bold", float) = .5

		_ShaderFlags		("Flags", float) = 0
		_ScaleRatioA		("Scale RatioA", float) = 1
		_ScaleRatioB		("Scale RatioB", float) = 1
		_ScaleRatioC		("Scale RatioC", float) = 1

		_MainTex			("Font Atlas", 2D) = "white" {}
		_TextureWidth		("Texture Width", float) = 512
		_TextureHeight		("Texture Height", float) = 512
		_GradientScale		("Gradient Scale", float) = 5
		_ScaleX				("Scale X", float) = 1
		_ScaleY				("Scale Y", float) = 1
		_PerspectiveFilter	("Perspective Correction", Range(0, 1)) = 0.875
		_Sharpness			("Sharpness", Range(-1,1)) = 0

		_VertexOffsetX		("Vertex OffsetX", float) = 0
		_VertexOffsetY		("Vertex OffsetY", float) = 0

		_ClipRect			("Clip Rect", vector) = (-32767, -32767, 32767, 32767)
		_MaskSoftnessX		("Mask SoftnessX", float) = 0
		_MaskSoftnessY		("Mask SoftnessY", float) = 0

		_StencilComp		("Stencil Comparison", Float) = 8
		_Stencil			("Stencil ID", Float) = 0
		_StencilOp			("Stencil Operation", Float) = 0
		_StencilWriteMask	("Stencil Write Mask", Float) = 255
		_StencilReadMask	("Stencil Read Mask", Float) = 255

		_CullMode			("Cull Mode", Float) = 0
		_ColorMask			("Color Mask", Float) = 15

		// ================= 血滴参数 =================
		[HDR]_DripColor		("Drip Color", Color) = (0.8,0.02,0.02,1)
		_DripLength			("Drip Length", Range(0,3)) = 1.2
		_DripDensity		("Drip Density", Range(0,1)) = 0.55
		_DripWidth			("Drip Width", Range(0.5,8)) = 2.5
		_DripSpeed			("Drip Speed", Range(0,4)) = 0.6
		_DripAlpha			("Drip Opacity", Range(0,1)) = 0.95
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
		}

		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull [_CullMode]
		ZWrite Off
		Lighting Off
		Fog { Mode Off }
		ZTest [unity_GUIZTestMode]
		Blend One OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
			CGPROGRAM
			#pragma vertex VertShader
			#pragma fragment PixShader

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP

			#include "UnityCG.cginc"

			// ---- 自带声明，不依赖 TMP 的 cginc，方便单独分发 ----
			sampler2D _MainTex;
			float4 _FaceColor;
			float4 _OutlineColor;
			float _FaceDilate;
			float _OutlineWidth;
			float _OutlineSoftness;
			float _WeightNormal;
			float _WeightBold;
			float _ScaleRatioA;
			float _ScaleRatioB;
			float _ScaleRatioC;
			float _TextureWidth;
			float _TextureHeight;
			float _GradientScale;
			float _ScaleX;
			float _ScaleY;
			float _PerspectiveFilter;
			float _Sharpness;
			float _VertexOffsetX;
			float _VertexOffsetY;
			float4 _ClipRect;
			float _MaskSoftnessX;
			float _MaskSoftnessY;

			float4 _DripColor;
			float _DripLength;
			float _DripDensity;
			float _DripWidth;
			float _DripSpeed;
			float _DripAlpha;

			struct vertex_t {
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4	vertex			: POSITION;
				float3	normal			: NORMAL;
				fixed4	color			: COLOR;
				float2	texcoord0		: TEXCOORD0;
				float2	texcoord1		: TEXCOORD1;
			};

			struct pixel_t {
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4	vertex			: SV_POSITION;
				fixed4	faceColor		: COLOR;
				fixed4	outlineColor	: COLOR1;
				float4	texcoord0		: TEXCOORD0;
				half4	param			: TEXCOORD1;
				half4	mask			: TEXCOORD2;
			};

			pixel_t VertShader(vertex_t input)
			{
				pixel_t output;

				UNITY_INITIALIZE_OUTPUT(pixel_t, output);
				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_TRANSFER_INSTANCE_ID(input, output);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				float bold = step(input.texcoord1.y, 0);

				float4 vert = input.vertex;
				vert.x += _VertexOffsetX;
				vert.y += _VertexOffsetY;
				float4 vPosition = UnityObjectToClipPos(vert);

				float2 pixelSize = vPosition.w;
				pixelSize /= float2(_ScaleX, _ScaleY) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

				float scale = rsqrt(dot(pixelSize, pixelSize));
				scale *= abs(input.texcoord1.y) * _GradientScale * (_Sharpness + 1);
				if (UNITY_MATRIX_P[3][3] == 0)
					scale = lerp(abs(scale) * (1 - _PerspectiveFilter), scale,
						abs(dot(UnityObjectToWorldNormal(input.normal.xyz), normalize(WorldSpaceViewDir(vert)))));

				float weight = lerp(_WeightNormal, _WeightBold, bold) / 4.0;
				weight = (weight + _FaceDilate) * _ScaleRatioA * 0.5;

				scale /= 1 + (_OutlineSoftness * _ScaleRatioA * scale);
				float bias = (0.5 - weight) * scale - 0.5;
				float outline = _OutlineWidth * _ScaleRatioA * 0.5 * scale;

				float opacity = input.color.a;

				fixed4 faceColor = fixed4(input.color.rgb, opacity) * _FaceColor;
				faceColor.rgb *= faceColor.a;

				fixed4 outlineColor = _OutlineColor;
				outlineColor.a *= opacity;
				outlineColor.rgb *= outlineColor.a;
				outlineColor = lerp(faceColor, outlineColor, sqrt(min(1.0, (outline * 2))));

				float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
				float2 maskUV = (vert.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);

				output.vertex = vPosition;
				output.faceColor = faceColor;
				output.outlineColor = outlineColor;
				output.texcoord0 = float4(input.texcoord0.x, input.texcoord0.y, maskUV.x, maskUV.y);
				output.param = half4(scale, bias - outline, bias + outline, bias);
				output.mask = half4(vert.xy * 2 - clampedRect.xy - clampedRect.zw,
					0.25 / (0.25 * half2(_MaskSoftnessX, _MaskSoftnessY) + pixelSize.xy));

				return output;
			}

			float hash11(float p)
			{
				p = frac(p * 0.1031);
				p *= p + 33.33;
				p *= p + p;
				return frac(p);
			}

			// 血滴遮罩：从字形向上采样，制造向下流淌的痕迹
			half BloodDrip(float2 uv, half scale, half bias)
			{
				float texelY = 1.0 / max(_TextureHeight, 1.0);

				// 按列切块，每块随机决定有没有血滴、滴多长
				float colPx = uv.x * _TextureWidth;
				float slab = floor(colPx / max(_DripWidth, 0.5));
				float rnd = hash11(slab * 1.371 + 11.7);
				float active = step(1.0 - _DripDensity, rnd);

				float lenPx = _DripLength * _GradientScale * (0.3 + 0.7 * hash11(slab * 7.113 + 3.1));
				// 缓慢脉动，模拟血在往下淌
				lenPx *= 1.0 + 0.18 * sin(_Time.y * _DripSpeed * 2.0 + rnd * 6.2831);
				// 关键：绝不能越过字形 padding，否则会采到图集里相邻字形，出现色块
				lenPx = min(lenPx, _GradientScale * 0.9);

				const int STEPS = 10;
				half acc = 0;
				[unroll]
				for (int i = 1; i <= STEPS; i++)
				{
					float t = (float)i / STEPS;
					float2 suv = uv + float2(0, t * lenPx * texelY);
					half a = saturate(tex2D(_MainTex, suv).a * scale - bias);
					float fade = 1.0 - t * 0.85;
					acc = max(acc, a * fade);
				}

				return saturate(acc * 1.6) * active;
			}

			fixed4 PixShader(pixel_t input) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(input);

				half d = tex2D(_MainTex, input.texcoord0.xy).a * input.param.x;
				half4 c = input.faceColor * saturate(d - input.param.w);

				// 血滴
				half drip = BloodDrip(input.texcoord0.xy, input.param.x, input.param.w) * _DripAlpha;
				half dripA = drip * _DripColor.a;
				c.rgb += _DripColor.rgb * dripA * (1 - c.a);
				c.a += dripA * (1 - c.a);

				#if UNITY_UI_CLIP_RECT
				half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(input.mask.xy)) * input.mask.zw);
				c *= m.x * m.y;
				#endif

				#if UNITY_UI_ALPHACLIP
				clip(c.a - 0.001);
				#endif

				return c;
			}
			ENDCG
		}
	}

	FallBack "TextMeshPro/Mobile/Distance Field"
}
