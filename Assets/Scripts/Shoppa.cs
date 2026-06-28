using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shoppa : MonoBehaviour
{
    public static Shoppa Instance;

    public GameObject Contents;
    public TextMeshProUGUI MoniesText;
    public Button ExitButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            SceneManager.sceneLoaded += SceneChanged;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        MoniesText.text = "MONIES: " + PlayerPrefs.GetInt("Monies", 0);
        
        ExitButton.onClick.AddListener(ExitToMainMenu);
        
        if (SceneManager.GetActiveScene().name == "Shoppa")
        {
            Contents.SetActive(true);
        }
        else
        {
            Contents.SetActive(false);
        }

        DontDestroyOnLoad(gameObject);
    }

    void SceneChanged(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Shoppa")
        {
            Contents.SetActive(true);
        }
        else
        {
            Contents.SetActive(false);
        }
    }

    private void ExitToMainMenu()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenuScene");
    }

    public void DetractMonies(int price)
    {
        int monies = PlayerPrefs.GetInt("Monies");
        monies -= price;
        PlayerPrefs.SetInt("Monies", monies);
        MoniesText.text = "MONIES: " + PlayerPrefs.GetInt("Monies", 0);
    }

    public bool HasMonies(int price)
    {
        int monies = PlayerPrefs.GetInt("Monies");
        return monies >= price;
    }

    public void AddMonies(int price)
    {
        int monies = PlayerPrefs.GetInt("Monies");
        monies += price;
        PlayerPrefs.SetInt("Monies", monies);
        MoniesText.text = "MONIES: " + PlayerPrefs.GetInt("Monies", 0);
        PlayerPrefs.Save();
    }
}
