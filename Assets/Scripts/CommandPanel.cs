using UnityEngine;
using UnityEngine.UI;

public enum CommandType
{
    Movement, 
    Attack,
    End,
}

public class CommandPanel : MonoBehaviour
{
    [SerializeField] Button moveButton;
    [SerializeField] Button attackButton;
    [SerializeField] Button endTurnButton;

    void Awake()
    {
        moveButton.onClick.AddListener(OnMoveButtonPressed);
        attackButton.onClick.AddListener(OnAttackButtonPressed);
        endTurnButton.onClick.AddListener(OnEndTurnButtonPressed);
    }

    void OnDestroy()
    {
        moveButton.onClick.RemoveListener(OnMoveButtonPressed);
        attackButton.onClick.RemoveListener(OnAttackButtonPressed);
        endTurnButton.onClick.RemoveListener(OnEndTurnButtonPressed);
    }

    public void OnMoveButtonPressed()
    {
        GameEvents.ON_CHANGE_PLAYER_STATE?.Invoke(PlayerState.Movement);
    }

    public void OnAttackButtonPressed()
    {
        GameEvents.ON_CHANGE_PLAYER_STATE?.Invoke(PlayerState.Attack);
    }

    public void OnEndTurnButtonPressed()
    {
        //end turn
        //switch to enemy turn
    }
}
