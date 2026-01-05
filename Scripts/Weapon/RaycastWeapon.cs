using UnityEngine;

[CreateAssetMenu(fileName = "RaycastWeapon", menuName = "Scriptable Objects/Weapon/Raycast Weapon")]
public class RaycastWeapon : WeaponSO
{
    public override void Fire(Weapon weapon)
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, weapon.WeaponData.FireRange, weapon.WeaponData.ShootableLayers))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * weapon.WeaponData.FireRange, Color.red, 2f);
            Debug.DrawRay(hit.point, hit.normal * 5, Color.blue, 4f);

            PhysicalMaterialData.PlayParticleAndSoundByLayer(hit.transform.gameObject.layer, hit);
            AddImpactForce(hit, weapon.WeaponData);

            if (hit.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(weapon.WeaponData.Damage, DamageType.Bullet);
            }
        }
    }

    private void AddImpactForce(RaycastHit hit, WeaponSO weaponData)
    {
        if (hit.transform.TryGetComponent(out Rigidbody rb))
        {
            if (!rb.isKinematic)
            {
                rb.AddForceAtPosition(-hit.normal * weaponData.ImpactForce, hit.point, ForceMode.Impulse);
            }
        }
    }
}
