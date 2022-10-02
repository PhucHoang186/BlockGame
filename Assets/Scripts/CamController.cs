using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CamController : MonoBehaviour
{
    public Transform focusMap;
    public List<VirtualCam> cams;
    
    public float minZoomRange;
    public float maxZoomRange;
    private float currentZoomRange;

    private VirtualCam currentCam;

    private Vector3 lastMousePos;
    void Start()
    {
        cams = GetComponentsInChildren<VirtualCam>().ToList();
        SetCurrentCam(CamType.MapViewCam, focusMap, focusMap);
        currentZoomRange = currentCam.virtualCamera.m_Lens.FieldOfView;
    }

    public void SetCurrentCam(CamType camType, Transform lookAt = null, Transform follow = null)
    {
        foreach (VirtualCam cam in cams)
        {
            if (cam.camType == camType)
            {
                cam.virtualCamera.Priority = 99;
                currentCam = cam;
            }
            else
            {
                cam.virtualCamera.Priority = 10;
            }
        }

        if (lookAt != null)
            currentCam?.SetLookAt(lookAt);
        if (follow != null)
            currentCam?.SetFollow(follow);
    }

    void Update()
    {
        if (currentCam.GetCamType() == CamType.MapViewCam)
        {
            if (Input.GetMouseButton(1))
            {
                Vector3 deltMousePos = (Input.mousePosition - lastMousePos).normalized;
                lastMousePos = Input.mousePosition;
                focusMap.Rotate(0f, deltMousePos.x * 150f * Time.deltaTime, 0f);
            }
            if (Input.mouseScrollDelta.y != 0)
            {   currentZoomRange -= Input.mouseScrollDelta.y * 5f;
                currentZoomRange = Mathf.Clamp(currentZoomRange, minZoomRange, maxZoomRange);
                currentCam.virtualCamera.m_Lens.FieldOfView = currentZoomRange;
            }
            
        }
    }

    public void HandleZoomCamera(float input)
    {
    }
}
