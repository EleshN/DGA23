using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This interface will be implemented by the animals, robot, bases
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="damageAmount">the damage dealt</param>
    /// <param name="damageSource">the game entity dealing the damage</param>
    void TakeDamage(float damageAmount, Transform damageSource);
}
