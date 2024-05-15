using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoBeetle : MeleeEnemy
{

    [Header("Beetle Specific Combat Stats")]
    [Tooltip("the entities and the damage they receive from the beetle. overrides all other damage stats")]
    [SerializeField] HitboxDamage[] damageValues;
    [SerializeField] RhinoBounceBox rhinoBounceBox;
    [SerializeField] float slideTime;
    [SerializeField] float slideSpeed;
    [SerializeField] float slideDistance;

    protected override void Start()
    {
        base.Start();
        // initialize hitbox damage values
        hitbox.Initialize();
        hitbox.SetDamage(damageValues);
        rhinoBounceBox.slideTime = slideTime;
        rhinoBounceBox.slideSpeed = slideSpeed;
        rhinoBounceBox.slideDistance = slideDistance;

        rhinoBounceBox.ShouldHit = true;
    }

    protected override void Attack()
    {
        if (!rhinoBounceBox.ShouldHit)
        {
            rhinoBounceBox.ShouldHit = true;
        }
        base.Attack();
    }

    private void OnDestroy()
    {
        rhinoBounceBox.EndSlide();
        if (GameManager.Instance != null)
            GameManager.Instance.Unregister(this);
    }

}
