
using UnityEngine;

public class ActionFireState : CharacterState
{
    public ActionFireState(BaseCharacter baseCharacter, StateMachine stateMachine) : base(baseCharacter, stateMachine) { }

    public override void Enter()
    {
        baseCharacter.GetCurrentWeapon().OnFire(true);
        baseCharacter.Animator.SetBool(AnimationIDs.IsFiring, true);
    }

    public override void Update()
    {

        // Exit the fire state if we ran out of ammo while firing
        if (!baseCharacter.GetCurrentWeapon().HasAmmo())
        {
            stateMachine.ChangeState(baseCharacter.IdleState);
        }
    }

    public override void Exit()
    {
        baseCharacter.GetCurrentWeapon().OnFire(false);
        baseCharacter.Animator.SetBool(AnimationIDs.IsFiring, false);
    }
}
