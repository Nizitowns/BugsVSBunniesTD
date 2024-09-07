using DefaultNamespace.OnDeathEffects;
using UnityEngine;
using UnityEngine.AI;

public interface IEnemyUnit : IDamagable
{
    // TODO Add Modifiable Struct Unit
    public EnemyScriptableObject Config { get; }
    public IDebuffHandler DebuffHandler { get; }
    public Transform mTransform { get; }
    public bool IsDead { get; }
    public float Speed { get; set; }
    public void ReachedTheBase(bool givesMoney = true);
    public void Kill();
}

public abstract class Enemy : MonoBehaviour, IEnemyUnit, IDebuffable
{
    public EnemyScriptableObject Config { get; private set; }

    public IDebuffHandler DebuffHandler { get; private set; }
    public bool IsDead { get; private set; }
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
        DebuffHandler = new EnemyDebuffHandler(this);
    }
    
    private void Update()
    {
        if (IsDead) return;
        
        DebuffHandler.HandleDebuff();
    }

    public bool TakeDamage(float amount, bool killAfter = true)
    {
        if (currentHealth - amount < 0)
        {
            IsDead = true;
            MoneyManager.instance.AddMoney(Config.moneyReward);
            if (killAfter) Kill();
            return true;
        }

        currentHealth -= amount;
        return false;
    }
    
    public void ReachedTheBase(bool givesMoney = true)
    {
        if (givesMoney)
        {
            MoneyManager.instance.AddMoney(Config.moneyReward);
        }

        Kill();
    }

    public void Kill()
    {
        IsDead = true;
        Destroy(gameObject);
    }

    public void ApplyDebuff(Debuff newDebuff)
    {
        DebuffHandler.ApplyDebuff(newDebuff);
    }
}