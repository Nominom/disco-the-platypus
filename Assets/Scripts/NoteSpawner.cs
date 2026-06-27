using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform[] noteSpawnPoints;

    public Dictionary<int, NoteScript> spawnedNotes {get; private set;} = new Dictionary<int, NoteScript>();
    public void SpawnNote(int index, NoteType noteType, NoteDir dir, float length)
    {
        Transform spawn = noteSpawnPoints[(int)dir];
        var go = Instantiate(notePrefab, spawn.position, spawn.rotation);
        go.GetComponent<NoteScript>().Initialize(index, noteType, dir, length);
        spawnedNotes[index] = go.GetComponent<NoteScript>();
    }

    public void SpawnNote(int index, NoteDef note) => SpawnNote(index, note.noteType, note.noteDir, note.length);
}
 