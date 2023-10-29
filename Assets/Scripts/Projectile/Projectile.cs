using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private protected float speed = 3;
    [SerializeField] private float maxDistance = 50f;
    private Vector3 startPosition;

    public void SetDirection(Vector3 direction)
    {
        direction.Normalize();
        rb.velocity = direction * speed;
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    // Use this for initialization
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);
        if (distanceTraveled > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
