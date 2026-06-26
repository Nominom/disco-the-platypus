using UnityEngine;
using UnityEngine.InputSystem;

public class TestVFX : MonoBehaviour
{
    public ParticleSystem vfx;
    public ParticleSystem badvfx;
    private InputAction useVfx;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        useVfx = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        if (useVfx.WasPressedThisFrame())
        {
            var rand =Random.Range(0, 2);
            if(rand == 0)
            {
                vfx.Play();
            }
            else
            {
                badvfx.Play();
            }
        }
    }
}
