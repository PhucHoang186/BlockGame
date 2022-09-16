using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VisualGridType
{
    Movement,
    Highlight,
    Attack,
}

public class VisualGridManager : MonoSingleton<VisualGridManager>
{
    [SerializeField] VisualGridType currentGridType;
    private Camera cam;
    private GridManager gridManager;
    private Node currentNodeOn;
    private Node previousNodeOn;
    public List<Node> inRangeNodes = new List<Node>();
    void Start()
    {
        cam = Camera.main;
        gridManager = GridManager.Instance;
        currentNodeOn = null;
    }

    void Update()
    {
        ToggleMousePosition();
        SelectedPlayer();
        // HandleVisualType();
    }

    public void ToggleMousePosition()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            currentNodeOn = gridManager.GetNodeByPosition(hit.transform.position);
            currentNodeOn.ToggleHover(true);
            if (previousNodeOn != currentNodeOn)
            {
                previousNodeOn?.ToggleHover(false);
            }
            previousNodeOn = currentNodeOn;
        }
    }


    private void ToggleMoveableNode(Node _startNode, int _moveRange)
    {
        inRangeNodes.Clear();
        inRangeNodes = gridManager.GetNodesInRange(_startNode, _moveRange);
        foreach (Node node in inRangeNodes)
        {
            var path = PathFinding.Instance.FindPath(_startNode, node);
            if (path?.Count > 0 && path?.Count <= _moveRange)
            {
                node.ToggleHighlight(true);
                node.canMove = true;
            }
        }
    }

    // public void HandleVisualType()
    // {
    //     switch (currentGridType)
    //     {
    //         case VisualGridType.Movement:
    //             if (Input.GetMouseButtonDown(0))
    //             {
    //                 if (currentNodeOn.canMove && currentNodeOn != null)
    //                 {
    //                     PathFinding.Instance.FindPath(PlayerController.Instance.currentNodePlaced, currentNodeOn);
    //                     PlayerController.ON_SELECT_PATH?.Invoke(GridManager.Instance.path, GameState.EnemyTurn);
    //                     SwitchVisualType(VisualGridType.Highlight);
    //                 }
    //             }
    //             ToggleMoveableNode(PlayerController.Instance.currentNodePlaced, PlayerController.Instance.moveRange);
    //             break;
    //         case VisualGridType.Highlight:
    //             break;
    //         case VisualGridType.Attack:
    //             break;
    //         default:
    //             break;
    //     }
    // }

    public void SwitchVisualType(VisualGridType _newType)
    {
        if (currentGridType == _newType) return;
        currentGridType = _newType;
    }

    public void SelectedPlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentNodeOn != null && !currentNodeOn.IsFreeNode() && currentNodeOn.GetEntityType() == EntityType.Player)
                currentNodeOn.ToggleSelect(true);
        }
    }
}
