using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    protected EnemyState state = EnemyState.SPAWN;

    [SerializeField] protected NavMeshObstacleAgent agent;

    protected int spawnTimeAvoidancePriority;

    [SerializeField] HealthBar healthBar;

    ColorIndicator colorIndicator;

    /// <summary>
    /// remaining timer before next attack (enemy is able to attack when this value is not greater than 0)
    /// </summary>
    protected float currentAttackTime;

    protected Transform targetTransform;

    [Header("Stats")]
    [SerializeField] float speed;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;

    [Header("Combat")]
    [Tooltip("Minimum distance between the enemy and target to initiate an attack")]
    [SerializeField] protected float attackRadius = 2f;
    [Tooltip("Delay between each attack")]
    [SerializeField] protected float attackCountDown = 5f;
    [SerializeField] protected float robotDamage;

    void Awake()
    {
        agent = GetComponent<NavMeshObstacleAgent>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health = maxHealth;
        healthBar.SetHealthBar(maxHealth);
        currentAttackTime = 0;
        // spawnTimeAvoidancePriority = GameManager.Instance.Register(this);
        // agent.avoidancePriority = spawnTimeAvoidancePriority;
        colorIndicator = GetComponent<ColorIndicator>();
        agent.SetSpeed(speed);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (currentAttackTime > 0)
        {
            currentAttackTime -= Time.deltaTime;
        }
        
        // target eliminated or no longer a target
        if (targetTransform == null || !GameManager.Instance.ValidEnemyTargets.Contains(targetTransform))
        {
            // agent.avoidancePriority = spawnTimeAvoidancePriority;
            state = EnemyState.STOP;
        }

        switch (state)
        {
            case EnemyState.SPAWN:
                state = EnemyState.STOP;
                break;

            case EnemyState.STOP:
                selectNewTarget();
                if (targetTransform != null && GameManager.Instance.ValidEnemyTargets.Contains(targetTransform))
                {
                    state = EnemyState.CHASE;
                }
                break;

            case EnemyState.CHASE:
                Move(targetTransform.position);
                break;

            case EnemyState.ATTACK:
                // too far to attack, transition to moving
                if (!CanAttack())
                {
                    // too far or something in the way
                    state = EnemyState.CHASE;
                    return;
                }
                agent.SetObstacleMode();
                if (currentAttackTime <= 0)
                {
                    // attack and reset attack cooldown timer
                    currentAttackTime = attackCountDown;
                    Attack();
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// without considering any cooldowns, this checks for conditions to initate an attack.
    /// for example, physical enemies need to be within a small range to start an attack.
    /// </summary>
    /// <returns>whether an enemy can initiate an attack</returns>
    protected virtual bool CanAttack()
    {
        return Vector3.Magnitude(targetTransform.position - transform.position) <= attackRadius;
    }

    /// <summary>
    /// moves the enemy forward to the target position. sets the enemy state to ATTACK when within attack radius.
    /// If enemy needs to be stopped from moving any further, modify stopping distance or destination.
    /// </summary>
    /// <param name="targetPosition"></param>
    protected virtual void Move(Vector3 targetPosition)
    {
        // always move the entity closer to target
        agent.SetAgentMode();
        agent.SetDestination(targetPosition);
        agent.SetStoppingDistance(0);
        if (Vector3.Magnitude(targetTransform.position - transform.position) <= attackRadius)
        {
            state = EnemyState.ATTACK;
        }
    }

    /// <summary>
    /// Attack() is invoked when enemy can attack its opponent (after cooldown is over).
    /// </summary>
    protected abstract void Attack();


    public void TakeDamage(float damage, Transform damageSource)
    {
        if (healthBar == null)
        {
            Debug.LogError("HealthBar not assigned in Enemy");
            return;
        }
        if (colorIndicator == null)
        {
            Debug.LogError("ColorIndicator not assigned in Enemy");
            return;
        }
        targetTransform = damageSource;
        //updateAgentPriority(targetTransform);
        health -= damage;
        healthBar.UpdateHealthBar(health);
        colorIndicator.IndicateDamage();

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void selectNewTarget()
    {
        GameObject target = GameManager.Instance.FindClosestTargetForEnemy(this);
        if (target == null)
        {
            targetTransform = null;
        }
        else
        {
            targetTransform = target.transform;
            //updateAgentPriority(targetTransform);
        }
    }

    private void updateAgentPriority(Transform targetTransform)
    {
        if (targetTransform.TryGetComponent<NavMeshAgent>(out var targetAgent))
        {
            // set our agent priority to moving target's so we do not avoid one another
            //agent.avoidancePriority = targetAgent.avoidancePriority;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.Unregister(this);
    }
}
