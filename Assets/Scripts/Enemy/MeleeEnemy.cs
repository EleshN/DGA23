using UnityEngine;
using System.Collections;

public class MeleeEnemy : Enemy
{
    private IDamageable entity;

    void OnCollisionStay(Collision collision)
    {
        GameObject other = collision.gameObject;
        entity = other.GetComponent<IDamageable>();
        if (entity != null && entity.isDamageable() &&
            GameManager.Instance.TeamPlayer.Contains(other.transform) && 
            currentAtackTime <= 0)
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        // Todo: does nothing for now, since melee attack is done on entity collision.
        // perhaps for melee enemies, this is where we animate the attack motion.
        entity.TakeDamage(robotDamage);
    }
}
