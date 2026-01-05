using Cinemachine;
using UnityEngine;
using System.Collections;

public class ThirdPersonController : MonoBehaviour
{
    /// <summary>
    /// Reference to InputReader to get LookInput Vector2 for Look()
    /// </summary>
    [SerializeField]
    private InputReader _inputReader;

    [Tooltip("The Cinemachine Virtual Camera that follows the active character. (Optional but recommended)")]
    public PlayerRuntimeSet PlayerRuntimeSet;

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

    [Tooltip("Camera's default FOV when not Zooming")]
    public int DefaultFOV = 60;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    private GameObject _cinemachineCameraTarget;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private const float _threshold = 0.01f;

    private void Start()
    {
        PlayerRuntimeSet.OnCharacterSwitched += ChangeFollowTarget;
        PlayerRuntimeSet.OnCharacterSwitched += ChangeCinemachineTarget;

        // Subscribe Zoom to all of the Players
        foreach (PlayerCharacter player in PlayerRuntimeSet.Items)
        {
            player.OnWeaponFired += Zoom;
        }

        ChangeFollowTarget();
        ChangeCinemachineTarget();
    }

    private void Update()
    {
        Look(_inputReader.LookInput);
    }

    private void Look(Vector2 lookInput)
    {
        // if there is an input and camera position is not fixed
        if (lookInput.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            _cinemachineTargetYaw += lookInput.x * LookSensivity * Time.deltaTime;
            _cinemachineTargetPitch += lookInput.y * LookSensivity * Time.deltaTime;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, LeftClamp, RightClamp);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        //Apply recoil
        if (PlayerRuntimeSet.ActivePlayer.Object && PlayerRuntimeSet.ActivePlayer.Object.GetCurrentWeapon())
        {
            PlayerRuntimeSet.ActivePlayer.Object.GetCurrentWeapon().ApplyRecoil(ref _cinemachineTargetYaw, ref _cinemachineTargetPitch);
        }

        // Cinemachine will follow this target
        _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void Zoom(bool zoom)
    {
        WeaponSO weaponData = PlayerRuntimeSet.ActivePlayer.Object.GetCurrentWeapon().WeaponData;

        if (zoom)
        {
            ChangeFOV(weaponData.ZoomFOV, weaponData.ZoomSpeed);
        }
        else
        {
            ChangeFOV(DefaultFOV, weaponData.ZoomSpeed);
        }
    }

    private void ChangeFOV(int targetFOV, float duration)
    {
        StartCoroutine(ChangeFOVCoroutine(targetFOV, duration));
    }

    IEnumerator ChangeFOVCoroutine(int targetFOV, float duration)
    {
        float time = 0;
        float initFOV = CinemachineVCam.m_Lens.FieldOfView;

        while (time <= duration)
        {
            time += Time.deltaTime;
            CinemachineVCam.m_Lens.FieldOfView = Mathf.Lerp(initFOV, targetFOV, time / duration);

            yield return null;
        }
    }

    private void ChangeFollowTarget()
    {
        if (PlayerRuntimeSet.ActivePlayer.Object != null)
        {
            CinemachineVCam.Follow = PlayerRuntimeSet.ActivePlayer.Object.CinemachineTarget;
        }
        else
        {
            Debug.LogError("new target is null.");
        }
    }

    private void ChangeCinemachineTarget()
    {
        if (PlayerRuntimeSet.ActivePlayer.Object != null)
        {
            _cinemachineCameraTarget = PlayerRuntimeSet.ActivePlayer.Object.CinemachineTarget.gameObject;
        }
        else
        {
            Debug.LogError("new target is null.");
        }
    }
}
