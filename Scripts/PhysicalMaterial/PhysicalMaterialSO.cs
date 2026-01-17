using UnityEngine;

[CreateAssetMenu(fileName = "PhysicalMaterial", menuName = "Scriptable Objects/PhysicalMaterial")]
public class PhysicalMaterialSO : ScriptableObject
{
    public string Name;

    [Tooltip("Each physical material should have only one layer")]
    public LayerMask Layer;

    public ParticleSystem hitPS;
    public SoundSO hitSound;
}
