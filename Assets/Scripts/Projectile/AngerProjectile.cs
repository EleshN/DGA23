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
            foreach (Animal follower in animals)
            {
                follower.SetEmotion(Emotion.ANGER);
                follower.targetTransform = other.transform;
            }
            animals.Clear();
        }
        else if (other.tag == "Animal")
        {
            Animal animal = other.GetComponent<Animal>();
            GameManager.Instance.followers.Remove(animal);
            animal.SetEmotion(Emotion.ANGER);
            animal.targetTransform = null; // assignment to null forces new target selection.
        }
        base.HandleCollision(collision);
    }
}
