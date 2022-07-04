using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static PathFinding Instance;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void FindPath(Node _startNode, Node _endNode)
    {
        List<Node> openNodeList = new List<Node>();
        List<Node> closeNodeList = new List<Node>();
        openNodeList.Add(_startNode);
        while (openNodeList.Count > 0)
        {
            Node currentNode = openNodeList[0];
            for (int i = 1; i < openNodeList.Count; i++)
            {
                if (openNodeList[i].fCost < currentNode.fCost || (openNodeList[i].fCost == currentNode.fCost && openNodeList[i].hCost < currentNode.hCost))
                {
                    currentNode = openNodeList[i];
                }
            }
            openNodeList.Remove(currentNode);
            closeNodeList.Add(currentNode);
            if (currentNode == _endNode)
            {
                RetracePath(_startNode, _endNode);
                return;
            }
            foreach (var neighborNode in GridManager.Instance.GetNeighborNode(currentNode))
            {
                if (neighborNode.isPlaced || closeNodeList.Contains(neighborNode))
                    continue;
                float newMovementCostToNeighborNode = currentNode.gCost + GetDistance(currentNode, neighborNode);
                if (newMovementCostToNeighborNode < neighborNode.gCost || !openNodeList.Contains(neighborNode))
                {
                    neighborNode.gCost = newMovementCostToNeighborNode;
                    neighborNode.hCost = GetDistance(neighborNode, _endNode);
                    neighborNode.parent = currentNode;
                    if (!openNodeList.Contains(neighborNode))
                    {
                        openNodeList.Add(neighborNode);
                    }
                }
            }
        }
    }

    public void RetracePath(Node _startNode, Node _endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = _endNode;
        while (currentNode != _startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        GridManager.Instance.path = path;
    }

    public float GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.x - nodeB.x);
        int dstY = Mathf.Abs(nodeA.y - nodeB.y);
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);

        return 14 * dstX + 10 * (dstY - dstX);
    }
}
