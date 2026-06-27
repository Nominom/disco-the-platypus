using UnityEngine;

public class NotesStage : MonoBehaviour
{
    public Material RainbowMat;
    
    void Start()
    {
        string stageMat = PlayerPrefs.GetString("NotesStageMat");

        switch (stageMat)
        {
            case "Rainbow":
                GetComponent<Renderer>().material = RainbowMat;
                break;
            default:
                break;
        }
    }
}
