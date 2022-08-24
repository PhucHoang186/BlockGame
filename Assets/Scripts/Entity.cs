using System.Collections;
using System;
using UnityEngine;
using DG.Tweening;
public enum EntityType
{
    Player, Enemy, Object
}
public class Entity : MonoBehaviour
{
    public static Action ON_FINISH_MOVEMENT;

    [SerializeField] protected float moveTime;
    [SerializeField] protected float rotateTime;
    public Vector3 offset;
    public EntityType entityType;
    public Node currentNodePlaced;
    protected Animator ani;
    public bool canMove;
    public bool canAttack;

    public virtual void MoveToDirection(Vector3 _dir, GameState _newState)
    {
        //convert position to node id
        GridManager gridManager = GridManager.Instance;
        var newNodeId = gridManager.ConvertPositionToNodeID(transform.position) + _dir;
        Node newNode = gridManager.GetNodeById(newNodeId);
        //move
        MoveToNode(newNode, _newState);
    }

    public virtual void MoveToNode(Node _newNode, GameState _newState)
    {
        canMove = false;
        transform.DORotateQuaternion(Quaternion.LookRotation(_newNode.transform.position - currentNodePlaced.transform.position), 0.5f);
        if (_newNode == null || _newNode.isPlaced)
            return;
        transform.DOJump(_newNode.transform.position + offset, 0.5f, 1, moveTime).SetEase(Ease.InBack).OnComplete(() =>
        {
            GridManager.Instance.MoveToNode(this.currentNodePlaced, _newNode, this);
            GameManager.Instance.SwitchState(_newState);
        });
    }

    public IEnumerator AttackCoroutine()
    {
        canAttack = false;
        ani.SetTrigger("Attack");
        yield return new WaitForSeconds(ani.GetCurrentAnimatorClipInfo(0).Length);
        canAttack = true;

    }

    public virtual void AttacK()
    {
        StartCoroutine(AttackCoroutine());
    }
}
[System.Serializable]
public class EntityObject
{
    public GameObject entityPref;
    public EntityType entityType;
    public Vector3 nodeId;
}
