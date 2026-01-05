using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float time = 3f;

    private void Start()
    {
        Destroy(gameObject, time);
    }
}
