using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Projectile : MonoBehaviour
{
    public void FallToPosition(Vector3 pos)
    {
        transform.DOMove(pos, 1f).SetEase(Ease.Flash).OnComplete(() =>{
            Destroy(gameObject);
        });
    }
   
}
