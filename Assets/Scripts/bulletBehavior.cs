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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("enemies"))
        {
            Debug.Log("health is now " +damage+" lower whenever we get around to that.");
            //Destroy(this);
        }
    }
}
