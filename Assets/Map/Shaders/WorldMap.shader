/*
    Thanks to January Desk for this shader
    https://github.com/yiyuezhuo
    https://www.youtube.com/channel/UCL4QE7LRQinmA0071J0kN9w
*/
Shader "Unlit/WorldMap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaterTex ("Texture", 2D) = "white" {}
        _RemapTex ("Texture", 2D) = "white" {}
        _PaletteTex("Texture", 2D) = "white" {}
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
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _WaterTex;
            sampler2D _RemapTex;
            sampler2D _PaletteTex;
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
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);                
                fixed4 c1 = tex2D(_MainTex, i.uv + float2(0.0001, 0));
                fixed4 c2 = tex2D(_MainTex, i.uv - float2(0.0001, 0));
                fixed4 c3 = tex2D(_MainTex, i.uv + float2(0, 0.0001));
                fixed4 c4 = tex2D(_MainTex, i.uv - float2(0, 0.0001));
                
                // Land grey borders 
                fixed4 w_index = tex2D(_WaterTex, i.uv);   
                if (w_index.r == 1 && (any(c1 != col) || any(c2 != col) || any(c3 != col) || any(c4 != col)) ) {                        
                        return fixed4(0.55, 0.55, 0.55, 1);                        
                }  

                fixed4 index = tex2D(_RemapTex, i.uv);                   
                return tex2D(_PaletteTex, index.xy * 255.0 / 256.0 + float2(0.001953125, 0.001953125));     

            }
            ENDCG
        }
    }
}
