using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [Tooltip("The target(s) the hitbox is trying to hurt")]
    [SerializeField] List<string> tagTargets;
    [SerializeField] float damage;

    [SerializeField] Transform attackerTransform;

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
    private void OnTriggerStay(Collider other)
    {
        print(other.gameObject.name);
        if (tagTargets.Contains(other.tag))
        {
            other.GetComponent<IDamageable>().TakeDamage(damage, attackerTransform);
            gameObject.SetActive(false);
        }
    }

}
