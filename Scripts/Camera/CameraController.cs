using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera component not found on this GameObject.");
        }
    }
}
