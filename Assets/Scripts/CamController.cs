using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CamController : MonoBehaviour
{
    public Transform focusMap;
    public List<VirtualCam> cams;
    private VirtualCam currentCam;

    private Vector3 lastMousePos;
    void Start()
    {
        cams = GetComponentsInChildren<VirtualCam>().ToList();
        SetCurrentCam(CamType.MapViewCam, focusMap, focusMap);
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
            if (Input.GetMouseButton(0))
            {
                Vector3 deltMousePos = (Input.mousePosition - lastMousePos).normalized;
                lastMousePos = Input.mousePosition;
                focusMap.Rotate(0f, deltMousePos.x * 50f * Time.deltaTime, 0f);
            }
            if (Input.mouseScrollDelta.x != 0)
            {

            }
        }
    }

    public void HandleZoomCamera(float input)
    {
    }
}
