using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : Enemy
{

    /// <summary>
    /// the minimum distance to maintain from target to shoot projectile
    /// </summary>
    [SerializeField] float shootingDistance = 4;

    [SerializeField] float verticalOffset = 0.5f;

    [SerializeField] float projectileEffectRadius = 1.5f;

    [SerializeField] GameObject projectilePrefab;

    /// <summary>
    /// the maximum number of times to attack per second
    /// </summary>
    [SerializeField] float attackRate = 2;

    float nextAttackTime = 0;

    protected override void Move(Vector3 targetPosition)
    {
        // always move the entity closer to target
        agent.stoppingDistance = shootingDistance/2;
        agent.destination = targetPosition;
    }

    protected override void Attack()
    {
        if (currentAtackTime > 0){
            return;
        }
        // check within shooting range
        if (Vector3.Magnitude(targetTransform.position - transform.position) <= shootingDistance)
        {
            // summon projectile when attacking cooldown is over
            if (Time.time > nextAttackTime){
                nextAttackTime = Time.time + 1.0f/attackRate;
                Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + verticalOffset, transform.position.z);
                GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
                // make sure summoned projectile does not hit the summoner
                Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
                EnemyProjectile bullet = projectile.GetComponent<EnemyProjectile>();
                print(bullet);
                bullet.initialize(robotDamage, projectileEffectRadius, transform);
                bullet.SetDirection(targetTransform.position - spawnPosition);
            }
        }
        
    }
}
