using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelCamera : MonoBehaviour
{
    public float translateSpeed;
    public Vector3 translatingTowards;

    public Vector3 offset;

    public bool stillGoing = true;

    //Screen shake
    private Vector3 originalPos;
    private float screenShakeMin = -.2f;
    private float screenShakeMax = .2f;

    // Update is called once per frame
    void Update()
    {
        if (stillGoing)
        {
            transform.position = Vector3.MoveTowards(transform.position, translatingTowards + offset, translateSpeed);
        }
        else {
            transform.position = new Vector3(
                originalPos.x + Random.Range(screenShakeMin, screenShakeMax),
                originalPos.y + Random.Range(screenShakeMin, screenShakeMax),
                originalPos.z);
        }
        if (transform.position == translatingTowards + offset) {
            stillGoing = false;
            originalPos = transform.position;
        }
    }
}
