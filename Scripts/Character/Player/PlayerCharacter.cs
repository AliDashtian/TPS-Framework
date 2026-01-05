using System;
using UnityEngine;

public class PlayerCharacter : BaseCharacter
{
    public RuntimeSet<PlayerCharacter> Set;

    public Transform CinemachineTarget;

    private bool _isActive;

    private void OnEnable()
    {
        if (Set != null)
        {
            Set.Add(this);
        }
    }

    private void OnDestroy()
    {
        RemoveSelfFromSet();
    }

    protected override void CharacterDied()
    {
        base.CharacterDied();
        RemoveSelfFromSet();
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    private void RemoveSelfFromSet()
    {
        if (Set != null && Set.Contains(this))
        {
            Set.Remove(this);
        }
    }
}
