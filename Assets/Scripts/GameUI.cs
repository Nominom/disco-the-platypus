using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;
    
    public TextMeshProUGUI ScoreText;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            UpdateScore(0);
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
}
