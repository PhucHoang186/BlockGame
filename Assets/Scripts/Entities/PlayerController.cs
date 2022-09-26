using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerController : MoveableEntity
{
    public static Action<List<Node>, Action> ON_SELECT_PATH;
    public static Action<List<Node>> ON_ATTACK;

    public int weaponRange;

    public override void Start()
    {
        base.Start();
        ON_SELECT_PATH += MoveToPath;
    }

    void OnDestroy()
    {
        ON_SELECT_PATH -= MoveToPath;
    }
}
