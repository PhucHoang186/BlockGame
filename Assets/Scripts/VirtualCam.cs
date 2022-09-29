using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum CamType
{
    MapViewCam,
    AttackCam,
}

public class VirtualCam : MonoBehaviour
{
    public CamType camType;
    public CinemachineVirtualCamera virtualCamera;

    public CamType GetCamType()
    {
        return camType;
    }

    public void SetLookAt(Transform target)
    {
        virtualCamera.LookAt = target;
    }

    public void SetFollow(Transform target)
    {
        virtualCamera.Follow = target;
    }
}
