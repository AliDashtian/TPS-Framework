using UnityEngine;

public class AddGameobjectToRuntimeSet : MonoBehaviour
{
    public GameobjectRuntimeSet Set;

    private void OnEnable()
    {
        if (Set != null)
        {
            Set.Add(gameObject);
        }
    }

    private void OnDisable()
    {
        if (Set != null)
        {
            Set.Remove(gameObject);
        }
    }
}
