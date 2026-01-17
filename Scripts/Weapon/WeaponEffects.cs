using Unity.Cinemachine;
using UnityEngine;

public class WeaponEffects : MonoBehaviour
{
    private Transform _muzzleTransform;
    private CinemachineImpulseSource _impulseSource;
    private Weapon _weapon;
    private WeaponSO _weaponData;

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
        _weaponData = _weapon.WeaponData;
        _muzzleTransform = _weapon.MuzzleTransform;

        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable()
    {
        _weapon.OnWeaponFired += PlayFireEffects;
    }

    private void OnDisable()
    {
        _weapon.OnWeaponFired -= PlayFireEffects;
    }

    public void PlayFireEffects()
    {
        if (_impulseSource != null)
        {
            _impulseSource.GenerateImpulse();
        }

        if (_weaponData.VfxChannel != null)
        {
            _weaponData.VfxChannel.SpawnParticle(_weaponData.MuzzlePS, _muzzleTransform.position, _muzzleTransform.rotation);
        }

        if (_weaponData.SfxChannel != null)
        {
            _weaponData.SfxChannel.Play3DSound(_weaponData.FireSound, transform.position);
        }

        _weapon.OwnerCharacter.Animator.SetTrigger(AnimationIDs.Fire);
    }
}
