using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class Animal : MonoBehaviour, IDamageable
{
    [SerializeField] NavMeshAgent agent;

    
    [SerializeField] Rigidbody rb;
    [SerializeField] float maxHealth;
    [SerializeField] float currHealth;
    [SerializeField] float animalDamage;
    [HideInInspector] public Emotion currEmotion = Emotion.EMOTIONLESS;
    [HideInInspector] public Transform targetTransform;
    
    Vector3 targetPosition;


    [Tooltip("Time in between choosing new patrol points")]
    [SerializeField] float patrolTime;
    float currTime = 0;
    [SerializeField] float searchRange;

    [Header("Stats")]
    [SerializeField] float emoSpeed;
    [SerializeField] float speed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Set health
        currHealth = maxHealth;
    }
    void Update()
    {
        // Damage
        if (currHealth <= 0)
        {
            currEmotion = Emotion.EMOTIONLESS;
        }

        // Movement
        switch (currEmotion)
        {
            case Emotion.ANGER:
                AngerTarget();
                break;
            case Emotion.LOVE:
                LoveTarget();
                break;
            default:
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

    ///// <summary>
    ///// Called when the animal losses all health
    ///// Change emotion of animal to emotionless
    ///// </summary>
    //public void Die()
    //{
    //    currEmotion = Emotion.EMOTIONLESS;
    //}

    /// <summary>
    /// Called every update, when the emotion is Love, targetPosition will update to player position
    /// </summary>
    void LoveTarget()
    {
        if (targetTransform != GameManager.Instance.PlayerTransform)
            targetTransform = GameManager.Instance.PlayerTransform;

        targetPosition = targetTransform.position;
    }

    /// <summary>
    /// Called every update, when the emotion is Anger, targetPosition will update to enemy position
    /// </summary>
    void AngerTarget()
    {
        if (targetTransform = null)
            GameManager.Instance.FindClosest(transform.position, GameManager.Instance.TeamEnemy);
        else
        {
            targetPosition = targetTransform.position;
        }
    }



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
            TargetSelect();
            currTime = 0;
        }
    }

    /// <summary>
    /// Select a random target on teh NavMesh around the player within the searchRange
    ///
    /// Function is called recursively until the point is found
    /// </summary>
    void TargetSelect()
    {
        Vector3 randomPoint = transform.position + UnityEngine.Random.insideUnitSphere * searchRange;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            targetPosition = hit.position;
        }
        else
        {
            TargetSelect();
        }
    }
}
