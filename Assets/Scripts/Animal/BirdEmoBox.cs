using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdEmoBox : MonoBehaviour
{
    //public GameObject parent;
    public Parrot parrot;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider collision)
    { 
        GameObject other = collision.gameObject;

        print("bird trigger: " + other.tag.ToString());

        if (other.tag == "Animal")
        {
            //print("bird trigger: " + other.tag.ToString());
            Animal animal = other.GetComponent<Animal>();
            if (parrot.GetEmotion() == Emotion.ANGER)
            {
                animal.ApplyEmotionEffect(Emotion.ANGER);
                GameManager.Instance.followers.Remove(animal);
            }
            else if (parrot.GetEmotion() == Emotion.LOVE)
            {
                animal.ApplyEmotionEffect(Emotion.LOVE, GameManager.Instance.PlayerTransform);
                GameManager.Instance.followers.Add(animal);
            }
        }
    }
}
