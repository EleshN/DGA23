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

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        Animal animal = other.GetComponent<Animal>();
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy) {
            HashSet<Animal> animals = sourcePlayer.followers;
            foreach (Animal follower in animals)
            {
                follower.currEmotion = Emotion.ANGER;
                follower.targetTransform = other;
            }
            animals.Clear();
        }
        else if (animal)
        {
            sourcePlayer.followers.Remove(animal);
            animal.currEmotion = Emotion.ANGER;
            animal.targetTransform = null; // assignment to null forces new target selection.
        }
        Destroy(gameObject);
    }
}
