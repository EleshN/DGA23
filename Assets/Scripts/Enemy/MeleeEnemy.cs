using UnityEngine;
using System.Collections;

public class MeleeEnemy : Enemy
{

    [Header("Melee Enemy Attack Stats")]
    [SerializeField] protected Hitbox hitbox;
 
    [SerializeField] float attackDelay = 1f;

    [Tooltip("The amount of time in seconds that the hitbox is active when attacking")]
    [SerializeField] float hitboxActiveTime = 2f;

    public Animator animator;
    public AudioSource enemyAudioSource;
    public AudioClip enemyAttackClip;

    protected override void Start()
    {
        base.Start();
        hitbox.Initialize();
        hitbox.SetUniformDamage(targets, robotDamage);
    }


    protected override void Attack()
    {
        // perhaps for melee enemies, this is where we animate the attack motion.
        StartCoroutine(ToggleHitbox());
    }

    /// <summary>
    /// turns on the hitbox to deal damage to opponents and then turns off the hitbox once damage time is over (as indicated by hitboxActiveTime)
    /// </summary>
    IEnumerator ToggleHitbox()
    {
        yield return new WaitForSeconds(attackDelay);
        enemyAudioSource.PlayOneShot(enemyAttackClip);
        animator.SetBool("isAttacking", true);
        hitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(hitboxActiveTime);
        animator.SetBool("isAttacking", false);
        hitbox.gameObject.SetActive(false);

    }
}
