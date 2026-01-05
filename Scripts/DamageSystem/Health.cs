using UnityEngine;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// A reusable component for managing health, taking damage, and healing.
/// It implements IDamageable, making it a concrete implementation that
/// damage-dealing scripts can interact with.
/// </summary>
public class Health : MonoBehaviour, IDamageable
{
    /// <summary>
    /// Public getter for current health, fulfilling the IDamageable interface.
    /// </summary>
    public float CurrentHealth => currentHealth.Value;

    /// <summary>
    /// Public getter for max health, fulfilling the IDamageable interface.
    /// </summary>
    public float MaxHealth => maxHealth.Value;

    /// <summary>
    /// what happens when health reaches certain percentages. e.g. when on 50% a car
    /// starts to emmit smoke, at 30 it catches fire and when Die() it explodes
    /// </summary>
    [Serializable]
    public struct DamageStages
    {
        public float percentage;
        public UnityEvent StageAction;
    }

    [SerializeField]
    [Tooltip("What happens when health reaches certain percentages")]
    private List<DamageStages> damageStages;

    /// <summary>
    /// Fired when health changes.
    /// Passes (currentHealth, maxHealth) for UI or other listeners.
    /// </summary>
    public event Action<float, float> OnHealthChanged;

    /// <summary>
    /// Fired when health reaches zero.
    /// </summary>
    public event Action OnDeath;

    [SerializeField]
    [Tooltip("The maximum health value.")]
    private FloatReference maxHealth;

    [SerializeField]
    [Tooltip("The current health value.")]
    private FloatReference currentHealth;

    private bool _isDead = false;

    private void Start()
    {
        // Initialize health
        currentHealth.Value = maxHealth.Value;
    }

    /// <summary>
    /// Implementation of the IDamageable interface.
    /// Applies damage and checks for death.
    /// </summary>
    /// <param name="damageAmount">The amount of damage to take.</param>
    public void TakeDamage(float damageAmount, DamageType damageType = DamageType.Bullet)
    {
        if (_isDead) return; // Already dead, do nothing.

        // Ensure damage is positive
        if (damageAmount < 0)
        {
            damageAmount = 0;
        }

        currentHealth.Value -= damageAmount;
        CheckDamageStage();

        ClampCurrentHealth();

        // Broadcast the health changed event
        OnHealthChanged?.Invoke(currentHealth.Value, maxHealth.Value);

        // Check for death
        if (currentHealth.Value <= 0)
        {
            Die();
        }
    }

    private void CheckDamageStage()
    {
        if (damageStages.Count <= 0) return;

        float healthPercentage = currentHealth.Value * 100 / maxHealth.Value;

        foreach (var stage in damageStages)
        {
            if (stage.percentage == healthPercentage)
            {
                stage.StageAction.Invoke();
            }
        }
    }

    /// <summary>
    /// Applies healing to the object.
    /// </summary>
    /// <param name="healAmount">The amount of health to restore.</param>
    public void Heal(int healAmount)
    {
        if (_isDead) return; // Cannot heal the dead

        // Ensure heal amount is positive
        if (healAmount < 0)
        {
            healAmount = 0;
        }

        currentHealth.Value += healAmount;

        ClampCurrentHealth();

        // Broadcast the health changed event
        OnHealthChanged?.Invoke(currentHealth.Value, maxHealth.Value);
    }

    /// <summary>
    /// Handles the death logic.
    /// Marked as 'virtual' so specific classes (e.g., PlayerHealth)
    /// can override this to add unique death behaviors (like ragdolling).
    /// </summary>
    protected virtual void Die()
    {
        if (_isDead) return; // Ensure Die() is only called once

        _isDead = true;

        // Broadcast the death event
        OnDeath?.Invoke();

        // Optional: Log the death
        Debug.Log($"{gameObject.name} has died.");

        // maybe add default behavior here, like:
        // Destroy(gameObject, 5f); // Destroy the object after 5 seconds
    }

    /// <summary>
    /// A simple way to reset health, e.g., on respawn.
    /// </summary>
    public void ResetHealth()
    {
        currentHealth.Value = maxHealth.Value;
        _isDead = false;
        OnHealthChanged?.Invoke(currentHealth.Value, maxHealth.Value);
    }

    /// <summary>
    /// Clamp health to not go below 0 or above maxHealth
    /// </summary>
    private void ClampCurrentHealth()
    {
        currentHealth.Value = Mathf.Clamp(currentHealth.Value, 0, maxHealth.Value);
    }
}
