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
    public AudioClip metronomeClip;
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
                    PlayMetronomeTick();
                }
            }
            yield return null;
        }
    }

    private void PlayMetronomeTick()
    {
        if (metronomeClip == null)
        {
            return;
        }

        AudioSource source = metronomeSource != null ? metronomeSource : audioSource;
        source.PlayOneShot(metronomeClip, metronomeVolume);
    }
}
