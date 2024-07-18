using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Pathfinder : MonoBehaviour
{
    private Rigidbody body;
    private NavMeshAgent agent;
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
    void Update()
    {
        body.isKinematic = agent.isOnNavMesh;

        if(agent.enabled)
        {
            goToBaseCenter();
        }
    }
}
