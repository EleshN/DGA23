using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Just a placeholder for the enemy behavior.
    void Start()
    {
        Rigidbody body = gameObject.AddComponent<Rigidbody>();
        body.useGravity = true;
    }
}
