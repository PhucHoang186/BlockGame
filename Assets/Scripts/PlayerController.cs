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

    void MoveBlock()
    {

        if (Input.GetKey(KeyCode.UpArrow))
        {
            MoveToPosition(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            MoveToPosition(Vector3.back);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveToPosition(Vector3.left);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveToPosition(Vector3.right);
        }
    }
}
