using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private protected float speed = 3;

    public void SetDirection(Vector3 direction)
    {
        direction.Normalize();
        rb.velocity = direction * speed;
    }

    // Use this for initialization
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
