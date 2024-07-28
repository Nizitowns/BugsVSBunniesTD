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
    public int damage;
    public GameObject enemy;
    void Start()
    {
        //Debug.Log(enemy);
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
        target = enemy.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

    }
    int checkHealth;
    private void OnTriggerEnter(Collider other)
    {
        checkHealth = other.GetComponent<Pathfinder>().health;
        if (other.CompareTag("enemies"))
        {
            //Debug.Log("health is now " +damage+" lower whenever we get around to that.");
            Destroy(this.gameObject);
            if ((checkHealth - damage) <= 0)
            {
                Destroy(other.gameObject);
                
                Debug.Log(other.GetInstanceID() + " is now dead");


                //prevent any tower from targeting an enemy that no longer exists
                GameObject[] towers = GameObject.FindGameObjectsWithTag("tower");
                foreach(GameObject t in towers)
                {
                    t.GetComponent<TowerBehavior>().targetList.Remove(other.gameObject);
                }
                
                


            }
            else
            {
                other.GetComponent<Pathfinder>().health-=damage;
                Debug.Log("health is now " + other.GetComponent<Pathfinder>().health + " on " + other.GetInstanceID());
            }
            //Debug.Log("detroyed");
        }
    }
}
