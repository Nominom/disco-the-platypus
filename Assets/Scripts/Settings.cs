using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Toggle MetronomeToggle;

    private void Awake()
    {
        MetronomeToggle.isOn = PlayerPrefs.GetInt("Metronome") == 1;
        MetronomeToggle.onValueChanged.AddListener(MetronomeToggled);
    }

    private void MetronomeToggled(bool isOn)
    {
        PlayerPrefs.SetInt("Metronome", isOn ? 1 : 0);
    }

    private void Back()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenuScene");
    }
}
