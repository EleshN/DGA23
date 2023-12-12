using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

[RequireComponent(typeof(NavMeshAgent), typeof(NavMeshObstacle))]
public abstract class Animal : MonoBehaviour, IDamageable
{

    public Animator anim;
    protected GameObject mainCam;
    protected NavMeshObstacleAgent agent;

    [SerializeField] protected Emotion currEmotion = Emotion.EMOTIONLESS;

    protected Vector3 spawnLocation;

    [SerializeField]
    protected Transform targetTransform;

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
    [SerializeField] protected float emoSpeed = 2f;
    [SerializeField] protected float loveSpeed = 3f;
    [SerializeField] protected float angerSpeed = 3f;
    public float damageMultiplier = 1f;
    public float healthMultiplier = 1f;

    [Header("Death Cool Down")]
    [Tooltip("The time between animal death and regain emotion")]
    protected float deathCoolDown = 5f;
    protected float currentCoolDownTime;
    bool isCoolDown = false;

    [Header("Emotionless")]
    [Tooltip("Time in between choosing new patrol points")]
    [SerializeField] float patrolTime = 5f;
    float currTime = 0;
    [SerializeField] float minRanDistance;
    [SerializeField] float maxRanDistance;
    float ranRange;

    [Header("Love")]
    [Tooltip("Minimum distance between the player and animal")]
    [SerializeField] protected float loveDistance = 5f;

    [Header("Combat")]
    public float attackRadius;
    public float attackRate;
    float attackCooldown;

    ColorIndicator colorIndicator;

    SpriteRenderer spriteRenderer;

    
    [SerializeField] [Range(0,1)] float animationSpeed = 1.0f;

    void Awake()
    {
        agent = GetComponent<NavMeshObstacleAgent>();
        ranRange = maxRanDistance - minRanDistance;

        // Get the Renderer component from the new cube (to change body color)
        cubeRenderer = animalBody.GetComponent<Renderer>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim.speed = animationSpeed;
        
    }

    public virtual void Start()
    {
        // Set health
        health = maxHealth;
        if (healthBar != null){
            healthBar.SetHealthBar(maxHealth);
            healthBar.gameObject.SetActive(false);
        }
        GameManager.Instance.Register(this);
        spawnLocation = transform.position;
        colorIndicator = GetComponent<ColorIndicator>();
        // Set color
        SetEmotion(Emotion.EMOTIONLESS);
        RandomPosition();
    }

    public virtual void Update()
    {
        anim.speed = animationSpeed;
        // Movement
        switch (currEmotion)
        {
            case Emotion.ANGER:
                agent.Speed = angerSpeed;
                AngerTarget();
                break;
            case Emotion.LOVE:
                agent.Speed = loveSpeed ;
                LoveTarget();
                break;
            default:
                agent.Speed = emoSpeed;
                EmoTarget();
                break;
        }

        agent.Destination = targetPosition;

        //Attack
        attackCooldown -= Time.deltaTime;
        bool withinAttackRadius = Vector3.Magnitude(targetPosition - transform.position) <= attackRadius;
        // allow attack if the entity has come to a distance within range and that it comes to a stop
        // or entity is guaranteed able to hit target because distance < 1 (but target might be moving away)
        bool canStartAttack = (withinAttackRadius && agent.Velocity.magnitude < 1e-3) || Vector3.Magnitude(targetPosition - transform.position) <= 1;
        if (currEmotion == Emotion.ANGER && attackCooldown <= 0 && canStartAttack)
        {
            Attack();
            agent.SetObstacleMode();
            attackCooldown = attackRate;
        }
        if (!withinAttackRadius){
            agent.SetAgentMode();
        }

        // Die
        if (isCoolDown)
        {
            currentCoolDownTime -= Time.deltaTime;
        }

        if (currentCoolDownTime <= 0)
        {
            isCoolDown = false;
        }

        if (healthBar != null)
        {
            // hide health bar when HP is at maximum
            if (health < maxHealth)
            {
                healthBar.UpdateHealthBar(health);
                healthBar.gameObject.SetActive(true);
            }
            else {
                healthBar.gameObject.SetActive(false);
            }
        }
        
        Animate();
    }

