using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Toggle MetronomeToggle;
    public Button BackButton;

    private void Awake()
    {
        MetronomeToggle.isOn = PlayerPrefs.GetInt("Metronome", 1) == 1;
        MetronomeToggle.onValueChanged.AddListener(MetronomeToggled);
        
        BackButton.onClick.AddListener(Back);
    }

    public void MetronomeToggled(bool isOn)
    {
        PlayerPrefs.SetInt("Metronome", isOn ? 1 : 0);
    }

    public void Back()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenuScene");
    }
}
