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

    public virtual void MoveToPosition(Vector3 _dir)
    {
        //convert position to node id
        GridManager gridManager = GridManager.Instance;
        var newNodeId = gridManager.ConvertPositionToNodeID(transform.position) + _dir;
        Node newNode = gridManager.GetNodeById(newNodeId);

        //move
        if (newNode == null || newNode.isPlaced)
            return;

        canMove = false;
        transform.DORotateQuaternion(Quaternion.LookRotation(_dir), 0.5f);
        transform.DOJump(newNode.transform.position + offset, 0.5f, 1, moveTime).SetEase(Ease.InBack).OnComplete(() =>
        {
            ON_FINISH_MOVEMENT?.Invoke();
            gridManager.MoveToNode(this.currentNodePlaced, newNode, this);
        });

    }

    public IEnumerator AttackCoroutine()
    {
        canAttack = false;
        ani.SetTrigger("Attack");
        yield return new WaitForSeconds(ani.GetCurrentAnimatorClipInfo(0).Length);
        canAttack = true;

    }

    public void AttacK()
    {
    }
}
[System.Serializable]
public class EntityObject
{
    public GameObject entityPref;
    public EntityType entityType;
    public Vector3 nodeId;
}
