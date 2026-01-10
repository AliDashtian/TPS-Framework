using Unity.Cinemachine;
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
    public CinemachineCamera CinemachineVCam;

    [Tooltip("Look sensivity in both axes")]
    public float LookSensivity = 10.0f;

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

    [Tooltip("Camera's default CameraDistance when not Zooming")]
    public float DefaultCamDistance = 2.6f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    private GameObject _cinemachineCameraTarget;
    private CinemachineThirdPersonFollow _cinemachineThirdPersonFollow;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private float _currentLookSensivity;

    private const float _threshold = 0.01f;

    private void Start()
    {
        _cinemachineThirdPersonFollow = CinemachineVCam.GetComponent<CinemachineThirdPersonFollow>();

        PlayerRuntimeSet.OnCharacterSwitched += ChangeFollowTarget;
        PlayerRuntimeSet.OnCharacterSwitched += ChangeCinemachineTarget;

        _currentLookSensivity = LookSensivity;

        // Subscribe Zoom to all of the Players
        foreach (PlayerCharacter player in PlayerRuntimeSet.Items)
        {
            player.OnTriggerPulled += Zoom;
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
            _cinemachineTargetYaw += lookInput.x * _currentLookSensivity * Time.deltaTime;
            _cinemachineTargetPitch += lookInput.y * _currentLookSensivity * Time.deltaTime;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, LeftClamp, RightClamp);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        //Apply recoil
        if (PlayerRuntimeSet.ActivePlayer.Object && PlayerRuntimeSet.ActivePlayer.Object.GetCurrentWeapon().WeaponData.RecoilData)
        {
            PlayerRuntimeSet.ActivePlayer.Object.GetCurrentWeapon().ApplyRecoil(ref _cinemachineTargetYaw, ref _cinemachineTargetPitch);
        }

        // Cinemachine will follow this target
        _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }

    void ChangeCurrentWeaponAim()
    {

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
            _currentLookSensivity = weaponData.AimSensivity;
            StartCoroutine(ChangeFOVCoroutine(weaponData.ZoomFOV, weaponData.ZoomDuration));
            StartCoroutine(ChangeCameraDistance(weaponData.ZoomCamDistance, weaponData.ZoomDuration));
        }
        else
        {
            _currentLookSensivity = LookSensivity;
            StartCoroutine(ChangeFOVCoroutine(DefaultFOV, weaponData.ZoomDuration));
            StartCoroutine(ChangeCameraDistance(DefaultCamDistance, weaponData.ZoomDuration));
        }
    }

    IEnumerator ChangeFOVCoroutine(int newFOV, float duration)
    {
        float time = 0;
        float initFOV = CinemachineVCam.Lens.FieldOfView;

        while (time <= duration)
        {
            time += Time.deltaTime;
            CinemachineVCam.Lens.FieldOfView = Mathf.Lerp(initFOV, newFOV, time / duration);

            yield return null;
        }
    }

    IEnumerator ChangeCameraDistance(float newCamDistance, float duration)
    {
        float time = 0;
        float initCamDist = _cinemachineThirdPersonFollow.CameraDistance;

        while (time <= duration)
        {
            time += Time.deltaTime;
            _cinemachineThirdPersonFollow.CameraDistance = Mathf.Lerp(initCamDist, newCamDistance, time / duration);

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
