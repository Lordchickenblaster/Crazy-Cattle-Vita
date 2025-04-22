// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "3DS/FakeVolumetricShadow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Transparent" }
        ZWrite Off
        Lighting Off
        Blend DstColor Zero

        Pass
        {
            Cull Back
            ZTest Greater
            ColorMask 0
            ZWrite Off

            Stencil {
                Ref 1
                Comp Always
                Pass Replace
            }
        }

        Pass
        {
            Cull Off
            ZTest Greater
            Lighting Off
            ZWrite Off

            Stencil {
                Ref 1
                Comp NotEqual
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            struct appdata
            {
                half4 vertex : POSITION;
                half2 uv : TEXCOORD0;
            };

            struct v2f
            {
                half4 vertex : SV_POSITION;
                half2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv) * 1.5;
                return color;
            }
            ENDCG
        }

        Pass
        {
            Cull Back
            ZTest Greater
            ColorMask 0

            Stencil {
                Ref 0
                Comp Always
                Pass Zero
            }
        }
    }
}
