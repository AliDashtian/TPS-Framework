using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public abstract class CharacterStance : MonoBehaviour
{
    public enum CharacterStances { Stand, Crouch, Prone, CoverStandR, CoverStandL, SideCoverCrouchR, SideCoverCrouchL, CoverCrouch }
    public CharacterStances InitialStance;

    private Animator _animator;

    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetInteger(AnimationIDs.StanceIndex, (int)InitialStance);


        //if (InitialStance == CharacterStances.Crouch && Spine2 != null)
        //{
        //    Spine2.weight = 0.4f;
        //}

        //if (isEnemy)
        //{
        //    if (InitialStance == CharacterStances.SideCoverCrouchR)
        //    {
        //        enemy.YawRotationOffset = 20;
        //    }
        //    if (InitialStance == CharacterStances.SideCoverCrouchL)
        //    {
        //        enemy.YawRotationOffset = 0;
        //    }
        //}
    }
}
