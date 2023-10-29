using System.Collections.Generic;
using UnityEngine;

public class LoveProjectile : Projectile
{
    private void OnTriggerEnter(Collider collision)
    {
        GameObject other = collision.gameObject;
        Animal animal = other.GetComponent<Animal>();
        if (animal)
        {
            GameManager.Instance.followers.Add(animal);
            animal.currEmotion = Emotion.LOVE;
            animal.targetTransform = GameManager.Instance.PlayerTransform;
        }
        //Removed delay for destroying projectile, can restore if some sort of splash mechanic is needed
        Destroy(gameObject);
    }
}
