using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : CollectableEntity
{
    [SerializeField] float healthAmount;

    public override void OnCollected(Entity _entity)
    {
        ((MoveableEntity)_entity).CurrentHealth += healthAmount;
    }
}
