using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int[] ammo;
    public Gun gun;
    public int ammoIndex;

    public Text ammoTypeText;  // displays the current ammo type
    public Text ammoCountText; // displays the current ammo count

    public float moveSpeed = 6.9f;

    [SerializeField] string[] ammoNames; // index 0 is Love and index 1 is Anger

    private void Update()
    {
        Inputs();
        Move();
        Scroll();
        UpdateUI();
    }

    private void Inputs()
    {
        if (Input.GetMouseButton(0)) // 0 represents left mouse button
        {
            if (ammo[ammoIndex] > 0)
            {
                //gun.Fire();
                ammo[ammoIndex]--;
                UpdateUI();
            }
            else
            {
                // play an empty gun sound or something here idk
            }
        }
    }


    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    private void Scroll()
    {
        // weapon switching here
        if (Input.mouseScrollDelta.y > 0)
        {
            ammoIndex = (ammoIndex + 1) % ammo.Length;
        }
        else if (Input.mouseScrollDelta.y < 0)
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
}

