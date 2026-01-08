
using Cysharp.Threading.Tasks;

/// <summary>
/// In this game mechainc, we don't have a separate state for Aiming
/// Aim and fire both are tied to a single button
/// </summary>
public class ActionAimFireState : CharacterState
{
    public ActionAimFireState(BaseCharacter baseCharacter, StateMachine stateMachine) : base(baseCharacter, stateMachine) { }

    public override void Enter()
    {
        baseCharacter.GetCurrentWeapon().OnFire(true);
        baseCharacter.Animator.SetBool(AnimationIDs.IsAiming, true);
        baseCharacter.OnWeaponAimed?.Invoke(true);
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
        if (baseCharacter.GetCurrentWeapon().WeaponData.FireType == WeaponFireType.SingleShot)
        {
            _ = ExitWithDelay(300);
        }
        else
        {
            _ = ExitWithDelay(0);
        }
    }

    private async UniTask ExitWithDelay(int delay)
    {
        await UniTask.Delay(delay);

        baseCharacter.Animator.SetBool(AnimationIDs.IsAiming, false);
        baseCharacter.OnWeaponAimed?.Invoke(false);
    }
}
