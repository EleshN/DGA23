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

    public Vector3 targetLocation {get; private set;}

    private void Start()
    {
        mainCamera = Camera.main;
        laserSight.enabled = false; // Starts with the laser sight disabled until we hit something
        // Create the red dot instance and hide it initially
        redDotInstance = Instantiate(redDotPrefab);
        redDotInstance.SetActive(false);
        targetLocation = Vector3.zero;
    }

    private void Update()
    {
        if (!PauseGame.isPaused)
        {
            AimAssist();
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
            Vector3 hit = hitInfo.point;
            // Show the red dot at the hit position
            //redDotInstance.SetActive(true);
            //redDotInstance.transform.position = new Vector3(hit.x, 0.1f, hit.z);
            
            //redDotInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            targetLocation = hit;
        }
        else
        {
            // The Raycast did not hit anything, hide the laser and reticle.
            laserSight.enabled = false;
            //redDotInstance.SetActive(false);
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
