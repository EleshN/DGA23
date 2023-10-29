using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [Tooltip("The target the hitbox is trying to hurt")]
    [SerializeField] string tagTarget;
    [SerializeField] float damage;


    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
    private void OnTriggerStay(Collider other)
    {
        print(other.gameObject.name);
        if (other.CompareTag(tagTarget))
        {
            other.GetComponent<IDamageable>().TakeDamage(damage);
            print("Damaged: " + other.gameObject.name);
            gameObject.SetActive(false);
        }
    }

}
