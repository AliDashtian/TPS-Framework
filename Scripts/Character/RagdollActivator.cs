using System.Collections.Generic;
using UnityEngine;

public class RagdollActivator : MonoBehaviour
{
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
        Debug.Log("YOYOYO");

        foreach (Rigidbody rb in _bodies)
        {
            rb.useGravity = value;
            rb.isKinematic = !value;
        }
    }
}
