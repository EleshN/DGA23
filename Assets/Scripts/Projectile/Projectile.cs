using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private protected float speed = 3;

    private protected Player sourcePlayer;

    /// <summary>
    /// Sets the projectile's source, which is the player that shot this projectile
    /// </summary>
    /// <param name="p">player object</param>
    public void SetPlayer(Player p)
    {
        sourcePlayer = p;
    }

    public void SetDirection(Vector3 direction)
    {
        direction.Normalize();
        rb.velocity = direction * speed;
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
