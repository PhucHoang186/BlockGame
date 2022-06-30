using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class PlayerController : Entity
{
    public static PlayerController Instance;
    public bool canMove;
    [SerializeField] private float moveTime;
    [SerializeField] private float roateTime;
    private Animator ani;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        ani = GetComponent<Animator>();
        canMove = true;
    }

    void Update()
    {
        if (!canMove)
            return;
        MoveBlock();
    }

    public void HandlePlayerTurn()
    {
        canMove = true;
    }

    public void MoveToPosition(Vector3 _newPos)
    {
        canMove = false;
        // GridManager.Instance.ReleaseNodeFromVisited(this);
        transform.DORotateQuaternion(Quaternion.LookRotation(_newPos), 0.5f);
        transform.DOJump(transform.position + _newPos, 0.5f, 1, moveTime).SetEase(Ease.OutBack).OnComplete(() =>
        {
            // GridManager.Instance.SetGridNodeToVisited(this);
            canMove = true;
        });

    }


    void MoveBlock()
    {

        if (Input.GetKey(KeyCode.UpArrow))
        {
            MoveToPosition(Vector3.forward * 2f);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            MoveToPosition(Vector3.back * 2f);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveToPosition(Vector3.left * 2f);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveToPosition(Vector3.right * 2f);
        }
    }
}
