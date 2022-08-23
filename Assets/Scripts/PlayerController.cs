using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class PlayerController : Entity
{
    public static PlayerController Instance;
    public bool playerCanMove;
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
        if (!playerCanMove || !canAttack)
            return;
        MoveBlock();
        if (Input.GetKeyDown(KeyCode.X))
            StartCoroutine(AttackCoroutine());
    }

    public void HandlePlayerTurn()
    {
        playerCanMove = true;

    }

    public override void MoveToPosition(Vector3 _dir)
    {
        //convert position to node id
        GridManager gridManager = GridManager.Instance;
        var newNodeId = gridManager.ConvertPositionToNodeID(transform.position) + _dir;
        Node newNode = gridManager.GetNodeById(newNodeId);

        transform.DORotateQuaternion(Quaternion.LookRotation(_dir), 0.5f);
        //move
        if (newNode == null || newNode.isPlaced)
            return;

        canMove = false;
        transform.DOJump(newNode.transform.position + offset, 0.5f, 1, moveTime).SetEase(Ease.InBack).OnComplete(() =>
        {
            // ON_FINISH_MOVEMENT?.Invoke();
            gridManager.MoveToNode(this.currentNodePlaced, newNode, this);
            GameManager.Instance.SwitchState(GameState.EnemyTurn);
        });
    }

    void MoveBlock()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveToPosition(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveToPosition(Vector3.back);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveToPosition(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveToPosition(Vector3.right);
        }
        return;
    }
}
