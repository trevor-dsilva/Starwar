Shader "SandalStudio/Rocket Fire"
{
	Properties
	{
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_SpeedXY("Speed XY", Vector) = (0,0,0,0)
		_TileXY(" Tile XY", Vector) = (1,1,0,0)
		_Mask("Mask", Float) = 0.73
		_Power("Power", Float) = 1
		[HDR]_Color1("Color 1", Color) = (0.4386792,0.7941277,1,1)
		[HDR]_Color2("Color 2", Color) = (1,0,0,1)
		[Toggle(_RG_ON)] _RG("R/G", Float) = 1
		[Toggle(_BRG_ON)] _BRG("B/RG", Float) = 0
		_Emissive("Emissive", Float) = 1

	}


	Category 
	{
		SubShader
		{
		LOD 0

			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend SrcAlpha OneMinusSrcAlpha
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
				uniform float2 _TileXY;
				uniform float2 _SpeedXY;
				uniform float _Emissive;
				uniform float _Mask;
				uniform float _Power;
				uniform float4 _Color1;
				uniform float4 _Color2;


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

					float2 panner6 = ( 1.0 * _Time.y * _SpeedXY + float2( 0,0 ));
					float2 texCoord4 = i.texcoord.xy * _TileXY + panner6;
					float4 tex2DNode1 = tex2D( _TextureSample0, texCoord4 );
					#ifdef _RG_ON
					float staticSwitch39 = tex2DNode1.r;
					#else
					float staticSwitch39 = tex2DNode1.g;
					#endif
					#ifdef _BRG_ON
					float staticSwitch40 = tex2DNode1.b;
					#else
					float staticSwitch40 = staticSwitch39;
					#endif
					float2 texCoord8 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float4 lerpResult24 = lerp( _Color1 , _Color2 , texCoord8.y);
					

					fixed4 col = ( ( ( ( staticSwitch40 * _Emissive ) * i.color ) * pow( saturate( ( texCoord8.y * _Mask ) ) , _Power ) ) * lerpResult24 );
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
	CustomEditor "ASEMaterialInspector"
	
	
}