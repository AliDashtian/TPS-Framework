using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponSO WeaponData;

    public int CurrentPouchAmmo;
    public int CurrentMagAmmo;

    public bool isFiring;

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

    [Tooltip("The transform representing the weapon's muzzle for spawning effects.")]
    [SerializeField]
    private Transform _muzzleTransform;

    private BaseCharacter _ownerCharacter;
    private AudioSource _audioSource;

    private float _nextFireTime;

    private int _maxPouchAmmo;
    private int _magCapacity;

    private bool _shouldApplyRecoil;

    private ParticleSystem _muzzlePS;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _maxPouchAmmo = WeaponData.PouchCapacity;
        _magCapacity = WeaponData.MagCapacity;

        CurrentPouchAmmo = _maxPouchAmmo;
        CurrentMagAmmo = _magCapacity;

        _ownerCharacter = GetComponentInParent<BaseCharacter>();
    }

    private void FixedUpdate()
    {
        if (WeaponData.FireType == WeaponFireType.Automatic && isFiring)
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
        isFiring = true;
    }

    public virtual void FireButtonReleased()
    {
        if (WeaponData.FireType == WeaponFireType.SingleShot)
        {
            Fire();
        }

        isFiring = false;
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
            _shouldApplyRecoil = true;
            _nextFireTime = Time.time + 1f / WeaponData.FireRate;
            WeaponData.Fire(this);
            PlayFireEffects();
            CurrentMagAmmo--;
        }
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
        if (isFiring && _shouldApplyRecoil)
        {
            pitch -= Random.Range(12f, 14f) * Time.deltaTime; // up down
            yaw -= Random.Range(-22f, 20f) * Time.deltaTime; // left right

            _shouldApplyRecoil = false;
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
}
