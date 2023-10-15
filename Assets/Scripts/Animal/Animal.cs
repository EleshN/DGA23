using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    float patrolTime;

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
        // Switch statement for move
        // Todo: incorporate target selection based on emotion when a new target entity is needed.
        // Note: emotionless animals have no target.
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
    /// Reduces the animal health by the damageAmount
    /// </summary>
    public void TakeDamage(float damageAmount)
    {

    }

    /// <summary>
    /// Called when the animal losses all health
    /// Change emotion of animal to emotionless
    /// </summary>
    public void Die()
    {

    }

    /// <summary>
    /// Called every update, when the emotion is Love, animal will move toward the player position
    /// </summary>
    /// <param name="target"></param>
    void LoveTarget()
    {
        if (targetTransform != GameManager.Instance.PlayerTransform)
            targetTransform = GameManager.Instance.PlayerTransform;

        targetPosition = targetTransform.position;
    }

    /// <summary>
    /// Called every update, when the emotion is Anger, animal will move toward the robot position
    /// </summary>
    /// <param name="robotPos"></param>
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
    /// Called every update, when there is no emotion, animal will move in some random direction
    /// </summary>
    void EmoTarget()
    {

    }

    // TODO: check to update emotion and the target to follow
}
