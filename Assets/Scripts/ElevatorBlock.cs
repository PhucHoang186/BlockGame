using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ElevatorBlock : MonoBehaviour
{
    [SerializeField] Transform elevatorBody;
    [SerializeField] float moveSpeed = 2f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(OnTriggerCo());
        }
    }

    IEnumerator OnTriggerCo()
    {
        yield return new WaitForSeconds(1f);
        RaiseUp(moveSpeed);
        yield return new WaitForSeconds(5f);
        SinkDown(moveSpeed);
    }

    void RaiseUp(float _moveTime)
    {
        elevatorBody.DOLocalMoveY(0f, _moveTime);
    }

    void SinkDown(float _moveTime)
    {
        elevatorBody.DOLocalMoveY(-0.9f, _moveTime);

    }
}
