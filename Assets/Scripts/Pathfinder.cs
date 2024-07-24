using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Pathfinder : MonoBehaviour
{
    public enum PathType {DebugChargeBase,FollowPaths };
    public PathType MoveType;


    private Rigidbody body;
    private NavMeshAgent agent;

    [HideInInspector]
    public PathNode currentNode;

    
    
    void Start()
    {
        body=GetComponent<Rigidbody>();
        agent=GetComponent<NavMeshAgent>();
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
}
