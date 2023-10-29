using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IDamageable
{

    public enum State
    {
        SPWAN,
        WANDER,
        CHASE,
        ATTACK
    }

    [SerializeField] State state;
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
        state = State.SPWAN;
    }

    // Update is called once per frame
    void Update()
    {
        currentAtackTime -= Update.deltaTime;
        changeStateIfAcceptable();
        switch(state){
            case State.WANDER:
                selectNewTarget();
                break;

            case State.CHASE:
                Move(targetTransform.position);
                break;
            
            case State.Attack:
                attack();
                break;
            
            default:
                break;
        }
        if (currentAtackTime <= 0){
            currentAtackTime = attackCountDown;
        }

        // if (targetTransform == null || !targetState.isDamageable())
        // {
        //     selectNewTarget();
        // }
        // // just stay still if there is no more targets on map, the game must end.
        // if (targetTransform != null)
        // {
        //     Move(targetTransform.position);
        //     Attack();
        // }
    }

    public void changeStateIfAcceptable(){
        switch(state){
            case State.SPWAN:
                state = State.WANDER;
                break;

            case State.WANDER:
                if (targetTransform != null && targetState.isDamageable())
                {
                    if (currentAtackTime <= 0){
                        state = state.ATTACK;
                    }else{
                        state = state.CHASE;
                    }
                }
                break;

            case State.CHASE:
                if (targetTransform == null || !targetState.isDamageable())
                {
                    state = state.WANDER;
                } else if (currentAtackTime <= 0){
                    state = state.ATTACK;
                }
                break;
            
            case State.ATTACK:
                if (targetTransform == null || !targetState.isDamageable())
                {
                    state = state.WANDER;
                } else{
                    state = state.CHASE;
                }
                break;

            default:
                break;

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
