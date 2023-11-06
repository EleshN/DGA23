using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    EnemyState state = EnemyState.SPAWN;
    EnemyState prevState = EnemyState.SPAWN;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] float maxHealth;
    [SerializeField] protected float health;
    [SerializeField] HealthBar healthBar;
    [SerializeField] protected float robotDamage;
    [SerializeField] protected float attackCountDown = 5f;
    protected float currentAtackTime;
    [SerializeField] protected float stopCountDown = 5f;
    protected float currentStopTime;

    IDamageable targetState;
    Transform targetTransform;

    private ColorIndicator colorIndicator;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.SetHealthBar(maxHealth);
        currentAtackTime = attackCountDown;
        GameManager.Instance.Register(this);
        colorIndicator = GetComponent<ColorIndicator>();
    }

    // Update is called once per frame
    void Update()
    {
        prevState = state;
        currentAtackTime -= Time.deltaTime;

        if (targetTransform == null || !GameManager.Instance.ValidEnemyTargets.Contains(targetTransform))
        {
            state = EnemyState.WANDER;
        }

        switch (state)
        {
            case EnemyState.SPAWN:
                state = EnemyState.WANDER;
                break;

            case EnemyState.WANDER:
                selectNewTarget();
                if (targetTransform != null && GameManager.Instance.ValidEnemyTargets.Contains(targetTransform))
                {
                    state = EnemyState.ATTACK;
                }
                else
                {
                    state = EnemyState.STOP;
                }
                break;

            case EnemyState.CHASE:
                Move(targetTransform.position);
                break;

            case EnemyState.STOP:
                currentStopTime -= Time.deltaTime;
                if (currentStopTime <= 0)
                {
                    state = EnemyState.WANDER;
                    currentStopTime = stopCountDown;
                }
                break;

            case EnemyState.ATTACK:
                Move(targetTransform.position);
                Attack();
                state = prevState;
                break;

            default:
                break;
        }
        if (currentAtackTime <= 0)
        {
            currentAtackTime = attackCountDown;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        state = EnemyState.STOP;
    }

    void OnCollisionExit(Collision collision)
    {
        if (targetTransform == null || !GameManager.Instance.ValidEnemyTargets.Contains(targetTransform))
        {
            state = EnemyState.WANDER;
        }
        else
        {
            state = EnemyState.CHASE;
        }
    }

    protected virtual void Move(Vector3 targetPosition)
    {
        // always move the entity closer to target
        agent.destination = targetPosition;
    }

    protected abstract void Attack();


    public void TakeDamage(float damage, Transform damageSource)
    {
        targetTransform = damageSource;
        health -= damage;
        healthBar.UpdateHealthBar(health);
        colorIndicator.IndicateDamage();

        if (health <= 0)
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
