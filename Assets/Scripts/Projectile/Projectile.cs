using UnityEngine;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;

public abstract class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    //Projectile speed
    [SerializeField] private protected float speed = 3;

    //Projectile lifetime
    [SerializeField] private float maxDistance = 50f;

    //Projectile start position
    private Vector3 startPosition;

    /// <summary>
    /// set to true once an first impact occurs.
    /// </summary>
    protected bool impacted;

    private void Start()
    {
        startPosition = transform.position;
        impacted = false;
    }

    // Use this for initialization
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    public void SetDirection(Vector3 direction)
    {
        direction.Normalize();
        rb.velocity = direction * speed;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);
        // If a time based metric is preferred, do Destroy(gameObject,lifeTime)
        if (distanceTraveled > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void HandleCollision(Collider collision)
    {
        //Removed delay for destroying projectile, can restore if some sort of splash mechanic is needed
        if (!collision.isTrigger) Destroy(gameObject); // Destroy projectile on collision by default
    }

    private void OnTriggerEnter(Collider collision)
    {
        HandleCollision(collision);
        impacted = true;
    }

    public void destroyMe() {
        //method exists for animations to call it with animation events 
        Destroy(gameObject);
    }
}
