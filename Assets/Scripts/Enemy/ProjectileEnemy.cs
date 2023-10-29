using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : Enemy
{

    /// <summary>
    /// the minimum distance to maintain from target to shoot projectile
    /// </summary>
    [SerializeField] float shootingDistance = 4;

    [SerializeField] float verticalOffset = 2.5f;

    [SerializeField] GameObject projectilePrefab;

    [SerializeField] float nextAttackTime = 0;

    /// <summary>
    /// the maximum number of times to attack per second
    /// </summary>
    [SerializeField] float attackRate = 2;

    protected override void Move(Vector3 targetPosition)
    {
        // always move the entity closer to target
        agent.stoppingDistance = shootingDistance;
        agent.destination = targetPosition;
    }

    protected override void Attack()
    {
        // check within shooting range
        if (Vector3.Magnitude(targetTransform.position - transform.position) <= shootingDistance)
        {
            // summon projectile when attacking cooldown is over
            if (Time.time > nextAttackTime){
                nextAttackTime = Time.time + 1.0f/attackRate;
                Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + verticalOffset, transform.position.z);
                GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
                EnemyProjectile bullet = projectile.GetComponent<EnemyProjectile>();
                bullet.SetDamage(robotDamage);
                print(targetTransform.position - spawnPosition);
                bullet.SetDirection(targetTransform.position - spawnPosition);
            }
        }
        
    }
}
