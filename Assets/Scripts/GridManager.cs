using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    Dictionary<Vector3, Node> grids;
    [SerializeField] GridGenerator gridGenerator;
    [SerializeField] float nodeSize;
    [SerializeField] List<EntityObject> entityObjects;
    // Init player, object position
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void Init()
    {
        grids = gridGenerator.GridInit(grids, nodeSize);
        foreach(EntityObject entity in entityObjects)
        {
            if(grids.ContainsKey(entity.nodeId))
            {
                var newEntityObject = Instantiate(entity.entityPref);
                var newEntity = newEntityObject.GetComponent<Entity>();
                grids[entity.nodeId].PlaceObjectOnNode(newEntity);
            }
        }
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
}