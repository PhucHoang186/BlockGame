using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum VisualGridType
{
    Movement,
    Hover,
    Attack,
    Waiting,
    All,
}

public class VisualGridManager : MonoSingleton<VisualGridManager>
{

    [SerializeField] VisualGridType currentGridType;
    private Camera cam;
    private GridManager gridManager;
    private Node currentNodeOn;
    private Node previousNodeOn;
    public List<Node> inRangeNodes = new List<Node>();
    private bool isAttackState;

    void Start()
    {
        cam = Camera.main;
        gridManager = GridManager.Instance;
        GameEvents.ON_CHANGE_STATE += SwitchVisualType;
        currentNodeOn = null;
        GameEvents.ON_CHANGE_STATE?.Invoke(VisualGridType.Movement);
    }

    void OnDestroy()
    {
        GameEvents.ON_CHANGE_STATE -= SwitchVisualType;
    }

    void Update()
    {
        GetCurrentNodeOn();
        if (currentNodeOn != null)
        {
            ToggleMousePosition();
            HandleVisualAttackState();
            previousNodeOn = currentNodeOn;
        }
    }

    private void HandleVisualAttackState()
    {
        if (isAttackState)
        {
            if (previousNodeOn != currentNodeOn)
            {
                ReleaseVisual();
            }
            else
            {
                ToggleNodes(currentNodeOn, BattleSystem.Instance.currentSelectedPlayer.attackRange, VisualGridType.Attack, false);
            }
        }
    }

    private void GetCurrentNodeOn()
    {
        currentNodeOn = gridManager.CurrentNodeOn;
    }

    public void ToggleMousePosition()
    {
        currentNodeOn.ToggleHover(true);
        if (previousNodeOn != currentNodeOn)
        {
            previousNodeOn?.ToggleHover(false);
        }
    }



    private void ToggleNodes(Node _startNode, int _range, VisualGridType _type, bool _isUsePath = true)
    {
        inRangeNodes = gridManager.GetNodesInRange(_startNode, _range);
        foreach (Node node in inRangeNodes)
        {
            if (_isUsePath)
            {
                var path = PathFinding.Instance.FindPath(_startNode, node);
                if (path?.Count > 0 && path?.Count <= _range)
                {
                    node.ToggleNodeByType(true, _type);
                    if (_type == VisualGridType.Movement)
                    {
                        node.canMove = true;
                    }
                }
            }
            else
            {
                node.ToggleNodeByType(true, _type);
            }
        }
    }

    public void ReleaseVisual()
    {
        foreach (Node node in inRangeNodes)
        {
            node.ToggleNodeByType(false, VisualGridType.All);
        }
        inRangeNodes.Clear();

    }

    public void SwitchVisualType(VisualGridType _newType)
    {
        ReleaseVisual();
        currentGridType = _newType;
        isAttackState = _newType == VisualGridType.Attack;
        switch (currentGridType)
        {
            case VisualGridType.Movement:
                ToggleNodes(BattleSystem.Instance.currentSelectedPlayer.currentNodePlaced, BattleSystem.Instance.currentSelectedPlayer.moveRange, VisualGridType.Movement);
                break;
            case VisualGridType.Attack:
                break;
            case VisualGridType.Waiting:
                break;
            case VisualGridType.All:
                break;
            default:
                break;
        }
    }
    
}
