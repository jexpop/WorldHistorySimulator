/*
    Thanks to January Desk for the tutorial
    https://github.com/yiyuezhuo
    https://www.youtube.com/channel/UCL4QE7LRQinmA0071J0kN9w
*/
Shader "Unlit/WorldMap"
{
    Properties
    {
        _RegionTex ("Region Texture", 2D) = "white" {}        
        _RiverTex ("River Texture", 2D) = "white" {}
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

            sampler2D _RegionTex;
            sampler2D _SeaTex;
            sampler2D _RiverTex;
            sampler2D _RemapTex;
            sampler2D _PaletteTex;
            float4 _RegionTex_ST;

            v2f vert (appdata v)
            {
                v2f o;                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _RegionTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {                
                fixed4 col = tex2D(_RegionTex, i.uv);                
                fixed4 c1 = tex2D(_RegionTex, i.uv + float2(0.0001, 0));
                fixed4 c2 = tex2D(_RegionTex, i.uv - float2(0.0001, 0));
                fixed4 c3 = tex2D(_RegionTex, i.uv + float2(0, 0.0001));
                fixed4 c4 = tex2D(_RegionTex, i.uv - float2(0, 0.0001));
                
                // Rivers
                fixed4 river_index = tex2D(_RiverTex, i.uv);

                // Sea regions
                fixed4 sea_index = tex2D(_SeaTex, i.uv);   

                
                if(river_index.r == 1 && river_index.g == 1 && river_index.b == 1)
                {
                    // Land grey borders 
                    if (sea_index.r == 1 && (any(c1 != col) || any(c2 != col) || any(c3 != col) || any(c4 != col)) ) {                             
                        return fixed4(0.55, 0.55, 0.55, 1);                    
                    }  
                }
                else{
                    // Current pixel is a river                    
                    return fixed4(0, 0.88, 1, 1);    
                }

                fixed4 index = tex2D(_RemapTex, i.uv);                   
                return tex2D(_PaletteTex, index.xy * 255.0 / 256.0 + float2(0.001953125, 0.001953125));     
            }
            ENDCG
        }
    }
}
