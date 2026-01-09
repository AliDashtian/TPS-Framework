using UnityEngine;

[CreateAssetMenu(fileName = "RecoilSettings", menuName = "Scriptable Objects/Weapon/Recoil")]
public class WeaponRecoilSO : ScriptableObject
{
    [Header("Recoil Settings")]
    // Use X and Y to define the Min and Max kick strength
    // TIP: Ensure RecoilKickY values are BOTH positive (e.g., 1 and 2) to avoid jitter
    public Vector2 RecoilKickX = new Vector2(-1.0f, 1.0f); // Randomly Left/Right
    public Vector2 RecoilKickY = new Vector2(1.0f, 3.0f);  // Always Up

    public float RecoilSnappiness = 20f; // Snappy rise
    public float RecoilReturnSpeed = 10f; // Smooth return
    public float RecoilRecoveryDelay = 0.2f; // TIME TO WAIT before returning
}
