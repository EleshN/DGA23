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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(int ammoIndex)
    {
        GameObject projectile = Instantiate(ammoPrefabs[ammoIndex], bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        if (projectileRb != null)
        {
            projectileRb.AddForce(bulletSpawn.forward * 20f, ForceMode.Impulse);
        }
    }
}