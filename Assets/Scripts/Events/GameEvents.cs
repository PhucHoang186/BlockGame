using UnityEngine;
using System;

public static class GameEvents
{
    public static Action<int> ON_HEALTH_CHANGED;
    public static Action<VisualGridType> ON_CHANGE_STATE;
}
