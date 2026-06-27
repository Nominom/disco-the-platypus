using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;
    
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI ComboText;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            UpdateScore(0);
            UpdateCombo(0);
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
}
