using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System;

public class EnemyController : MoveableEntity
{
    public static Action<List<Node>, Action> ON_ENEMY_TURN;
    public PlayerController playerTarget;

    public override void Start()
    {
        base.Start();
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        playerTarget = playerObj.GetComponent<PlayerController>();
        ON_ENEMY_TURN += MoveToPath;
    }

    void OnDestroy()
    {
        ON_ENEMY_TURN -= MoveToPath;
    }

    public void DetectNearestPlayer()
    {
        // playerTarget = 
    }

    public override IEnumerator MoveToPathCroutine(List<Node> _path, Action cb)
    {
        int moveStep = 0;
        moveParticle.Play();
        SetTriggerAnimation(Run);

        while (moveStep < _path.Count && moveStep < moveRange)
        {
            MoveToNode(_path.FirstOrDefault());
            if (_path.Count > 0)
                _path.RemoveAt(0);
            yield return new WaitForSeconds(moveTime);
            AttacK(GameState.GenerateGrid);
            moveStep += 1;
        }
        canMove = false;
        SetTriggerAnimation(Idle);
        moveParticle.Stop();
        cb?.Invoke();
    }

    public void AttacK(GameState _newState)
    {
        CheckAttackRange(_newState);
    }

    public void CheckAttackRange(GameState _newState)
    {
        var neighborNodes = GridManager.Instance.GetNeighborNode(currentNodePlaced);
        foreach (Node node in neighborNodes)
        {
            if ((node.IsPlayerNode()))
            {
                Debug.LogError("Is Player");
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
        yield return new WaitForSeconds(ani.GetCurrentAnimatorClipInfo(0).Length);

        // GameManager.Instance.SwitchState(_newState);
    }

    public override void MinusHealth(int _healthAmount)
    {
        CurrentHealth -= _healthAmount;
        if (currentHealth <= 0)
        {
            CurrentHealth = 0;
            GameEvents.ON_ENEMY_DESTROY(this);
            SetTriggerAnimation(Defeat);
        }
    }

    public void ReleaseOnDestroy()
    {
        currentNodePlaced.ReleaseNode();
        currentNodePlaced.ToggleNodeByType(false, VisualNodeType.ToggleEnemy);
    }
}
