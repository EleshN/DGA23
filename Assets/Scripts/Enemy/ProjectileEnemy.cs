using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : Enemy
{

    /// <summary>
    /// the y-offset from the enemy's position to spawn the projectile
    /// </summary>
    [SerializeField] float verticalOffset = 0.5f;

    [SerializeField] float projectileEffectRadius = 15f;

    [SerializeField] GameObject projectilePrefab;

    protected override void Attack()
    {
        agent.destination = transform.position; // stop moving
        print("set agent to stop moving");
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + verticalOffset, transform.position.z);
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        // make sure summoned projectile does not hit the summoner
        Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
        EnemyProjectile bullet = projectile.GetComponent<EnemyProjectile>();
        bullet.initialize(robotDamage, projectileEffectRadius, transform);
        bullet.SetDirection(targetTransform.position - spawnPosition);   

        // // check whether target can be seen:
        // Ray ray = new Ray(transform.position, transform.forward);
        // RaycastHit hit;
        // if (Physics.Raycast(ray, out hit, attackRadius))
        // {
            
        //     print("hit: " + hit.collider.gameObject.name);
        //     if (hit.collider.gameObject.transform == targetTransform)
        //     {
                
        //     }
        //     else {
        //         state = EnemyState.CHASE;
        //         return;
        //     }
            
        // }
    }
}
