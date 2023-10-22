using UnityEngine;
using System.Collections;

public class MeleeEnemy : Enemy
{

    void OnCollisionStay(Collision collision)
    {
        GameObject other = collision.gameObject;
        IDamageable entity = other.GetComponent<IDamageable>();
        if (entity != null && entity.isDamageable() &&
            GameManager.Instance.TeamPlayer.Contains(other.transform))
        {
            entity.TakeDamage(robotDamage);
        }
    }



    protected override void Attack()
    {
        // Todo: does nothing for now, since melee attack is done on entity collision.
        // perhaps for melee enemies, this is where we animate the attack motion.
        return;
    }
}
