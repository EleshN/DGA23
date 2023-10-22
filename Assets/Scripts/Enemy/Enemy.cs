using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float robotDamage;

    IDamageable targetState;
    Transform targetTransform;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        GameManager.Instance.Register(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform == null || !targetState.isDamageable())
        {
            selectNewTarget();
        }
        // just stay still if there is no more targets on map, the game must end.
        if (targetTransform != null)
        {
            Move(targetTransform.position);
            Attack();
        }
    }

    protected virtual void Move(Vector3 targetPosition)
    {
        // always move the entity closer to target
        agent.stoppingDistance = 0;
        agent.destination = targetPosition;
    }

    protected abstract void Attack();
    

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GameManager.Instance.Unregister(this);
            Destroy(gameObject);
        }
    }

    public void selectNewTarget()
    {
        GameObject target = GameManager.Instance.FindClosestTargetForEnemmy(this);
        if (target == null)
        {
            targetTransform = null;
            targetState = null;
        }
        else
        {
            targetTransform = target.transform;
            targetState = target.GetComponent<IDamageable>();
        }
    }
}
