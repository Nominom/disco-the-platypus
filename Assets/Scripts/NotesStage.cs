using UnityEngine;

public class NotesStage : MonoBehaviour
{
    public Material RainbowMat;
    
    void Start()
    {
        GetComponent<Renderer>().material = SongSelector.Instance.CurrentFlag.FlagMaterial;
    }
}
