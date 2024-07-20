using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class towerBehavior : MonoBehaviour
{

    SphereCollider myCollider;
    public float AttackRadius;
    //per second
    public float fireRate;
    // Start is called before the first frame update
    List<int> list = new List<int>();
    void Start()
    {
        myCollider=GetComponent<SphereCollider>();
        myCollider.radius=AttackRadius;
        Debug.Log("hello");


    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other+" "+other.GetInstanceID()+" entered");
        list.Add(other.GetInstanceID());
        //printList(list);
        StartCoroutine(startFire(other.gameObject));

    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(other + " " + other.GetInstanceID()+ " exited");
        list.Remove(other.GetInstanceID());
        //printList(list);
        StopCoroutine(startFire(other.gameObject));

    }
    //for testing
    void printList(List<int> num)
    {
        string numbs = "{ ";
        for (int i = 0; i < num.Count; i++)
        {
            numbs = numbs+ (num[i].ToString()+", ");
        }
        numbs += " }";
        Debug.Log(numbs);

    }
    //pick a random enemy to attack
    int enemyID;

    private IEnumerator startFire(GameObject unit)
    {
        enemyID = Random.Range(0, list.Count-1);
        while (true)
        {
            Debug.Log("PEW! hit enemy "+ enemyID+ " with id "+ unit.GetComponent<Collider>().GetInstanceID()+ " at location "+ unit.transform.position);
            yield return new WaitForSeconds(fireRate);
        }

        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
