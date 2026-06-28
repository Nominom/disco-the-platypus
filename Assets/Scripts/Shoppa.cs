using System;
using System.Collections.Generic;
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

    private int selectedHat;
    public List<HatScOBj> hats;
    public List<bool> bought;
    public TextMeshProUGUI PriceText;

    public int SelectedHat => selectedHat;

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

        for (int i = 0; i < hats.Count; i++)
        {
            bool b = PlayerPrefs.GetInt($"Hat{i}", 0) == 1;
            if (i == 0)
            {
                b = true;
            }
            bought.Add(b);
        }

        selectedHat = PlayerPrefs.GetInt($"Hat", 0);
        UpdateStore();

        int monies = PlayerPrefs.GetInt("Monies", 0);

        if (monies < 0)
        {
            monies = 0;
            PlayerPrefs.SetInt("Monies", 0);
        }
        
        MoniesText.text = "MONIES: " + monies.ToString();
        
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
        
        selectedHat = PlayerPrefs.GetInt($"Hat", 0);

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

    public void NextHat()
    {
        selectedHat++;
        selectedHat %= hats.Count;
    }

    public void PreviousHat()
    {
        selectedHat--;
        if (selectedHat < 0)
            selectedHat = hats.Count - 1;
    }

    public void BuyHat()
    {
        if (!bought[selectedHat])
        {
            if (!HasMonies(hats[selectedHat].hatCost))
            {
                return;
            }
            DetractMonies(hats[selectedHat].hatCost);
            bought[selectedHat] = true;
            PlayerPrefs.SetInt($"Hat{selectedHat}", 1);
            PlayerPrefs.SetInt($"Hat", selectedHat);
        }
        else
        {
            PlayerPrefs.SetInt($"Hat", selectedHat);
        }
        
        UpdateStore();
    }

    public void UpdateStore()
    {
        PriceText.text = bought[selectedHat] ? "Owned" : $"{hats[selectedHat].hatCost}$";
    }
}
