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
            previousNodeOn = currentNodeOn;
        }
    }

    public VisualNodeType GetCurrentType()
    {
        return currentNodeType;
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
        var gridList = GridManager.Instance.GetGridList();
        foreach (Node node in gridList)
        {
            node.ToggleNodeByType(false, VisualNodeType.All);
        }
    }
}
