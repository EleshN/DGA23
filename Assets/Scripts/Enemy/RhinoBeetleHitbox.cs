using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoBeetleHitbox : Hitbox
{
    protected override void OnTriggerStay(Collider other)
    {
        if (base.tagTargets.Contains(other.tag))
        {
            if (other.gameObject.CompareTag("PlayerBase")) {
                other.GetComponent<IDamageable>().TakeDamage(base.damage * 2, attackerTransform);
                gameObject.SetActive(false);
            }
            other.GetComponent<IDamageable>().TakeDamage(base.damage, attackerTransform);
            gameObject.SetActive(false);
        }
    }
}
