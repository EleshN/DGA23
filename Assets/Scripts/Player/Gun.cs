using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //same index as ammo index
    [Tooltip("Make sure the index of the ammo is same as Player script")]
    [SerializeField] GameObject[] ammoPrefabs;
    // Start is called before the first frame update

    //spawn of the bullet
    Transform bulletSpawn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(int ammoIndex)
    {
        Instantiate(ammoPrefabs[ammoIndex], bulletSpawn.position, bulletSpawn.rotation);
    }
}
