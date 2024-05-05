using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

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

    public static KeyCode prevEmotionKey = KeyCode.Q;
    public static KeyCode nextEmotionKey = KeyCode.E;

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

    public Animator anim;

    //Scrolling
    private float scrolltimer = 1;
    private float currScrollTimer = 0;

    [Header("Particle Effect")]
    [SerializeField] ParticleSystem emotionSystemLove;
    [SerializeField] ParticleSystem emotionSystemAnger;
    [SerializeField] ParticleSystem emotionSystemDefence;
    //[SerializeField] Material loveMat;
    //[SerializeField] Material angerMat;
    //[SerializeField] Material defenceMat;

    void Start()
    {
        colorIndicator = GetComponent<ColorIndicator>();
        random = new System.Random();
        resetEmotionSystem();
    }

    private void Awake()
    {
        playerAudioSource.PlayOneShot(uiSoundClip);
        ammoIndex = (ammoIndex + 1) % ammo.Length;
    }


    private void Update()
    {
        if (!PauseGame.isPaused)
        {
            Inputs();
            Move();
            
            iframes -= Time.deltaTime;
            knockbackTimer -= Time.deltaTime;

            // Check if player is moving to play walking sound
            if (IsMoving() && !playerAudioSource.isPlaying)
            {
                playerAudioSource.PlayOneShot(walkSoundClip);
            }

            if (currScrollTimer > 0)
            {
                currScrollTimer -= Time.deltaTime;
            }
                Scroll();
        }
    }

    private bool IsMoving()
    {
        return rb.velocity.magnitude > 0.1f; // Adjust the threshold as needed
    }

    private void Inputs()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) // 0 represents left mouse button
        {
            if (ammo[ammoIndex] > 0)
            {
                string animtrigger = gun.Shoot(ammoIndex);
                //print("Gun should have triggered " + animtrigger);
                anim.SetTrigger(animtrigger);
                ammo[ammoIndex]--;
                updateAmmo();
                GameManager.Instance.incrementBulletsFired();
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
        if ((Input.mouseScrollDelta.y > 0 && currScrollTimer <= 0) || Input.GetKeyDown(nextEmotionKey))
        {
            currScrollTimer = scrolltimer;
            nextEmotion();
        }
        else if ((Input.mouseScrollDelta.y < 0 && currScrollTimer <= 0) || Input.GetKeyDown(prevEmotionKey))
        {
            currScrollTimer = scrolltimer;
            previousEmotion();
        }

    }

    private void nextEmotion() {
        if (GameManager.Instance.gunUI.nextEmotion()) {
            playerAudioSource.PlayOneShot(uiSoundClip);
            ammoIndex = (ammoIndex + 1) % ammo.Length;
            updateAmmo();
        }
    }
    private void previousEmotion() {
        if (GameManager.Instance.gunUI.previousEmotion()) {
            playerAudioSource.PlayOneShot(uiSoundClip);
            ammoIndex = (ammoIndex - 1 + ammo.Length) % ammo.Length;
            updateAmmo();
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
                updateAmmo();
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
                playAmmoClip(i);
            }
        }
        //resetEmotionSystem();
        updateAmmo();
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

    /// <summary>
    /// update the Gun UI ammo count
    /// </summary>
    private void updateAmmo() {
        GameManager.Instance.gunUI.updateAmmoCount(GetCurrentAmmoCount());
    }

    public void resetEmotionSystem()
    {
        emotionSystemLove.Pause();
        emotionSystemLove.Clear();
        emotionSystemAnger.Pause();
        emotionSystemAnger.Clear();
        emotionSystemDefence.Pause();
        emotionSystemDefence.Clear();
    }

    public void playAmmoClip(int ammoidx)
    {
        if (ammoidx == 0) // love
        {
            print("love");
            //emotionSystemLove.GetComponent<ParticleSystemRenderer>().material = loveMat;
            emotionSystemLove.Play();
        }
        else if(ammoidx == 1) // anger
        {
            print("anger");
            //emotionSystemAnger.GetComponent<ParticleSystemRenderer>().material = angerMat;
            emotionSystemAnger.Play();
        }
        else if (ammoidx == 2)
        {
            //emotionSystemDefence.GetComponent<ParticleSystemRenderer>().material = defenceMat;
            emotionSystemDefence.Play();
        }
        else resetEmotionSystem();

       
    }

    
}

