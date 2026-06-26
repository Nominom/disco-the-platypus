using UnityEngine;

[CreateAssetMenu(fileName = "SongScObj", menuName = "Song/SongScObj")]
public class SongScObj : ScriptableObject
{
    public string songName;
    public AudioClip songClip;
    public float notesSpeed = 1f;
}
