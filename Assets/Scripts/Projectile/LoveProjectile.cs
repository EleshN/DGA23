using System.Collections.Generic;
using UnityEngine;

public class LoveProjectile : Projectile
{
    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        Animal animal = other.GetComponent<Animal>();
        if (animal)
        {
            sourcePlayer.followers.Add(animal);
            animal.currEmotion = Emotion.LOVE;
            animal.targetTransform = GameManager.Instance.PlayerTransform;
        }
        Destroy(gameObject, 0.1f);
    }
}
