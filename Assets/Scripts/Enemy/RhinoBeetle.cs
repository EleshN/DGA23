using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoBeetle : MeleeEnemy
{

    [Header("Beetle Specific Combat Stats")]
    [Tooltip("the entities and the damage they receive from the beetle. overrides all other damage stats")]
    [SerializeField] HitboxDamage[] damageValues;

    protected override void Start()
    {
        base.Start();
        // initialize hitbox damage values
        hitbox.Initialize();
        hitbox.SetDamage(damageValues);
    }
}
