using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour, IDamageable
{
    [SerializeField] Rigidbody rd; 
    public float maxHealth;

    public float currHealth;
    public float animalDamage;
    public Emotion currEmotion = Emotion.EMOTIONLESS;
    public Vector3 targetPosition;
    
    [Header("Speeds")]
    [SerializeField] float emolessSpeed;
    [SerializeField] float emoSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // Set health
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Switch statement for move
        switch (currEmotion)
        {
            case: Emotion.ANGER:
                AngerMove(targetPosition);
                break;
            case: Emotion.LOVE:
                LoveMove(targetPosition);
                break;
            default:
                EmolessMove();
                break;

        }
    }


    /// <summary>
    /// Sets the emotion of the animal when called
    /// </summary>
    /// <param name="emotion"></param>
    void SetEmotion(Emotion emotion)
    {
        currEmotion = emotion;
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
    /// <param name="target"></param>
    void LoveMove(Vector3 target)
    {

    }

    /// <summary>
    /// Called every update, when the emotion is Anger, animal will move toward the robot position
    /// </summary>
    /// <param name="robotPos"></param>
    void AngerMove(Vector3 target)
    {

    }

    /// <summary>
    /// Called every update, when there is no emotion, animal will move in some random direction
    /// </summary>
    void EmolessMove()
    {

    }

    void setTarget(Vector3 target)
    {

    }

    // TODO: check to update emotion and the target to follow
}
