Shader "Unlit/FlatUnlitShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Angle("Angle", Range(0, 360)) = 1
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" "DisableBatching" = "True" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                uniform float _Angle;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;

                float4 RotateAroundYInDegrees(float4 vertex, float degrees)
                {
                    float alpha = degrees * UNITY_PI / 180.0;
                    float sina, cosa;
                    sincos(alpha, sina, cosa);
                    float2x2 m = float2x2(cosa, -sina, sina, cosa);
                    return float4(mul(m, vertex.xz), vertex.yw).xzyw;
                }

                v2f vert(appdata v)
                {
                    float4 origin = mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
                    v2f o;

                    o.vertex = UnityObjectToClipPos(v.vertex * float4(1, 1, 0.01f, 1));
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // sample the texture
                    fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
        }
}
