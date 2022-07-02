using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Entity currentObjectPlaced;
    public bool isPlaced;
    [SerializeField] GameObject highlightNodeOn;
    public void PlaceObjectOnNode(Entity _entity)
    {
        currentObjectPlaced = _entity;
        _entity.currentNodePlaced = this;
        currentObjectPlaced.transform.position = transform.position + currentObjectPlaced.offset;
        isPlaced = true;
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
