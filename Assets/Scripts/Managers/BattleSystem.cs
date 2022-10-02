using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BattleState
{
    PlayerTurn,
    EnemyTurn,
    Waiting,
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
    private Node previousNodeOn;

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
            if (pairs.Value.IsPlayerNode())
            {
                playersList.Add((MoveableEntity)pairs.Value.currentObjectPlaced);
            }
            else if (pairs.Value.IsEnemyNode())
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
            HandleEnemyTurn();
        }
    }

    private void HandleEnemyTurn()
    {
        SetBattleState(BattleState.Waiting);
        EnemyController.Instance.canMove = true;
        EnemyController.ON_ENEMY_TURN?.Invoke(PathFinding.Instance.FindPath(EnemyController.Instance.currentNodePlaced, EnemyController.Instance.playerTarget.currentNodePlaced), () =>
        {
            SetBattleState(BattleState.PlayerTurn);
            SwitchPlayerState(PlayerState.Movement);
        });
    }

    private void HandlePlayerTurn()
    {
        SelectedPlayer();

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

        if (currentPlayerState == PlayerState.Attack)
            HandlePlayerWeaponRange();

        if (Input.GetMouseButtonDown(0))
        {
            Node currentNodeOn = gridManager.CurrentNodeOn;
            if (currentNodeOn == null || currentNodeOn.IsPlayerNode()) return;
            if (currentNodeOn.canInteract)
            {
                if (currentPlayerState == PlayerState.Movement)
                {
                    VisualGridManager.Instance.ReleaseVisual();
                    gridManager.ReleaseNodesState();
                    PlayerController.ON_SELECT_PATH?.Invoke(PathFinding.Instance.FindPath(currentSelectedPlayer.currentNodePlaced, currentNodeOn), () =>
                    {
                        HandlePlayerMovement(); // temporary
                    });
                }
                else if (currentPlayerState == PlayerState.Attack)
                {
                    PlayerController.ON_ATTACK?.Invoke(nodesInAttackRangeList);
                }
            }
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
            if (currentNodeOn != null && !currentNodeOn.IsFreeNode() && currentNodeOn.IsPlayerNode())
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
            {
                player.canMove = true;
                player.canAttack = true;
            }
            else
            {
                player.canMove = false;
                player.canAttack = false;
            }
        }
    }

    public void SwitchPlayerState(PlayerState _state)
    {
        VisualGridManager.Instance.ReleaseVisual();
        gridManager.ReleaseNodesState();

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
                HandlePlayerEndTurn();
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
        int range = currentSelectedPlayer.attackRange;

        List<Node> attackNodeList = new List<Node>();
        attackNodeList = gridManager.GetNodesInRange(startNode, range);
        attackNodeList.Remove(currentSelectedPlayer.currentNodePlaced);
        foreach (Node node in attackNodeList)
        {
            node.ToggleNodeByType(true, VisualNodeType.Attack);
            node.canInteract = true;
        }
    }

    void HandlePlayerWeaponRange()
    {
        Node currentNodeOn = gridManager.CurrentNodeOn;
        if (currentNodeOn != null && currentNodeOn.canInteract)
        {

            if (previousNodeOn != gridManager.CurrentNodeOn)
            {
                foreach (Node node in nodesInAttackRangeList)
                {
                    node.ToggleNodeByType(false, VisualNodeType.WeaponRange);
                }
                nodesInAttackRangeList = gridManager.GetNodesInRange(gridManager.CurrentNodeOn, currentSelectedPlayer.weaponRange);

                foreach (Node node in nodesInAttackRangeList)
                {
                    node.ToggleNodeByType(true, VisualNodeType.WeaponRange);
                }
            }
            previousNodeOn = gridManager.CurrentNodeOn;
        }
    }

    public void HandlePlayerEndTurn()
    {
        VisualGridManager.Instance.ReleaseVisual();
        gridManager.ReleaseNodesState();
        SetBattleState(BattleState.EnemyTurn);
    }
}
