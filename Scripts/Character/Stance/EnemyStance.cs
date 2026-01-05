using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyStance : CharacterStance
{
    public List<StanceSetting> StanceSettings = new();

    [Serializable]
    public struct StanceSetting
    {
        public CharacterStances Stance;
        public int YawOffset;
    }

    protected override void Start()
    {
        base.Start();

        float yawOffset = StanceSettings.Where(s => s.Stance == InitialStance).ToList()[0].YawOffset;

        if (TryGetComponent(out Enemy enemy))
        {
            FaceTarget(enemy.Target, yawOffset);
        }
    }

    public void FaceTarget(Transform target, float yawOffset)
    {
        Vector3 dir = target.position - transform.position;
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, targetAngle + yawOffset, 0f);
    }
}
