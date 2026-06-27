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
    public TMP_Dropdown SongDropdown;
    private List<SongScObj> Songs;
    public string GameSceneName = "GameScene";

    private void Start()
    {
        StartGameButton.onClick.AddListener(StartGame);
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

    private void ChangeCurrentSong(SongScObj song)
    {
        SongSelector.Instance.CurrentSong = song;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(GameSceneName);
    }
}
