using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : Explosive
{
    [Header("Settings")]
    [SerializeField] private float _reactDelay = 5f;
    [SerializeField] private float _launchUpwardForce;
    [SerializeField] private bool _destroyOnImpact = true;

    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();

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

        //gameObject.transform.forward = Camera.main.transform.forward;

        LaunchProjectile(launchForce);

        // React after a delay
        Invoke(nameof(React), _reactDelay);
    }

    private void LaunchProjectile(float launchForce)
    {
        // Find the exact hit point of the crosshair
        Vector3 targetPoint;

        // Raycast from the center of the camera
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 500f))
        {
            targetPoint = hit.point;
        }
        else
        {
            // If we look at the sky, aim at a point far away
            targetPoint = Camera.main.transform.position + Camera.main.transform.forward * 500f;
        }

        // Calculate direction from the THROW POINT to the TARGET POINT
        Vector3 direction = (targetPoint - transform.position).normalized;

        transform.forward = direction;

        // Add force in that specific direction
        Vector3 forceToAdd = direction * launchForce /*+ transform.up * _launchUpwardForce*/;

        rb.AddForce(forceToAdd, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_destroyOnImpact) return;

        React();
    }
}
