using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : Enemy
{

    [SerializeField] Transform spawnLocationTransform;

    [SerializeField] float projectileEffectRadius = 15f;

    [SerializeField] GameObject projectilePrefab;

    /// <summary>
    /// the radius of the bullet (used for capsule casting in order to determine whether target can be attacked)
    /// </summary>
    float bulletRadius;

    protected override void Start()
    {
        base.Start();
        SphereCollider collider = spawnLocationTransform.GetComponent<SphereCollider>();
        bulletRadius = collider.radius;
    }

    protected override void Attack()
    {
        agent.Destination = transform.position; // stop moving, then shoot
        Vector3 spawnPosition = spawnLocationTransform.position;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        // make sure summoned projectile does not hit the summoner
        Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
        EnemyProjectile bullet = projectile.GetComponent<EnemyProjectile>();
        bullet.Initialize(robotDamage, projectileEffectRadius, transform);
        bullet.SetDirection(targetTransform.position - spawnPosition);   
    }

    protected override bool CanAttack()
    {
        if (Vector3.Magnitude(targetTransform.position - transform.position) > attackRadius)
        {
            return false;
        }
        bool raycastResult = false;
        Vector3 dir = targetTransform.position - transform.position;
        if (Physics.CapsuleCast(transform.position, targetTransform.position, bulletRadius, dir, out RaycastHit hit, attackRadius, Physics.DefaultRaycastLayers,  QueryTriggerInteraction.Ignore))
        {
            raycastResult = hit.collider.gameObject.transform == targetTransform;
        }
        return raycastResult;
    }
}
