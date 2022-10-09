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
    public EnemyController currentEnemy;
    private int enemyIndex;

    private Node previousNodeOn;
    private bool hasTargetInRange;

    void Awake()
    {
        GameEvents.ON_CHANGE_PLAYER_STATE += SwitchPlayerState;
        GameEvents.ON_ENEMY_DESTROY += OnEnemyDestroy;
    }

    void OnDestroy()
    {
        GameEvents.ON_CHANGE_PLAYER_STATE -= SwitchPlayerState;
        GameEvents.ON_ENEMY_DESTROY -= OnEnemyDestroy;
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

    public void OnEnemyDestroy(EnemyController enemy)
    {
        enemiesList.Remove(enemy);
        enemy.ReleaseOnDestroy();
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

    #region Player
    private void HandlePlayerTurn()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

        SelectedPlayer();

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
                else if (currentPlayerState == PlayerState.Attack && hasTargetInRange)
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
            if (path?.Count > 0 && path?.Count <= range && !node.isPlaced)
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
        attackNodeList.Remove(startNode);
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
                    node.ToggleNodeByType(false, node.IsEnemyNode() ? VisualNodeType.ToggleEnemy : VisualNodeType.WeaponRange);
                    hasTargetInRange = false;
                }
                nodesInAttackRangeList = gridManager.GetNodesInRange(gridManager.CurrentNodeOn, currentSelectedPlayer.weaponRange);

                foreach (Node node in nodesInAttackRangeList)
                {
                    node.ToggleNodeByType(true, node.IsEnemyNode() ? VisualNodeType.ToggleEnemy : VisualNodeType.WeaponRange);
                    if(node.IsEnemyNode())
                        hasTargetInRange = true;
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
    #endregion

    #region Enemy

    private void HandleEnemyTurn()
    {
        GameUIManager.ON_UI_BLOCK_INPUT?.Invoke(true);
        SetBattleState(BattleState.Waiting);
        enemyIndex = 0;
        EnemyHandle(enemyIndex);
    }

    private void EnemyHandle(int enemyIndex)
    {
        if (enemyIndex < enemiesList.Count)
        {
            currentEnemy = (EnemyController)enemiesList[enemyIndex];
            currentEnemy.canMove = true;
            currentEnemy.DetectNearestPlayer();
            EnemyController.ON_ENEMY_TURN?.Invoke(PathFinding.Instance.FindPath(currentEnemy.currentNodePlaced, currentEnemy.playerTarget.currentNodePlaced), () =>
            {
                StartCoroutine(NextEnemyTurn());
            });
        }
        else
        {
            GameUIManager.ON_UI_BLOCK_INPUT?.Invoke(false);
            SetBattleState(BattleState.PlayerTurn);
            SwitchPlayerState(PlayerState.Movement);
        }
    }

    private IEnumerator NextEnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        enemyIndex += 1;
        EnemyHandle(enemyIndex);
    }
    #endregion
}
