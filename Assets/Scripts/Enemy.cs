using System.Collections.Generic;
using DefaultNamespace.OnDeathEffects;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IDebuffable
{
    public EnemyScriptableObject Config { get; private set; }

    public bool isDead { get; private set; }
    public Transform mTransform { get; private set; }
    [SerializeField] protected float currentHealth;
    public NavMeshAgent agent;
    
    public virtual void Initialize(EnemyScriptableObject Config)
    {
        this.Config = Config;
        
        gameObject.tag = "enemies";
        currentHealth = Config.maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Config.speed;

        mTransform = transform;
    }
    
    public bool TakeDamage(float amount)
    {
        if (currentHealth - amount < 0)
        {
            KillEnemy();
            return true;
        }

        currentHealth -= amount;
        return false;
    }
   
    public void KillEnemy(bool givesMoney = true, bool killAfter = false)
    {
        if (isDead) return;
        
        if (killAfter)
        {
            Destroy(gameObject);
        }
        else
        {
            if (GetComponent<Rigidbody>())
                Destroy(GetComponent<Rigidbody>()); //Destroy Rigidbody
            if (GetComponent<Collider>())
                Destroy(GetComponent<Collider>()); //Destroy Trigger Collider
            if (GetComponent<Collider>())
                Destroy(GetComponent<Collider>()); //Destroy Real Collider
            if (GetComponent<Pathfinder>())
                Destroy(GetComponent<Pathfinder>()); //Destroy Pathfinder
            if (GetComponent<NavMeshAgent>())
                Destroy(GetComponent<NavMeshAgent>()); //Destroy NavMeshAgent

            enabled = false;
        }

        if (givesMoney)
            MoneyManager.instance.AddMoney(Config.moneyReward);

        // Debug.Log(GetInstanceID() + " is now dead");
        isDead = true;
    }

    private void Update()
    {
        UpdateDebuffs();
    }
    
    #region Debuff Logic

    public List<DebuffBase> WearDebuffs { get; private set; } = new();

    public void AddDebuff(DebuffBase debuffBase)
    {
        foreach (var debuff in WearDebuffs)
            if (debuff.GetType() == debuffBase.GetType())
            {
                debuff.WhatHappensOnStack();
                return;
            }
        
        WearDebuffs.Add(debuffBase);
        debuffBase.ApplyDebuff(this);
    }

    public void UpdateDebuffs()
    {
        for (int i = WearDebuffs.Count - 1; i >= 0; i--)
        {
            if (WearDebuffs[i].IsWearOff)
            {
                WearDebuffs.RemoveAt(i);
                continue;
            }
            WearDebuffs[i].UpdateDebuff(this);
        }
    }
    #endregion
}