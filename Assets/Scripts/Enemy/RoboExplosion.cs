using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboExplosion : MonoBehaviour
{
    [SerializeField]
    List<GameObject> pieces;

    [Header("Velocity Constraints")]
    [SerializeField]
    float minSpeedX;
    [SerializeField]
    float maxSpeedX;
    [SerializeField]
    float minSpeedY;
    [SerializeField]
    float maxSpeedY;
    [SerializeField]
    float minspeedZ;
    [SerializeField]
    float maxSpeedZ;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject g in pieces) {
            Vector3 vel = getRandomVelocity();
            print(vel);
            g.GetComponent<Rigidbody>().AddForce(vel);
        }
    }

    private Vector3 getRandomVelocity() {
        float rx = Random.Range(minSpeedX, maxSpeedX);
        float ry = Random.Range(minSpeedY, maxSpeedY);
        float rz = Random.Range(minspeedZ, maxSpeedZ);
        return new Vector3(rx, ry, rz);
    }
}
