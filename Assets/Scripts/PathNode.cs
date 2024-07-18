using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public PathNode nextNode;
    public PathNode altNextNode;

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
