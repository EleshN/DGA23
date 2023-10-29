using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{

    [SerializeField] float damage;

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        IDamageable entity = other.GetComponent<IDamageable>();
        if (entity != null && GameManager.Instance.TeamPlayer.Contains(other.transform))
        {
            entity.TakeDamage(this.damage);
        }
        Destroy(gameObject, 0.1f);
    }
}
