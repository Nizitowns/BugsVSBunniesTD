using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public int MoneyReward;
    public float MaxHealth=30;
    private float health;

}