using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;

public class Cat : Animal
{
    [Header("Cat Attack Stats")]
    [SerializeField] Hitbox hitbox;

    [Tooltip("the entities that the cat can attak")]
    [SerializeField] Tag[] targets;

    [SerializeField] float attackDelay;
    [Tooltip("The amount of time in seconds that the hitbox is active when attacking")]
    [SerializeField] float hitboxActiveTime;

    [Header("Cat Damage in Radius")]
    [SerializeField] float damageRadius;
    [SerializeField] float radiusDamage;

    public override void Start()
    {
        base.Start();
        hitbox.Initialize();
        hitbox?.SetUniformDamage(targets, animalDamage * damageMultiplier);
        InvokeRepeating(nameof(DamageInRadius), 0, 1); // Changed from Debuff to DamageInRadius
    }

    public override void Update()
    {
        base.Update();
    }


    private void DamageInRadius()
    {
        if (currEmotion != Emotion.EMOTIONLESS)
        {
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, damageRadius);

            foreach (Collider col in nearbyColliders)
            {
                if (col.gameObject.CompareTag(Tag.Animal.ToString()) && col.gameObject != gameObject)
                {
                    if (col.TryGetComponent<IDamageable>(out IDamageable damageable))
                    {
                        damageable.TakeDamage(radiusDamage, transform);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Set the damage of the hitbox based on the cat's base damage. Call CatAttack
    /// </summary>
    public override void Attack()
    {
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

}
