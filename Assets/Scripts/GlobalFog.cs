using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class GlobalFog : MonoBehaviour
{
    public bool Fog;
    public Color FogColor;
    public float FogStart;
    public float FogEnd;
    
    private GlobalKeyword fogKeyword;
    
    private void Start()
    {
        fogKeyword = GlobalKeyword.Create("_PS1FOG");
    }

    // Update is called once per frame
    void Update()
    {
        if (Fog)
        {
            Shader.EnableKeyword("_PS1FOG");
        }
        else
        {
            Shader.DisableKeyword("_PS1FOG");
        }
        Shader.SetGlobalFloat("_PS1FogStart", FogStart);
        Shader.SetGlobalFloat("_PS1FogEnd", FogEnd);
        Shader.SetGlobalColor("_PS1FogColor", FogColor);
    }
}
