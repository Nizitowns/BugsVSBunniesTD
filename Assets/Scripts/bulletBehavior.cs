using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class bulletBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public Vector3 target;
    public float damage;
    public GameObject enemy;
    public float despawnTime = 20;

    private float destroyAfter;
    Vector3 moveDir;
    public bool DestroyIfEnemyDies;
    public bool SeenEnemy;

    public SpecialEffects specialEffects;

    public float SpecialEffectLength;
    public enum SpecialEffects {None,SlowEnemies };

    //Spawns + Entangles with enemies when killed (Bubbles foreach enemy etc.)
    public GameObject EntangleWhenKillEnemy;
    void Start()
    {

        destroyAfter = Time.timeSinceLevelLoad + despawnTime;
        //Debug.Log(enemy);
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (enemy != null&&enemy.activeInHierarchy&&enemy.GetComponent<EnemyCharacteristics>()!=null)
        {
            SeenEnemy = true;
            target = enemy.transform.position;
            moveDir = (target - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            if (DestroyIfEnemyDies||!SeenEnemy)
                Destroy(gameObject);
            else
                transform.position=Vector3.MoveTowards(transform.position,transform.position+ moveDir*10,speed * Time.deltaTime);
        }
        if(Time.timeSinceLevelLoad> destroyAfter)
        {//If the bullet has missed and its been destroyAfter seconds we should kill the bullet so we dont accumulate hundreds of missed bullets.
            Destroy(gameObject);
        }

    }
    int checkHealth;
    public int MaxHits = 1;
    int hits;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemies")&&other.GetComponent<EnemyCharacteristics>()!=null)
        {
            //Debug.Log("health is now " +damage+" lower whenever we get around to that.");
            if(specialEffects==SpecialEffects.SlowEnemies)
            {
                other.GetComponent<EnemyCharacteristics>().Freeze(SpecialEffectLength);
            }

            if(other.GetComponent<EnemyCharacteristics>().Damage(damage)&& EntangleWhenKillEnemy!=null)
            {
                GameObject g= Instantiate(EntangleWhenKillEnemy, transform.position, transform.rotation, transform.parent);
                g.transform.localScale*=other.transform.localScale.x;
                other.transform.parent = g.transform;
                other.transform.localPosition = new Vector3(0,-0.25f,0);
                other.gameObject.layer = g.gameObject.layer;
                other.transform.tag = g.gameObject.tag;
                Destroy(other.GetComponent<EnemyCharacteristics>());
            }

            hits++;
            if(hits>= MaxHits)
                Destroy(this.gameObject);
            else if(hits<=1)
            {
                Destroy(this.gameObject, 0.1f);
            }
            //Debug.Log("detroyed");
        }
    }
}
