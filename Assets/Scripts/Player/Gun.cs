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
        GameObject obj = Instantiate(ammoPrefabs[ammoIndex], bulletSpawn.position, bulletSpawn.rotation);
        Projectile projectile = obj.GetComponent<Projectile>();
        // todo: determine direction of aim
        fireDirection.Set(0,0,1);
        projectile.SetDirection(fireDirection);
    }
}
