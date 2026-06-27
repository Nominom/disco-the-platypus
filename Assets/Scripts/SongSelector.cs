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
            Destroy(gameObject);
            Debug.Log("Multiple instances of SongSelector. Deleting self");
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
