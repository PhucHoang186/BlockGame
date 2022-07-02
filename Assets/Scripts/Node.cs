using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    [HideInInspector] public float gCost;
    [HideInInspector] public float hCost;
    [HideInInspector] public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    public Node parent;

    public int x;
    public int y;

    public Entity currentObjectPlaced;
    public bool isPlaced;
    [SerializeField] GameObject highlightNodeOn;
    public void PlaceObjectOnNode(Entity _entity)
    {
        if (isPlaced)
            return;
        currentObjectPlaced = _entity;
        _entity.currentNodePlaced = this;
        currentObjectPlaced.transform.position = transform.position + currentObjectPlaced.offset;
        isPlaced = true;
        if (_entity.entityType != EntityType.Object)
            ToggleHighlightNode(true);
    }
    public void ReleaseNode()
    {
        currentObjectPlaced = null;
        isPlaced = false;
        ToggleHighlightNode(false);
    }
    public void ToggleHighlightNode(bool _isActive)
    {
        highlightNodeOn?.SetActive(_isActive);
    }
}
