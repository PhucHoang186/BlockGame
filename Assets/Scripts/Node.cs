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
    [SerializeField] SpriteRenderer hoverSprite;
    [SerializeField] SpriteRenderer attackSprite;
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
        ToggleHighlight(false);
    }
    public void ToggleHighlight(bool _isActive)
    {
        highlightSprite.gameObject.SetActive(_isActive);
    }

    public void ToggleHover(bool _isActive)
    {
        if (_isActive != hoverSprite.gameObject.activeSelf)
            hoverSprite.gameObject.SetActive(_isActive);
    }

    public void ToggleAttack(bool _isActive)
    {
        attackSprite.gameObject.SetActive(_isActive);
    }

    void OnMouseEnter()
    {
        // OnMouseHover(true);
    }

    void OnMouseExit()
    {
        ToggleHover(false);
        ToggleAttack(false);
    }

    void OnMouseOver()
    {
        ToggleHover(GameManager.Instance.currentState == GameState.PlayerTurn  && currentObjectPlaced == null);
        ToggleAttack(currentObjectPlaced != null && currentObjectPlaced.entityType != EntityType.Player);
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
