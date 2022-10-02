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
    public virtual void MoveToPath(List<Node> _path, Action cb = null)
    {
        if (canMove)
            StartCoroutine(MoveToPathCroutine(_path, cb));
    }

    public virtual IEnumerator MoveToPathCroutine(List<Node> _path, Action cb = null)
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
        SetTriggerAnimation(Idle);
        cb?.Invoke();
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
        Debug.LogError("take damage " + _damageAmount);
    }
    #endregion

    #region Animation

    public static readonly int Idle = Animator.StringToHash("Character_Idle");
    public static readonly int Attack = Animator.StringToHash("Character_Attack");
    public static readonly int Attack_Bow = Animator.StringToHash("Character_Attack_Bow");
    public static readonly int Run = Animator.StringToHash("Character_Run");
    public static readonly int Jump = Animator.StringToHash("Character_Jump");

    protected void SetTriggerAnimation(int _state, float _transitionTime = 0f)
    {
        ani.CrossFade(_state, _transitionTime, 0);
    }
    #endregion
}
