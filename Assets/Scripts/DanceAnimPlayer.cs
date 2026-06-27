using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DanceAnimPlayer : MonoBehaviour
{
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        var input = DanceInput.GetInputPressed();

        if ((input & (InputDir.Left | InputDir.Right)) == (InputDir.Left | InputDir.Right))
        {
            anim.SetTrigger("ComboRightLeft");
        }
        
        if ((input & InputDir.Right) != 0)
        {
            anim.SetTrigger("DanceInputRight");
        }
        
        if ((input & InputDir.Left) != 0)
        {
            anim.SetTrigger("DanceInputLeft");
        }
        
        if ((input & InputDir.Up) != 0)
        {
            anim.SetTrigger("DanceInputUp");
        }

        if ((input & InputDir.Down) != 0)
        {
            anim.SetTrigger("DanceInputDown");
        }
    }
}
