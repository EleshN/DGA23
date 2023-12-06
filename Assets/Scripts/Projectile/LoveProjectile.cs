﻿using System.Collections.Generic;
using UnityEngine;

public class LoveProjectile : Projectile
{
    public void Awake()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), GameManager.Instance.PlayerObject.GetComponent<Collider>());
    }
    
    protected override void HandleCollision(Collider collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Animal")
        {
            Animal animal = other.GetComponent<Animal>();
            if (animal.ApplyEmotionEffect(Emotion.LOVE, GameManager.Instance.PlayerTransform)){
                GameManager.Instance.followers.Add(animal);
            }
        }
        base.HandleCollision(collision);
    }
}
