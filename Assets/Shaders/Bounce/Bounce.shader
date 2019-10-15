Shader "Custom/Bounce"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Main Texture", 2D) = "white" { }
		_ImpactPosition("Impact Position", Vector) = (0, 0, 0, 0)
		_ImpactDirection("Impact Direction", Vector) = (0, 0, 0, 0)
		_DamageRadius("Damage Radius", float) = 1
		_BounceAmplitude("Bounce Amplitude", float) = 1
		_AnimationValue("Animation Value", float) = 0
	}

	SubShader
	{
		Pass
		{
			HLSLPROGRAM
			#pragma vertex Vertex
			#pragma fragment Fragment
			#include "UnityCG.cginc"

			float4 _Color;
			sampler2D _MainTex;
			float4 _ImpactPosition;
			float4 _ImpactDirection;
			float _DamageRadius;
			float _BounceAmplitude;
			float _AnimationValue;

			struct Attributes
			{
				float4 positionOS : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Varyings
			{
				float4 positionCS : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			Varyings Vertex( Attributes input )
			{
				float4 worldPosition = mul(unity_ObjectToWorld, input.positionOS);
				float4 worldPositionOffset = (_ImpactDirection * _AnimationValue * _BounceAmplitude) * (1 - clamp(distance(_ImpactPosition, worldPosition) / _DamageRadius, 0, 1));

				Varyings output;
				output.positionCS = UnityWorldToClipPos(worldPosition + worldPositionOffset);
				output.uv = input.uv;
				return output;
			}

			half4 Fragment( Varyings input ) : SV_TARGET
			{
				return tex2D(_MainTex, input.uv) * _Color;
			}
			ENDHLSL
		}
	}
}