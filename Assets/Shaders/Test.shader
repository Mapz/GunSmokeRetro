
Shader "Custom/Test"
{
     Properties {
        _MainTex ("Font Texture", 2D) = "white" {}
        _Color ("Text Color", Color) = (1,1,1,1)
        _ScanColor ("Scan Color", Color) = (1,1,1,1)
        _YPos("YPos",float) = 0
        _ScanHeight("_ScanHeight",float) = 10
        
    }
 
    SubShader {
 
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "DisableBatching" = "true"}
        Lighting Off Cull Off ZTest Always ZWrite Off Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha
 
        Pass { 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            
 
            #include "UnityCG.cginc"
 
            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
 
            struct v2f {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : TEXCOORD1;
            };
 
            sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform float4 _ScanColor;
            half _YPos;
            half _ScanHeight;
            
           
            v2f vert (appdata_t v)
            {
                v2f o;
                const float PI = 3.14159;
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                float time = _Time.z % 2*PI - PI;
                if(time > 0.5 * PI && time < PI){
                    time -= PI;
                }else if(time > -PI && time < -0.5* PI){
                    time+=PI;
                }
                if( v.vertex.y < sin(time*2)*_ScanHeight/2 + _YPos ){
                    o.color = _ScanColor;
                }else{
                    o.color = _Color;
                }
                o.vertex = UnityObjectToClipPos(v.vertex);
               
                return o;
            }
 
            half4 frag (v2f i) : COLOR
            {
                float4 col = i.color; 
                col.a *= tex2D(_MainTex, i.texcoord).a;
                return col;
            }
            ENDCG
        }
    }  
 
    SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Lighting Off Cull Off ZTest Always ZWrite Off Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            Color [_Color]
            SetTexture [_MainTex] {
                combine primary, texture * primary
            }
        }
    }
}