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

    //So we only call reachmaxdist once
    private bool reachedMaxDist = false;

    private void Start()
    {
        startPosition = transform.position;
    }

    // Use this for initialization
    protected virtual void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    public void SetDirection(Vector3 direction, float speed)
    {
        this.speed = speed;
        SetDirection(direction);
    }

    public void SetDirection(Vector3 direction)
    {
        // transform.rotation = Quaternion.Euler(0, Quaternion.LookRotation(direction).eulerAngles.y, 0);
        direction.Normalize();
        direction.y = 0;
        print("Shooting in direction " + direction);
        rb.AddForce(direction * speed, ForceMode.Impulse); //Direction times speed
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    /// <summary>
    /// <para>the bottom of a projectiles will collide first if shot downwards.</para>
    /// <para>if spawn positions are calculated using center, adjust spawn using vertical offset based on collider radius</para>
    /// </summary>
    public void AdjustYSpawn()
    {
        startPosition.y += 0.25f;
        rb.transform.position = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);
        // If a time based metric is preferred, do Destroy(gameObject,lifeTime)
        if (distanceTraveled > maxDistance && !reachedMaxDist)
        {
            reachedMaxDist = true;
            reachMaxDist();
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
    }

    protected abstract void reachMaxDist();
}
