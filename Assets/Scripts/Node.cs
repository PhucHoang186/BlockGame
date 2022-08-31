using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [HideInInspector] public float gCost;
    [HideInInspector] public float hCost;
    [HideInInspector]
    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    public Node parent;

    public int x;
    public int y;

    [Header("Properties")]
    public Entity currentObjectPlaced;
    public bool isPlaced;
    public bool canMove;
    [SerializeField] Transform checkPoint;
    [SerializeField] SpriteRenderer highlightSprite;
    [SerializeField] SpriteRenderer toggleSprite;
    [SerializeField] private LayerMask entityLayer;
    [SerializeField] private LayerMask nodeLayer;

    public void Init()
    {
        // get object place on Node
        var colliders = Physics.OverlapSphere(checkPoint.position, 1f, entityLayer);
        if (colliders.Length > 0)
        {
            Entity entity = colliders[0].transform.gameObject.GetComponent<Entity>();
            if (entity)
            {
                PlaceObjectOnNode(entity);
                entity.currentNodePlaced = this;
            }
        }
        // get Node
        if (Physics.OverlapSphere(transform.position, 1f, nodeLayer).Length == 0)
        {
            Destroy(this);
        }
    }

    public void PlaceObjectOnNode(Entity _entity)
    {
        if (isPlaced)
            return;
        currentObjectPlaced = _entity;
        _entity.currentNodePlaced = this;
        currentObjectPlaced.transform.position = transform.position + currentObjectPlaced.offset;
        if (_entity.entityType != EntityType.Collectable)
        {
            isPlaced = true;
        }
    }

    public void ReleaseNode()
    {
        currentObjectPlaced = null;
        isPlaced = false;
        ToggleHighlightNode(false);
    }
    public void ToggleHighlightNode(bool _isActive)
    {
        highlightSprite.gameObject.SetActive(_isActive);
    }

    private void OnMouseHover(bool _isActive)
    {
        if (_isActive != toggleSprite.gameObject.activeSelf)
            toggleSprite.gameObject.SetActive(_isActive);
    }

    void OnMouseEnter()
    {
        // OnMouseHover(true);
    }

    void OnMouseExit()
    {
        OnMouseHover(false);
    }

    void OnMouseOver()
    {
        OnMouseHover(GameManager.Instance.currentState == GameState.PlayerTurn);
        if (Input.GetMouseButtonDown(0))
        {
            if (canMove)
            {
                PathFinding.Instance.FindPath(PlayerController.Instance.currentNodePlaced, this);
                PlayerController.ON_SELECT_PATH?.Invoke(GridManager.Instance.path, GameState.EnemyTurn);
            }
        }
    }
}
