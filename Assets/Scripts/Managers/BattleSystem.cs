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
        SetCurrentPlayer((PlayerController)playersList[0]);
    }

    void Update()
    {
        HandleBattleState();
        SelectedPlayer();
    }

    private void HandleBattleState()
    {
        if (currentBattleState == BattleState.PlayerTurn)
        {
            HandlePlayerTurn();
        }
        else if (currentBattleState == BattleState.EnemyTurn)
        {

        }
    }

    private void HandlePlayerTurn()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Node currentNodeOn = GridManager.Instance.CurrentNodeOn;
            if (currentNodeOn == null) return;
            if (currentNodeOn.canMove)
            {
                PathFinding.Instance.FindPath(currentSelectedPlayer.currentNodePlaced, currentNodeOn);
                PlayerController.ON_SELECT_PATH?.Invoke(GridManager.Instance.path, GameState.EnemyTurn);
                GameEvents.ON_CHANGE_STATE?.Invoke(VisualGridType.Waiting);
            }
            else if (currentNodeOn.GetEntity() != null && currentNodeOn.GetEntityType() != EntityType.Player && currentNodeOn.GetEntity().GetComponent<MoveableEntity>())
            {
                currentSelectedPlayer.AttacK(GameState.EnemyTurn);
            }
        }
    }

    public void SelectedPlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Node currentNodeOn = GridManager.Instance.CurrentNodeOn;
            if (currentNodeOn != null && !currentNodeOn.IsFreeNode() && currentNodeOn.GetEntityType() == EntityType.Player)
            {
                SetCurrentPlayer((PlayerController)currentNodeOn.GetEntity());
            }
        }
    }

    private void SetCurrentPlayer(PlayerController _player)
    {
        currentSelectedPlayer = _player;
        GameEvents.ON_CHANGE_STATE?.Invoke(VisualGridType.Movement);
        foreach (PlayerController player in playersList)
        {
            if (player == currentSelectedPlayer)
                player.canMove = true;
            else
                player.canMove = false;
        }
    }
}
