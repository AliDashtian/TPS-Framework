using UnityEngine;

public static class AnimationIDs
{
    // --- STANCE / LOCOMOTION ---
    public static int StanceIndex = Animator.StringToHash("Stance"); // 0=Stand, 1=Crouch, 2=Prone

    // --- ACTIONS ---
    public static int IsIdle = Animator.StringToHash("IsIdle");
    public static int IsAiming = Animator.StringToHash("IsAiming");
    public static int Fire = Animator.StringToHash("Fire");
    //public static int IsAiming = Animator.StringToHash("IsAiming");
    public static int IsReloading = Animator.StringToHash("IsReloading");
    public static int IsSwapping = Animator.StringToHash("IsSwapping");

    // --- TRIGGERS ---
    public static int Die = Animator.StringToHash("Die");
}
