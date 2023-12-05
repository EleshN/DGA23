using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cat : Animal
{
    [Header("Cat Attack Stats")]
    [SerializeField] Hitbox hitbox;
    [SerializeField] float attackDelay;
    [Tooltip("The amount of time in seconds that the hitbox is active when attacking")]
    [SerializeField] float hitboxActiveTime;

    [Header("Cat Damage in Radius")]
    [SerializeField] float damageRadius;
    [SerializeField] float radiusDamage;

    [SerializeField] ParticleSystem ps;

    public override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(DamageInRadius), 0, 1); // Changed from Debuff to DamageInRadius
        ps.Pause();
        ps.Clear();
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
        if (Vector3.Magnitude(targetTransform.position - transform.position) > loveDistance)
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

    private void DamageInRadius()
    {
        if (currEmotion != Emotion.EMOTIONLESS)
        {
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, damageRadius);

            foreach (Collider col in nearbyColliders)
            {
                if (col.gameObject.CompareTag("Animal") && col.gameObject != gameObject)
                {
                    if (col.TryGetComponent<IDamageable>(out IDamageable damageable))
                    {
                        damageable.TakeDamage(radiusDamage, transform);
                    }
                }
            }

            // Particle system effect for visual feedback
            if (nearbyColliders.Length > 0 && !ps.isPlaying)
            {
                ps.Play();
            }
            else if (ps.isPlaying)
            {
                ps.Pause();
                ps.Clear();
            }
        }
    }

    /// <summary>
    /// Set the damage of the hitbox based on the cat's base damage * damageMultiplier.  Call CatAttack
    /// </summary>
    public override void Attack()
    {
        hitbox.SetDamage(animalDamage * damageMultiplier);
        StartCoroutine(CatAttack());
    }

    /// <summary>
    /// delay the attack by attackDelay.  Then enable the hitbox to damage enemy
    /// </summary>

    IEnumerator CatAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        hitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(hitboxActiveTime);
        hitbox.gameObject.SetActive(false);

    }

    public override void Animate()
    {
        // whether the cat should be facing right (default is left)
        //bool flipped = agent.velocity.x > 0;
        //gameObject.transform.rotation = Quaternion.Euler(
        //        new Vector3(0f, flipped ? 180f : 0f, 0f));
        anim.SetFloat("FBspeed", agent.velocity.z);
    }
}
