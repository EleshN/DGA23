using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    float attackRadius;
    public AudioClip hitSound;
    public AudioSource audioController;
    float damage;
    public AudioClip spawnSound;
    Transform projectileSource;

    //Animator
    [SerializeField]
    Animator anim;
    bool hasHit = false; //So we don't hit twice while playing hit anim
    [SerializeField] AnimatorCallback callback;

    private void Start()
    {
        startPosition = transform.position;
        callback.AddCallback("onFinish", () => Destroy(this.gameObject));
    }

    /// <summary>
    /// initialize projectile with the given stats
    /// </summary>
    /// <param name="damage">the damage dealt to targets</param>
    /// <param name="attackRadius">the radius of the projectile effect</param>
    public void Initialize(float damage, float attackRadius, Transform projectileSource)
    {
        this.damage = damage;
        AudioSource.PlayClipAtPoint(spawnSound, transform.position);
        this.attackRadius = attackRadius;
        this.projectileSource = projectileSource;
    }

    protected override void HandleCollision(Collider collision)
    {
        // make sure we hit colliders of game objects.
        //Make sure we did not hit anything yet
        if (collision.isTrigger || hasHit)
        {
            return;
        }
        bool didCollide = false;
        Collider[] neabyColliders = Physics.OverlapSphere(transform.position, attackRadius);
        // find all animals and playerbases and damage them if possible
        foreach (Collider col in neabyColliders)
        {
            if (col.gameObject.CompareTag(Tag.Animal.ToString()) || col.gameObject.CompareTag(Tag.PlayerBase.ToString()))
            {
                IDamageable entity = col.gameObject.GetComponent<IDamageable>();
                entity.TakeDamage(damage, projectileSource);
                didCollide = true;
                hasHit = true;
            }
        }

        if (didCollide)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        if (!collision.isTrigger) {
            anim.SetTrigger("Hit");
        }
    }

    protected override void reachMaxDist()
    {
        Destroy(this.gameObject);
    }
}
