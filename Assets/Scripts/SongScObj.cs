using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SongScObj", menuName = "Song/SongScObj")]
public class SongScObj : ScriptableObject
{
    public string songName;
    public AudioClip songClip;
    public float notesSpeed = 1f;
    public int bpm = 100;
    public float bpmOffset;
    public List<NoteDef> notes;

    private void OnValidate()
    {
        for (int i = 0; i < notes.Count; i++)
        {
            notes[i].index = i;
        }
    }
}
