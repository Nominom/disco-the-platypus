using UnityEngine;

public class ScaleRenderTexture : MonoBehaviour
{
    public RenderTexture rt;
    private int _lastWidth;
    private int _lastHeight;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        OnScreenSizeChanged();
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.width != _lastWidth || Screen.height != _lastHeight)
        {
            _lastWidth  = Screen.width;
            _lastHeight = Screen.height;
            OnScreenSizeChanged();
        }
    }

    void OnScreenSizeChanged()
    {
        float width = Screen.width;
        float height = Screen.height;
        float aspect = width / height;
        rt.Release();
        rt.width = (int)(320 * aspect);
        rt.height = 320;
        rt.Create();
        
        _lastWidth  = Screen.width;
        _lastHeight = Screen.height;
    }
}