    /// <summary>
    /// Sets the emotion of the animal when called
    /// Changes the color of the animal to its corresponding emotion
    /// </summary>
    /// <param name="emotion"></param>
    protected void SetEmotion(Emotion emotion)
    {
        if (emotion == Emotion.EMOTIONLESS || emotion == Emotion.LOVE)
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
        if (agent.isActiveAndEnabled)
        {
            // stop moving in this frame of emotional transition because the agent updates destination on next frame.
            agent.Destination = transform.position;
        }
    }

    /// <summary>
    /// attempts to apply the given emotion onto the animal. Nothing happens if the animal has recently experience emotional transitions.
    /// </summary>
    /// <param name="emotion">the emotion that an effect carries (projectiles with love, etc)</param>
    /// <param name="newTarget">a game object to follow upon receiving the effect (explicit), null if specific target is to be found by the animal (implicit)</param>
    /// <returns>true if effect was applied successfully.</returns>
    public virtual bool ApplyEmotionEffect(Emotion emotion, Transform newTarget = null)
    {
        if (currentCoolDownTime <= 0)
        {
            SetEmotion(emotion);
            // an animal set to anger state will be qualified to become a target of enemies
            if (emotion == Emotion.ANGER){
                GameManager.Instance.ValidEnemyTargets.Add(this.transform);
            }
            else{
                GameManager.Instance.ValidEnemyTargets.Remove(this.transform);
            }
            targetTransform = newTarget;
            return true;
        }
        return false;
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
    public virtual void TakeDamage(float damageAmount, Transform damageSource)
    {
        if (currEmotion == Emotion.ANGER){
            health -= damageAmount;
            colorIndicator.IndicateDamage();
        }
        if (health <= 0)
        {
            isCoolDown = true;
            //currEmotion = Emotion.EMOTIONLESS;
            SetEmotion(Emotion.EMOTIONLESS);
            // an animal set to anger state will be qualified to become a target of enemies
            GameManager.Instance.ValidEnemyTargets.Remove(transform);
            health = maxHealth;
            currentCoolDownTime = deathCoolDown;
        }
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
    protected virtual void EmoTarget()
    {
        currTime += Time.deltaTime;
        if (currTime >= patrolTime || agent.Velocity.magnitude <= 1e-3)
        {
            RandomPosition();
            currTime = 0;
        }
    }

    /// <summary>
    /// Select and move to a random target on the NavMesh around
    /// the player within the searchRange
    ///
    /// Function is called recursively until the point is found
    /// </summary>
    protected virtual void RandomPosition()
    {
        float ranX = UnityEngine.Random.Range(-ranRange, ranRange);
        float ranZ = UnityEngine.Random.Range(-ranRange, ranRange);
        if (ranX < 0f) { ranX -= minRanDistance; } else { ranX += minRanDistance; }
        if (ranZ < 0f) { ranZ -= minRanDistance; } else { ranZ += minRanDistance; }
        targetPosition = new Vector3(spawnLocation.x + ranX, transform.position.y, spawnLocation.z + ranZ);
        // print("Name is " + gameObject.name + " target pos is " + targetPosition);

    }

    /// <summary>
    /// Defines the attack of the animal.  This method is called when the attack cooldown <= 0
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// Changes the animation of the animal depending on its movement direction
    /// </summary>
    public virtual void Animate()
    {
      
        // Vector3 referenceZVelocity = Vector3.Project(agent.Velocity, mainCam.transform.forward);

        // Convert the object's velocity from world space to camera space
        Vector3 velocityInCameraSpace = mainCam.transform.InverseTransformDirection( agent.Velocity);

        // Check if the x component of the velocity in camera space is positive (moving to the right)
        if (spriteRenderer != null)
        {
            bool flipX = spriteRenderer.flipX;
            if (velocityInCameraSpace.x != 0 && Mathf.Abs(velocityInCameraSpace.x) >= 0.1f)
            {
                // change x orientation  when horizontal direction changes.
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

        }

        if (anim != null){
            anim.SetFloat("FBspeed", -velocityInCameraSpace.z);
        }
        else
        {
            Debug.Log("no animation set for animal " + gameObject.name );
        }
    }
}
