using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    public EnemyScriptableObject Config { get; private set; }

    public bool isDead { get; private set; }
    public Transform mTransform { get; private set; }
    [SerializeField] protected float currentHealth;
    protected NavMeshAgent agent;
    
    private float timeForSlowDown;
    private float initalSpeed;

    public virtual void Initialize(EnemyScriptableObject Config)
    {
        this.Config = Config;
        
        gameObject.tag = "enemies";
        currentHealth = Config.maxHealth;
        agent = GetComponent<NavMeshAgent>();
        initalSpeed = Config.speed;

        mTransform = transform;
    }

    
    public bool TakeDamage(float amount)
    {
        if (currentHealth - amount < 0)
        {
            Die();
            return true;
        }

        currentHealth -= amount;
        return false;
    }
   
   
    public void Freeze(float duration)
    {
        timeForSlowDown = Time.timeSinceLevelLoad + duration;
    }

    public void Die(bool givesMoney = true, bool killAfter = false)
    {
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
        if (Time.timeSinceLevelLoad < timeForSlowDown)
            //If we are frozen right now
            agent.speed = initalSpeed * 0.5f;
        else
            agent.speed = initalSpeed;
        // if (currentHealth <= 0) Die();
    }

}