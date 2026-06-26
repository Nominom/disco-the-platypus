using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DanceAnimPlayer : MonoBehaviour
{
    private InputAction DanceInputAction;
    
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();

        DanceInputAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        if (DanceInputAction.ReadValue<Vector2>().x > 0.1f)
        {
            anim.Play("DanceInputRight");
        }
        
        if (DanceInputAction.ReadValue<Vector2>().x < -0.1f)
        {
            anim.Play("DanceInputLeft");
        }
        
        if (DanceInputAction.ReadValue<Vector2>().y > 0.1f)
        {
            anim.Play("DanceInputUp");
        }

        if (DanceInputAction.ReadValue<Vector2>().y < -0.1f)
        {
            anim.Play("DanceInputDown");
        }
    }
}
