using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BattleState
{
    PlayerTurn,
    EnemyTurn,
}

public enum PlayerState
{
    Movement,
    Attack,
    End,
}

public class BattleSystem : MonoSingleton<BattleSystem>
{
    public PlayerController currentSelectedPlayer;
    public List<MoveableEntity> playersList;
    public List<MoveableEntity> enemiesList;
    private GridManager gridManager;
    private List<Node> nodesInAttackRangeList;
    private BattleState currentBattleState;
    private PlayerState currentPlayerState;

    void Awake()
    {
        GameEvents.ON_CHANGE_PLAYER_STATE += SwitchPlayerState;
    }

    void OnDestroy()
    {
        GameEvents.ON_CHANGE_PLAYER_STATE -= SwitchPlayerState;
    }

    public void Init()
    {
        playersList = new List<MoveableEntity>();
        enemiesList = new List<MoveableEntity>();
        nodesInAttackRangeList = new List<Node>();

        gridManager = GridManager.Instance;
        foreach (var pairs in gridManager.GetGrid)
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
        // SetBattleState(BattleState.PlayerTurn);
        SetCurrentPlayer((PlayerController)playersList[0]);
    }

    void Update()
    {
        HandleBattleState();

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
        SelectedPlayer();

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
            Node currentNodeOn = gridManager.CurrentNodeOn;
            if (currentNodeOn == null) return;
            if (currentNodeOn.canInteract)
            {
                if (currentPlayerState == PlayerState.Movement)
                {
                    PathFinding.Instance.FindPath(currentSelectedPlayer.currentNodePlaced, currentNodeOn);
                    PlayerController.ON_SELECT_PATH?.Invoke(gridManager.path, GameState.EnemyTurn);
                }
                else if (currentPlayerState == PlayerState.Attack)
                {
                    PlayerController.ON_ATTACK?.Invoke(nodesInAttackRangeList);
                }
            }
            // else if (currentNodeOn.GetEntity() != null && currentNodeOn.GetEntityType() != EntityType.Player && currentNodeOn.GetEntity().GetComponent<MoveableEntity>())
            // {
            //     currentSelectedPlayer.AttacK(GameState.EnemyTurn);
            // }
        }

    }
    private void SetBattleState(BattleState _state)
    {
        currentBattleState = _state;
    }

    public void SelectedPlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Node currentNodeOn = gridManager.CurrentNodeOn;
            if (currentNodeOn != null && !currentNodeOn.IsFreeNode() && currentNodeOn.GetEntityType() == EntityType.Player)
            {
                SetCurrentPlayer((PlayerController)currentNodeOn.GetEntity());
            }
        }
    }

    private void SetCurrentPlayer(PlayerController _player)
    {
        currentSelectedPlayer = _player;
        GameEvents.ON_CHANGE_PLAYER_STATE?.Invoke(PlayerState.Movement);
        foreach (PlayerController player in playersList)
        {
            if (player == currentSelectedPlayer)
                player.canMove = true;
            else
                player.canMove = false;
        }
    }

    public void SwitchPlayerState(PlayerState _state)
    {
        currentPlayerState = _state;
        switch (_state)
        {
            case PlayerState.Movement:
                HandlePlayerMovement();
                break;
            case PlayerState.Attack:
                HandlePlayerAttack();
                break;
            case PlayerState.End:
                break;
            default:
                break;
        }
    }

    void HandlePlayerMovement()
    {
        Node startNode = currentSelectedPlayer.currentNodePlaced;
        int range = currentSelectedPlayer.moveRange;

        List<Node> movementNodeList = new List<Node>();
        movementNodeList = gridManager.GetNodesInRange(startNode, range);
        
        foreach (Node node in movementNodeList)
        {
            var path = PathFinding.Instance.FindPath(startNode, node);
            if (path?.Count > 0 && path?.Count <= range)
            {
                node.ToggleNodeByType(true, VisualNodeType.Movement);
                node.canInteract = true;
            }
        }
    }

    void HandlePlayerAttack()
    {
        Node startNode = currentSelectedPlayer.currentNodePlaced;
        int range = currentSelectedPlayer.moveRange;

        List<Node> attackNodeList = new List<Node>();
        attackNodeList = gridManager.GetNodesInRange(startNode, range);
        foreach (Node node in attackNodeList)
        {
            var path = PathFinding.Instance.FindPath(startNode, node);
            if (path?.Count > 0 && path?.Count <= range)
            {
                node.ToggleNodeByType(true, VisualNodeType.Attack);
                node.canInteract = true;
            }
        }
    }

    void HandlePlayerWeaponRange()
    {

    }

}
