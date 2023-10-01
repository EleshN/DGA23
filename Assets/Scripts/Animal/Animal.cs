using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour, IDamageable
{
    [SerializeField] Rigidbody rd; 

    public float Health;
    public float animalDamage;
    public AnimalEmotion currEmotion;
    
    [Header("Speeds")]
    [SerializeField] float emolessSpeed;
    [SerializeField] float emoSpeed;

    public enum AnimalEmotion {
        Emotionless, // emoless
        Love, // emo
        Anger // emoless
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set default
    }

    // Update is called once per frame
    void Update()
    {
        // Switch statement for move
    }


    /// <summary>
    /// Sets the emotion of the animal when called
    /// </summary>
    /// <param name="emotion"></param>
    void SetEmotion(AnimalEmotion emotion)
    {

    }

    /// <summary>
    /// Reduces the animal health by the damageAmount
    /// </summary>
    /// <param name="damageAmount"></param>
    public void TakeDamage(float damageAmount)
    {

    }

    /// <summary>
    /// Called when the animal losses all health
    /// Change emotion of animal to emotionless
    /// </summary>
    public void Die()
    {

    }

    /// <summary>
    /// Called every update, when the emotion is Love, animal will move toward the player position
    /// </summary>
    /// <param name="playerPos"></param>
    void LoveMove(Vector3 playerPos)
    {

    }

    /// <summary>
    /// Called every update, when the emotion is Anger, animal will move toward the robot position
    /// </summary>
    /// <param name="robotPos"></param>
    void AngerMove(Vector3 robotPos)
    {

    }

    /// <summary>
    /// Called every update, when there is no emotion, animal will move in some random direction
    /// </summary>
    void EmolessMove()
    {

    }
}
