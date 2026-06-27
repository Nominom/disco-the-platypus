using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public SongScObj currentSong;
    public float songTime;
    public bool isPaused;
    
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
    private int _score = 0;

    public GameObject goodNoteVfx;
    public GameObject badNoteVfx;

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
        }
    }
    
    public int ScoreForGood = 10;
    public int ScoreForNice = 20;
    public int ScoreForPerfect = 50;
    
    private float _scoreMultiplier = 1f;
    
    [Header("Recording Mode")]
    public bool recordingMode;
    [Range(0.25f, 1.5f)]
    public float recordingSpeed = 1f;
    private InputAction playPauseAction;
    private InputAction eraseAction;
    private InputAction rewindAction;

    private int _combo = 0;
    private int _highestCombo = 0;
    private float _highestMult = 1.0f;
    
    public Action OnMetronomeTick;

    public int Combo
    {
        get
        {
            return _combo;
        }
        set
        {
            _combo = value;
            if (value > _highestCombo)
                _highestCombo = value;
            _scoreMultiplier = 1f + value / 10f;
            if (_scoreMultiplier > _highestMult)
                _highestMult = _scoreMultiplier;
            GameUI.Instance.UpdateCombo(_combo);
            GameUI.Instance.UpdateMultiplier(_scoreMultiplier.ToString("F"));
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        isPaused = false;
        
        playPauseAction = InputSystem.actions.FindAction("Play");
        eraseAction = InputSystem.actions.FindAction("Erase");
        rewindAction = InputSystem.actions.FindAction("Rewind");
        
        metronomeEnabled = PlayerPrefs.GetInt("Metronome", 1) == 1;

        if (recordingMode)
        {
            StartCoroutine(RecordSong(currentSong));
        }
        else
        {
            if (SongSelector.Instance)
            {
                globalAudioOffset = SongSelector.Instance.globalAudioOffset;
                StartCoroutine(PlaySong(SongSelector.Instance.CurrentSong));
            }
            else
                StartCoroutine(PlaySong(currentSong));
        }
        
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
            float heardTime = songTime - globalAudioOffset;
            
            int currentBeat = Mathf.FloorToInt((songTime) / beatInterval);


            bool spawned;
            do
            {
                spawned = false;
                if (currentSpawnNote < song.notes.Count)
                {
                    var note = song.notes[currentSpawnNote];
                    float noteTime = note.beat * beatInterval;
                    if (heardTime >= noteTime - noteSpawnOffsetInSongTime)
                    {
                        Debug.Log(
                            $"SPAWN: spawnTime: {heardTime}, noteTime: {note.beat * beatInterval}, offset: {noteSpawnOffsetInSongTime}");
                        noteSpawner.SpawnNote(currentSpawnNote, note);
                        currentSpawnNote++;
                        spawned = true;
                    }
                }
            } while (spawned);

            NoteDef[] currentBeatNotes = song.notes.Where(x => (x.beat == currentBeat || x.beat == currentBeat + 1) && !processedNotes.Contains(x.index)).ToArray();

            var input = DanceInput.GetInputPressed();
            if (input != InputDir.None)
            {
                Debug.Log($"{input.ToString()} at time: {heardTime}");
                bool inputHit = false;
                foreach (var note in currentBeatNotes)
                {
                    if (input.MatchesNote(note.noteDir))
                    {
                        inputHit = true;
                    }
                }
                
                if (!inputHit)
                    Combo = 0;
            }
            foreach (var note in currentBeatNotes)
            {
                float noteTime = note.beat * beatInterval;
                // Debug.Log($"Note time:{noteTime} song {songTime}");
                if (heardTime > noteTime + halfMissTiming)
                {
                    TriggerBeat(note.index, BeatHitType.Miss);
                }else if (heardTime > noteTime - halfMissTiming)
                {
                    
                    if (input.MatchesNote(note.noteDir))
                    {
                        float hitOffset = Mathf.Abs(heardTime - noteTime);
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

        SongEnded();
    }

    private void SongEnded()
    {
        Shoppa.Instance.AddMonies(Score);
        GameUI.Instance.ShowSongEndedScoreScreen(Score, _highestCombo, _highestMult);
    }

    private IEnumerator RecordSong(SongScObj song)
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
            audioSource.pitch = recordingSpeed;
            float beatInterval = 60f / Mathf.Max(1, song.bpm);
            songTime = audioSource.time - (song.bpmOffset * beatInterval);
            float heardTime = songTime - (globalAudioOffset * recordingSpeed);
            
            int currentBeatSongTime = Mathf.FloorToInt((songTime) / beatInterval);
            int currentBeatHeardRounded = Mathf.FloorToInt((heardTime) / beatInterval + 0.5f);

            bool spawned;
            do
            {
                spawned = false;
                if (currentSpawnNote < song.notes.Count)
                {
                    var note = song.notes[currentSpawnNote];
                    float noteTime = note.beat * beatInterval;
                    if (heardTime >= noteTime - noteSpawnOffsetInSongTime)
                    {
                        Debug.Log(
                            $"SPAWN: spawnTime: {heardTime}, noteTime: {note.beat * beatInterval}, offset: {noteSpawnOffsetInSongTime}");
                        noteSpawner.SpawnNote(currentSpawnNote, note);
                        currentSpawnNote++;
                        spawned = true;
                    }
                }
            } while (spawned);
            
            var input = DanceInput.GetInputPressed();
            if (input != InputDir.None)
            {
                Debug.Log($"{input.ToString()} at time: {heardTime} on beat {currentBeatHeardRounded}");

                var newNote = new NoteDef()
                {
                    beat = currentBeatHeardRounded,
                    subBeat = 0,
                    noteType = NoteType.Tap,
                    length = 0
                };

                if ((input & InputDir.Left) != 0 && !song.notes.Any(x =>  x.beat == currentBeatHeardRounded && x.noteDir == NoteDir.Left))
                {
                    song.notes.Add(newNote.With(NoteDir.Left));
                    currentSpawnNote++;
                }
                if ((input & InputDir.Right) != 0 && !song.notes.Any(x =>  x.beat == currentBeatHeardRounded && x.noteDir == NoteDir.Right))
                {
                    song.notes.Add(newNote.With(NoteDir.Right));
                    currentSpawnNote++;
                }
                if ((input & InputDir.Up) != 0 && !song.notes.Any(x =>  x.beat == currentBeatHeardRounded && x.noteDir == NoteDir.Up))
                {
                    song.notes.Add(newNote.With(NoteDir.Up));
                    currentSpawnNote++;
                }
                if ((input & InputDir.Down) != 0 && !song.notes.Any(x =>  x.beat == currentBeatHeardRounded && x.noteDir == NoteDir.Down))
                {
                    song.notes.Add(newNote.With(NoteDir.Down));
                    currentSpawnNote++;
                }
                
                song.notes.Sort((x, y) => x.beat.CompareTo(y.beat));
            }
            
            if (currentBeatSongTime >= 0 && currentBeatSongTime != lastBeat)
            {
                lastBeat = currentBeatSongTime;
                if (metronomeEnabled)
                {
                    PlayMetronomeTick(currentBeatSongTime);
                }
            }

            if (playPauseAction.WasPressedThisFrame())
            {
                PlayPause();
            }
            if (eraseAction.WasPressedThisFrame())
            {
                song.notes.RemoveAll(x => x.beat == currentBeatHeardRounded);
            }
            if (rewindAction.WasPressedThisFrame())
            {
                float newTime = Mathf.Clamp(audioSource.time - 5, 0, song.songClip.length);
                audioSource.time = newTime;

                foreach (var note in song.notes)
                {
                    float noteTime = note.beat * beatInterval;
                    if (newTime - (song.bpmOffset * beatInterval) < noteTime - noteSpawnOffsetInSongTime)
                    {
                        currentSpawnNote = note.index;
                        Debug.Log("Rewinded to note: " + currentSpawnNote);
                        break;
                    }
                }
                
                foreach (var noteScript in FindObjectsByType<NoteScript>())
                {
                    Destroy(noteScript.gameObject);
                }
            }
            
            yield return null;
        }
    }
    

    private void TriggerBeat(int index, BeatHitType hit)
    {
        Debug.Log($"PLAY: index: {index}, hitType: {hit}, songTime: {songTime}");
        
        // Skip scoring if note has been processed already
        // TODO: handle held notes
        if (processedNotes.Contains(index))
        {
            return;
        }
        
        noteSpawner.spawnedNotes.TryGetValue(index, out NoteScript noteScript);
        
        processedNotes.Add(index);
        switch (hit)
        {
            case BeatHitType.Good:
                Score += Mathf.RoundToInt(ScoreForGood * _scoreMultiplier);
                Combo++;
                break;
            case BeatHitType.Nice:
                Score += Mathf.RoundToInt(ScoreForNice * _scoreMultiplier);
                Combo += 2;
                break;
            case BeatHitType.Perfect:
                Score += Mathf.RoundToInt(ScoreForPerfect * _scoreMultiplier);
                Combo += 3;
                break;
            default:
                break;
        }
        
        if (hit != BeatHitType.Miss)
        {
            GameUI.Instance.UpdateScore(Score);
            NiceUI.Instance.SpawnNice(hit);
            
            if (noteScript)
            {
                Vector3 pos = noteScript.transform.position;
                pos.z = triggerPoint.position.z;
                Instantiate(goodNoteVfx, pos + new Vector3(0, 0.15f,0), noteScript.transform.rotation);
            }
        }
        else
        {
            if (noteScript)
            {
                Vector3 pos = noteScript.transform.position;
                pos.z = triggerPoint.position.z;
                Instantiate(badNoteVfx, pos+ new Vector3(0, 0.15f,0), noteScript.transform.rotation);
            }
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
        OnMetronomeTick?.Invoke();
    }

    private void PlayPause()
    {
        if (!isPaused)
        {
            audioSource.Pause();
            isPaused = true;
        }
        else
        {
            audioSource.UnPause();
            isPaused = false;
        }
    }

    public int GetCurrentScore()
    {
        return Score;
    }
}
