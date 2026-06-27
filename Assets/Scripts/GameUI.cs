using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;
    
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI ComboText;
    public TextMeshProUGUI MultiplierText;

    public GameObject ScoreScreen;
    public TextMeshProUGUI ScoreScreenScoreText;
    public TextMeshProUGUI ScoreScreenComboText;
    public TextMeshProUGUI ScoreScreenMultiplierText;
    public Button MainMenuButton;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            UpdateScore(0);
            UpdateCombo(0);
            UpdateMultiplier("1.00");

            ScoreScreen.SetActive(true);
            MainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0));
            ScoreScreen.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("More than one GameUI found!");
        }
    }

    public void UpdateScore(int score)
    {
        ScoreText.SetText("SCORE: " + score);
    }

    public void UpdateCombo(int combo)
    {
        ComboText.SetText("COMBO: " + combo);
    }

    public void UpdateMultiplier(string mult)
    {
        MultiplierText.SetText("MULTIPLIER: " + mult + "x");
    }

    public void ShowSongEndedScoreScreen(int score, int highestCombo, float highestMult)
    {
        ScoreScreen.SetActive(true);
        ScoreScreenScoreText.SetText("SCORE: " + score);
        ScoreScreenComboText.SetText("HIGHEST COMBO: " + highestCombo);
        ScoreScreenMultiplierText.SetText("HIGHEST MULTIPLIER: " + highestMult);
    }
}
