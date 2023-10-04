using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerProjectile : MonoBehaviour
{
    private Rigidbody rb;

    public float speed = 3;

    public void SetDirection(Vector3 direction)
    {
        direction.Normalize();
        rb.velocity = direction * speed;
    }

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // temporary testing
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         this.SetDirection(new Vector3(1, 1, 1));
    //     }
    // }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy") {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Dog"))
            {
                Dog d = obj.GetComponent<Dog>();
                // check if dog target can be adjusted (dog in love will have target redirected)
            }
        }
        Destroy(gameObject);
    }
}
