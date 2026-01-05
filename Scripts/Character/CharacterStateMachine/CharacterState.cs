
public abstract class CharacterState : IState
{
    protected BaseCharacter baseCharacter;
    protected StateMachine stateMachine;

    public CharacterState(BaseCharacter baseCharacter, StateMachine stateMachine)
    {
        this.baseCharacter = baseCharacter;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void FixedUpdate() { }

    public virtual void Update() { }

    public virtual void AnimationTriggerEvent(AnimationTriggerTypes triggerType) { }
}
