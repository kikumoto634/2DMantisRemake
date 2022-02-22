Shader "Unlit/PerlinNoise"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "" {}
    }

    SubShader
    {
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

            sampler2D _MainTex;
            float4 _MainTex_ST;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //モザイク処理
                /*float density = 50;
                fixed4 col = tex2D(_MainTex, floor(i.uv * density) / density);
                return col;*/


                //モザイク処理（正方形）
                float density = 10;
                fixed4 col = tex2D(_MainTex,
                    floor(i.uv * _ScreenParams.xy / density) * density / _ScreenParams.xy);
                return col;


                //スキャンノイズ
                /*float _Speed = 0.5;
                float _Width = 0.025;
                float _Power = 0.25;

                float sbTime = frac(_Time.y * _Speed);
                float seTime = sbTime + _Width;
                float2 uv = float2(
                    i.uv.x + sin(smoothstep(sbTime, seTime, i.uv.y) * 2 * 3.14159) * _Power,
                    i.uv.y
                );

                fixed4 col = tex2D(_MainTex, uv);
                return col;*/

            }
            ENDCG
        }
    }
}
