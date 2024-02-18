using System;
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
    public Transform targetTransform;

    protected Vector3 targetPosition;

    protected Transform damageSourceTransform;

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
    [SerializeField] protected float animalDefense;
    [SerializeField] protected float emoSpeed = 2f;
    [SerializeField] protected float loveSpeed = 3f;
    [SerializeField] protected float angerSpeed = 3f;
    public float damageMultiplier = 1f;
    public float healthMultiplier = 1f;
    public float defenceMultiplier = 1f;

    [Header("Death Cool Down")]
    [Tooltip("The time between animal death and regain emotion")]
    protected float deathCoolDown = 5f;
    protected float currentCoolDownTime;
    bool isCoolDown = false;

    [Header("Defence")]
    [SerializeField] DefenceRadius defenceRadius;

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

    [Header("Particle Effect")]
    [SerializeField] ParticleSystem emotionSystem;
    [SerializeField] Material loveMat;
    [SerializeField] Material angerMat;

    ColorIndicator colorIndicator;

    SpriteRenderer spriteRenderer;


    [SerializeField][Range(0, 1)] float animationSpeed = 1.0f;

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
        if (healthBar != null)
        {
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

    // Virtual method to be overridden by derived classes
    protected virtual void OnEmotionChanged(Emotion newEmotion)
    {
        // This method can be overridden in derived classes
    }
    public virtual void Update()
    {
        if (currEmotion == Emotion.DEFENCE)
        {
            GameManager.Instance.ValidEnemyTargets.Add(this.transform);
        }

        anim.speed = animationSpeed;
        // Movement
        print("check emotion: " + currEmotion.ToString());
        switch (currEmotion)
        {
            case Emotion.ANGER:
                defenceRadius.gameObject.SetActive(false);
                agent.Speed = angerSpeed;
                AngerTarget();
                spriteRenderer.color = angerColor;
                break;
            case Emotion.LOVE:
                defenceRadius.gameObject.SetActive(false);
                agent.Speed = loveSpeed;
                LoveTarget();
                spriteRenderer.color = loveColor;
                break;
            case Emotion.DEFENCE:
                defenceRadius.gameObject.SetActive(true);
                agent.Speed = 0;
                DefenceTarget();
                // TODO ADD COLOR
                break;
            default:
                //defenceRadius.gameObject.SetActive(false);
                agent.Speed = emoSpeed;
                EmoTarget();
                spriteRenderer.color = isCoolDown ? emotionlessColor : Color.white;
                break;
        }
        // shift destination to be usually in front of the enemy base
        Vector3 destination = targetPosition;
        if (targetTransform != null && targetTransform.CompareTag(Tag.EnemyBase.ToString()))
        {
            destination.z -= 0.5f;
        }
        agent.Destination = destination;

        //Defend
        //if (currEmotion == Emotion.DEFENCE)
        //{
        //    Defend();
        //}

        //Attack
        attackCooldown -= Time.deltaTime;

        if ((currEmotion == Emotion.ANGER || currEmotion == Emotion.DEFENCE) && targetTransform != null)
        {
            float dist = Vector3.Magnitude(targetTransform.position - transform.position);
            // allow attack if the entity has come to a distance within range and that it comes to a stop
            // or entity is guaranteed able to hit target because distance < 1 (but target might be moving away)
            bool canStartAttack = (dist <= attackRadius && agent.Velocity.magnitude < 1e-3) || dist <= 1;
            if (attackCooldown <= 0 && canStartAttack)
            {
                Attack();
                agent.SetObstacleMode();
                attackCooldown = attackRate;
            }
            if (dist > attackRadius)
            {
                agent.SetAgentMode();
            }
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

        // hide health bar when HP is at maximum
        if (health < maxHealth)
        {
            healthBar?.UpdateHealthBar(health);
            healthBar?.gameObject.SetActive(true);
        }
        else
        {
            healthBar?.gameObject.SetActive(false);
        }

        Animate();
    }

    /// <summary>
    /// Sets the emotion of the animal when called
    /// Changes the color of the animal to its corresponding emotion
    /// When emotion is set to defence, the target position is set to null
    /// </summary>
    /// <param name="emotion"></param>
    protected void SetEmotion(Emotion emotion)
    {
        if (currEmotion != emotion)
        {
            currEmotion = emotion;
            OnEmotionChanged(emotion); // Notify the derived class of the emotion change
        }
        if (emotion == Emotion.EMOTIONLESS || emotion == Emotion.LOVE)
        {
            health = maxHealth;
            agent.SetAgentMode();
        }

        currEmotion = emotion;

        switch (currEmotion)
        {
            case Emotion.ANGER:
                cubeRenderer.material.color = angerColor;
                print("Previous material is " + emotionSystem.GetComponent<ParticleSystemRenderer>().material);
                emotionSystem.GetComponent<ParticleSystemRenderer>().material = angerMat;
                emotionSystem.Play();
                break;
            case Emotion.LOVE:
                cubeRenderer.material.color = loveColor;
                emotionSystem.GetComponent<ParticleSystemRenderer>().material = loveMat;
                emotionSystem.Play();
                break;
            case Emotion.DEFENCE:
                
                //TODO
            default:
                cubeRenderer.material.color = emotionlessColor;
                emotionSystem.Pause();
                emotionSystem.Clear();
                break;
        }
        // stop moving in this frame of emotional transition because the agent updates destination on next frame.
        agent.Destination = transform.position;
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
            if (emotion == Emotion.ANGER)
            {
                GameManager.Instance.ValidEnemyTargets.Add(this.transform);
            }
            else
            {
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
        damageSourceTransform = damageSource;
        if (currEmotion == Emotion.ANGER)
        {
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
    public virtual void LoveTarget()
    {
        if (targetTransform != GameManager.Instance.PlayerTransform)
        {
            targetTransform = GameManager.Instance.PlayerTransform;
        }
        if (Vector3.Magnitude(targetTransform.position - transform.position) > loveDistance)
        {
            targetPosition = targetTransform.position;
        }
        else
        {
            targetPosition = transform.position;
        }
    }

    /// <summary>
    /// Called every update, when the emotion is Anger, targetPosition will update to enemy position
    /// </summary>
    public virtual void AngerTarget()
    {
        if (targetTransform == null) {
            targetTransform = GameManager.Instance.FindClosest(transform.position, GameManager.Instance.TeamEnemy);
            agent.SetAgentMode(); // make sure to allow movement after current round of combat is over
        }
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
    /// Called every update, when emotion is defence, the target Destination is
    /// the animal's location where the emotion is applied.
    ///
    /// </summary>
    protected virtual void DefenceTarget()
    {
        targetTransform = agent.transform;

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
        Vector3 velocityInCameraSpace = mainCam.transform.InverseTransformDirection(agent.Velocity);

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
                    Vector3 offsetInCameraSpace = mainCam.transform.InverseTransformDirection(targetTransform.position - transform.position);
                    flipX = offsetInCameraSpace.x > 0;
                }
            }
            spriteRenderer.flipX = flipX;

        }

        if (anim != null)
        {
            anim.SetFloat("FBspeed", -velocityInCameraSpace.z);
        }
        else
        {
            Debug.Log("no animation set for animal " + gameObject.name);
        }
    }
}