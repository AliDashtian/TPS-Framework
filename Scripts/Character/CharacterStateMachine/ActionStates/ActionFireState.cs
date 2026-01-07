
using UnityEngine;

public class ActionFireState : CharacterState
{
    private float _minimumFireDuration = 0.2f;
    private float _time = 0;

    public ActionFireState(BaseCharacter baseCharacter, StateMachine stateMachine) : base(baseCharacter, stateMachine) { }

    public override void Enter()
    {
        _time = 0;
        baseCharacter.GetCurrentWeapon().OnFire(true);
        baseCharacter.Animator.SetBool(AnimationIDs.IsFiring, true);
        baseCharacter.OnWeaponFired?.Invoke(true);
    }

    public override void Update()
    {
        _time += Time.deltaTime;

        // Exit the fire state if we ran out of ammo while firing
        if (!baseCharacter.GetCurrentWeapon().HasAmmo())
        {
            stateMachine.ChangeState(baseCharacter.IdleState);
        }

        if (baseCharacter.GetCurrentWeapon().WeaponData.FireType == WeaponFireType.SingleShot && _time >= _minimumFireDuration)
        {
            stateMachine.ChangeState(baseCharacter.IdleState);
        }
    }

    public override void Exit()
    {
        baseCharacter.GetCurrentWeapon().OnFire(false);
        baseCharacter.Animator.SetBool(AnimationIDs.IsFiring, false);
        baseCharacter.OnWeaponFired?.Invoke(false);
    }
}
