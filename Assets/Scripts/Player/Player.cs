using System.Collections;
using System.Collections.Generic;
using System;
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

    ColorIndicator colorIndicator;
    [SerializeField] float iframeDuration = 3.0f;
    float iframes;
    System.Random random;

    public virtual void Start()
    {
        colorIndicator = GetComponent<ColorIndicator>();
        random = new System.Random();
    }

    private void Update()
    {
        if (!PauseGame.isPaused)
        {
            Inputs();
            Move();
            Scroll();
            iframes -= Time.deltaTime;
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

    public void RefreshAmmo()
    {
        Debug.Log("Ammo Refreshed");
        for (int i = 0; i < ammo.Length; i++)
        {
            ammo[i] = initialAmmo[i];
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

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (iframes <= 0)
            {
                for (int i = 0; i < ammo.Length; i++)
                {
                    ammo[i] = Math.Max(ammo[i] - random.Next(1, 3), 0);
                }
                iframes = iframeDuration;
                colorIndicator.IndicateDamage();
            }
        }
    }
}

