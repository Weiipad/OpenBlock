// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/WireFrameShader"
{
    Properties
    {
        _LineWidth("Line Rate", Range(0, 1)) = 0.1
        _Color("Color", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Pass 
        {
            Tags 
            { 
                "RenderType" = "Transparent" 
                "RenderQueue" = "Transparent"
            }
            
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float _LineWidth;
            fixed4 _Color;

            struct a2v
            {
                float4 vertex: POSITION;
                float4 uv: TEXCOORD;
            };

            struct v2f
            {
                float4 position: SV_POSITION;
                float2 uv: TEXCOORD0;
            };

            v2f vert(a2v i)
            {
                v2f o;
                o.position = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv.xy;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float sx, ex;
                float sy, ey;
                sx = step(_LineWidth, i.uv.x); // x < _LineWidth ? false : true
                ex = step(i.uv.x, 1 - _LineWidth); 
                sy = step(_LineWidth, i.uv.y);
                ey = step(i.uv.y, 1 - _LineWidth);

                return lerp(_Color, fixed4(0, 0, 0, 0), sx * ex * sy * ey);
                // return (sx || ex || sy || ey) ?  _Color : fixed4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
}
