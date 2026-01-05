/// <summary>
/// Defines a contract for any game object that can take damage.
/// This follows the Open/Closed and Interface Segregation principles.
/// Any new damageable object (player, enemy, crate) can implement this
/// without the damaging source (bullet, sword) needing to change.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// The current health of the object.
    /// </summary>
    float CurrentHealth { get; }

    /// <summary>
    /// The maximum health of the object.
    /// </summary>
    float MaxHealth { get; }

    /// <summary>
    /// Applies a specified amount of damage to the object.
    /// </summary>
    /// <param name="damageAmount">The amount of damage to apply.</param>
    void TakeDamage(float damageAmount, DamageType damageType);
}
