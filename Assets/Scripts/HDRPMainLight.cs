using UnityEngine;

[ExecuteAlways]
public class HDRPMainLight : MonoBehaviour
{
    public Color ambientColor = new Color(0.05f, 0.05f, 0.05f);
    public Light[] lights;

    void Update()
    {
        Shader.SetGlobalVector("_PS1AmbientColor", new Vector4(ambientColor.r, ambientColor.g, ambientColor.b, 1.0f));

        Vector4[] directions = new Vector4[10];
        Vector4[] colors = new Vector4[10];
        Vector4[] ranges = new Vector4[10];
        Vector4[] spotDirections = new Vector4[10];

        for (int i = 0; i < 10; i++)
        {
            if (i < lights.Length && lights[i])
            {
                Light l = lights[i];
                float intensity;

                switch (l.type)
                {
                    case LightType.Directional:
                        // Lux, typical sunlight is ~130000
                        intensity = l.intensity / 130000f;
                        directions[i] = new Vector4(-l.transform.forward.x, -l.transform.forward.y, -l.transform.forward.z, 0f);
                        break;
                    case LightType.Spot:
                        intensity = l.intensity / 800f;
                        directions[i] = new Vector4(l.transform.position.x, l.transform.position.y, l.transform.position.z, 1f);
                        // x = range, y = spot angle cosine, z = inner angle cosine
                        float outerAngle = Mathf.Cos(l.spotAngle * 0.5f * Mathf.Deg2Rad);
                        float innerAngle = Mathf.Cos(l.spotAngle * 0.3f * Mathf.Deg2Rad);
                        ranges[i] = new Vector4(l.range, outerAngle, innerAngle, 1f); // w=1 flags as spot
                        break;
                    case LightType.Point:
                        intensity = l.intensity / 800f;
                        directions[i] = new Vector4(l.transform.position.x, l.transform.position.y, l.transform.position.z, 1f);
                        ranges[i] = new Vector4(l.range, 0f, 0f, 0f); // w=0 flags as point
                        break;
                    default:
                        intensity = l.intensity / 130000f;
                        directions[i] = new Vector4(-l.transform.forward.x, -l.transform.forward.y, -l.transform.forward.z, 0f);
                        break;
                }

                if (l.type == LightType.Spot)
                {
                    spotDirections[i] = new Vector4(l.transform.forward.x, l.transform.forward.y, l.transform.forward.z, 0f);
                }
                else
                {
                    spotDirections[i] = Vector4.zero;
                }

                colors[i] = new Vector4(l.color.r, l.color.g, l.color.b, intensity);
                // ranges[i] = new Vector4(l.range, 0f, 0f, 0f);
            }
            else
            {
                directions[i] = Vector4.zero;
                colors[i] = Vector4.zero;
                ranges[i] = Vector4.zero;
                spotDirections[i] = Vector4.zero;
            }
        }

        Shader.SetGlobalVectorArray("_PS1LightDirections", directions);
        Shader.SetGlobalVectorArray("_PS1LightColors", colors);
        Shader.SetGlobalVectorArray("_PS1LightRanges", ranges);
        Shader.SetGlobalVectorArray("_PS1SpotDirections", spotDirections);
    }
}