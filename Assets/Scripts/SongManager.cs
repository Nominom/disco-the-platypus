using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Game Settings")] 
    public float globalAudioOffset;
    public float perfectTiming = 0.1f;
    public float niceTiming = 0.15f;
    public float missTiming = 0.3f;
    
    
    private float halfPerfectTiming => perfectTiming * 0.5f;
    private float halfNiceTiming => niceTiming * 0.5f;
    private float halfMissTiming => missTiming * 0.5f;
    
    private HashSet<int> processedNotes  = new HashSet<int>();
    private int Score = 0;
    public int ScoreForMiss = 10;
    public int ScoreForNice = 20;
    public int ScoreForPerfect = 50;

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
        
        float noteSpawnOffsetInSongTime = Mathf.Abs(spawn.transform.position.z - triggerPoint.position.z) / song.notesSpeed;

        int lastBeat = -1;

        while (audioSource.isPlaying)
        {
            float beatInterval = 60f / Mathf.Max(1, song.bpm);
            songTime = audioSource.time - (song.bpmOffset * beatInterval);
            
            int currentBeat = Mathf.FloorToInt((songTime) / beatInterval);

            if (currentSpawnNote < song.notes.Count)
            {
                var note  = song.notes[currentSpawnNote];
                float noteTime = note.beat * beatInterval;
                if (songTime >= noteTime - noteSpawnOffsetInSongTime)
                {
                    Debug.Log($"SPAWN: spawnTime: {songTime}, noteTime: {note.beat * beatInterval}, offset: {noteSpawnOffsetInSongTime}");
                    noteSpawner.SpawnNote(currentSpawnNote, note);
                    currentSpawnNote++;
                }
            }

            NoteDef[] currentBeatNotes = song.notes.Where(x => (x.beat == currentBeat || x.beat == currentBeat + 1) && !processedNotes.Contains(x.index)).ToArray();

            var input = DanceInput.GetInputPressed();
            if (input != InputDir.None)
            {
                Debug.Log($"{input.ToString()} at time: {songTime}");
            }
            foreach (var note in currentBeatNotes)
            {
                float noteTime = note.beat * beatInterval;
                // Debug.Log($"Note time:{noteTime} song {songTime}");
                if (songTime > noteTime + halfMissTiming)
                {
                    TriggerBeat(note.index, BeatHitType.Miss);
                }else if (songTime > noteTime - halfMissTiming)
                {
                    
                    if (input.MatchesNote(note.noteDir))
                    {
                        float hitOffset = Mathf.Abs(songTime - noteTime);
                        if (hitOffset < halfPerfectTiming)
                            TriggerBeat(note.index, BeatHitType.Perfect);
                        else if (hitOffset < halfNiceTiming)
                            TriggerBeat(note.index, BeatHitType.Nice);
                        else
                            TriggerBeat(note.index, BeatHitType.Good);
                    }
                }
            }
            
            if (currentBeat >= 0 && currentBeat != lastBeat)
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

    private void TriggerBeat(int index, BeatHitType hit)
    {
        Debug.Log($"PLAY: index: {index}, hitType: {hit}, songTime: {songTime}");
        processedNotes.Add(index);
        switch (hit)
        {
            case BeatHitType.Miss:
                Score += ScoreForMiss;
                break;
            case BeatHitType.Nice:
                Score += ScoreForNice;
                break;
            case BeatHitType.Perfect:
                Score += ScoreForPerfect;
                break;
            default:
                break;
        }
        GameUI.Instance.UpdateScore(Score);
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
