using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This interface will only be implemented by the animal and robot
/// </summary>
public interface IDamageable
{
    void TakeDamage(float damageAmount);
    void Die();
}
