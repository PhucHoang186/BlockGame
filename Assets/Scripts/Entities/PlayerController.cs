using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerController : MoveableEntity
{
    public static Action<List<Node>, GameState> ON_SELECT_PATH;

    public override void Start()
    {
        base.Start();
        ON_SELECT_PATH += MoveToPath;
    }

    void OnDestroy()
    {
        ON_SELECT_PATH -= MoveToPath;
    }

    public override IEnumerator MoveToPathCroutine(List<Node> _path, GameState _newState)
    {
        yield return base.MoveToPathCroutine(_path, _newState);
        GameEvents.ON_CHANGE_STATE?.Invoke(VisualGridType.Movement);
    }

}
