using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform[] noteSpawnPoints;

    public void SpawnNote(int index, NoteType noteType, NoteDir dir, float length)
    {
        Transform spawn = noteSpawnPoints[(int)dir];
        var go = Instantiate(notePrefab, spawn.position, spawn.rotation);
        go.GetComponent<NoteScript>().Initialize(index, noteType, dir, length);
    }
}
 