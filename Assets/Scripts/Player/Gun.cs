using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //same index as ammo index
    [Tooltip("Make sure the index of the ammo is same as Player script")]
    public GameObject[] ammoPrefabs;

    //spawn of the bullet
    [SerializeField] Transform bulletSpawn;
    
    [SerializeField] Targetting aim;

    [SerializeField] float bulletSpeed;

    public void Shoot(int ammoIndex)
    {
        print("bullet at: " + bulletSpawn.position);
        GameObject projectile = Instantiate(ammoPrefabs[ammoIndex], transform.position, bulletSpawn.rotation);
        // Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        // if (projectileRb != null)
        // {
        //     projectileRb.AddForce(bulletSpawn.forward * 20f, ForceMode.Impulse);
        // }
        Projectile bullet = projectile.GetComponent<Projectile>();
        bullet.SetDirection(aim.targetLocation - bullet.transform.position, bulletSpeed);
        bullet.AdjustYSpawn();
    }
}