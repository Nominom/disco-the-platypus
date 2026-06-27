using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AudioDelayCalibrator : MonoBehaviour
{
    
    public AudioClip metronomeClipHigh;
    public AudioClip metronomeClipLow;
    public AudioSource metronomeSource;
    
    [Header("PulseText")]
    public RectTransform text;
    public AnimationCurve textPulse;
    public float pulseTime;
    
    public int numBeats = 30;
    private InputAction anyAction;

    private List<float> delays = new List<float>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anyAction = InputSystem.actions.FindAction("AnyKey");
        StartCoroutine(CalibrateDelay());
    }


    IEnumerator CalibrateDelay()
    {
        metronomeClipHigh.LoadAudioData();
        metronomeClipLow.LoadAudioData();

        yield return new WaitForSeconds(1);
        
        float delayBetweenBeats = 60f / 80f;
        

        int beat = 0;

        for (beat = 0; beat < numBeats; beat++)
        {
            float currWait = 0f;
            
            var clip = beat % 4 == 0 ? metronomeClipHigh : metronomeClipLow;
            metronomeSource.clip = clip;
            metronomeSource.Play();

            bool canPress = true;
            while (currWait < delayBetweenBeats)
            {
                yield return null;
                currWait += Time.deltaTime;

                if (canPress && anyAction.WasPressedThisFrame() && currWait < delayBetweenBeats * 0.7)
                {
                    delays.Add(currWait);
                    canPress = false;
                    StartCoroutine(PulseText());

                    text.GetComponentInChildren<TextMeshProUGUI>().text =
                        $"Match the beat by pressing any key\n{(int)(GetMedianDelay() * 1000)}ms";
                }
            }
        }

        if (delays.Count > numBeats / 2)
        {
            float medianDelay = GetMedianDelay();
            if (SongSelector.Instance)
            {
                SongSelector.Instance.globalAudioOffset = medianDelay;
                SongSelector.Instance.SaveAudioOffset();
                Debug.Log("Saved offset: " + medianDelay);
            }
        }
        
        text.GetComponentInChildren<TextMeshProUGUI>().text =
            $"Saved delay!\n{(int)(GetMedianDelay() * 1000)}ms";

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene("MainMenuScene");
    }

    private float GetMedianDelay()
    {
        float medianDelay = 0f;
        if (delays.Count > 0)
        {
            delays.Sort();
            medianDelay = delays[delays.Count / 2];
        }
        return medianDelay;
    }

    IEnumerator PulseText()
    {
        float time = 0;
        while (time < pulseTime)
        {
            float f = textPulse.Evaluate(time / pulseTime);
            text.transform.localScale = new Vector3(f, f, f);
            time += Time.deltaTime;
            yield return null;
        }
        text.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
