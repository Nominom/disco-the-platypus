using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Popcat : MonoBehaviour
{
    public Sprite closedMouth;
    public Sprite openMouth;
    public float popDuration = 0.15f;
    public Image img;
    
    private float delay;
    
    private SongManager  songManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        delay = PlayerPrefs.GetFloat("GlobalAudioOffset");

        songManager = FindAnyObjectByType<SongManager>();
        songManager.OnMetronomeTick += Pop;
        img.sprite = closedMouth;
    }

    private void OnDestroy()
    {
        songManager.OnMetronomeTick -= Pop;
    }

    private void Pop()
    {
        StartCoroutine(PopImpl());
    }
    
    IEnumerator PopImpl()
    {
        yield return new WaitForSeconds(delay);

        img.sprite = openMouth;
        yield return new WaitForSeconds(popDuration);
        img.sprite = closedMouth;
    }
}
