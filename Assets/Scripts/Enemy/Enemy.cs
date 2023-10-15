using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Implements IDamageable Interface
public class Enemy : MonoBehaviour, IDamageable
{
    public NavMeshAgent agent;
    [SerializeField] Rigidbody rb;
    public float maxHealth;
    public float currentHealth;
    public float myDamage;
    Transform target;
    // Position of target

    // Start is called before the first frame update
    void Start()
    {
        // Find the closest base
        // Some foreach method: FindClosestTarget(gameObject.transform.position)
        // Set what base to home-in on (Nearest from last step)
        //target = GameObject.Find("Base").transform;
        rb.useGravity = true;
        currentHealth = maxHealth;
        GameManager.Instance.TeamEnemy.Add(gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if collision has player tag
        //if (collision.gameObject.CompareTag(""))
        //{
        //    GameObject attackedObject = collision.gameObject;
        //    // attackedObject.TakeDamage(myDamage);
        //}
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
        // setting target to
        //agent.destination = target.position;
        // Movement - rb.velocity...
    }

    public void Die()
    {
        // Destory object and play animation
        Debug.Log("An enemy has died!");
        Destroy(gameObject);
    }
}
