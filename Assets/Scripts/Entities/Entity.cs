using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
public enum EntityType
{
    Player, Enemy, Object, Collectable
}
public class Entity : MonoBehaviour
{
    public Vector3 offset;
    public EntityType entityType;
    public Node currentNodePlaced;
    protected Animator ani;
}
    
[System.Serializable]
public class EntityObject
{
    public GameObject entityPref;
    public EntityType entityType;
    public Vector3 nodeId;
}
