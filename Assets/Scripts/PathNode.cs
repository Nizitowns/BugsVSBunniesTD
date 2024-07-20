using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public PathNode nextNode;
    public PathNode altNextNode;
    [Tooltip("How close enemies must get to this node before it is counted as reached.")]
    public float NodeRadius=0.5f;

    public PathNode getNext()
    {
        if(altNextNode == null)
        {
            return nextNode;
        }

        if(Random.Range(0f,1.0f)<=0.5)
        {
            return nextNode;
        }
        return altNextNode;
    }
}
