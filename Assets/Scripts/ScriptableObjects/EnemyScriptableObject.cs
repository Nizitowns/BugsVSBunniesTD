using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "New Data / Enemy Data")]
public class EnemyScriptableObject : ScriptableObject
{
    public GameObject prefab;
    public int moneyReward;
    public float maxHealth;
    public int carrotDamage;
    public float speed;
    [Tooltip("Helps to recenter Prefab")]
    public float offset;
}