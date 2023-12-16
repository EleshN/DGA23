using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempbirdbody : MonoBehaviour
{
    [SerializeField] Animator anim;
    //private void OnTriggerEnter(Collider collision)
    //{
    //    print("Dummy bird script collided with " + collision.gameObject.name);
    //    if (collision.gameObject.name.Contains("Bullet") || collision.gameObject.name.Contains("Player")) {
    //        anim.SetTrigger("Takeoff");
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        print("Dummy bird script collided with " + other.gameObject.name);
        if (other.gameObject.name.Contains("Bullet") || other.gameObject.name.Contains("Player"))
        {
            anim.SetTrigger("Takeoff");
        }
    }

}
