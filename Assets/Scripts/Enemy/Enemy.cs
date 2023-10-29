using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] EnemyState state = EnemyState.SPAWN;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float robotDamage;
    [SerializeField] protected float enemyDamage;
    [SerializeField] protected float attackCountDown;
    [SerializeField] protected float currentAtackTime;

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
        currentAtackTime = attackCountDown;
        GameManager.Instance.Register(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentAtackTime -= Time.deltaTime;
        changeStateIfAcceptable();

        switch(state){
            case EnemyState.WANDER:
                selectNewTarget();
                break;

            case EnemyState.CHASE:
                Move(targetTransform.position);
                break;
            
            case EnemyState.ATTACK:
                Attack();
                break;
            
            default:
                break;
        }
        if (currentAtackTime <= 0){
            currentAtackTime = attackCountDown;
        }
    }

    public void changeStateIfAcceptable(){
        switch(state){
            case EnemyState.SPAWN:
                state = EnemyState.WANDER;
                break;

            case EnemyState.WANDER:
                if (targetTransform != null && targetState.isDamageable())
                {
                    if (currentAtackTime <= 0){
                        state = EnemyState.ATTACK;
                    }else{
                        state = EnemyState.CHASE;
                    }
                }
                break;

            case EnemyState.CHASE:
                if (targetTransform == null || !targetState.isDamageable())
                {
                    state = EnemyState.WANDER;
                } else if (currentAtackTime <= 0){
                    state = EnemyState.ATTACK;
                }
                break;
            
            case EnemyState.ATTACK:
                if (targetTransform == null || !targetState.isDamageable())
                {
                    state = EnemyState.WANDER;
                } else{
                    state = EnemyState.CHASE;
                }
                break;

            default:
                break;

        }
    }

    protected virtual void Move(Vector3 targetPosition)
    {
        // always move the entity closer to target
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
