using UnityEngine;
using System.Collections;

public class MeleeEnemy : Enemy
{
    private float attackCooldown = 1;
    private float lastAttack;

    void OnCollisionStay(Collision collision)
    {
        print("enemy colliding with something");
        GameObject other = collision.gameObject;
        IDamageable entity = other.GetComponent<IDamageable>();
        if (entity != null && entity.isDamageable() &&
            GameManager.Instance.TeamPlayer.Contains(other.transform) && 
            Time.time > lastAttack + attackCooldown)
        {
            entity.TakeDamage(robotDamage);
            lastAttack = Time.time;
        }
    }



    protected override void Attack()
    {
        // Todo: does nothing for now, since melee attack is done on entity collision.
        // perhaps for melee enemies, this is where we animate the attack motion.
        return;
    }
}
