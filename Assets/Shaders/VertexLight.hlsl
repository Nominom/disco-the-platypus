float4 _PS1LightDirections[10];
float4 _PS1LightColors[10];
float4 _PS1LightRanges[10];
float4 _PS1SpotDirections[10];
float4 _PS1AmbientColor;

void VertexLight_float(float3 PositionOS, float3 NormalOS, bool useNormals, out float3 Diffuse)
{
    float3 posWS = TransformObjectToWorld(PositionOS) + _WorldSpaceCameraPos.xyz;
    float3 normalWS = TransformObjectToWorldNormal(NormalOS);

    Diffuse = _PS1AmbientColor.rgb;

    for (int i = 0; i < 10; i++)
    {
        float3 lightDir;
        float attenuation = 1.0;

        if (_PS1LightDirections[i].w == 0)
        {
            lightDir = normalize(_PS1LightDirections[i].xyz);
        }
        else
        {
            float3 toLight = _PS1LightDirections[i].xyz - posWS;
            float dist = length(toLight);
            lightDir = toLight / dist;

            float range = _PS1LightRanges[i].x;
            float normalizedDist = saturate(dist / range);
            attenuation = saturate(1.0 - normalizedDist * normalizedDist);

            if (_PS1LightRanges[i].w > 0.5)
            {
                float outerAngle = _PS1LightRanges[i].y;
                float innerAngle = _PS1LightRanges[i].z;
                float3 spotDir = normalize(_PS1SpotDirections[i].xyz);
                float cosAngle = dot(-lightDir, spotDir);
                float spotAttenuation = saturate((cosAngle - outerAngle) / (innerAngle - outerAngle));
                attenuation *= spotAttenuation;
            }
        }

        float NdotL = useNormals ? saturate(dot(normalWS, lightDir)) : 1.0;

        Diffuse += _PS1LightColors[i].rgb * _PS1LightColors[i].a * NdotL * attenuation;
    }
}
