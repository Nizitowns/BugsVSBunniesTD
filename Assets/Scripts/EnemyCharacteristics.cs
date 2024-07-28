using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacteristics : MonoBehaviour
{
    public float MaxHealth=30;
    [HideInInspector]
    private float health;
    void Start()
    {
        gameObject.tag = "enemies";
        health = MaxHealth;
    }
    public void Damage(float amount)
    {
        health = Mathf.Max(0, health - amount);
        if(health <= 0)
        {
            Die();
        }
        else
        {
        //    Debug.Log("health is now " + health + " on " + GetInstanceID());
        }
    }
    public void Die()
    {

        Destroy(gameObject);
    //    Debug.Log(GetInstanceID() + " is now dead");


    }
    void Update()
    {
        if(health <= 0)
        {
            Die();
        }
    }
}
