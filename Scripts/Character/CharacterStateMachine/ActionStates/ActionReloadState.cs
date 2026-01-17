using UnityEngine;

public class ActionReloadState : CharacterState
{
    private float _reloadDuration = 0f;

    public ActionReloadState(BaseCharacter baseCharacter, StateMachine stateMachine) : base(baseCharacter, stateMachine) { }

    public override void Enter()
    {
        _reloadDuration = baseCharacter.GetCurrentWeapon().WeaponData.ReloadDuration;
        baseCharacter.Animator.SetBool(AnimationIDs.IsReloading, true);
        AudioManager.Instance.PlaySound(baseCharacter.GetCurrentWeapon().WeaponData.ReloadSound, baseCharacter.transform.position);
        baseCharacter.OnReload?.Invoke();
    }

    public override void Update()
    {
        _reloadDuration -= Time.deltaTime;

        // Exit out of reload state when reload duration is passed
        if (_reloadDuration <= 0)
        {
            baseCharacter.GetCurrentWeapon().Reload();
            stateMachine.ChangeState(baseCharacter.IdleState);
        }
    }

    public override void Exit()
    {
        baseCharacter.Animator.SetBool(AnimationIDs.IsReloading, false);
    }
}
