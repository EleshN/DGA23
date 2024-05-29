using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    protected EnemyState state = EnemyState.SPAWN;

    protected NavMeshObstacleAgent agent;

    [SerializeField] HealthBar healthBar;

    SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer additionalSprite;
    //Relevant for scorpion tail and crab winder

    ColorIndicator colorIndicator;

    protected GameObject mainCam;

    /// <summary>
    /// remaining timer before next attack (enemy is able to attack when this value is not greater than 0)
    /// </summary>
    protected float currentAttackTime;

    public Transform targetTransform;

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

    [Tooltip("the entities that this enemy can attack")]
    [SerializeField] protected Tag[] targets;

    [SerializeField]
    GameObject clusterPrefab;

    [SerializeField] GameObject attackPrefab;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshObstacleAgent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        colorIndicator = GetComponent<ColorIndicator>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GameManager.Instance.Register(this);
        health = maxHealth;
        // Set health
        health = maxHealth;
        healthBar?.SetHealthBar(maxHealth);
        healthBar?.gameObject.SetActive(false);
        currentAttackTime = 0;
        agent.Speed = speed;
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

        // hide health bar when HP is at maximum
        if (health < maxHealth)
        {
            healthBar?.UpdateHealthBar(health);
            healthBar?.gameObject.SetActive(true);
        }
        else {
            healthBar?.gameObject.SetActive(false);
        }

        Animate();
    }

    /// <summary>
    /// without considering any cooldowns, this checks for conditions to initate an attack.
    /// for example, physical enemies need to be within a small range to start an attack.
    /// </summary>
    /// <returns>whether an enemy can initiate an attack</returns>
    protected virtual bool CanAttack()
    {
        // allow attack if the entity has come to a distance within range and that it comes to a stop
        // or entity is guaranteed able to hit target because distance < 1 (but target might be moving away)
        float dist = Vector3.Magnitude(targetTransform.position - transform.position);
        return (dist <= attackRadius && agent.Velocity.magnitude < 1e-3) || (dist <= 1);
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
        agent.Destination = targetPosition;
        agent.StoppingDistance = 0;
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

        GameObject attack = Instantiate(attackPrefab, new Vector3(transform.position.x + Random.Range(-.2f,.2f),
            transform.position.y + .5f, transform.position.z + Random.Range(-.2f, .2f))
                , Quaternion.identity);
        Destroy(attack, .5f);

        if (health <= 0)
        {
            Instantiate(clusterPrefab, transform.position, Quaternion.identity);
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

    private void Animate()
    {
        Vector3 velocityInCameraSpace = mainCam.transform.InverseTransformDirection( agent.Velocity);
        if (spriteRenderer != null)
        {
            // Check if the x component of the velocity in camera space is positive (moving to the right)
            bool flipX = spriteRenderer.flipX;
            if (velocityInCameraSpace.x != 0 && Mathf.Abs(velocityInCameraSpace.x) >= 0.1f)
            {
                // change x orientation when horizontal direction changes (positive = right).
                flipX = velocityInCameraSpace.x > 0;
            }
            else 
            {
                if (targetTransform != null)
                {
                    // face target if stationary
                    Vector3 offsetInCameraSpace = mainCam.transform.InverseTransformDirection( targetTransform.position - transform.position );
                    flipX = offsetInCameraSpace.x > 0;
                }
            }
            spriteRenderer.flipX = flipX;
            if (additionalSprite) {
                additionalSprite.flipX = flipX;
            }
            
        }
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
            GameManager.Instance.Unregister(this);
    }
}
