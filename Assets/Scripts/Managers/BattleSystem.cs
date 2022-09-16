using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    PlayerTurn,
    EnemyTurn,
}

public class BattleSystem : MonoSingleton<BattleSystem>
{
    public PlayerController currentSelectedPlayer;
    public List<MoveableEntity> playersList;
    public List<MoveableEntity> enemiesList;
    private BattleState currentBattleState;

    public void Init()
    {
        playersList = new List<MoveableEntity>();
        enemiesList = new List<MoveableEntity>();

        foreach (var pairs in GridManager.Instance.GetGrid)
        {
            if (pairs.Value.GetEntityType() == EntityType.Player)
            {
                playersList.Add((MoveableEntity)pairs.Value.currentObjectPlaced);
            }
            else if (pairs.Value.GetEntityType() == EntityType.Enemy)
            {
                enemiesList.Add((MoveableEntity)pairs.Value.currentObjectPlaced);
            }
        }
    }

    void Update()
    {
        HandleBattleState();
    }

    private void HandleBattleState()
    {
        if (currentBattleState == BattleState.PlayerTurn)
        {

        }
        else if (currentBattleState == BattleState.EnemyTurn)
        {

        }
    }
}
