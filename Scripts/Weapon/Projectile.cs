using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : Explosive
{
    [Header("Settings")]
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private Vector3 launchDirectionOffset;
    [SerializeField] private bool destroyOnImpact = true;
    [SerializeField] private ParticleSystem impactEffect;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Initializes the projectile with its necessary values
    /// </summary>
    public void Initialize(float damage, float impactForce, float explosionRadius, float launchForce)
    {
        _damage = damage;
        _impactForce = impactForce;
        _explosionRadius = explosionRadius;

        LaunchProjectile(launchForce);
        // Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifeTime);
    }

    private void LaunchProjectile(float launchForce)
    {
        Vector3 force =
        new Vector3(Camera.main.transform.forward.x + launchDirectionOffset.x,
        Camera.main.transform.forward.y + launchDirectionOffset.y,
        Camera.main.transform.forward.z + launchDirectionOffset.z);

        rb.AddForce(force * launchForce, ForceMode.Impulse);
    }

    private void OnDestroy()
    {
        React();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!destroyOnImpact) return;

        // Play impact effect
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
