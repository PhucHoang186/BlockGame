using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

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

    public override void Start()
    {
        base.Start();
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        playerTarget = playerObj.GetComponent<PlayerController>();
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

    private bool DetectTarget()
    {
        return false;
    }

    public override IEnumerator MoveToPathCroutine(List<Node> _path, GameState _newState)
    {
         int moveStep = 0;
        SetTriggerAnimation(Run);        

        while (moveStep < moveRange)
        {
            moveStep += 1;

            MoveToNode(_path.FirstOrDefault());
            if (_path.Count > 0)
                _path.RemoveAt(0);
            AttacK(_newState);
            yield return new WaitForSeconds(moveTime);
        }
        canMove = false;
        GameManager.Instance.SwitchState(_newState);
        SetTriggerAnimation(Idle);
    }

    public override void MoveToNode(Node _newNode)
    {
        base.MoveToNode(_newNode);
    }
}
