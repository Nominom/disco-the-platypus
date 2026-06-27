using UnityEngine;

public class InputVisualizer : MonoBehaviour
{
    public GameObject InputUpIndicator;
    public GameObject InputDownIndicator;
    public GameObject InputLeftIndicator;
    public GameObject InputRightIndicator;
    
    private void Update()
    {
        if (DanceInput.GetInputPressed() == InputDir.Up || DanceInput.GetInputHeld() == InputDir.Up)
        {
            InputUpIndicator.SetActive(true);
        }
        else
        {
            InputUpIndicator.SetActive(false);
        }
        
        if (DanceInput.GetInputPressed() == InputDir.Down || DanceInput.GetInputHeld() == InputDir.Down)
        {
            InputDownIndicator.SetActive(true);
        }
        else
        {
            InputDownIndicator.SetActive(false);
        }
        
        if (DanceInput.GetInputPressed() == InputDir.Left || DanceInput.GetInputHeld() == InputDir.Left)
        {
            InputLeftIndicator.SetActive(true);
        }
        else
        {
            InputLeftIndicator.SetActive(false);
        }
        
        if (DanceInput.GetInputPressed() == InputDir.Right || DanceInput.GetInputHeld() == InputDir.Right)
        {
            InputRightIndicator.SetActive(true);
        }
        else
        {
            InputRightIndicator.SetActive(false);
        }
    }
}
