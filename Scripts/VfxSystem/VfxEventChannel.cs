using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VFX Channel", menuName = "Scriptable Objects/VFX/VFX Channel")]
public class VfxEventChannel : ScriptableObject
{
    // We pass the Prefab reference (as the ID) and the spawn info
    public UnityAction<ParticleSystem, Vector3, Quaternion> OnPlayVfx;

    public void SpawnParticle(ParticleSystem vfxPrefab, Vector3 position, Quaternion rotation)
    {
        if (vfxPrefab == null) return;

        if (OnPlayVfx != null)
        {
            OnPlayVfx.Invoke(vfxPrefab, position, rotation);
        }
        else
        {
            Debug.LogWarning("VFX Requested, but nobody is listening!");
        }
    }
}