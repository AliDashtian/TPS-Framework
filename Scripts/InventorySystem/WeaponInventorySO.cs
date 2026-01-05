using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInventory", menuName = "Scriptable Objects/Inventory/WeaponInventory")]
public class WeaponInventorySO : ScriptableObject
{
    public Weapon MainWeapon;
    public Weapon SecondaryWeapon;
}
