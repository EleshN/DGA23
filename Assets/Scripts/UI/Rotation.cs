using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotation : MonoBehaviour
{
    public RectTransform beltRectTransform; // Assign this in the inspector
    private bool isRotating = false;
    private float targetAngle = 0f;
    private float rotationSpeed = 180f; // Speed of rotation, higher for faster rotation

    [SerializeField]
    List<UnityEngine.Sprite> gunSprites;
    [SerializeField]
    Image gunSprite;

    public void updateAmmoCount(int amount) {
        gunSprite.sprite = gunSprites[amount];
    }

    private void Awake()
    {
        targetAngle += 30f;
        isRotating = true;
    }

    public bool nextEmotion() {
        if (isRotating)
        {
            return false;
        }
        else {
            isRotating = true;
            // Rotate anti-clockwise by 45 degrees
            //Only rotate if we are at target angle
            targetAngle += 30;
            return true;
        }
    }

    public bool previousEmotion() {
        if (isRotating) {
            return false;
        }
        else {
            isRotating = true;
            // Rotate clockwise by 45 degrees
            targetAngle -= 30;
            return true;
        }
    }

    public bool doneRotating() {
        return !isRotating;
    }
        

    private void Update()
    {
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
