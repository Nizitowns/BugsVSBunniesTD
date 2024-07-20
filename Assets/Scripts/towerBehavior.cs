using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerBehavior : MonoBehaviour
{
    public float AttackRadius;
    SphereCollider myCollider;
    // Start is called before the first frame update
    void Start()
    {
        myCollider=GetComponent<SphereCollider>();
        myCollider.radius=AttackRadius;
        Debug.Log("hello");
    }
    
    List<int> list = new List<int>();
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other+" "+other.GetInstanceID()+" entered");
        list.Add(other.GetInstanceID());
        //printList(list);

    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(other + " " + other.GetInstanceID()+ " exited");
        list.Remove(other.GetInstanceID());
        //printList(list);
        

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
    // Update is called once per frame
    void Update()
    {
        
    }
}
