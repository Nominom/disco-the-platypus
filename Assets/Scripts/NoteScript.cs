using System;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    public int index;
    public NoteType noteType;
    public NoteDir dir;
    public float length;

    public Material upMat;
    public Material downMat;
    public Material leftMat;
    public Material rightMat;

    public void Initialize(int index, NoteType noteType, NoteDir dir, float length)
    {
        this.index = index;
        this.noteType = noteType;
        this.dir = dir;
        this.length = length;
        var mr = GetComponent<MeshRenderer>();
        switch (dir)
        {
            case NoteDir.Left:
                mr.material = leftMat;
                break;
            case NoteDir.Up:
                mr.material = upMat;
                break;
            case NoteDir.Down:
                mr.material = downMat;
                break;
            case NoteDir.Right:
                mr.material = rightMat;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SongManager.Instance.audioSource.isPlaying)
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
