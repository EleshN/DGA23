using System.Collections.Generic;
using UnityEngine;

public class AngerProjectile : Projectile
{
    // temporary testing
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         this.SetDirection(new Vector3(1, 1, 1));
    //     }
    // }

    protected override void HandleCollision(Collider collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Enemy") {
            HashSet<Animal> animals = GameManager.Instance.followers;
            HashSet<Animal> parrots = new HashSet<Animal>();
            foreach (Animal follower in animals)
            {
                if (follower.TryGetComponent<Parrot>(out Parrot parrot))
                {
                    parrots.Add(parrot);
                }
                else
                {
                    follower.ApplyEmotionEffect(Emotion.ANGER, other.transform);
                }
            }
            animals.Clear(); // no more animals following player
            animals.UnionWith(parrots);
            // GameManager.Instance.followers = parrots;
        }
        else if (other.tag == "Animal")
        {
            Animal animal = other.GetComponent<Animal>();
            {
                if (animal.ApplyEmotionEffect(Emotion.ANGER))
                {
                    GameManager.Instance.followers.Remove(animal);
                }
            }
        }
        base.HandleCollision(collision);
    }
}
