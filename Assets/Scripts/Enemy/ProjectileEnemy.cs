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
        agent.Destination = transform.position; // stop moving, then shoot
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + verticalOffset, transform.position.z);
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        // make sure summoned projectile does not hit the summoner
        Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
        EnemyProjectile bullet = projectile.GetComponent<EnemyProjectile>();
        bullet.Initialize(robotDamage, projectileEffectRadius, transform);
        bullet.SetDirection(targetTransform.position - spawnPosition);   
    }

    protected override bool CanAttack()
    {
        bool raycastResult = false;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, attackRadius, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            raycastResult = hit.collider.gameObject.transform == targetTransform;
        }
        return base.CanAttack() && raycastResult;
    }
}
