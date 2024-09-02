using UnityEngine;
using UnityEngine.AI;

public interface IEnemyUnit : IDamagable
{
    public EnemyScriptableObject Config { get; }
    public IDebuffable Debuffable { get; }
    public Transform mTransform { get; }
    public float Speed { get; set; }
}

public abstract class Enemy : MonoBehaviour, IEnemyUnit
{
    public EnemyScriptableObject Config { get; private set; }

    public IDebuffable Debuffable { get; private set; }
    public bool isDead { get; private set; }
    public Transform mTransform { get; private set; }
    [SerializeField] protected float currentHealth;
    public NavMeshAgent agent;

    public float Speed
    {
        get => agent.speed;
        set =>agent.speed = value;
    }
 
    public virtual void Initialize(EnemyScriptableObject Config)
    {
        this.Config = Config;
        mTransform = transform;
        agent = GetComponent<NavMeshAgent>();
        Speed = Config.speed;
        currentHealth = Config.maxHealth;
        gameObject.tag = "enemies";
        Debuffable = new EnemyDebuffHandler(this);
    }
    
    public bool TakeDamage(float amount)
    {
        if (currentHealth - amount < 0)
        {
            KillThis();
            return true;
        }

        currentHealth -= amount;
        return false;
    }
   
    public void KillThis(bool givesMoney = true, bool killAfter = false)
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
        Debuffable.HandleDebuff();
    }
}