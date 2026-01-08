using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] internal float _damage = 100;
    [SerializeField] internal float _impactForce = 20f;
    [SerializeField] internal float _explosionRadius = 20f;

    [SerializeField] private ParticleSystem _impactEffect;

    private void Awake()
    {
        if (TryGetComponent(out Health health))
        {
            health.OnDeath += React;
        }
    }

    internal void React()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage, DamageType.Explosion);
            }

            if (collider.gameObject.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(_impactForce, transform.position, _explosionRadius, 1f, ForceMode.Impulse);
            }
        }

        PlayImpactEffect();
    }

    protected void PlayImpactEffect()
    {
        // Play impact effect
        if (_impactEffect != null)
        {
            Instantiate(_impactEffect.gameObject, transform.position, Quaternion.identity);
        }
    }
}
