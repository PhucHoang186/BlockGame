using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;
public class MoveableEntity : Entity, IDamageable
{
    public static Action ON_FINISH_MOVEMENT;
    public float moveTime;
    public float rotateTime;
    public int moveRange;
    public int attackRange;
    public bool canMove;
    public bool canAttack;
    public int damageAmount = 0;
    [Space(5)]
    public int maxHealth = 10;
    public int currentHealth;
    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            healthDisplay.UpdateHealthUI(value);
        }
    }

    public virtual void Start()
    {
        ani = GetComponent<Animator>();
        canAttack = true;
        currentHealth = maxHealth;
        healthDisplay.SetMaxHealth(maxHealth);
    }

    public HealthDisplay healthDisplay;

    #region  Movement
    public virtual void MoveToPath(List<Node> _path, GameState _newState)
    {
        if(canMove)
            StartCoroutine(MoveToPathCroutine(_path, _newState));
    }

    public virtual IEnumerator MoveToPathCroutine(List<Node> _path, GameState _newState)
    {
        int moveStep = 0;
        int maxMoveStep = _path.Count;
        SetTriggerAnimation(Run);

        while (moveStep < maxMoveStep && maxMoveStep <= moveRange)
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

    public void CheckAttackRange(GameState _newState)
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


        var damageable = _entity.GetComponent<IDamageable>();
        if (damageable != null)
            damageable.TakeDamage(damageAmount);

        canAttack = true;
        canMove = false;
        yield return new WaitForSeconds(ani.GetCurrentAnimatorClipInfo(0).Length);
        
        GameManager.Instance.SwitchState(_newState);
    }
    #endregion

    #region Health

    public virtual void AddHealth(int _healthAmount)
    {
        CurrentHealth += _healthAmount;
        if (CurrentHealth > maxHealth) CurrentHealth = maxHealth;
    }

    public virtual void MinusHealth(int _healthAmount)
    {
        CurrentHealth -= _healthAmount;
        if (currentHealth <= 0)
        {
            CurrentHealth = 0;
            Destroy(this.gameObject);
        }
    }

    public virtual void TakeDamage(int _damageAmount)
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
