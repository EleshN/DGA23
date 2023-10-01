using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implements IDamageable Interface
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] Rigidbody rb;
    public float maxHealth;
    public float currentHealth;
    public float myDamage;
    // Position of target
    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if collision has player tag
        if (collision.gameObject.CompareTag("Animal"))
        {
            GameObject attackedObject = collision.gameObject;
            // attackedObject.TakeDamage(myDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        //currentHealth -= damage;
        // if (currentHealth <= 0){Die()}
    }

    void Move()
    {
        // target = (GameManager) Find target NOTE: Prioritize Bases, only
        // target animal that attacks
        // SetTarget(target)
        // Movement - rb.velocity...
    }

    public void Die()
    {
        // Destory object and play animation
        Debug.Log("An enemy has died!");
        Destroy(gameObject);
    }
}
