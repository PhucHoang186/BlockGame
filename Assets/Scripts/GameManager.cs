using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    GenerateGrid,
    PlayerTurn,
    EnemyTurn,
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameState currentState = GameState.GenerateGrid;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    void Start()
    {
        SwitchState(GameState.GenerateGrid);
    }
    public void SwitchState(GameState newGameState)
    {
        currentState = newGameState;
        switch (currentState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.Init();
                break;
            case GameState.PlayerTurn:
                PlayerController.Instance.HandlePlayerTurn();
                break;
            case GameState.EnemyTurn:
                break;
            default:
                break;
        }
    }
}
