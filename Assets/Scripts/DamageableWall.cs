using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This interface will be implemented by walls
/// </summary>
public interface DamageableWall
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="damageAmount">the damage dealt</param>
    void TakeDamage(float damageAmount);
}
