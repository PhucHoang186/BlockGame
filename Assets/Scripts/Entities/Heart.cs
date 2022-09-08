using UnityEngine;
using System;

public class Heart : CollectableEntity
{
    [SerializeField] int healthAmount;

    public override void OnCollected(Entity _entity)
    {
        ((MoveableEntity)_entity).AddHealth(healthAmount);
    }
}
