using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implements IDamageable Interface
public class Enemy : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public int health;
    public int damage;
    // Position of player???
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if collision has player tag
    }

    void TakeDamage()
    {
        //health-=passed in damage
        // if (damage >= health){Die()}
    }

    void Move()
    {
        // SetTarget()
    }

    void Die()
    {
        // Destory object and play animation
    }
}
