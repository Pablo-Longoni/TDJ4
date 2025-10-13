/*Shader "Unlit/RippleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
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
*/

Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float3 _WaterCenter;
            float2 _WaterSizeXZ;
            float _AlphaClipThreshold;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                OUT.worldPos = TransformObjectToWorld(IN.positionOS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                // Calcular límites X y Z
                float halfX = _WaterSizeXZ.x * 0.5;
                float halfZ = _WaterSizeXZ.y * 0.5;
                float minX = _WaterCenter.x - halfX;
                float maxX = _WaterCenter.x + halfX;
                float minZ = _WaterCenter.z - halfZ;
                float maxZ = _WaterCenter.z + halfZ;

                float x = IN.worldPos.x;
                float z = IN.worldPos.z;

                // Chequear si está dentro de los límites
                bool insideX = (x >= minX) && (x <= maxX);
                bool insideZ = (z >= minZ) && (z <= maxZ);

                // Si está fuera -> descartar píxel
                if (!(insideX && insideZ))
                {
                    float falloff = 0.2; // controlá este valor desde una propiedad si querés

                    float edgeX = smoothstep(minX, minX + falloff, x) * (1 - smoothstep(maxX - falloff, maxX, x));
                    float edgeZ = smoothstep(minZ, minZ + falloff, z) * (1 - smoothstep(maxZ - falloff, maxZ, z));
                    float mask = edgeX * edgeZ;

                    col.a *= mask;
                    clip(col.a - _AlphaClipThreshold);
                }

                // Alpha clip (por si tu textura tiene bordes suaves)
                clip(col.a - _AlphaClipThreshold);

                return col;
            }
            ENDHLSL
        }
    }
}
