using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRuntimeObject", menuName = "Scriptable Objects/RuntimeObjects/PlayerRuntimeObject")]
public class PlayerRuntimeObject : RuntimeObject<PlayerCharacter>
{
    /// <summary>
    /// Helper methode for Firing for the use of UI
    /// </summary>
    /// <param name="newFire"></param>
    public void Fire(bool newFire)
    {
        Object.Fire(newFire);
    }

    public void Reload()
    {
        Object.AttemptReload();
    }

    public void SwapWeapon()
    {
        Object.AttemptSwapWeapon();
    }
}
