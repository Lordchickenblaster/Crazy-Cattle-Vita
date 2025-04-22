// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/StencilLight"
{
    Properties
    {
        [HDR]_Color("Color",Color) = (1,1,1,0.5)       
    }   
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }    
        CGINCLUDE
        float4 _Color;
        struct appdata
        {
            float4 vertex : POSITION;
        };
        
        struct v2f
        {
            float4 vertex : SV_POSITION;
        };
 
        v2f vert(appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            return o;
        }
        
        fixed4 frag(v2f i) : SV_Target
        {
            return _Color * _Color.a;
        }
 
        ENDCG       
        Pass
        {
            Name "Mask"
            Ztest Greater
            Zwrite Off
            Cull Front
            Colormask 0
            
            Stencil
            {
                Ref 1
                Comp Greater              
                Pass Replace
            }
        }

        
        Pass
        {
            Name "Light Outside"
            Zwrite Off
            Ztest Lequal
            Cull Back
            Blend DstColor One
            
            Stencil
            {
                Comp Equal
                Ref 1
                Pass Zero
                Fail Zero
                Zfail Zero
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            ENDCG
        }
        
        Pass
        {
            Name "Light Inside"
            ZTest Off
            ZWrite Off
            Cull Front
            Blend DstColor One
            
            Stencil
            {
                Ref 1
                Comp Equal
                Pass Zero
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            ENDCG
        }
    }
}