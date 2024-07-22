using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager Instance;
    public PathNode EntryNode;

    void Start()
    {
        Instance = this;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        PathNode node =transform.GetChild(0).GetComponent<PathNode>();
        List<PathNode> visited = new List<PathNode>();
        Queue<PathNode> queue = new Queue<PathNode>();
        queue.Enqueue(node);
        if (EntryNode == null)
            EntryNode = node;
        Gizmos.DrawLine(transform.position, node.transform.position);
        while(queue.Count > 0)
        {
            PathNode cur= queue.Dequeue();
            visited.Add(cur);
            if (cur != null)
            {
                if (cur.nextNode != null)
                {
                    Gizmos.DrawLine(cur.transform.position, cur.nextNode.transform.position);
                    if(!visited.Contains(cur.nextNode))
                        queue.Enqueue(cur.nextNode);
                }
                if (cur.altNextNode != null)
                {
                    Gizmos.DrawLine(cur.transform.position, cur.altNextNode.transform.position);
                    if (!visited.Contains(cur.altNextNode))
                        queue.Enqueue(cur.altNextNode);
                }

            }
        }
    }
}
