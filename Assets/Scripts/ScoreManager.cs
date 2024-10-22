using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private TextMeshProUGUI ScoreText;
    
    public int TotalScore { get; private set; }

    private void Awake()
    {
        Instance = this;
        TotalScore = 0;
    }

    public void EnemyDied(IEnemyUnit unit)
    {
        var nodePassed = unit.pathNodeCount - 1;

        float scorableNodesPassed = 0;
        float scoreRatio = 0;
        
        switch (unit.ePhase)
        {
            case ePhase.FirstPhase:
                scorableNodesPassed = nodePassed - (PathManager.Instance.firstPhaseNodeCount * 0.1f);
                scoreRatio = scorableNodesPassed / PathManager.Instance.firstPhaseNodeCount;
                break;
            case ePhase.SecondPhase:
                scorableNodesPassed = nodePassed - (PathManager.Instance.secondPhaseNodeCount * 0.1f);
                scoreRatio = scorableNodesPassed / PathManager.Instance.secondPhaseNodeCount;
                break;
        }

        int calculatedScore = (100 - (int)(scoreRatio * 100));
        
        TotalScore += (int)(calculatedScore * (unit.Config.isBoss ? 1.5f : 1));

        ScoreText.text = TotalScore.ToString();
    }
}