using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    float attackRadius;

    float damage;

    Transform projectileSource;

    /// <summary>
    /// initialize projectile with the given stats
    /// </summary>
    /// <param name="damage">the damage dealt to targets</param>
    /// <param name="attackRadius">the radius of the projectile effect</param>
    public void Initialize(float damage, float attackRadius, Transform projectileSource)
    {
        this.damage = damage;
        this.attackRadius = attackRadius;
        this.projectileSource = projectileSource;
    }

    protected override void HandleCollision(Collider collision)
    {
        // make sure we hit colliders of game objects.
        if (collision.isTrigger)
        {
            return;
        }
        Collider[] neabyColliders = Physics.OverlapSphere(transform.position, attackRadius);
        // find all animals and playerbases and damage them if possible
        foreach (Collider col in neabyColliders)
        {
            if (col.gameObject.CompareTag("Animal") || col.gameObject.CompareTag("PlayerBase"))
            {
                IDamageable entity = col.gameObject.GetComponent<IDamageable>();
                entity.TakeDamage(damage, projectileSource);
            }
        }
        base.HandleCollision(collision);
    }
}
