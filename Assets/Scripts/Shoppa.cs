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
    public Button Buy1;
    public int Buy1Price;
    public Button Buy2;
    public int Buy2Price;
    public Button Buy3;
    public int Buy3Price;

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
        }
        
        MoniesText.text = "MONIES: " + PlayerPrefs.GetInt("Monies", 0);
        Buy1.onClick.AddListener(BuyUno);
        Buy2.onClick.AddListener(BuyDos);
        Buy3.onClick.AddListener(BuyTres);
        ExitButton.onClick.AddListener(ExitToMainMenu);
        
        Buy1.GetComponentInChildren<TextMeshProUGUI>().text = "BUY " + Buy1Price;
        Buy2.GetComponentInChildren<TextMeshProUGUI>().text = "BUY " + Buy2Price;
        Buy3.GetComponentInChildren<TextMeshProUGUI>().text = "BUY " + Buy3Price;

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

    public void BuyUno()
    {
        if (HasMonies(Buy1Price))
        {
            PlayerPrefs.SetString("NotesStageMat", "Rainbow");
            DetractMonies(Buy1Price);
            Buy1.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
            Buy1.GetComponent<Button>().interactable = false;
        }
    }

    public void BuyDos()
    {
        if (HasMonies(Buy2Price))
        {
            // TBD
            DetractMonies(Buy2Price);
        }
    }

    public void BuyTres()
    {
        if (HasMonies(Buy3Price))
        {
            // TBD
            DetractMonies(Buy3Price);
        }
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
