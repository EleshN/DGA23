using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class Ram : Animal
{
    [Header("Ram Attack Stats")]
    [SerializeField] Hitbox hitbox;

    [Tooltip("the entities and the damage they receive from the ram")]
    [SerializeField] HitboxDamage[] damageValues;

    [SerializeField] float attackDelay;
    [Tooltip("The amount of time in seconds that the hitbox is active when attacking")]
    [SerializeField] float hitboxActiveTime;

    public override void Start()
    {
        base.Start();
        hitbox.Initialize();
        hitbox.SetDamage(damageValues);
    }

    public override void Update()
    {
        base.Update();
    }


    /// <summary>
    /// Set the damage of the hitbox based on the dog's base damage * damageMultiplier.  Call DogAttack
    /// </summary>
    public override void Attack()
    {
        StartCoroutine(RamAttack());
    }

    /// <summary>
    /// delay the attack by attackDelay.  Then enable the hitbox to damage enemy
    /// </summary>

    IEnumerator RamAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        hitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(hitboxActiveTime);
        hitbox.gameObject.SetActive(false);
    }
}
