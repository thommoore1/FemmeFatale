// Use on FLOORS and OBSTACLES. Looks normal when visible, desaturated + darkened when fogged.
Shader "FogOfWar/SpriteFogged"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

        _FogDesaturation ("Fog Desaturation", Range(0,1)) = 1
        _FogBrightness ("Fog Brightness", Range(0,1)) = 0.55
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex FogVert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "FogOfWarCommon.cginc"

            float _FogDesaturation;
            float _FogBrightness;

            fixed4 frag(FogV2F IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

                float visible = SampleFogVisibility(IN.screenPos);
                float grey = dot(c.rgb, float3(0.299, 0.587, 0.114));
                float3 fogged = lerp(c.rgb, grey.xxx, _FogDesaturation) * _FogBrightness;

                c.rgb = lerp(fogged, c.rgb, visible);
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}
