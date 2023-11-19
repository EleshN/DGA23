using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamHitbox : Hitbox
{
    [SerializeField] float bonusBaseDamage;

    
    protected override void OnTriggerStay(Collider other)
    {
        if (tagTargets.Contains(other.tag))
        {
            float bonusDamage = 0;
            Debug.Log(other.GetComponent<IDamageable>().GetType().Name);
            if (other.GetComponent<IDamageable>() is EnemyBase)
            {
                bonusDamage += bonusBaseDamage;
                
            }
            other.GetComponent<IDamageable>().TakeDamage(damage + bonusDamage, attackerTransform);
            gameObject.SetActive(false);
        }
    }
    

}
