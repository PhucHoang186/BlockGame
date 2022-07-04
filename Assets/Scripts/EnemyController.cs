using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyController : Entity
{
    public static EnemyController Instance;
    public bool enemyCanMove;
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
        var playerObj =GameObject.FindGameObjectWithTag("Player");
        playerTarget = playerObj.GetComponent<PlayerController>();
        canAttack = true;
    }

    void Update()
    {
        if (!enemyCanMove || !canAttack)
            return;
        if (Input.GetKeyDown(KeyCode.X))
            StartCoroutine(AttackCoroutine());
    }

    public void HandleEnemyTurn()
    {
        enemyCanMove = true;
        PathFinding.Instance.FindPath(this.currentNodePlaced, playerTarget.currentNodePlaced);
        if(GridManager.Instance.path.Count>1)
            MoveToNode(GridManager.Instance.path[0]);
        else if(GridManager.Instance.path.Count == 1)
        {
            StartCoroutine(AttackCoroutine());
        }
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

        enemyCanMove = false;
        transform.DOJump(newNode.transform.position + offset, 0.5f, 1, moveTime).SetEase(Ease.InBack).OnComplete(() =>
        {
            // ON_FINISH_MOVEMENT?.Invoke();
            gridManager.MoveToNode(this.currentNodePlaced, newNode, this);
            GameManager.Instance.SwitchState(GameState.PlayerTurn); 
        });
    }

    public void MoveToNode(Node _newNode)
    {
        GridManager gridManager = GridManager.Instance;
        transform.DOJump(_newNode.transform.position + offset, 0.5f, 1, moveTime).SetEase(Ease.InBack).OnComplete(() =>
        {
            // ON_FINISH_MOVEMENT?.Invoke();
            enemyCanMove = false;
            gridManager.MoveToNode(this.currentNodePlaced, _newNode, this);
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
