using UnityEngine;

public class SongSelector : MonoBehaviour
{
    public static SongSelector Instance;

    public SongScObj CurrentSong;
    public float globalAudioOffset;
    
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Expected when changing scenes back and forth
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        LoadAudioOffset();
    }

    public void SaveAudioOffset()
    {
        PlayerPrefs.SetFloat("GlobalAudioOffset", globalAudioOffset);
    }

    public void LoadAudioOffset()
    {
        globalAudioOffset = PlayerPrefs.GetFloat("GlobalAudioOffset");
    }
}
