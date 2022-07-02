using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum EntityType
{
    Player, Enemy, Object
}
public class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected float moveTime;
    [SerializeField] protected float rotateTime;
    public Vector3 offset;
    public Node currentNodePlaced;
    protected Animator ani;
    public bool canMove;

    public virtual void MoveToPosition(Vector3 _dir)
    {
        //convert position to node id
        GridManager gridManager = GridManager.Instance;
        var newNodeId = gridManager.ConvertPositionToNodeID(transform.position) + _dir;
        Node newNode = gridManager.GetNodeById(newNodeId);

        //move
        if (newNode == null)
            return;

        canMove = false;
        transform.DORotateQuaternion(Quaternion.LookRotation(_dir), 0.5f);
        transform.DOJump(newNode.transform.position + offset, 0.5f, 1, moveTime).SetEase(Ease.InBack).OnComplete(() =>
        {
            canMove = true;
            gridManager.MoveToNode(this.currentNodePlaced, newNode, this);
        });

    }
}
[System.Serializable]
public class EntityObject
{
    public GameObject entityPref;
    public EntityType entityType;
    public Vector3 nodeId;
}
