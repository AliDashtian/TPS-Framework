using UnityEngine;

public class ActionDeadState : CharacterState
{
    public ActionDeadState(BaseCharacter baseCharacter, StateMachine stateMachine) : base(baseCharacter, stateMachine) { }

    public override void Enter()
    {
        baseCharacter.Animator.Play(AnimationIDs.Die);
    }

    public override void Update()
    {

    }

    public override void Exit() 
    {

    }
}
