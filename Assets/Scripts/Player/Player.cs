using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public int[] ammo;
    public int[] initialAmmo;
    public Gun gun;
    int ammoIndex;
    [SerializeField] Rigidbody rb;

    public string[] ammoNames; // make sure that the indices match up with emotions index

    public float moveSpeed = 3f;

    public HashSet<Animal> followers = new HashSet<Animal>(); //all animals following this player

    public KeyCode switchEmotion = KeyCode.Q;

    private void Update()
    {
        if (!PauseGame.isPaused)
        {
            Inputs();
            Move();
            Scroll();
        }
    }

    private void Inputs()
    {
        if (Input.GetMouseButtonDown(0)) // 0 represents left mouse button
        {
            if (ammo[ammoIndex] > 0)
            {
                gun.Shoot(ammoIndex);
                ammo[ammoIndex]--;
            }
        }
        //handle empty shooting (an effect maybe)
    }


    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        Quaternion anglevector = Quaternion.Euler(0, 45, 0); //Rotate player movement to be on 45 degrees like the camera

        rb.velocity = anglevector * movement * moveSpeed;
    }

    private void Scroll()
    {
        // weapon switching here
        if (Input.mouseScrollDelta.y > 0 || Input.GetKeyDown(switchEmotion))
        {
            ammoIndex = (ammoIndex + 1) % ammo.Length;
        }
        else if (Input.mouseScrollDelta.y < 0 || Input.GetKeyDown(switchEmotion))
        {
            ammoIndex = (ammoIndex - 1 + ammo.Length) % ammo.Length;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Reduce ammo
            if (ammo[ammoIndex] > 0)
            {
                ammo[ammoIndex]--;
            }

            //Knockback
        }
    }

    public void RefreshAmmo()
    {
        Debug.Log("Ammo Refreshed +1 for Each Type");
        for (int i = 0; i < ammo.Length; i++)
        {
            if (ammo[i] < initialAmmo[i])
            {
                ammo[i] += 1;
            }
        }
    }

    public string GetCurrentAmmoType()
    {
        return ammoNames[ammoIndex];
    }

    /// <returns>the ammo count of the current selected ammo type</returns>
    public int GetCurrentAmmoCount()
    {
        return ammo[ammoIndex];
    }

}

