Shader "Custom/Flicker"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Speed("Speed", Range(1.0,30)) = 15
        _Transparent("Transparent", Float) = 1
        
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
            float4 _MainTex_ST;
            float _Speed;
            float _Transparent;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv); 
                if(_Transparent < 1){
                    col.a = _Transparent;   
                }else{
                    half curSin = abs(sin(_Time.a * _Speed));
                    half alpha;
                    if(curSin > 0.5){
                        alpha =  1 ;
                    }else{
                        alpha =  0 ;
                    }
                    col.a = alpha;   
                }
                
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
