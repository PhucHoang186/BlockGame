using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
public enum EntityType
{
    Null, Player, Enemy, Decoration, Collectable,
}
public class Entity : MonoBehaviour
{
    public Vector3 offset;
    public EntityType entityType;
    public Node currentNodePlaced;
    protected Animator ani;

    public EntityType GetEntityType()
    {
        return entityType;
    }
}

