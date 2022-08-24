using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyController : Entity
{
    public static EnemyController Instance;
    public PlayerController playerTarget;
    // private trans
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        ani = GetComponent<Animator>();
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        playerTarget = playerObj.GetComponent<PlayerController>();
        canAttack = true;
    }

    void Update()
    {
        if (!canMove || !canAttack)
            return;
    }

    public void HandleEnemyTurn()
    {
        canMove = true;
        PathFinding.Instance.FindPath(this.currentNodePlaced, playerTarget.currentNodePlaced);
        if (GridManager.Instance.path.Count > 1)
            MoveToNode(GridManager.Instance.path[0], GameState.PlayerTurn);
        else
        {
            AttacK();
        }
    }

    public override void AttacK()
    {
        base.AttacK();
        GameManager.Instance?.SwitchState(GameState.PlayerTurn);
    }


    public override void MoveToNode(Node _newNode, GameState _newState)
    {
        base.MoveToNode(_newNode, _newState);
    }
}
