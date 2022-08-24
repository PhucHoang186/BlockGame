using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class PlayerController : Entity
{
    public static PlayerController Instance;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        ani = GetComponent<Animator>();
        canAttack = true;
    }

    void Update()
    {
        if (!canMove || !canAttack)
            return;
        MoveBlock();
        if (Input.GetKeyDown(KeyCode.X))
            AttacK();
    }

    public void HandlePlayerTurn()
    {
        canMove = true;

    }

    public override void MoveToDirection(Vector3 _dir, GameState _newState)
    {
        base.MoveToDirection(_dir, _newState);
    }

    public override void AttacK()
    {
        base.AttacK();
        GameManager.Instance?.SwitchState(GameState.EnemyTurn);
    }

    void MoveBlock()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveToDirection(Vector3.forward, GameState.EnemyTurn);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveToDirection(Vector3.back, GameState.EnemyTurn);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveToDirection(Vector3.left, GameState.EnemyTurn);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveToDirection(Vector3.right, GameState.EnemyTurn);
        }
        return;
    }
}
