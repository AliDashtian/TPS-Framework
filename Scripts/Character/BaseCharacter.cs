using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(WeaponInventory))]
public abstract class BaseCharacter : MonoBehaviour
{
    public CharacterSO CharacterData;

    public Animator Animator { get; set; }

    public Action OnReload;
    public Action OnWeaponSwapped;
    public Action<bool> OnWeaponFired;

    #region State Machine

    public StateMachine ActionStateMachine { get; private set; }

    /// <summary>
    /// I don't currently use the Stance State Machine because stance state doesn't change during gameplay,
    /// instead I'm using a simple enum to let me select initial stance
    /// but later if I want to expand on this as a true TPS, I should use stance state machine
    /// </summary>
    //public StateMachine StanceStateMachine { get; private set; }

    public ActionFireState FireState;
    public ActionIdleState IdleState;
    public ActionReloadState ReloadState;
    public ActionSwapWeaponState SwapState;
    public ActionDeadState DeadState;

    //public StanceStandState StandState;
    //public StanceCrouchState CrouchState;
    //public StanceProneState ProneState;

    #endregion State Machine

    protected WeaponInventory weaponInventory;

    protected virtual void Awake()
    {
        weaponInventory = GetComponent<WeaponInventory>();
        Animator = GetComponent<Animator>();

        if (TryGetComponent(out Health health))
        {
            health.OnDeath += CharacterDied;
        }

        ActionStateMachine = new StateMachine();
        //StanceStateMachine = new StateMachine();

        FireState = new ActionFireState(this, ActionStateMachine);
        IdleState = new ActionIdleState(this, ActionStateMachine);
        ReloadState = new ActionReloadState(this, ActionStateMachine);
        SwapState = new ActionSwapWeaponState(this, ActionStateMachine);
        DeadState = new ActionDeadState(this, ActionStateMachine);

        //StandState = new StanceStandState(this, StanceStateMachine);
        //CrouchState = new StanceCrouchState(this, StanceStateMachine);
        //ProneState = new StanceProneState(this, StanceStateMachine);
    }

    protected virtual void Start()
    {
        ActionStateMachine.Initialize(IdleState);
        //StanceStateMachine.Initialize(StandState);
    }

    private void Update()
    {
        ActionStateMachine?.Update();
        //StanceStateMachine?.Update();
    }

    /// <summary>
    /// Start or stop firing based on input parameter
    /// </summary>
    /// <param name="newFire"> if true start firing, if false stop firing</param>
    public virtual void Fire(bool newFire)
    {
        //pass the input to the current state, or decide to switch states
        if (newFire)
        {
            // Only transition to firing if we are Idle (or add logic to interrupt reload)
            if (ActionStateMachine.CurrentState == IdleState && GetCurrentWeapon().HasAmmo())
            {
                ActionStateMachine.ChangeState(FireState);
                OnWeaponFired?.Invoke(true);
            }
        }
        else
        {
            // If we are firing, go back to idle
            if (ActionStateMachine.CurrentState == FireState)
            {
                ActionStateMachine.ChangeState(IdleState);
                OnWeaponFired?.Invoke(false);
            }
        }
    }

    public virtual void AttemptReload()
    {
        if (ActionStateMachine.CurrentState == IdleState && GetCurrentWeapon().CanReload())
        {
            ActionStateMachine.ChangeState(ReloadState);
        }
    }

    public void AttemptSwapWeapon()
    {
        if (ActionStateMachine.CurrentState == IdleState && weaponInventory.CanSwap())
        {
            ActionStateMachine.ChangeState(SwapState);
        }
    }

    protected virtual void CharacterDied()
    {
        ActionStateMachine.ChangeState(DeadState);
    }

    private void AnimationTriggerEvent(AnimationTriggerTypes triggerType)
    {
        ActionStateMachine?.CurrentState.AnimationTriggerEvent(triggerType);
    }

    public Weapon GetCurrentWeapon()
    {
        return weaponInventory.CurrentWeapon;
    }

    public WeaponInventory GetWeaponInventory()
    {
        return weaponInventory;
    }
}
