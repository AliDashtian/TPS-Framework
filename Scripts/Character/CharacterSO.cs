using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Scriptable Objects/Character/CharacterData")]
public class CharacterSO : ScriptableObject
{
    public string Name;
    public int Id;
    public FloatReference MaxHealth;
    public float WeaponSwapDuration = 0.7f;
}
