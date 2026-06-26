using UnityEngine;

public class SongSelector : MonoBehaviour
{
    public static SongSelector Instance;

    public SongScObj CurrentSong;

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
            Debug.LogError("Multiple instances of SongSelector");
        }
    }
}
