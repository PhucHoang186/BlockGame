using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VisualGridManager : MonoSingleton<VisualGridManager>
{

    [SerializeField] VisualNodeType currentNodeType;
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
        currentNodeOn = null;
    }

    void OnDestroy()
    {
    }

    void Update()
    {
        GetCurrentNodeOn();
        if (currentNodeOn != null)
        {
            ToggleMousePosition();
            // HandleVisualAttackState();
            previousNodeOn = currentNodeOn;
        }
    }

    public VisualGridType GetCurrentType()
    {
        return currentGridType;
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
                // ToggleNodes(currentNodeOn, BattleSystem.Instance.currentSelectedPlayer.weaponRange, VisualNodeType.WeaponRange, false);
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

    public void ReleaseVisual()
    {
        foreach (Node node in inRangeNodes)
        {
            node.ToggleNodeByType(false, VisualNodeType.All);
        }
        inRangeNodes.Clear();
    }
}
