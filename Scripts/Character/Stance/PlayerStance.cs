using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerStance : CharacterStance
{
    public MultiAimConstraint Hip;
    public MultiAimConstraint Spine2;

    protected override void Start()
    {
        base.Start();

        if (InitialStance == CharacterStances.Crouch && Spine2 != null)
        {
            Spine2.weight = 0.4f;
        }
    }
}
