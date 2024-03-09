using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public RectTransform beltRectTransform; // Assign this in the inspector
    private bool isRotating = false;
    private float targetAngle = 0f;
    private float rotationSpeed = 360f; // Speed of rotation, higher for faster rotation

    private void Awake()
    {
        targetAngle += 30f;
        isRotating = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(Player.prevEmotionKey) && !isRotating)
        {
            // Rotate clockwise by 45 degrees
            targetAngle -= 30f;
            isRotating = true;
        }
        else if (Input.GetKeyDown(Player.nextEmotionKey) && !isRotating)
        {
            // Rotate anti-clockwise by 45 degrees
            targetAngle += 30f;
            isRotating = true;
        }

        if (isRotating)
        {
            RotateToAngle();
        }
    }

    private void RotateToAngle()
    {
        // Determine the current angle
        float step = rotationSpeed * Time.deltaTime;
        float angle = Mathf.MoveTowardsAngle(beltRectTransform.eulerAngles.z, targetAngle, step);
        beltRectTransform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Check if the rotation is complete
        if (Mathf.Approximately(angle, targetAngle))
        {
            isRotating = false;
        }
    }
}
