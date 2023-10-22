using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public abstract class Animal : MonoBehaviour, IDamageable
{
    protected NavMeshAgent agent;
    Rigidbody rb;
    public Emotion currEmotion = Emotion.EMOTIONLESS;
    [HideInInspector] public Transform targetTransform;
    
    protected Vector3 targetPosition;

    [Header("Stats")]
    [SerializeField] float maxHealth;
    [SerializeField] float currHealth;
    [SerializeField] float animalDamage;
    [SerializeField] float emoSpeed = 2f;
    [SerializeField] float loveSpeed = 3f;
    [SerializeField] float angerSpeed = 3f;
    public float damageMultiplier = 1f;
    public float healthMultiplier = 1f;

    [Header("Emotionless")]
    [Tooltip("Time in between choosing new patrol points")]
    [SerializeField] float patrolTime = 5f;
    float currTime = 0;
    [SerializeField] float minRanDistance = 1.5f;
    [SerializeField] float maxRanDistance = 4f;

    [Header("Love")]
    [Tooltip("Minimum distance between the player and animal")]
    [SerializeField] protected float loveDistance = 5f;

    float ranRange;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        ranRange = maxRanDistance - minRanDistance;
    }

    void Start()
    {
        // Set health
        currHealth = maxHealth;
        GameManager.Instance.Register(this);
    }
    public virtual void Update()
    {
        // Movement
        switch (currEmotion)
        {
            case Emotion.ANGER:
                if (agent.speed != angerSpeed) agent.speed = angerSpeed;
                AngerTarget();
                break;
            case Emotion.LOVE:
                if (agent.speed != loveSpeed) agent.speed = loveSpeed;
                LoveTarget();
                break;
            default:
                if (agent.speed != emoSpeed) agent.speed = emoSpeed;
                EmoTarget();
                break;
        }
        agent.destination = targetPosition;
    }


    /// <summary>
    /// Sets the emotion of the animal when called
    /// </summary>
    /// <param name="emotion"></param>
    void SetEmotion(Emotion emotion)
    {
        currEmotion = emotion;
    }

    /// <summary>
    /// The enemy gives damage to the animal 
    /// Reduces the animal health by the damageAmount
    /// </summary>
    public void TakeDamage(float damageAmount)
    {
        currHealth -= damageAmount;
    }

    /// <summary>
    /// Called every update, when the emotion is Love, targetPosition will update to player position
    /// </summary>
    public abstract void LoveTarget();

    /// <summary>
    /// Called every update, when the emotion is Anger, targetPosition will update to enemy position
    /// </summary>
    public abstract void AngerTarget();



    /// <summary>
    /// Called every update, when there is no emotion, a random target will be chosen
    /// after a certain amount of time has passed (currTime = patrolTime).
    ///
    /// The Target will be chosen using the TargetSelect helper function
    /// </summary>
    void EmoTarget()
    {
        currTime += Time.deltaTime;
        if (currTime >= patrolTime)
        {
            RandomPosition();
            currTime = 0;
        }
    }

    /// <summary>
    /// Select a random target on teh NavMesh around the player within the searchRange
    ///
    /// Function is called recursively until the point is found
    /// </summary>
    void RandomPosition()
    {
        float ranX = UnityEngine.Random.Range(-ranRange, ranRange);
        float ranZ = UnityEngine.Random.Range(-ranRange, ranRange);
        if(ranX < 0f) { ranX -= minRanDistance; } else { ranX += minRanDistance; }
        if (ranZ < 0f) { ranZ -= minRanDistance; } else { ranZ += minRanDistance; }
        targetPosition = new Vector3(ranX, transform.position.y, ranZ);

    }

    public bool isDamageable()
    {
        return currEmotion == Emotion.ANGER;
    }
}
