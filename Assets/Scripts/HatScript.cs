using UnityEngine;

public class HatScript : MonoBehaviour
{
    public int hatIndex = 0;

    private Shoppa shoppa;
    private MeshRenderer hatRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hatRenderer = GetComponent<MeshRenderer>();
        shoppa = Shoppa.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        hatRenderer.enabled = shoppa.SelectedHat == hatIndex;
    }
}
