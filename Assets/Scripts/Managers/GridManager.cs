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
    public Dictionary<Vector3, Node> GetGrid => grids;
    private Node currentNodeOn;
    private Camera cam;

    public Node CurrentNodeOn => currentNodeOn;
    
    // Init player, object position
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Init()
    {
        grids = gridGenerator.Init(grids, nodeSize);
        BattleSystem.Instance.Init();
        GameManager.Instance.SwitchState(GameState.PlayerTurn);
        cam = Camera.main;
    }

    void Update()
    {
        GetCurrentNodeOn();
    }

    private void GetCurrentNodeOn()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            currentNodeOn = GetNodeByPosition(hit.transform.position);
        }
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
        float x = Mathf.Ceil(_pos.x / nodeSize);
        float z = Mathf.Ceil(_pos.z / nodeSize);

        var nodeId = new Vector3(x, 0f, z);
        if (grids.ContainsKey(nodeId))
        {
            return grids[nodeId];
        }
        else
        {
            Debug.LogError("false");
            return null;
        }
    }

    public Vector3 GetNodeIDByPosition(Vector3 _pos)
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
    public List<Node> GetNodesInRange(Node _startNode, int _range)
    {
        List<Node> inRangeNodes = new List<Node>();
        List<Node> previousStepNodes = new List<Node>();
        inRangeNodes.Add(_startNode);
        previousStepNodes.Add(_startNode);
        int step = 0;
        while (step < _range)
        {
            var neighborNodes = new List<Node>();
            foreach (Node node in previousStepNodes)
            {
                neighborNodes.AddRange(GridManager.Instance.GetNeighborNode(node));
            }
            inRangeNodes.AddRange(neighborNodes);
            previousStepNodes = neighborNodes;
            step++;
        }
        return inRangeNodes;
    }

    public List<Node> GetNeighborNodeHasEntity(Node _node)
    {
        List<Node> neighborNodes = new List<Node>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (Mathf.Abs(i) == Mathf.Abs(j))
                    continue;
                int x = _node.x + i;
                int y = _node.y + j;
                if (x >= 0 && x < gridGenerator.weight && y >= 0 && y < gridGenerator.height)
                {
                    if (grids[new Vector3(x, 0f, y)].currentObjectPlaced?.GetComponent<Entity>())
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