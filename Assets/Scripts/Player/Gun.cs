using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //same index as ammo index
    [Tooltip("Make sure the index of the ammo is same as Player script")]
    public GameObject[] ammoPrefabs;
    // Start is called before the first frame update

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
        projectile.GetComponent<Projectile>().SetPlayer(gameObject.GetComponent<Player>());
    }
}
