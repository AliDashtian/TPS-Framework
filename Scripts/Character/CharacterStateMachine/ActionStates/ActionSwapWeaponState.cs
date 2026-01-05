using UnityEngine;

public class ActionSwapWeaponState : CharacterState
{
    private float _swapDuration = 0f;

    // Safety flags to track progress
    private bool _hasHolstered = false;
    private bool _hasEquipped = false;

    public ActionSwapWeaponState(BaseCharacter baseCharacter, StateMachine stateMachine) : base(baseCharacter, stateMachine) { }

    public override void Enter()
    {
        _swapDuration = baseCharacter.CharacterData.WeaponSwapDuration;

        // Reset flags
        _hasHolstered = false;
        _hasEquipped = false;

        baseCharacter.Animator.SetBool(AnimationIDs.IsSwapping, true);
        baseCharacter.Animator.CrossFadeInFixedTime(baseCharacter.GetCurrentWeapon().WeaponData.UnequipAnimName, 0.2f);
    }

    public override void Update()
    {
        _swapDuration -= Time.deltaTime;

        if (_swapDuration <= 0f)
        {
            stateMachine.ChangeState(baseCharacter.IdleState);
        }
    }

    public override void AnimationTriggerEvent(AnimationTriggerTypes triggerType)
    {
        if (triggerType == AnimationTriggerTypes.EquipWeapon)
        {
            EquipWeapon();
        }
        else
        {
            HolsterWeapon();
        }
    }

    private void HolsterWeapon()
    {
        if (!_hasHolstered)
        {
            baseCharacter.GetWeaponInventory().HolsterCurrentWeapon();
            _hasHolstered = true;
        }
    }

    private void EquipWeapon()
    {
        if (!_hasEquipped)
        {
            baseCharacter.GetWeaponInventory().EquipOtherWeapon();
            baseCharacter.OnWeaponSwapped?.Invoke(); // Update UI
            _hasEquipped = true;
        }
    }

    public override void Exit()
    {
        baseCharacter.Animator.SetBool(AnimationIDs.IsSwapping, false);

        // FAILSAFE: If the animation was interrupted (e.g. by getting hit or dying)
        // before the swap finished, force the swap to complete so the player 
        // doesn't lose their gun or get stuck holding the wrong one.
        if (!_hasHolstered)
        {
            baseCharacter.GetWeaponInventory().HolsterCurrentWeapon();
        }
        if (!_hasEquipped)
        {
            baseCharacter.GetWeaponInventory().EquipOtherWeapon();
            baseCharacter.OnWeaponSwapped?.Invoke();
        }
    }
}
