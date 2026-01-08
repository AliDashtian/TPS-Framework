using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] private bool _drawRadiusGizmo;
    [SerializeField] internal float _damage = 100;
    [SerializeField] internal float _impactForce = 20f;
    [SerializeField] internal float _explosionRadius = 20f;

    [SerializeField] private ParticleSystem _impactEffect;

    private bool _hasExploded;

    private void Awake()
    {
        if (TryGetComponent(out Health health))
        {
            health.OnDeath += React;
        }
    }

    public virtual void React()
    {
        if (_hasExploded) return;

        _hasExploded = true;

        Invoke(nameof(Explode), 0.2f);
    }

    private void Explode()
    {
        PlayImpactEffect();

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider collider in colliders)
        {
            // Skip this iteration if the collider belongs to the explosive itself
            if (collider.gameObject == gameObject) continue;

            if (collider.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage, DamageType.Explosion);
            }

            if (collider.gameObject.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(_impactForce, transform.position, _explosionRadius, 1f, ForceMode.Impulse);
            }
        }

        Destroy(gameObject, 0.1f);
    }

    protected void PlayImpactEffect()
    {
        // Play impact effect
        if (_impactEffect != null)
        {
            Instantiate(_impactEffect.gameObject, transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        if (_drawRadiusGizmo)
        {
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}
