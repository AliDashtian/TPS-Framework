using Unity.VisualScripting;
using UnityEngine;

public class BodyPartHitBox : MonoBehaviour, IDamageable
{
    [Tooltip("Main Health that actually takes damage, will look for parent's health if left empty")]
    [SerializeField]
    private Health _healthBrain;

    [SerializeField]
    private BodyPartSO _bodyPartData;

    [Tooltip("If should use this class's multiplier instead, will use this if BodyPartData is null")]
    [SerializeField]
    private bool overrideDamageMultiplier = false;

    [SerializeField]
    private float _damageMultiplier = 1;

    public float CurrentHealth { get; set; }

    public float MaxHealth { get; set; }

    private void Start()
    {
        if (_healthBrain == null)
        {
            _healthBrain = GetComponentInParent<Health>();
        }

        if (_bodyPartData == null)
        {
            overrideDamageMultiplier = true;
        }
    }

    public void TakeDamage(float damageAmount, DamageType damageType)
    {
        float finalMultiplier = overrideDamageMultiplier ? _damageMultiplier : _bodyPartData.DamageMultiplier;
        damageAmount *= finalMultiplier;

        if (_healthBrain != null)
        {
            _healthBrain.TakeDamage(damageAmount, damageType);
        }
    }
}
