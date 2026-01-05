using Unity.Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineCamera>();
        if (virtualCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera component not found on this GameObject.");
        }
    }
}
