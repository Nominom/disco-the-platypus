using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[System.Serializable]
public class NoteDef
{
    [HideInInspector] public int index;
    public int beat; // full beat
    public int subBeat; // quarter beat
    public NoteType noteType;
    public NoteDir noteDir;
    public float length;
}
