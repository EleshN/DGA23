using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public abstract class Animal : MonoBehaviour, IDamageable
{
    protected NavMeshAgent agent;
    [SerializeField] protected Emotion currEmotion = Emotion.EMOTIONLESS;

    private Vector3 spawnLocation;
    [HideInInspector] public Transform targetTransform;

    protected Vector3 targetPosition;

    [Header("Animal Colors")]
    [Tooltip("Change the color of the animal body")]
    [SerializeField] GameObject animalBody;
    Renderer cubeRenderer;
    [SerializeField] Color emotionlessColor = Color.grey;
    [SerializeField] Color angerColor = new Color32(250, 11, 20, 170);
    [SerializeField] Color loveColor = new Color32(251, 98, 177, 178);

    [Header("Stats")]
    [SerializeField] float maxHealth;
    [SerializeField] float health;
    [SerializeField] HealthBar healthBar;
    [SerializeField] protected float animalDamage;
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
    float ranRange;

    [Header("Love")]
    [Tooltip("Minimum distance between the player and animal")]
    [SerializeField] protected float loveDistance = 5f;

    [Header("Combat")]
    public float attackRadius;
    public float attackRate;
    float attackCooldown;

    private ColorIndicator colorIndicator;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ranRange = maxRanDistance - minRanDistance;

        // Get the Renderer component from the new cube (to change body color)
        cubeRenderer = animalBody.GetComponent<Renderer>();
    }

    public virtual void Start()
    {
        // Set health
        health = maxHealth;
        healthBar.SetHealthBar(maxHealth);
        GameManager.Instance.Register(this);
        spawnLocation = transform.position;
        colorIndicator = GetComponent<ColorIndicator>();
        // Set color
        SetEmotion(Emotion.EMOTIONLESS);
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

        //Attack
        attackCooldown -= Time.deltaTime;
        if (currEmotion == Emotion.ANGER && attackCooldown <= 0 &&
            Vector3.Magnitude(targetPosition - transform.position) <= attackRadius)
        {
            Attack();
            attackCooldown = attackRate;
        }


    }


    /// <summary>
    /// Sets the emotion of the animal when called
    /// Changes the color of the animal to its corresponding emotion
    /// </summary>
    /// <param name="emotion"></param>
    public void SetEmotion(Emotion emotion)
    {
        if (currEmotion == Emotion.EMOTIONLESS &&
            emotion != Emotion.EMOTIONLESS)
        {
            health = maxHealth;
        }

        currEmotion = emotion;

        switch (currEmotion)
        {
            case Emotion.ANGER:
                cubeRenderer.material.color = angerColor;
                break;
            case Emotion.LOVE:
                cubeRenderer.material.color = loveColor;
                break;
            default:
                cubeRenderer.material.color = emotionlessColor;
                break;
        }
    }

    /// <summary>
    /// Gets the animal's current emotion
    /// </summary>
    /// <returns name="curremotion">Current emotion of this animal</returns>
    public Emotion GetEmotion()
    {
        return currEmotion;
    }

    /// <summary>
    /// The enemy gives damage to the animal. Reduces the animal health
    /// by the damageAmount. If animal's current health reduces to 0,
    /// its emotion will be set to emotionless.
    /// </summary>
    public void TakeDamage(float damageAmount)
    {
        if (currEmotion == Emotion.ANGER)
            health -= damageAmount;
            colorIndicator.IndicateDamage();
        if (health <= 0)
        {
            //currEmotion = Emotion.EMOTIONLESS;
            SetEmotion(Emotion.EMOTIONLESS);
            health = maxHealth;
        }
        healthBar.UpdateHealthBar(health);
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
        if (ranX < 0f) { ranX -= minRanDistance; } else { ranX += minRanDistance; }
        if (ranZ < 0f) { ranZ -= minRanDistance; } else { ranZ += minRanDistance; }
        targetPosition = new Vector3(spawnLocation.x + ranX, transform.position.y, spawnLocation.z + ranZ);

    }

    /// <summary>
    /// Defines the attack of the animal.  This method is called when the attack cooldown <= 0
    /// </summary>
    public abstract void Attack();


    public bool isDamageable()
    {
        return currEmotion == Emotion.ANGER;
    }
}
