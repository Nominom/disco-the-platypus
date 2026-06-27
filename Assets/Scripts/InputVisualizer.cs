using UnityEngine;

public class InputVisualizer : MonoBehaviour
{
    public GameObject InputUpIndicator;
    public GameObject InputDownIndicator;
    public GameObject InputLeftIndicator;
    public GameObject InputRightIndicator;
    
    private void Update()
    {
        if (DanceInput.GetInputHeld().MatchesNote(NoteDir.Up))
        {
            InputUpIndicator.SetActive(true);
        }
        else
        {
            InputUpIndicator.SetActive(false);
        }
        
        if (DanceInput.GetInputHeld().MatchesNote(NoteDir.Down))
        {
            InputDownIndicator.SetActive(true);
        }
        else
        {
            InputDownIndicator.SetActive(false);
        }
        
        if (DanceInput.GetInputHeld().MatchesNote(NoteDir.Left))
        {
            InputLeftIndicator.SetActive(true);
        }
        else
        {
            InputLeftIndicator.SetActive(false);
        }
        
        if (DanceInput.GetInputHeld().MatchesNote(NoteDir.Right))
        {
            InputRightIndicator.SetActive(true);
        }
        else
        {
            InputRightIndicator.SetActive(false);
        }
    }
}
