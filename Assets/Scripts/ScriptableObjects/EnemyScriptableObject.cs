using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "New Data / Enemy Data")]
public class EnemyScriptableObject : ScriptableObject
{
    public int moneyReward;
    public float maxHealth=30;
    public float _health;

}