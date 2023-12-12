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

    public AudioSource gunAudioSource;

    public AudioClip shootSoundClip;

    public void Shoot(int ammoIndex)
    {
        gunAudioSource.PlayOneShot(shootSoundClip);
        print("bullet at: " + bulletSpawn.position);
        GameObject projectile = Instantiate(ammoPrefabs[ammoIndex], transform.position, Quaternion.identity); //bulletSpawn.rotation
        // Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        // if (projectileRb != null)
        // {
        //     projectileRb.AddForce(bulletSpawn.forward * 20f, ForceMode.Impulse);
        // }
        Projectile bullet = projectile.GetComponent<Projectile>();
        bullet.SetDirection(aim.targetLocation - bullet.transform.position, bulletSpeed);
        bullet.AdjustYSpawn();

        Vector3 direction = aim.targetLocation - transform.position;
        float angle = (Mathf.Atan(direction.z / direction.x) * Mathf.Rad2Deg);
        if (direction.z < 0)
        {
            angle = angle + 180;
        }
        print("Difference in positions is " + direction);
        print("Shot, Angle is " + (Mathf.Atan(direction.z / direction.x) * Mathf.Rad2Deg));
    }

    private void Update()
    {
        Vector3 direction = aim.targetLocation - transform.position;
        float angle = Mathf.Abs(Mathf.Atan(direction.z / direction.x) * Mathf.Rad2Deg);
        if (direction.x < 0)
        {
            if (direction.z > 0)
            {
                angle = 90 + (90 - angle);
            }
            else
            {
                angle += 180;
            }
        }
        else if (direction.z < 0)
        {
            angle = 270 + (90 - angle);
        }
        print("Difference in positions is " + direction);
        print("Shot, Angle is " + angle);
    }
}