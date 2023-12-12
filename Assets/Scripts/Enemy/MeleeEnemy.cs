using UnityEngine;
using System.Collections;

public class MeleeEnemy : Enemy
{

    [Header("Melee Enemy Attack Stats")]
    [SerializeField] Hitbox hitbox;
    [SerializeField] float attackDelay = 1f;
    [Tooltip("The amount of time in seconds that the hitbox is active when attacking")]
    [SerializeField] float hitboxActiveTime = 2f;


    public AudioSource enemyAudioSource;
    public AudioClip enemyAttackClip;

    // void OnCollisionStay(Collision collision)
    // {
    //     print("enemy colliding with something");
    //     GameObject other = collision.gameObject;
    //     entity = other.GetComponent<IDamageable>();
    //     if (entity != null && entity.isDamageable() &&
    //         GameManager.Instance.ValidEnemyTargets.Contains(other.transform) && 
    //         currentAtackTime <= 0)
    //     {
    //         Attack();
    //     }
    // }

    protected override void Attack()
    {
        // perhaps for melee enemies, this is where we animate the attack motion.
        hitbox.SetDamage(robotDamage);
        StartCoroutine(ToggleHitbox());
    }

    /// <summary>
    /// turns on the hitbox to deal damage to opponents and then turns off the hitbox once damage time is over (as indicated by hitboxActiveTime)
    /// </summary>
    IEnumerator ToggleHitbox()
    {
        yield return new WaitForSeconds(attackDelay);
        enemyAudioSource.PlayOneShot(enemyAttackClip);
        hitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(hitboxActiveTime);
        hitbox.gameObject.SetActive(false);

    }
}
