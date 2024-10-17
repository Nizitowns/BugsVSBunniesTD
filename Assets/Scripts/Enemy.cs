using DefaultNamespace;
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
    public void Kill(bool givesMoney);
    public float offset => Config.offset;
}

public abstract class Enemy : MonoBehaviour, IEnemyUnit, IDebuffable
{
    public EnemyScriptableObject Config { get; private set; }

    public IDebuffHandler DebuffHandler { get; private set; }
    public bool IsDead { get; private set; }
    public Transform mTransform { get; private set; }
    [SerializeField] protected float currentHealth;
    public NavMeshAgent agent;
    private UIEnemyHealthBar _healthBar;

    [Header("Sound FX")]
    [SerializeField] private AudioClip DeathSX;

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

        if (Config.isBoss)
        {
            _healthBar = Instantiate(CommonPrefabHolder.Instance.HealtBarPrefab).GetComponent<UIEnemyHealthBar>();
            _healthBar.Init(mTransform);
            _healthBar.gameObject.SetActive(false);
        }
    }
    
    private void Update()
    {
        if (IsDead) return;
        
        DebuffHandler.HandleDebuff();
    }

    public bool TakeDamage(float amount, bool killAfter = true)
    {
        if (IsDead) return false;
        
        if (currentHealth - amount < 0)
        {
            IsDead = true;
            if (killAfter) Kill(true);
            return true;
        }

        currentHealth -= amount;
        if (Config.isBoss)
        {
            _healthBar.UpdateHeahtBar(currentHealth, Config.maxHealth);
            
            if(!_healthBar.gameObject.activeInHierarchy)
                _healthBar.gameObject.SetActive(true);

        }
        return false;
    }

    public void Kill(bool givesMoney)
    {
        EnemySpawner.LatestLaunched?.LogKilledEnemy();
        if (givesMoney)
        {
            MoneyManager.instance.AddMoney(Config.moneyReward);
            SoundFXPlayer.PlaySFX(SoundFXPlayer.Source, DeathSX);
        }
        IsDead = true;
        if(_healthBar != null)
            Destroy(_healthBar.gameObject);
        Destroy(gameObject);
    }

    public void ApplyDebuff(Debuff newDebuff)
    {
        DebuffHandler.ApplyDebuff(newDebuff);
    }
}