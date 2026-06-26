using System.Collections;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public SongScObj currentSong;
    public float songTime;

    public NoteSpawner noteSpawner;
    public Transform spawn;
    public Transform triggerPoint;

    [Header("Metronome")]
    public bool metronomeEnabled;
    public AudioClip metronomeClipHigh;
    public AudioClip metronomeClipLow;
    public AudioSource metronomeSource;
    [Range(0f, 1f)]
    public float metronomeVolume = 1f;

    private int currentSpawnNote = 0;
    private int currentPlayNote = 0;

    [Header("Game Settings")] 
    public float globalAudioOffset;
    public float perfectTiming = 0.1f;
    public float goodTiming = 0.15f;
    public float missTiming = 0.3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;

        if (SongSelector.Instance)
            StartCoroutine(PlaySong(SongSelector.Instance.CurrentSong));
        else
            StartCoroutine(PlaySong(currentSong));
    }

    private IEnumerator PlaySong(SongScObj song)
    {
        while (!song.songClip.LoadAudioData())
        {
            yield return new WaitForSeconds(0.1f);
        }

        audioSource.loop = false;
        audioSource.clip = song.songClip;
        audioSource.Play();
        songTime = 0;
        
        float noteSpawnOffsetInSongTime = (spawn.transform.position.z - triggerPoint.position.z) / song.notesSpeed;

        int lastBeat = -1;

        while (audioSource.isPlaying)
        {
            float beatInterval = 60f / Mathf.Max(1, song.bpm);
            songTime = audioSource.time;

            if (currentSpawnNote < song.notes.Count)
            {
                var note  = song.notes[currentSpawnNote];
                if (songTime >= note.beat * beatInterval - noteSpawnOffsetInSongTime)
                {
                    Debug.Log($"SPAWN: songTime: {songTime}, spawnTime: {note.beat * beatInterval}, offset: {noteSpawnOffsetInSongTime}");
                    noteSpawner.SpawnNote(currentSpawnNote, note);
                    currentSpawnNote++;
                }
            }

            if (currentPlayNote < song.notes.Count)
            {
                var note  = song.notes[currentPlayNote];
                Debug.Log($"PLAY: songTime: {songTime}, spawnTime: {note.beat * beatInterval}, offset: {noteSpawnOffsetInSongTime}");
                
            }
            
            int currentBeat = Mathf.FloorToInt((songTime + (song.bpmOffset * beatInterval)) / beatInterval);
            if (currentBeat != lastBeat)
            {
                lastBeat = currentBeat;
                if (metronomeEnabled)
                {
                    PlayMetronomeTick(currentBeat);
                }
            }
            yield return null;
        }
    }

    private void PlayMetronomeTick(int currentBeat)
    {
        if (metronomeClipHigh == null || metronomeClipLow == null)
        {
            return;
        }
        var metronomeClip = currentBeat % 4 == 0 ? metronomeClipHigh : metronomeClipLow;

        AudioSource source = metronomeSource != null ? metronomeSource : audioSource;
        source.PlayOneShot(metronomeClip, metronomeVolume);
    }
}
