using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] Rigidbody rd; 

    public int animalHealth;
    public int animalDamage;
    public enum animalEmotion {
        Emotionless, // emoless
        Love, // emo
        Anger // emoless
    }

    [Header("Speeds")]
    [SerializeField] float emolessSpeed;
    [SerializeField] float emoSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
