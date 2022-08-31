using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyController : MoveableEntity
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
        currentHealth = maxHealth;
    }

    void Update()
    {
    }

    public void HandleEnemyTurn()
    {
        canMove = true;
        PathFinding.Instance.FindPath(this.currentNodePlaced, playerTarget.currentNodePlaced);
        if (GridManager.Instance.path.Count > 1)
            MoveToPath(GridManager.Instance.path, GameState.PlayerTurn);
        else
        {
            AttacK(GameState.PlayerTurn);
        }
    }

    public override void AttacK(GameState _newState)
    {
        base.AttacK(_newState);
    }


    public override void MoveToNode(Node _newNode)
    {
        base.MoveToNode(_newNode);
    }
}
