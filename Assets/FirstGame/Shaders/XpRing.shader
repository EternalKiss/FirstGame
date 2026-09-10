Shader "FirstGame/XpRing"
{
    Properties
    {
        _FillColor ("Fill Color", Color) = (1, 0.76, 0.12, 1)
        _EmptyColor ("Empty Color", Color) = (0.1, 0.1, 0.1, 1)
        _FillAmount ("Fill Amount", Range(0, 1)) = 0
        _InnerRadius ("Inner Radius", Range(0, 0.5)) = 0.35
        _OuterRadius ("Outer Radius", Range(0, 0.5)) = 0.5
        _StartAngle ("Start Angle", Range(0, 360)) = 90
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _FillColor;
            fixed4 _EmptyColor;
            float _FillAmount;
            float _InnerRadius;
            float _OuterRadius;
            float _StartAngle;

            v2f vert(appdata input)
            {
                v2f output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                return output;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                float2 centered = input.uv - float2(0.5, 0.5);
                float distance = length(centered);

                if (distance < _InnerRadius || distance > _OuterRadius)
                {
                    discard;
                }

                float angle = atan2(centered.y, centered.x);
                float angleDegrees = degrees(angle);

                if (angleDegrees < 0)
                {
                    angleDegrees += 360;
                }

                float normalizedAngle = angleDegrees - _StartAngle;

                if (normalizedAngle < 0)
                {
                    normalizedAngle += 360;
                }

                float progress = 1 - (normalizedAngle / 360);

                if (progress <= _FillAmount)
                {
                    return _FillColor;
                }

                return _EmptyColor;
            }
            ENDCG
        }
    }
}