using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [Tooltip("The target(s) the hitbox is trying to hurt")]
    [SerializeField] protected List<string> tagTargets;
    [SerializeField] protected float damage;

    // [SerializeField] public List<string> tagTargets;
    // [SerializeField] public float damage;

    public Transform attackerTransform;

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if (tagTargets.Contains(other.tag))
        {
            other.GetComponent<IDamageable>().TakeDamage(damage, attackerTransform);
            gameObject.SetActive(false);
        }
    }

}
