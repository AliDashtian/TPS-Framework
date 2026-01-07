using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseCharacter
{
    public RuntimeSet<Enemy> Set;

    public Transform Target;

    public float FireDelay;

    protected override void Start()
    {
        base.Start();

        // Enemy currently uses pretend (fake) weapon, 
        // meaning that weapons won't actuallt Cast rays, they just damage their target
        GetCurrentWeapon().Target = Target;

        StartCoroutine(FireCoroutine());
    }

    private void OnEnable()
    {
        if (Set != null)
        {
            Set.Add(this);
        }
    }

    private void OnDisable()
    {
        RemoveSelfFromSet();
    }

    IEnumerator FireCoroutine()
    {
        // TODO : simple fire pattern, change it later
        while (true)
        {
            yield return new WaitForSeconds(FireDelay);
            AimOrFire(true);
            yield return new WaitForSeconds(3f);
            AimOrFire(false);
            yield return new WaitForSeconds(0.2f);
            AttemptReload();
            yield return null;
        }
    }

    protected override void CharacterDied()
    {
        base.CharacterDied();
        RemoveSelfFromSet();
    }

    private void RemoveSelfFromSet()
    {
        if (Set != null && Set.Contains(this))
        {
            Set.Remove(this);
        }
    }
}
