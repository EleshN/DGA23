using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetting : MonoBehaviour
{
    [Tooltip("Make sure the ground has the ground layer mask")]
    [SerializeField] private LayerMask groundMask;
    [Tooltip("Assign this to the transform of the gun")]
    [SerializeField] private Transform gunTransform;
    [Tooltip("Assign a LineRenderer for the laser sight")]
    [SerializeField] private LineRenderer laserSight;
    [Tooltip("Red Dot GameObject")]
    [SerializeField] private GameObject redDotPrefab;

    private Camera mainCamera;
    private GameObject redDotInstance;

    private void Start()
    {
        mainCamera = Camera.main;
        laserSight.enabled = false; // Starts with the laser sight disabled until we hit something
        // Create the red dot instance and hide it initially
        redDotInstance = Instantiate(redDotPrefab);
        redDotInstance.SetActive(false);
    }

    private void Update()
    {
        AimUpdate();
        AimAssist();
    }

    private void AimUpdate()
    {
        var (success, hitInfo) = GetMousePosition();
        if (success)
        {
            // Calculate the direction
            var direction = hitInfo.point - transform.position;

            // Ignore the height difference.
            // direction.y = 0;

            // Make the transform look in the direction, but only rotate around the y-axis.
            transform.rotation = Quaternion.Euler(0, Quaternion.LookRotation(direction).eulerAngles.y, 0);
            //Commented out for now by Noah. Is this necessary? It makes the sprite go super wonky
        }
    }

    private void AimAssist()
    {
        var (success, hitInfo) = GetMousePosition();
        if (success)
        {
            // The Raycast hit something, show the laser sight.
            laserSight.enabled = true;
            laserSight.SetPosition(0, gunTransform.position);
            laserSight.SetPosition(1, hitInfo.point);

            // Show the red dot at the hit position
            redDotInstance.SetActive(true);
            redDotInstance.transform.position = hitInfo.point;

            redDotInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
        else
        {
            // The Raycast did not hit anything, hide the laser and reticle.
            laserSight.enabled = false;
            redDotInstance.SetActive(false);
        }
    }

    private (bool success, RaycastHit hitInfo) GetMousePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
        {
            return (true, hit);
        }

        return (false, default(RaycastHit));
    }
}
