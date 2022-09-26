using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System;

public class EnemyController : MoveableEntity
{
    public static Action <List<Node>, Action> ON_ENEMY_TURN;
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
        ON_ENEMY_TURN += MoveToPath;
    }

    void OnDestroy()
    {
        ON_ENEMY_TURN -= MoveToPath;
    }

    public override IEnumerator MoveToPathCroutine(List<Node> _path, Action cb)
    {
        int moveStep = 0;
        SetTriggerAnimation(Run);

        while (moveStep < moveRange)
        {
            moveStep += 1;

            MoveToNode(_path.FirstOrDefault());
            if (_path.Count > 0)
                _path.RemoveAt(0);
            // AttacK(_newState);
            yield return new WaitForSeconds(moveTime);
        }
        canMove = false;
        SetTriggerAnimation(Idle);
        cb?.Invoke();
    }
}
