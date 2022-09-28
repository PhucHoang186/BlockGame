using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;

public class PlayerController : MoveableEntity
{
    public static Action<List<Node>, Action> ON_SELECT_PATH;
    public static Action<List<Node>> ON_ATTACK;

    public int weaponRange;

    public override void Start()
    {
        base.Start();
        ON_SELECT_PATH += MoveToPath;
        ON_ATTACK += OnAttack;
    }

    void OnDestroy()
    {
        ON_SELECT_PATH -= MoveToPath;
        ON_ATTACK -= OnAttack;
    }

    public void OnAttack(List<Node> attackNodeList)
    {
        if (canAttack)
        {
            var lookDir = attackNodeList[0].transform.position - this.transform.position;
            this.transform.DORotateQuaternion(Quaternion.Euler(0f, lookDir.y, 0f), 1f);
            Debug.LogError("Attack");
        }
    }
}
