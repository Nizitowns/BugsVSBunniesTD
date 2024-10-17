using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private TextMeshProUGUI ScoreText;
    private int currentScore = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddScore()
    {
        // currentScore += 
    }
}