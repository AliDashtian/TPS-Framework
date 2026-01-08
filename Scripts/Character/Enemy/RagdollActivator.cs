using System.Collections.Generic;
using UnityEngine;

public class RagdollActivator : MonoBehaviour
{
    [SerializeField]
    private float PhysicsSleepDelay = 5f;

    private List<Rigidbody> _bodies = new();

    private void Start()
    {
        _bodies.AddRange(GetComponentsInChildren<Rigidbody>());
    }

    public void SetActive(bool value)
    {
        if (TryGetComponent(out Animator animator))
        {
            animator.enabled = !value;
        }

        foreach (Rigidbody rb in _bodies)
        {
            rb.useGravity = value;
            rb.isKinematic = !value;
        }

        Invoke(nameof(SleepPhyscis), PhysicsSleepDelay);
    }

    void SleepPhyscis()
    {
        foreach (Rigidbody rb in _bodies)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }
}
