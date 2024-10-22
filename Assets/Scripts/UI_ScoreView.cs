using System;
using TMPro;
using UnityEngine;

public class UI_ScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalScore;

    private void OnEnable()
    {
        UpdateText(ScoreManager.Instance.TotalScore);
    }

    public void UpdateText(float totalScore)
    {
        _totalScore.text = ((int)totalScore).ToString();
    }
}