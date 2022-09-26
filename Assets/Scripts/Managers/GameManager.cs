using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    GenerateGrid,
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState = GameState.GenerateGrid;

    void Awake()
    {
        if (Instance == null)
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
          
        }
    }
}
