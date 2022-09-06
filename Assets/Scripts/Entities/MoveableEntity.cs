using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;
public class MoveableEntity : Entity, IDamageable
{
    public static Action ON_FINISH_MOVEMENT;
    [SerializeField] protected float moveTime;
    [SerializeField] protected float rotateTime;
    public int moveRange = 2;
    public bool canMove;
    public bool canAttack;
    public float damageAmount = 0;
    public float maxHealth = 10f;
    protected float currentHealth;
    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }



    #region  Movement
    public virtual void MoveToPath(List<Node> _path, GameState _newState)
    {
        StartCoroutine(MoveToPathCroutine(_path, _newState));
    }

    public virtual IEnumerator MoveToPathCroutine(List<Node> _path, GameState _newState)
    {
        int moveStep = 0;
        SetTriggerAnimation(Run);        

        while (moveStep < moveRange)
        {
            moveStep += 1;

            MoveToNode(_path.FirstOrDefault());
            if (_path.Count > 0)
                _path.RemoveAt(0);
            yield return new WaitForSeconds(moveTime);
        }
        canMove = false;
        GameManager.Instance.SwitchState(_newState);
        SetTriggerAnimation(Idle);
    }

    public virtual void MoveToNode(Node _newNode)
    {
        if (_newNode == null || _newNode.isPlaced)
            return;

        transform.DORotateQuaternion(Quaternion.LookRotation(_newNode.transform.position - currentNodePlaced.transform.position), 0.5f);
        transform.DOMove(_newNode.transform.position + offset, moveTime).SetEase(Ease.Flash).OnComplete(() =>
        {
            GridManager.Instance.MoveToNode(this.currentNodePlaced, _newNode, this);
        });
    }
    #endregion

    #region Attack

    public virtual void AttacK(GameState _newState)
    {
        CheckAttackRange(_newState);
    }

    private void CheckAttackRange(GameState _newState)
    {
        var neighborNodes = GridManager.Instance.GetNeighborNodeHasEntity(currentNodePlaced);
        foreach (Node node in neighborNodes)
        {
            if (node.isPlaced && (node.currentObjectPlaced.GetComponent<MoveableEntity>()))
            {
                transform.DORotateQuaternion(Quaternion.LookRotation(node.transform.position - currentNodePlaced.transform.position), 0.5f).OnComplete(()
                =>
                {
                    StartCoroutine(AttackCoroutine(_newState, node.currentObjectPlaced));
                    return;
                }
                );
            }
        }
    }

    public IEnumerator AttackCoroutine(GameState _newState, Entity _entity)
    {
        canAttack = false;
        SetTriggerAnimation(Attack);

        yield return new WaitForSeconds(ani.GetCurrentAnimatorClipInfo(0).Length);

        var damageable = _entity.GetComponent<IDamageable>();
        if (damageable != null)
            damageable.TakeDamage(damageAmount);

        canAttack = true;
        canMove = false;
        GameManager.Instance.SwitchState(_newState);

    }
    #endregion

    #region Health

    public virtual void AddHealth(float _healthAmount)
    {
        currentHealth += _healthAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public virtual void MinusHealth(float _healthAmount)
    {
        currentHealth -= _healthAmount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            Destroy(this.gameObject);
        }
    }

    public virtual void TakeDamage(float _damageAmount)
    {
        MinusHealth(_damageAmount);
    }
    #endregion

    #region Animation

    public static readonly int Idle = Animator.StringToHash("Character_Idle");
    public static readonly int Attack = Animator.StringToHash("Character_Attack");
    public static readonly int Run = Animator.StringToHash("Character_Run");

    protected void SetTriggerAnimation(int _state, float _transitionTime = 0f)
    {
        ani.CrossFade(_state, _transitionTime, 0);
    }

    #endregion

}
