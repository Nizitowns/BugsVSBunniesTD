using UnityEngine;
using System.Collections;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class Pathfinder : MonoBehaviour
{
    public enum PathType {DebugChargeBase,FollowPaths };
    public PathType MoveType;


    private Rigidbody body;
    private NavMeshAgent agent;

    private IEnumerator softlock;


    [HideInInspector]
    public PathNode currentNode;
    public int health;
    
    void Start()
    {
        body=GetComponent<Rigidbody>();
        agent=GetComponent<NavMeshAgent>();
        softlock = softlockChecker();
        StartCoroutine(softlock);
    }
    void goToMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 intersectionPoint = hit.point;

            agent.SetDestination(intersectionPoint);
        }
    }
    void goToBaseCenter()
    {
        if (BaseCenter.Instance != null)
        {
            agent.SetDestination(BaseCenter.Instance.transform.position);
            
        }
    }
    bool goToNextNode()
    {
        if(currentNode == null)
        {
            currentNode = PathManager.Instance.getEntryNode();
        }

        agent.SetDestination(currentNode.transform.position);
        if (Vector3.Distance(transform.position, currentNode.transform.position) < currentNode.NodeRadius)
        {
            return true;
        }
        else
            return false;
    }
    void Update()
    {
        body.isKinematic = agent.isOnNavMesh;

        if(agent.enabled&&agent.isOnNavMesh)
        {
            if (MoveType == PathType.DebugChargeBase)
            {
                goToBaseCenter();
            }
            else
            {

                if (goToNextNode())
                {
                    //Just reached the nextNode
                    PathNode nextNode = currentNode.getNext();
                    if (nextNode != null)
                    {
                        currentNode = nextNode;
                    }

                }
            }
        }
    }
    Vector3 a, b;
    float distance;
    Collider m_collider;
    //check distance of self to previous location 5 seconds ago
    //if this distance is still within 20 of the old location, turn off 
    //collission to other enemy tagged items for 0.3 seconds
    //20 and 0.3 were chosen arbitrarily by playing around 
    IEnumerator softlockChecker()
    {
        
        while (true)
        {
            a = this.transform.position;
            yield return new WaitForSeconds(5);
            b = this.transform.position;
            distance = Vector3.Distance(a, b);
            m_collider=this.GetComponent<Collider>();
            //Debug.Log(this.gameObject+":\t" +distance);
            if (distance<20)
            {
                m_collider.enabled= false;
                //Debug.Log(this.gameObject.GetInstanceID() + " off at "+Time.time);
                yield return new WaitForSeconds(0.3f);
                m_collider.enabled = true;
                //Debug.Log(this.gameObject.GetInstanceID() + " on at " + Time.time);

            }
        }
    }
}
