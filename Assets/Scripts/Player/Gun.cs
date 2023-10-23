using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //same index as ammo index
    [Tooltip("Make sure the index of the ammo is same as Player script")]
    public GameObject[] ammoPrefabs;

    private Vector3 fireDirection = new();

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
        Projectile bullet = projectile.GetComponent<Projectile>();
        // TODO: so you must set a direction for the projectile, otherwise it will not move. The exact direction will be determined by our aim
        fireDirection.Set(0,0,1); 
        bullet.SetDirection(fireDirection);
    }
}