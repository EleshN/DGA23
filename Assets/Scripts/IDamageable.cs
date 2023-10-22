using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This interface will be implemented by the animals, robot, bases
/// </summary>
public interface IDamageable
{
    void TakeDamage(float damageAmount);

    /// <summary>
    /// Damageable entities may switch to a temporary state in which an opposing
    /// entity may not directly inflict damage. One such case is when an angry
    /// dog becomes happy and becomes immune to further attacks.
    /// </summary>
    /// <returns>true if entity can be damaged </returns>
    bool isDamageable()
    {
        return true;
    }
}
