
public class ActionAimState : CharacterState
{
    public ActionAimState(BaseCharacter baseCharacter, StateMachine stateMachine) : base(baseCharacter, stateMachine) { }

    public override void Enter()
    {
        baseCharacter.Animator.SetBool(AnimationIDs.IsAiming, true);

        if (baseCharacter.GetCurrentWeapon().WeaponData.FireType == WeaponFireType.Automatic)
        {
            stateMachine.ChangeState(baseCharacter.FireState);
        }
    }

    public override void Exit()
    {
        baseCharacter.Animator.SetBool(AnimationIDs.IsAiming, false);
    }
}
