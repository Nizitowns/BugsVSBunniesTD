using System;
using TMPro;
using UnityEngine;

public class UI_ScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalScore;
    [SerializeField] private TextMeshProUGUI _carrotBonus;
    [SerializeField] private TextMeshProUGUI _finalScore;

    private void OnEnable()
    {
        int test = 100000;
        UpdateText(ScoreManager.Instance.TotalScore,test , ScoreManager.Instance.TotalScore + test);
    }

    public void UpdateText(float totalScore, float carrotScore, float finalScore)
    {
        _totalScore.text = ((int)totalScore).ToString();
        _carrotBonus.text = ((int)carrotScore).ToString();
        _finalScore.text = ((int)finalScore).ToString();
    }
}