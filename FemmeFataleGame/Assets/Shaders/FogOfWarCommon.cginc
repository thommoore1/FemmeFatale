#ifndef FOG_OF_WAR_COMMON_INCLUDED
#define FOG_OF_WAR_COMMON_INCLUDED

#include "UnityCG.cginc"
#include "UnitySprites.cginc" // _MainTex, _Color, _RendererColor, _Flip, SampleSpriteTexture

// Global texture published by FogOfWarVision: white = visible, black = fogged.
sampler2D _FogMask;

struct FogAppData
{
    float4 vertex   : POSITION;
    float4 color    : COLOR;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct FogV2F
{
    float4 vertex    : SV_POSITION;
    fixed4 color     : COLOR;
    float2 texcoord  : TEXCOORD0;
    float4 screenPos : TEXCOORD1;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

FogV2F FogVert(FogAppData IN)
{
    FogV2F OUT;
    UNITY_SETUP_INSTANCE_ID(IN);
    UNITY_INITIALIZE_OUTPUT(FogV2F, OUT);
    UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

    OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
    OUT.vertex = UnityObjectToClipPos(OUT.vertex);
    OUT.texcoord = IN.texcoord;
    OUT.color = IN.color * _Color * _RendererColor;
#ifdef PIXELSNAP_ON
    OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif
    OUT.screenPos = ComputeScreenPos(OUT.vertex);
    return OUT;
}

float SampleFogVisibility(float4 screenPos)
{
    return tex2D(_FogMask, screenPos.xy / screenPos.w).r;
}

#endif
