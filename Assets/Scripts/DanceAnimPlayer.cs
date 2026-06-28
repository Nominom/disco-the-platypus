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
        input |= SongManager.Instance.extraInput;

        if ((input & (InputDir.Left | InputDir.Right)) == (InputDir.Left | InputDir.Right))
        {
            anim.SetTrigger("ComboRightLeft");
        }
        
        else if ((input & InputDir.Right) != 0)
        {
            anim.SetTrigger("DanceInputRight");
        }
        
        else if ((input & InputDir.Left) != 0)
        {
            anim.SetTrigger("DanceInputLeft");
        }
        
        else if ((input & InputDir.Up) != 0)
        {
            anim.SetTrigger("DanceInputUp");
        }

        else if ((input & InputDir.Down) != 0)
        {
            anim.SetTrigger("DanceInputDown");
        }
    }
}
