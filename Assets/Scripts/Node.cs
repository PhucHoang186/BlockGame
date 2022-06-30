using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public float nodeSize;
    public Entity currentObjectPlaced;
    public bool isPlaced;
    public void PlaceObjectOnNode(Entity _entity)
    {
        currentObjectPlaced = _entity;
        isPlaced = true;
    }
    public void ReleaseNode(GameObject _Obj)
    {
        currentObjectPlaced = null;
        isPlaced = false;
    }
}
