using UnityEngine;

public class NotesStage : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().material = SongSelector.Instance.CurrentFlag.FlagMaterial;
    }
}
