using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetting : MonoBehaviour
{
    [Tooltip("Make sure the ground has the ground layer mask")]
    [SerializeField] private LayerMask groundMask;
    [Tooltip("Assign this to the transform of the gun")]
    [SerializeField] private Transform gunTransform;
    [Tooltip("Assign a prefab for the reticle")]
    [SerializeField] private GameObject reticlePrefab;
    [Tooltip("Assign a LineRenderer for the laser sight")]
    [SerializeField] private LineRenderer laserSight;

    private Camera mainCamera;
    private GameObject reticle;

    private void Start()
    {
        mainCamera = Camera.main;
        reticle = Instantiate(reticlePrefab); // Instantiate the reticle at the start
        laserSight.enabled = false; // Starts with the laser sight disabled until we hit something
    }

    private void Update()
    {
        AimUpdate();
        ReticleUpdate();
    }

    private void AimUpdate()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            // Calculate the direction
            var direction = position - transform.position;

            // Ignore the height difference.
            direction.y = 0;

            // Make the transform look in the direction.
            transform.forward = direction;
        }
    }

    private void ReticleUpdate()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            // The Raycast hit something, show the reticle and laser sight.
            // reticle.SetActive(true);
            laserSight.enabled = true;

            // Reticle is positioned where the cursor is.
            // reticle.transform.position = position;

            // Draw laser sight from the gun tip to the cursor position.
            laserSight.SetPosition(0, gunTransform.position);
            laserSight.SetPosition(1, position);
        }
        else
        {
            // The Raycast did not hit anything, hide reticle and laser.
            // reticle.SetActive(false);
            laserSight.enabled = false;
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            // The Raycast hit something, return with the position.
            return (success: true, position: hitInfo.point);
        }
        else
        {
            // The Raycast did not hit anything.
            return (success: false, position: Vector3.zero);
        }
    }
}
