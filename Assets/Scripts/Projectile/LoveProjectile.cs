using System.Collections.Generic;
using UnityEngine;

public class LoveProjectile : Projectile
{
    protected override void HandleCollision(Collider collision)
    {
        GameObject other = collision.gameObject;
        Animal animal = other.GetComponent<Animal>();
        if (animal)
        {
            GameManager.Instance.followers.Add(animal);
            animal.SetEmotion(Emotion.LOVE);
            animal.targetTransform = GameManager.Instance.PlayerTransform;
        }
        base.HandleCollision(collision);
    }
}
