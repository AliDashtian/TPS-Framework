using Cysharp.Threading.Tasks;

/// <summary>
/// In this game mechainc, we don't have a separate state for Aiming
/// Aim and fire both are tied to a single button
/// </summary>
public class ActionAimFireState : CharacterState
{
    private Weapon _currentWeapon = null;
    public ActionAimFireState(BaseCharacter baseCharacter, StateMachine stateMachine) : base(baseCharacter, stateMachine) { }

    public override void Enter()
    {
        _currentWeapon = baseCharacter.GetCurrentWeapon();
        _currentWeapon.OnFire(true);
        baseCharacter.Animator.SetBool(AnimationIDs.IsAiming, true);
        _currentWeapon.OnAimed?.Invoke(true);
        baseCharacter.OnTriggerPulled?.Invoke(true);
    }

    public override void Update()
    {
        // Exit the fire state if we ran out of ammo while firing
        if (!_currentWeapon.HasAmmo())
        {
            stateMachine.ChangeState(baseCharacter.IdleState);
        }
    }

    public override void Exit()
    {
        _currentWeapon.OnFire(false);
        if (_currentWeapon.WeaponData.FireType == WeaponFireType.SingleShot)
        {
            _ = ExitWithDelay(600);
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
        _currentWeapon.OnAimed?.Invoke(false);
        baseCharacter.OnTriggerPulled?.Invoke(false);
    }
}
