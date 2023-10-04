using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    public abstract float Speed { get; }

    private protected Player sourcePlayer;

    /// <summary>
    /// Sets the projectile's source, which is the player which launched the projectile
    /// </summary>
    /// <param name="p">player object</param>
    public void SetPlayer(Player p)
    {
        sourcePlayer = p;
    }

    public void SetDirection(Vector3 direction)
    {
        direction.Normalize();
        rb.velocity = direction * Speed;
    }

    // Use this for initialization
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
