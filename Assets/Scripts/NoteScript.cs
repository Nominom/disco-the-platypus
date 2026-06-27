using UnityEngine;

public class NoteScript : MonoBehaviour
{
    public int index;
    public NoteType noteType;
    public NoteDir dir;
    public float length;

    public void Initialize(int index, NoteType noteType, NoteDir dir, float length)
    {
        this.index = index;
        this.noteType = noteType;
        this.dir = dir;
        this.length = length;
    }

    // Update is called once per frame
    void Update()
    {
        if (!SongManager.Instance.isPaused)
        {
            float moveAmount = SongManager.Instance.currentSong.notesSpeed * Time.deltaTime;
            if (SongManager.Instance.recordingMode)
            {
                moveAmount *= SongManager.Instance.recordingSpeed;
            }
            transform.position =
                new Vector3(transform.position.x, transform.position.y, transform.position.z - moveAmount);
        }
    }
}
