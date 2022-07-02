using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RotateBlock : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] LayerMask blockMask;
    [SerializeField] float rotateTime;
    [SerializeField] float radius;
    private PlayerController playerController;
    private Collider[] colliders;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
            Debug.Log("Collide with " + other.name);
            // playerController.isTrigger = true;
            PlayerController.ON_FINISH_MOVEMENT += CheckOntriggerEnter;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController.ON_FINISH_MOVEMENT -= CheckOntriggerEnter;
        }
    }

    public void CheckOntriggerEnter()
    {
        colliders = (Physics.OverlapBox(transform.position, new Vector3(radius, 0.5f, radius), Quaternion.identity, blockMask));
        Debug.Log(colliders.Length);
        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Block") || collider.CompareTag("Player"))
                {
                    collider.transform.parent = transform;
                    Debug.Log(collider.gameObject.name);
                }
            }
            RotateBlockAngle(90f, 1f);
        }
    }
    void RotateBlockAngle(float _angle, float _rotateTime)
    {
        Debug.Log("gO HERE");
        transform.DOPunchScale(new Vector3(-0.2f, -0.2f, -0.2f), _rotateTime * 0.5f, 0, 0);

        transform.DORotate(transform.eulerAngles + new Vector3(0f, _angle, 0f), _rotateTime).OnComplete(() =>
      {
          playerController.canMove = true;
          ReleaseBlock(colliders);
      });

    }

    void ReleaseBlock(Collider[] _colliders)
    {
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Block") || collider.CompareTag("Player"))
                {
                    collider.transform.parent = null;
                }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(0f, 1f, 0f), Vector3.one * radius * 2);
    }
}
