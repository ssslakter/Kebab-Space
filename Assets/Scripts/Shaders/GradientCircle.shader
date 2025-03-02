Shader "Unlit/GradientCircle"
{
    Properties
    {
        _Color ("Center Color", Color) = (1, 0, 0, 1)
        _EdgeColor ("Edge Color", Color) = (0, 0, 1, 1)
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _GradPow ("Gradient Power", Float) = 6.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _Color;
            float4 _EdgeColor;
            float _GradPow;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float transition = pow(i.uv.x, _GradPow);
                return lerp(_Color, _EdgeColor, transition);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
