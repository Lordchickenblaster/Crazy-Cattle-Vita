Shader "Katworks/Fog/Texture"
{
    Properties
    {
       _MainTex ("Base (RGB)", 2D) = "white" {}
       _FogTex("Fog (RGBA)", 2D) = "gray" {}
       _FogExponent ("Fog Exponent", float) = 0.5
       _FogMultiplier ("Fog Multiplier", float) = 0.05
       [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Cull [_Cull]

        Pass // Main color
        {
            Tags { "Queue"="Overlay" } // Makes sure it renders after opaque objects.
            Blend SrcAlpha OneMinusSrcAlpha // Transparency blend mode

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // Add transparency based on alpha channel
                col.a = col.a; // This keeps the original alpha channel for transparency.
                return col;
            }
            ENDCG
        }

        Pass // Fog
        {
            Blend SrcAlpha OneMinusSrcAlpha // Maintain transparency blending for fog
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _FogTex;
            float4 _FogTex_ST;
            float4 _Global_PlayerWorldPosition;
            float _FogExponent;
            float _FogMultiplier;

            v2f vert (appdata v)
            {
                v2f o;
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float dist = distance(_Global_PlayerWorldPosition, worldPos);
                dist = pow(dist * _FogMultiplier, _FogExponent);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(float2(clamp(dist, 0, 1), 0.5), _FogTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_FogTex, i.uv);
                // Set the alpha based on the texture's alpha channel for fog transparency.
                col.a = col.a;
                return col;
            }
            ENDCG
        }
    }

    // Fallback for non-supported hardware
    Fallback "Diffuse"
}