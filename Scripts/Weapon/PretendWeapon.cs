using UnityEngine;

/// <summary>
/// Pretend weapon is a weapon that doesn't cast rays or instantiate projectiles to detect target and damages it,
/// it just tries to damage a target that is assigned to it
/// </summary>
[CreateAssetMenu(fileName = "PretendWeapon", menuName = "Scriptable Objects/Weapon/Pretend Weapon")]
public class PretendWeapon : WeaponSO
{
    public override void Fire(Weapon weapon)
    {
        if (weapon.Target.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(weapon.WeaponData.Damage, DamageType.Bullet);
        }
    }
}
