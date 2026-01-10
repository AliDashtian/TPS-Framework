using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public WeaponSO WeaponData;

    [Tooltip("The transform representing the weapon's muzzle for spawning effects.")]
    [SerializeField]
    private Transform _muzzleTransform;

    public Action<bool> OnAimed;

    public int CurrentPouchAmmo {get; private set;}
    public int CurrentMagAmmo { get; private set;}

    public Transform Target
    {
        get
        {
            return _target;
        }
        set
        {
            if (WeaponData is PretendWeapon)
            {
                _target = value;
            }
        }
    }

    private Transform _target;

    public Transform MuzzleTransform => _muzzleTransform;

    private BaseCharacter _ownerCharacter;
    private AudioSource _audioSource;
    private CinemachineImpulseSource _impulseSource;

    private RecoilSystem _recoilSystem;

    private float _nextFireTime;

    private int _maxPouchAmmo;
    private int _magCapacity;

    private bool _isFiring;

    private ParticleSystem _muzzlePS;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        SetupAmmo();

        _ownerCharacter = GetComponentInParent<BaseCharacter>();

        if (WeaponData.RecoilData != null)
        {
            _recoilSystem = new RecoilSystem(WeaponData.RecoilData);
        }
    }

    private void Update()
    {
        if (WeaponData.FireType == WeaponFireType.Automatic && _isFiring)
        {
            Fire();
        }
    }

    public void OnFire(bool newFire)
    {
        if (newFire)
        {
            FireButtonPressed();
        }
        else
        {
            FireButtonReleased();
        }
    }

    public virtual void FireButtonPressed()
    {
        _isFiring = true;
    }

    public virtual void FireButtonReleased()
    {
        if (WeaponData.FireType == WeaponFireType.SingleShot)
        {
            Fire();
        }

        _isFiring = false;
    }

    public void Fire()
    {
        if (WeaponData == null)
        {
            Debug.LogError("Weapon data or firing strategy is not set!");
            return;
        }

        //if (_isReloading) return;

        if (CurrentMagAmmo <= 0)
        {
            // Optional: Play an empty clip sound
            FireButtonReleased();
            return;
        }

        if (Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + 1f / WeaponData.FireRate;
            WeaponData.Fire(this);
            PlayFireEffects();
            CurrentMagAmmo--;

            if (_recoilSystem != null)
            {
                _recoilSystem.UpdateRecoilTarget();
            }

            if (_impulseSource != null)
            {
                _impulseSource.GenerateImpulse();
            }
        }

        // Turn off the mesh if we don't have ammo, used for throwables like Grenade
        if (WeaponData.DisableMeshIfNoAmmo && !HasAmmo())
        {
            SetMeshEnabled(false);
        }
    }

    private void SetupAmmo()
    {
        _maxPouchAmmo = WeaponData.PouchCapacity;
        _magCapacity = WeaponData.MagCapacity;

        CurrentPouchAmmo = _maxPouchAmmo;
        CurrentMagAmmo = _magCapacity;
    }

    public void Reload()
    {
        int roundsNeededToRefillMag = GetMagAmmoFiredNumber();

        if (roundsNeededToRefillMag <= CurrentPouchAmmo)
        {
            CurrentMagAmmo += roundsNeededToRefillMag;
            CurrentPouchAmmo -= roundsNeededToRefillMag;
        }
        else
        {
            CurrentMagAmmo += CurrentPouchAmmo;
            CurrentPouchAmmo = 0;
        }
    }

    public void PlayFireEffects()
    {
        if (WeaponData.MuzzlePS != null)
        {
            // Instantiate the muzzle flash at the muzzle's position and orientation
            if (_muzzlePS == null)
            {
                _muzzlePS = Instantiate(WeaponData.MuzzlePS, _muzzleTransform.position, _muzzleTransform.rotation, _muzzleTransform);
            }
            _muzzlePS.Play();
        }

        if (WeaponData.FireSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(WeaponData.FireSound);
        }

        _ownerCharacter.Animator.SetTrigger(AnimationIDs.Fire);
    }

    public void ApplyRecoil(ref float yaw, ref float pitch)
    {
        if (_recoilSystem != null)
        {
            _recoilSystem.ApplyRecoil(ref yaw, ref pitch);
        }
    }

    public bool CanReload()
    {
        return CurrentMagAmmo < WeaponData.MagCapacity && CurrentPouchAmmo > 0;
    }

    public bool HasAmmo()
    {
        return CurrentMagAmmo > 0;
    }

    int GetMagAmmoFiredNumber()
    {
        int ShotsFired = _magCapacity - CurrentMagAmmo;
        return ShotsFired;
    }

    void SetMeshEnabled(bool enable)
    {
        GetComponent<MeshRenderer>().enabled = enable;
    }
}
