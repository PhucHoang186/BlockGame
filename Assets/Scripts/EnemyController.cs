using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyController : Entity
{
    public static EnemyController Instance;
    public bool enemyCanMove;

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
        if (!enemyCanMove || !canAttack)
            return;
        MoveBlock();
        if (Input.GetKeyDown(KeyCode.X))
            StartCoroutine(AttackCoroutine());
    }

    public void HandleEnemyTurn()
    {
        enemyCanMove = true;
        PathFinding.Instance.FindPath(this.currentNodePlaced, GridManager.Instance.GetNodeById(new Vector3(9f,0f,5f)));
    }

    public override void MoveToPosition(Vector3 _dir)
    {
            //convert position to node id
        GridManager gridManager = GridManager.Instance;
        var newNodeId = gridManager.ConvertPositionToNodeID(transform.position) + _dir;
        Node newNode = gridManager.GetNodeById(newNodeId);

        //move
        if (newNode == null || newNode.isPlaced)
            return;

        transform.DORotateQuaternion(Quaternion.LookRotation(_dir), 0.5f);
        transform.DOJump(newNode.transform.position + offset, 0.5f, 1, moveTime).SetEase(Ease.InBack).OnComplete(() =>
        {
            // ON_FINISH_MOVEMENT?.Invoke();
            enemyCanMove = false;
            gridManager.MoveToNode(this.currentNodePlaced, newNode, this);
            GameManager.Instance.SwitchState(GameState.PlayerTurn); 
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
