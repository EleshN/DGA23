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
    [SerializeField] float iframeDuration = 1.0f;
    float iframes;
    System.Random random;

    [SerializeField] float knockbackForce = 20;
    [SerializeField] float knockbackDuration = 0.3f;
    float knockbackTimer;

    // Local AudioSource for player-specific sounds
    public AudioSource playerAudioSource;

    // Audio clips for player actions
    public AudioClip walkSoundClip;
    public AudioClip uiSoundClip;
    public AudioClip refreshClip;

    void Start()
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
            knockbackTimer -= Time.deltaTime;

            // Check if player is moving to play walking sound
            if (IsMoving() && !playerAudioSource.isPlaying)
            {
                playerAudioSource.PlayOneShot(walkSoundClip);
            }
        }
    }

    private bool IsMoving()
    {
        return rb.velocity.magnitude > 0.1f; // Adjust the threshold as needed
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
        //rb.velocity = anglevector * movement * moveSpeed;
        if (knockbackTimer <= 0)
        {
            rb.velocity = anglevector * movement * moveSpeed;
        }
    }

    private void Scroll()
    {
        // weapon switching here
        if (Input.mouseScrollDelta.y > 0 || Input.GetKeyDown(switchEmotion))
        {
            playerAudioSource.PlayOneShot(uiSoundClip);
            ammoIndex = (ammoIndex + 1) % ammo.Length;
        }
        else if (Input.mouseScrollDelta.y < 0 || Input.GetKeyDown(switchEmotion))
        {
            playerAudioSource.PlayOneShot(uiSoundClip);
            ammoIndex = (ammoIndex - 1 + ammo.Length) % ammo.Length;
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tag.Enemy.ToString()) || collision.gameObject.CompareTag(Tag.EnemyBase.ToString()))
        {
            if (iframes <= 0)
            {
                knockbackTimer = knockbackDuration;

                // Apply knockback
                Vector3 direction = transform.position - collision.transform.position;
                direction.y = 0;
                direction = direction.normalized * knockbackForce; // Set knockback force and direction
                GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);

                for (int i = 0; i < ammo.Length; i++)
                {
                    ammo[i] = Math.Max(ammo[i] - random.Next(1, 3), 0);
                }
                iframes = iframeDuration;
                colorIndicator.IndicateDamage();
            }
        }
    }

    public void RefreshAmmo()
    {
        playerAudioSource.PlayOneShot(refreshClip);

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

