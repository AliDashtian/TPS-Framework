using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileWeapon", menuName = "Scriptable Objects/Weapon/Projectile Weapon")]
public class ProjectileWeapon : WeaponSO
{
    [Header("Projectile Specifics")]
    [Tooltip("The projectile prefab to be instantiated.")]
    [SerializeField] private Projectile projectilePrefab;

    [Tooltip("The force with which the projectile is launched.")]
    [SerializeField] private float launchForce = 50f; 

    [Tooltip("Radius of explosion of the projectile.")]
    [SerializeField] private float explosionRadius = 20f;

    [Tooltip("Projectile Spawn Delay in miliseconds")]
    [SerializeField] private int ProjectileSpawnDelay = 200;

    public override void Fire(Weapon weapon)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab is not set for this firing strategy.");
            return;
        }

        _ = InstantiateProjectileWithDelay(weapon, ProjectileSpawnDelay);
    }

    private async UniTask InstantiateProjectileWithDelay(Weapon weapon, int delay)
    {
        await UniTask.Delay(delay);

        // Instantiate the projectile at the muzzle position
        Projectile projectile = Instantiate(projectilePrefab, weapon.MuzzleTransform.position, Camera.main.transform.rotation);
        projectile.Initialize(weapon.WeaponData.Damage, weapon.WeaponData.ImpactForce, explosionRadius, launchForce);
    }
}
