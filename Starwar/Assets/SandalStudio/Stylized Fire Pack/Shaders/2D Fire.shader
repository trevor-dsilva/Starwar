Shader "SandalStudio/2D Fire"
{
	Properties
	{
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		[HDR]_Color0("Color 0", Color) = (0.1367925,0.8397518,1,1)
		[HDR]_Color1("Color 1", Color) = (0.7075472,0.09678713,0.7075338,1)
		_Noise_SpeedTille("Noise_Speed/Tile", Vector) = (-0.67,0,1,1)
		_TextureSample0("Texture", 2D) = "white" {}
		_Trail_("Internal flame", Float) = 6.01
		_Color("Color ratio", Float) = 1
		_Float1("Fire noise size", Float) = 0.43
		[Toggle(_RG_ON)] _RG("R/G", Float) = 1
		[Toggle(_BRG_ON)] _BRG("B/RG", Float) = 0

	}


	Category 
	{
		SubShader
		{
		LOD 0

			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend One OneMinusSrcAlpha
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off
			ZTest LEqual
			
			Pass {
			
				CGPROGRAM
				
				#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
				#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
				#endif
				
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				#include "UnityShaderVariables.cginc"
				#define ASE_NEEDS_FRAG_COLOR
				#pragma shader_feature_local _BRG_ON
				#pragma shader_feature_local _RG_ON


				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
					
				};
				
				
				#if UNITY_VERSION >= 560
				UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
				#else
				uniform sampler2D_float _CameraDepthTexture;
				#endif

				//Don't delete this comment
				// uniform sampler2D_float _CameraDepthTexture;

				uniform sampler2D _MainTex;
				uniform fixed4 _TintColor;
				uniform float4 _MainTex_ST;
				uniform float _InvFade;
				uniform sampler2D _TextureSample0;
				uniform float4 _Noise_SpeedTille;
				uniform float _Float1;
				uniform float _Trail_;
				uniform float4 _Color0;
				uniform float4 _Color1;
				uniform float _Color;


				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					

					v.vertex.xyz +=  float3( 0, 0, 0 ) ;
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID( i );
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );

					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate (_InvFade * (sceneZ-partZ));
						i.color.a *= fade;
					#endif

					float2 appendResult23 = (float2(_Noise_SpeedTille.x , _Noise_SpeedTille.y));
					float2 appendResult84 = (float2(_Noise_SpeedTille.z , _Noise_SpeedTille.w));
					float2 texCoord20 = i.texcoord.xy * appendResult84 + float2( 0,0 );
					float2 panner21 = ( 1.0 * _Time.y * appendResult23 + texCoord20);
					float4 tex2DNode29 = tex2D( _TextureSample0, panner21 );
					#ifdef _RG_ON
					float staticSwitch82 = tex2DNode29.r;
					#else
					float staticSwitch82 = tex2DNode29.g;
					#endif
					#ifdef _BRG_ON
					float staticSwitch83 = tex2DNode29.b;
					#else
					float staticSwitch83 = staticSwitch82;
					#endif
					float2 texCoord14 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float clampResult35 = clamp( step( ( ( i.color.a * saturate( ( staticSwitch83 * pow( ( 1.0 - texCoord20.x ) , _Float1 ) ) ) ) - ( 1.0 - ( pow( ( texCoord14.y * _Trail_ ) , 1.0 ) * pow( ( 1.0 - ( texCoord14.y * 1.0 ) ) , 1.0 ) * pow( ( 1.0 - ( texCoord14.x * 1.0 ) ) , 1.0 ) ) ) ) , 0.0 ) , 0.0 , 1.0 );
					float2 temp_cast_0 = (_Color).xx;
					float2 texCoord6 = i.texcoord.xy * temp_cast_0 + float2( 0,0 );
					float4 lerpResult1 = lerp( _Color0 , _Color1 , ( texCoord6.x * i.color.a ));
					

					fixed4 col = ( ( 1.0 - clampResult35 ) * lerpResult1 );
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
	CustomEditor "ASEMaterialInspector"
	
	
}