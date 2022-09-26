using UnityEngine;
using System;

public static class GameEvents
{
    public static Action<int> ON_HEALTH_CHANGED;
    public static Action<PlayerState> ON_CHANGE_PLAYER_STATE;
}
