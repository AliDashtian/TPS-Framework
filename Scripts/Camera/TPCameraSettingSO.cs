using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TPCameraSettingSO : ScriptableObject
{
    public const int DEFAULT_FOV = 60;

    [Header("Cinemachine")]
    [Tooltip("The Cinemachine Virtual Camera that follows the active character. (Optional but recommended)")]
    public CinemachineVirtualCamera CinemachineVCam;

    [Tooltip("How far in degrees can you move the camera up")]
    public float LookSensivity = 70.0f;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("How far in degrees can you move the camera up")]
    public float RightClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float LeftClamp = -70.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    private GameObject _cinemachineCameraTarget;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private const float _threshold = 0.01f;
}
