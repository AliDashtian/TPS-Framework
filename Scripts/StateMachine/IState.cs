
public interface IState
{
    void Enter();
    void Update();
    void FixedUpdate();
    void AnimationTriggerEvent(AnimationTriggerTypes triggerType);
    void Exit();
}
