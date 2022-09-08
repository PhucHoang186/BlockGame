using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualGridManager : MonoBehaviour
{
    [SerializeField] LayerMask detectLayer;
    private Camera cam;
    private GridManager gridManager;
    private Node currentNodeOn;
    private Node previousNodeOn;
    void Start()
    {
        cam = Camera.main;
        gridManager = GridManager.Instance;
        currentNodeOn = null;
    }

    void Update()
    {
        GetNodeByMousePosition();
    }

    public void GetNodeByMousePosition()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            currentNodeOn = gridManager.GetNodeByPosition(hit.transform.position);
            currentNodeOn.ToggleHover(true);
            if (previousNodeOn != currentNodeOn)
            {
                previousNodeOn?.ToggleHover(false);
            }
            previousNodeOn = currentNodeOn;
        }
    }
}
