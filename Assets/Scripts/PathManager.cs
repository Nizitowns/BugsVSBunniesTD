using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager Instance;
    [Tooltip("The start pathnodes that enemies can choose to spawn from.")]
    public List<PathNode> EntryNodes;

    public int firstPhaseNodeCount { get; private set; }
    public int secondPhaseNodeCount { get; private set; }

    void Start()
    {
        if (Instance == null)
        {
            var nodeCount = FindObjectsOfType<PathTileObject>();
            firstPhaseNodeCount = nodeCount.Length;
            Instance = this;
        }
        else if (Instance != this)
        {
            var nodeCount = FindObjectsOfType<PathTileObject>();
            firstPhaseNodeCount = Instance.firstPhaseNodeCount;
            secondPhaseNodeCount = nodeCount.Length - firstPhaseNodeCount;
            Instance = this;
        }
        
        // Debug.Log(firstPhaseNodeCount);
    }
    public PathNode getEntryNode()
    {
        return EntryNodes[Random.Range(0, EntryNodes.Count)];
    }
    private void OnDrawGizmos()
    {
        PathNode node =transform.GetChild(0).GetComponent<PathNode>();
        List<PathNode> visited = new List<PathNode>();
        Queue<PathNode> queue = new Queue<PathNode>();
        if (EntryNodes==null|| EntryNodes.Count <= 0)
        {
            EntryNodes = new List<PathNode>() { node };
            queue.Enqueue(node);
        }

        Gizmos.color = Color.gray;
        foreach (PathNode n in EntryNodes)
        {
            queue.Enqueue(n);
            Gizmos.DrawLine(transform.position, n.transform.position);
        }
        Gizmos.color = Color.yellow;
        while (queue.Count > 0)
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
