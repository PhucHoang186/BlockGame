using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    Dictionary<Vector3, Node> grids;
    [SerializeField] GridGenerator gridGenerator;
    [SerializeField] float nodeSize;
    // [SerializeField] List<EntityObject> entityObjects;

    public List<Node> path;
    // Init player, object position
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void Init()
    {
        grids = gridGenerator.GridInit(grids, nodeSize);
        GameManager.Instance.SwitchState(GameState.PlayerTurn);
    }

    public void MoveToNode(Node _currentNode, Node _newNode, Entity _entity)
    {
        _currentNode.ReleaseNode();
        _newNode.PlaceObjectOnNode(_entity);
    }

    public Node GetNodeById(Vector3 _nodeId)
    {
        if (grids.ContainsKey(_nodeId))
        {
            return grids[_nodeId];
        }
        else
        {
            return null;
        }
    }

    public Node GetNodeByPosition(Vector3 _pos)
    {
        float x = _pos.x / nodeSize;
        float y = _pos.y / nodeSize - 1;
        float z = _pos.z / nodeSize;

        if (grids.ContainsKey(_pos / nodeSize))
        {
            return grids[_pos / nodeSize];
        }
        else
        {
            return null;
        }
    }

    public Vector3 ConvertPositionToNodeID(Vector3 _pos)
    {
        Vector3 newId = new Vector3(_pos.x / nodeSize, 0f, _pos.z / nodeSize);
        return newId;
    }

    public List<Node> GetNeighborNode(Node _node)
    {
        List<Node> neighborNodes = new List<Node>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                // if (i == 0 && j == 0)
                if (Mathf.Abs(i) == Mathf.Abs(j))
                    continue;
                int x = _node.x + i;
                int y = _node.y + j;
                if (x >= 0 && x < gridGenerator.weight && y >= 0 && y < gridGenerator.height)
                {
                    neighborNodes.Add(grids[new Vector3(x, 0f, y)]);
                }

            }
        }
        return neighborNodes;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var node in path)
        {
            Gizmos.DrawCube(node.transform.position + new Vector3(0f, 2f, 0f), Vector3.one * nodeSize);
        }
    }
}