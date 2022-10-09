using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;

public enum PlayerType
{
    Knight,
    Mage,
    Barbarian,
    Acher,
}
public class PlayerController : MoveableEntity
{
    public static Action<List<Node>, Action> ON_SELECT_PATH;
    public static Action<List<Node>> ON_ATTACK;
    public PlayerType playerType;
    public int weaponRange;

    public override void Start()
    {
        base.Start();
        ON_SELECT_PATH += MoveToPath;
        ON_ATTACK += OnAttack;
    }

    void OnDestroy()
    {
        ON_SELECT_PATH -= MoveToPath;
        ON_ATTACK -= OnAttack;
    }

    public void OnAttack(List<Node> attackNodeList)
    {
        if (canAttack)
            StartCoroutine(IEAttack(attackNodeList));
    }

    public virtual IEnumerator IEAttack(List<Node> attackNodeList)
    {
        GameUIManager.ON_UI_BLOCK_INPUT?.Invoke(true);
        var lookDir = (GridManager.Instance.CurrentNodeOn.transform.position - currentNodePlaced.transform.position).normalized;
        this.transform.DORotateQuaternion(Quaternion.LookRotation(lookDir), 1f);
        yield return new WaitForSeconds(1f);
        SetTriggerAnimation(playerType == PlayerType.Acher ? Attack_Bow : Attack);
        yield return new WaitForSeconds(0.5f);

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
