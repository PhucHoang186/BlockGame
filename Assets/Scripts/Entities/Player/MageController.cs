using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MageController : PlayerController
{
    public Projectile meteoritePref;
    public override IEnumerator IEAttack(List<Node> attackNodeList)
    {
        GameUIManager.ON_UI_BLOCK_INPUT?.Invoke(true);
        var lookDir = (GridManager.Instance.CurrentNodeOn.transform.position - currentNodePlaced.transform.position).normalized;
        this.transform.DORotateQuaternion(Quaternion.LookRotation(lookDir), 1f);

        SetTriggerAnimation(Attack);
        yield return new WaitForSeconds(1f);
        foreach (Node node in attackNodeList)
        {
            var meteoriteOb = Instantiate(meteoritePref);
            meteoriteOb.FallToPosition(node.transform.position + new Vector3(0f, 0.5f, 0f));
        }

        yield return new WaitForSeconds(1.5f);

        foreach (Node node in attackNodeList)
        {
            if (node.IsEnemyNode())
            {
                var damageable = node.GetEntity().GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(this.damageAmount);
                }
            }
        }
        GameUIManager.ON_UI_BLOCK_INPUT?.Invoke(false);
    }
}
