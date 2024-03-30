Shader "Unlit/EditorWorldMap"
{
    Properties
    {
        _RegionTex ("Region Texture", 2D) = "white" {}        
        _RiverTex ("River Texture", 2D) = "white" {}

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv: TEXCOORD0;
            };

            struct Varyings
            {
                // The positions in this struct must have the SV_POSITION semantic.
                float4 positionCS  : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
            sampler2D _RegionTex;
            sampler2D _SeaTex;
            sampler2D _RiverTex;            
            sampler2D _RemapTex;
            sampler2D _PaletteTex;
            float _DrawRiver;
            float4 _RegionTex_ST;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _RegionTex);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {                
                float4  col = tex2D(_RegionTex, IN.uv);                
                float4  c1 = tex2D(_RegionTex, IN.uv + float2(0.0001, 0));
                float4  c2 = tex2D(_RegionTex, IN.uv - float2(0.0001, 0));
                float4  c3 = tex2D(_RegionTex, IN.uv + float2(0, 0.0001));
                float4  c4 = tex2D(_RegionTex, IN.uv - float2(0, 0.0001));
                
                // Rivers
                float4  river_index = tex2D(_RiverTex, IN.uv);

                // Sea regions
                float4  sea_index = tex2D(_SeaTex, IN.uv);   
                

                // There are rivers
                if(_DrawRiver == 1)
                {
                        if(river_index.r == 1 && river_index.g == 1 && river_index.b == 1)
                        {
                            // Land grey borders 
                            if (sea_index.r == 1 && (any(c1 != col) || any(c2 != col) || any(c3 != col) || any(c4 != col)) ) {                             
                                return float4 (0.55, 0.55, 0.55, 1);                    
                            }  
                        }
                        else{
                            // Current pixel is a river                    
                            return float4 (0, 0.88, 1, 1);    
                        }
                }
                else
                {// There are not rivers
                        // Land grey borders 
                        if (sea_index.r == 1 && (any(c1 != col) || any(c2 != col) || any(c3 != col) || any(c4 != col)) ) {                             
                            return float4 (0.55, 0.55, 0.55, 1);                    
                        }  
                }

                float4 index = tex2D(_RemapTex, IN.uv);      
                return tex2D(_PaletteTex, index.xy * 255.0 / 256.0 + float2(0.001953125, 0.001953125));     
            }
            ENDHLSL
        }
    }
}
