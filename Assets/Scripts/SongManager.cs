using System.Collections;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public SongScObj currentSong;
    public float songTime;

    [Header("Metronome")]
    public bool metronomeEnabled;
    public AudioClip metronomeClipHigh;
    public AudioClip metronomeClipLow;
    public AudioSource metronomeSource;
    [Range(0f, 1f)]
    public float metronomeVolume = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;

        StartCoroutine(PlaySong(SongSelector.Instance.CurrentSong));
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

        float beatInterval = 60f / Mathf.Max(1, song.bpm);
        int lastBeat = -1;

        while (audioSource.isPlaying)
        {
            songTime = audioSource.time;
            
            int currentBeat = Mathf.FloorToInt(songTime / beatInterval);
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
