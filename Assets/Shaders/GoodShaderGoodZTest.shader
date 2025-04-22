Shader "Custom/3DSInvert_NormalZTest"
{
    Properties
    {
        _MainTex ("Font Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        ZWrite Off
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha
        Lighting Off
        Cull Off
        Fog { Mode Off }

        Pass
        {
            SetTexture[_MainTex]
            {
                constantColor [_Color]
                combine constant - texture, texture
            }
        }
    }

    Fallback Off
}