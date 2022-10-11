using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;
using Cinemachine;

public class CamController : MonoSingleton<CamController>
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

    public VirtualCam GetCamByType(CamType type)
    {
        return cams.Where(c => c.camType == type).FirstOrDefault();
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
                HandleRotateCam();
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                HandleZoomCamera(Input.mouseScrollDelta.y);
            }
        }
    }

    private void HandleRotateCam()
    {
        Vector3 deltaMousePos = (Input.mousePosition - lastMousePos).normalized;
        lastMousePos = Input.mousePosition;
        focusMap.Rotate(0f, deltaMousePos.x * 150f * Time.deltaTime, 0f);
    }

    public void HandleZoomCamera(float input)
    {
        currentZoomRange -= input * 5f;
        currentZoomRange = Mathf.Clamp(currentZoomRange, minZoomRange, maxZoomRange);
        currentCam.virtualCamera.m_Lens.FieldOfView = currentZoomRange;
    }

    public void SetCamPositionByType(Vector3 newPos, CamType type)
    {
        var cam = GetCamByType(type);
        cam.transform.position = newPos;
    }

    public void SetCurrentCamPosition(Vector3 newPos)
    {
        currentCam.transform.position = newPos;
    }

    public void MoveCurrentCamPosition(Vector3 newPos, Action cb = null)
    {
        currentCam.transform.DOMove(newPos, 1f).OnComplete(() => cb?.Invoke());
    }

    public void SetCamFollowPlayer(PlayerController player)
    {
        currentCam?.SetFollow(null);
        currentCam?.SetLookAt(null);
        if (currentCam.virtualCamera.GetCinemachineComponent<CinemachineTransposer>())
        {
            var camOffset = currentCam.virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
            var camPos = player.transform.position + camOffset;
            MoveCurrentCamPosition(camPos, () =>
            {
                currentCam.SetFollow(player.transform);
                currentCam.SetLookAt(player.transform);
            });
        }
    }

}
