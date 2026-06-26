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
        return;
        
        if ((input & InputDir.Right) != 0)
        {
            anim.Play("DanceInputRight");
        }
        
        if ((input & InputDir.Left) != 0)
        {
            anim.Play("DanceInputLeft");
        }
        
        if ((input & InputDir.Up) != 0)
        {
            anim.Play("DanceInputUp");
        }

        if ((input & InputDir.Down) != 0)
        {
            anim.Play("DanceInputDown");
        }
    }
}
