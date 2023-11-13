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
    public void initialize(float damage, float attackRadius, Transform projectileSource)
    {
        this.damage = damage;
        this.attackRadius = attackRadius;
        this.projectileSource = projectileSource;
    }

    protected override void HandleCollision(Collider collision)
    {
        Collider[] neabyColliders = Physics.OverlapSphere(transform.position, attackRadius);
        // find all animals and playerbases and damage them if possible
        foreach (Collider col in neabyColliders)
        {
            if (col.gameObject.tag == "Animal" || col.gameObject.tag == "PlayerBase")
            {
                IDamageable entity = col.gameObject.GetComponent<IDamageable>();
                entity.TakeDamage(damage, projectileSource);
            }
        }
        // base.HandleCollision(collision);
        Destroy(gameObject);
    }
}
