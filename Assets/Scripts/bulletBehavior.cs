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
    void Start()
    {

        destroyAfter = Time.timeSinceLevelLoad + despawnTime;
        //Debug.Log(enemy);
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (enemy != null&&enemy.activeInHierarchy)
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemies"))
        {
            //Debug.Log("health is now " +damage+" lower whenever we get around to that.");
            Destroy(this.gameObject);
            other?.GetComponent<EnemyCharacteristics>().Damage(damage);

            //Debug.Log("detroyed");
        }
    }
}
