using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCharacteristics : MonoBehaviour
{
    public int MoneyReward=1;
    public float MaxHealth=30;
    [HideInInspector]
    private float health;
    NavMeshAgent agent;
    void Start()
    {
        gameObject.tag = "enemies";
        health = MaxHealth;
        agent=GetComponent<NavMeshAgent>();
        initalSpeed= agent.speed;
    }
    float timeForSlowDown;
    float initalSpeed;

    public void Freeze(float duration)
    {
        timeForSlowDown = Time.timeSinceLevelLoad + duration;
    }
    public bool Damage(float amount)
    {
        health = Mathf.Max(0, health - amount);
        if(health <= 0)
        {
            Die();
            return true;
        }
        else
        {
            //    Debug.Log("health is now " + health + " on " + GetInstanceID());
            return false;
        }
    }
    public void Die(bool givesMoney = true,bool killAfter=false)
    {
        if (killAfter)
        {
            Destroy(gameObject);
        }
        else
        {
            if (GetComponent<Rigidbody>())
                Destroy(GetComponent<Rigidbody>());//Destroy Rigidbody
            if (GetComponent<Collider>())
                Destroy(GetComponent<Collider>());//Destroy Trigger Collider
            if(GetComponent<Collider>())
                Destroy(GetComponent<Collider>());//Destroy Real Collider


            if (GetComponent<Pathfinder>())
                Destroy(GetComponent<Pathfinder>());//Destroy Pathfinder


            if (GetComponent<NavMeshAgent>())
                Destroy(GetComponent<NavMeshAgent>());//Destroy NavMeshAgent

            this.enabled = false;
        }
        if(givesMoney)
            MoneyManager.instance.AddMoney(MoneyReward);
    //    Debug.Log(GetInstanceID() + " is now dead");


    }
    void Update()
    {
        if(Time.timeSinceLevelLoad<timeForSlowDown)
        {//If we are frozen right now
            agent.speed = initalSpeed * 0.5f;
        }
        else
        {
            agent.speed = initalSpeed;
        }
        if(health <= 0)
        {
            Die();
        }
    }
}
