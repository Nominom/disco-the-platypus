using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button StartGameButton;
    public Button CalibrationButton;
    public Button QuitButton;
    public Button ShoppaButton;
    public Button SettingsButton;
    public TMP_Dropdown SongDropdown;
    private List<SongScObj> Songs;
    public string GameSceneName = "GameScene";

    private void Start()
    {
        StartGameButton.onClick.AddListener(StartGame);
        CalibrationButton.onClick.AddListener(Calibration);
        QuitButton.onClick.AddListener(QuitGame);
        ShoppaButton.onClick.AddListener(Shoppa);
        SettingsButton.onClick.AddListener(SettingsMenu);
        
        SongDropdown.options.Clear();

        SongDropdown.AddOptions(new List<string> { "Easy", "Medium", "Hard" });
        SongDropdown.options.Clear();
        var songs = Resources.LoadAll("Songs");
        Songs = songs.Select(s => (SongScObj)s).ToList();
        foreach (var song in Songs)
        {
            SongDropdown.options.Add(new TMP_Dropdown.OptionData(song.songName));
        }
        
        SongDropdown.onValueChanged.AddListener(v => ChangeCurrentSong(Songs[v]));
        ChangeCurrentSong(Songs[0]);
    }

    private void SettingsMenu()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    private void ChangeCurrentSong(SongScObj song)
    {
        SongSelector.Instance.CurrentSong = song;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(GameSceneName);
    }

    public void Calibration()
    {
        SceneManager.LoadScene("CalibrationScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Shoppa()
    {
        SceneManager.LoadScene("Shoppa");
    }
}
