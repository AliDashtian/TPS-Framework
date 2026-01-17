using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PhysicalMaterialDataBase", menuName = "Scriptable Objects/PhysicalMaterialDataBase")]
public class PhysicalMaterialDataBaseSO : ScriptableObject
{
    public List<PhysicalMaterialSO> physicalMaterials;

    [Header("Broadcasting")]
    [SerializeField] private AudioEventChannel _sfxChannel;
    [SerializeField] private VfxEventChannel _vfxChannel;

    /// <summary>
    /// Call this from Weapon or Projectile script
    /// </summary>
    public void HandleImpact(RaycastHit hit)
    {
        // 1. Find the material (Optimization: You could use a Dictionary lookup here if the list is huge)
        PhysicalMaterialSO material = GetMaterialForLayer(hit.collider.gameObject.layer);

        if (material == null) return;

        // 2. Request VFX (if assigned)
        if (material.hitPS != null)
        {
            _vfxChannel.SpawnParticle(material.hitPS, hit.point, Quaternion.LookRotation(hit.normal));
        }

        // 3. Request SFX (if assigned)
        if (material.hitSound != null)
        {
            _sfxChannel.Play3DSound(material.hitSound, hit.point);
        }
    }

    private PhysicalMaterialSO GetMaterialForLayer(int layerIndex)
    {
        // Simple linear search is fine for < 50 materials
        foreach (var material in physicalMaterials)
        {
            // Bitmask check: (1 << layerIndex) compares the layer integer to the LayerMask bit
            if ((material.Layer.value & (1 << layerIndex)) != 0)
            {
                return material;
            }
        }
        return null;
    }
}