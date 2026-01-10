using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    [field: SerializeField]
    public WeaponInventorySO WeaponLoadout { get; private set; }

    /// <summary>
    /// List of Weapon Sockets on character's body
    /// </summary>
    private List<WeaponSocket> WeaponSockets = new();

    /// <summary>
    /// Gives a socket transform based on its socket type
    /// </summary>
    private Dictionary<WeaponSocketTypes, Transform> SocketMap = new();

    private Weapon _mainWeapon;
    private Weapon _secondaryWeapon;
    public Weapon CurrentWeapon { get; private set; }

    private void Awake()
    {
        GetWeaponSocketsAndMap();

        _mainWeapon = Instantiate(WeaponLoadout.MainWeapon);
        AttachWeaponToSocket(_mainWeapon, WeaponSocketTypes.RightHand);

        if (WeaponLoadout.SecondaryWeapon != null)
        {
            _secondaryWeapon = Instantiate(WeaponLoadout.SecondaryWeapon);
            AttachWeaponToSocket(_secondaryWeapon, _secondaryWeapon.WeaponData.HolsterType);
        }

        CurrentWeapon = _mainWeapon;
    }

    /// <summary>
    /// Get all of the WeaponSocket(s) in children gameobjects and maps their transform to their SocketType
    /// </summary>
    private void GetWeaponSocketsAndMap()
    {
        WeaponSockets.AddRange(GetComponentsInChildren<WeaponSocket>().ToList());

        foreach (WeaponSocket weaponSocket in WeaponSockets)
        {
            if (!SocketMap.ContainsKey(weaponSocket.SocketType))
            {
                SocketMap.Add(weaponSocket.SocketType, weaponSocket.transform);
            }
        }
    }

    private void AttachWeaponToSocket(Weapon weapon, WeaponSocketTypes socketType)
    {
        if (SocketMap.TryGetValue(socketType, out Transform holsterSocket))
        {
            weapon.transform.SetParent(holsterSocket, true);
        }

        if (socketType == WeaponSocketTypes.RightHand)
        {
            weapon.transform.SetLocalPositionAndRotation(weapon.WeaponData.WeaponAttachmentPositions.InHandPos,
                                             Quaternion.Euler(weapon.WeaponData.WeaponAttachmentPositions.InHandRot));

            return;
        }

        weapon.transform.SetLocalPositionAndRotation(weapon.WeaponData.WeaponAttachmentPositions.InHolsterPos,
                                             Quaternion.Euler(weapon.WeaponData.WeaponAttachmentPositions.InHolsterRot));
    }

    /// <summary>
    /// Attaches the current weapon to it's holster,
    /// We should change this to UnequipCurrentWeapon if we want to unequip weapon and use "Fist"
    /// </summary>
    public void HolsterCurrentWeapon()
    {
        if (CurrentWeapon == null) return;

        AttachWeaponToSocket(CurrentWeapon, CurrentWeapon.WeaponData.HolsterType);
    }

    /// <summary>
    /// Method suitable for 2-Weapon inventory system (Main & Secondary Weapon)
    /// </summary>
    public void EquipOtherWeapon()
    {
        if (CurrentWeapon == _mainWeapon)
        {
            CurrentWeapon = _secondaryWeapon;
        }
        else
        {
            CurrentWeapon = _mainWeapon;
        }

        AttachWeaponToSocket(CurrentWeapon, WeaponSocketTypes.RightHand);
    }

    public bool CanSwap()
    {
        return (_secondaryWeapon != null) && (_mainWeapon != null);
    }
}
