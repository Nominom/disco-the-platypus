float4 _PS1FogColor;
float _PS1FogStart;
float _PS1FogEnd;

void Fog_float(float3 Position, float4 Color, out float4 Out)
{
    float dist = length(Position);
    float fogFactor = saturate(1 - (_PS1FogEnd - dist) / (_PS1FogEnd - _PS1FogStart));
    float4 alphaFactored = lerp(Color, _PS1FogColor, _PS1FogColor.a);
    Out = lerp(Color, alphaFactored, fogFactor);
}
