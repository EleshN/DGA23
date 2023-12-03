using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class Dog : Animal
{
    [Header("Dog Attack Stats")]
    [SerializeField] Hitbox hitbox;
    [SerializeField] float attackDelay;
    [Tooltip("The amount of time in seconds that the hitbox is active when attacking")]
    [SerializeField] float hitboxActiveTime;

    [Header("Dog Buffs")]
    [SerializeField] float buffRadius;
    [SerializeField] float healthBuffConst = 1.5f;
    [SerializeField] float damageBuffConst = 1.5f;

    [SerializeField] ParticleSystem ps;

    public override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(Buff), 0, 1);
        ps.Pause();
        ps.Clear();
        // print("Name is " + gameObject.name + " spawnpos is " + spawnLocation);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void LoveTarget()
    {
        if (targetTransform != GameManager.Instance.PlayerTransform)
        {
            targetTransform = GameManager.Instance.PlayerTransform;
        }
        if(Vector3.Magnitude(targetTransform.position - transform.position) > loveDistance)
        {
            targetPosition = targetTransform.position;
        }
        else
        {
            targetPosition = transform.position;
        }
    }

    public override void AngerTarget()
    {
        if (targetTransform == null)
            targetTransform = GameManager.Instance.FindClosest(transform.position, GameManager.Instance.TeamEnemy);
        else
        {
            targetPosition = targetTransform.position;
        }
    }

    private void Buff()
    {
        if (currEmotion != Emotion.EMOTIONLESS)
        {
            Collider[] neabyColliders = Physics.OverlapSphere(transform.position, buffRadius);
            int nearByDogs = 0; // total number of dog within the radius, including itself

            // Check if there's any Gameobject that is a Dog component
            foreach (Collider col in neabyColliders)
            {
                if (col.gameObject.GetComponent<Dog>() != null &&
                    col.gameObject != gameObject) nearByDogs += 1;
            }

            if (nearByDogs > 0)
            {
                if (!ps.isPlaying)
                {
                    ps.Play();
                }
                damageMultiplier = damageBuffConst * nearByDogs;
                healthMultiplier = healthBuffConst * nearByDogs;
            }
            else
            {
                if (ps.isPlaying)
                {
                    ps.Pause();
                    ps.Clear();
                }
                damageMultiplier = 1f;
                healthMultiplier = 1f;
            }
        }
        else{
            if (ps.isPlaying)
                {
                    ps.Pause();
                    ps.Clear();
                }
        }
    }

    /// <summary>
    /// Set the damage of the hitbox based on the dog's base damage * damageMultiplier.  Call DogAttack
    /// </summary>
    public override void Attack()
    {
        hitbox.SetDamage(animalDamage * damageMultiplier);
        StartCoroutine(DogAttack());
    }

    /// <summary>
    /// delay the attack by attackDelay.  Then enable the hitbox to damage enemy
    /// </summary>

    IEnumerator DogAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        hitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(hitboxActiveTime);
        hitbox.gameObject.SetActive(false);

    }

    public override void Animate()
    {

    }
}
