using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableEntity : Entity
{
    public void OnTriggerEnter(Collider other)
    {
        var entity = other.GetComponent<MoveableEntity>();
        OnCollected(entity);
        this.gameObject.SetActive(false);
    }

    public virtual void OnCollected(Entity _entity)
    {

    }
}
