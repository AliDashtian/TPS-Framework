
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PhysicalMaterialDataBase", menuName = "Scriptable Objects/PhysicalMaterialDataBase")]
public class PhysicalMaterialDataBaseSO : ScriptableObject
{
    public List<PhysicalMaterialSO> physicalMaterials;

    public PhysicalMaterialSO GetPhysicalMaterialByName(string name)
    {
        foreach (var material in physicalMaterials)
        {
            if (material.name == name)
            {
                return material;
            }
        }
        return null; // Return null if no material with the given name is found
    }

    public void PlayParticleAndSoundByLayer(LayerMask layer, RaycastHit hit)
    {
        foreach (var material in physicalMaterials)
        {
            if (1 << layer.value == material.Layer.value)
            {
                if (material.hitPS != null)
                    Instantiate(material.hitPS, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
                
                if (material.hitSound != null)
                    AudioSource.PlayClipAtPoint(material.hitSound, hit.point);
            }
        }
    }
}
