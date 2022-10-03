using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VisualNodeType
{
    Movement,
    Hover,
    Attack,
    WeaponRange,
    ToggleEnemy,
    All,
}

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
    public bool canInteract;
    [SerializeField] private LayerMask entityLayer;
    [SerializeField] private LayerMask nodeLayer;
    [SerializeField] Transform checkPoint;
    [SerializeField] SpriteRenderer moveSprite;
    [SerializeField] SpriteRenderer highlightSprite;
    [SerializeField] SpriteRenderer attackSprite;
    [SerializeField] SpriteRenderer weaponRangeSprite;
    [SerializeField] SpriteRenderer ToggleEnemySprite;

    public void Init()
    {
        // get object place on Node
        var colliders = Physics.OverlapSphere(checkPoint.position, 0.5f, entityLayer);
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

    public EntityType GetEntityType()
    {
        if (!IsEmptyNode())
            return currentObjectPlaced.entityType;

        return EntityType.Null;
    }

    public bool IsPlayerNode()
    {
        return GetEntityType() == EntityType.Player;
    }

    public bool IsEnemyNode()
    {
        return GetEntityType() == EntityType.Enemy;
    }

    public bool IsCollectableNode()
    {
        return GetEntityType() == EntityType.Collectable;
    }

    public Entity GetEntity()
    {
        return currentObjectPlaced;
    }

    public bool IsEmptyNode()
    {
        return currentObjectPlaced == null;
    }

    public bool HasEntity()
    {
        return currentObjectPlaced != null;
    }

    public bool IsMoveableNode()
    {
        return currentObjectPlaced.entityType == EntityType.Player || currentObjectPlaced.entityType == EntityType.Collectable;
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

    public bool IsFreeNode()
    {
        return currentObjectPlaced == null;
    }

    public void ReleaseNode()
    {
        currentObjectPlaced = null;
        isPlaced = false;
    }

    public void ToggleNodeByType(bool _isActive, VisualNodeType _type)
    {
        switch (_type)
        {
            case VisualNodeType.Movement:
                ToggleMovement(_isActive);
                break;
            case VisualNodeType.Attack:
                ToggleAttack(_isActive);
                break;
            case VisualNodeType.WeaponRange:
                ToggleWeaponRange(_isActive);
                break;
            case VisualNodeType.ToggleEnemy:
                ToggleEnemy(_isActive);
                break;
            case VisualNodeType.All:
                ToggleHover(_isActive);
                ToggleMovement(_isActive);
                ToggleAttack(_isActive);
                ToggleWeaponRange(_isActive);
                ToggleEnemy(_isActive);
                break;
            default:
                break;
        }
    }

    public void ToggleMovement(bool _isActive)
    {
        moveSprite.gameObject.SetActive(_isActive);
    }

    public void ToggleHover(bool _isActive)
    {
        if (_isActive != highlightSprite.gameObject.activeSelf)
            highlightSprite.gameObject.SetActive(_isActive);
    }

    public void ToggleAttack(bool _isActive)
    {
        attackSprite.gameObject.SetActive(_isActive);
    }

    public void ToggleWeaponRange(bool _isActive)
    {
        weaponRangeSprite.gameObject.SetActive(_isActive);
    }

    public void ToggleEnemy(bool _isActive)
    {
        ToggleEnemySprite.gameObject.SetActive(_isActive);
    }
}
