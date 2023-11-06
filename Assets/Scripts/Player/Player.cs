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

    public TMP_Text ammoTypeText;  // displays the current ammo type
    public TMP_Text ammoCountText; // displays the current ammo count

    public string[] ammoNames; // make sure that the indices match up with emotions index

    public float moveSpeed = 3f;

    public HashSet<Animal> followers = new HashSet<Animal>(); //all animals following this player

    public KeyCode switchEmotion = KeyCode.Q;

    private void Update()
    {
        Inputs();
        Move();
        Scroll();
        UpdateUI();
    }

    private void Inputs()
    {
        if (Input.GetMouseButtonDown(0)) // 0 represents left mouse button
        {
            if (ammo[ammoIndex] > 0)
            {
                gun.Shoot(ammoIndex);
                ammo[ammoIndex]--;
                UpdateUI();
            }
        }
        //handle empty shooting (an effect maybe)
    }


    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        rb.velocity = movement * moveSpeed;
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

    private void UpdateUI()
    {
        ammoTypeText.text = ammoNames[ammoIndex];
        ammoCountText.text = ammo[ammoIndex].ToString();
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
        Debug.Log("Ammo Refreshed");
        for (int i = 0; i < ammo.Length; i++)
        {
            ammo[i] = initialAmmo[i];
        }
    }

}

