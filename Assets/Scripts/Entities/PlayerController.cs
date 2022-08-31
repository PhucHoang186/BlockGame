using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class PlayerController : MoveableEntity
{
    public static Action<List<Node>, GameState> ON_SELECT_PATH;
    public static PlayerController Instance;
    private List<Node> inRangeNodes = new List<Node>();
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        ani = GetComponent<Animator>();
        ON_SELECT_PATH += MoveToPath;
        canAttack = true;
        currentHealth = maxHealth;
    }

    void OnDestroy()
    {
        ON_SELECT_PATH -= MoveToPath;
    }

    void Update()
    {
        if (!canMove || !canAttack)
            return;

        if (Input.GetKeyDown(KeyCode.X))
            AttacK(GameState.EnemyTurn);
    }

    public void HandlePlayerTurn()
    {
        ToggleMoveableNode(currentNodePlaced);
        canMove = true;
    }

    private void ToggleMoveableNode(Node _startNode)
    {
        List<Node> previousStepNodes = new List<Node>();
        inRangeNodes.Add(_startNode);
        previousStepNodes.Add(_startNode);
        int step = 0;
        while (step < moveRange)
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
        foreach (Node node in inRangeNodes)
        {
            var path = PathFinding.Instance.FindPath(currentNodePlaced, node);
            if (path?.Count > 0 && path?.Count <= moveRange)
            {
                node.ToggleHighlightNode(true);
                node.canMove = true;
            }
        }
    }

    public override void MoveToPath(List<Node> _path, GameState _newState)
    {
        foreach (Node node in inRangeNodes)
        {
            node.ToggleHighlightNode(false);
            node.canMove = false;
        }
        inRangeNodes.Clear();

        base.MoveToPath(_path, _newState);
    }


    public override void AttacK(GameState _newState)
    {
        base.AttacK(_newState);
    }


}
