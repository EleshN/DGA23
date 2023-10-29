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
            HashSet<Animal> animals = GameManager.Instance.followers;
            foreach (Animal follower in animals)
            {
                follower.SetEmotion(Emotion.ANGER);
                follower.targetTransform = other.transform;
            }
            animals.Clear();
        }
        else if (animal)
        {
            GameManager.Instance.followers.Remove(animal);
            animal.SetEmotion(Emotion.ANGER);
            animal.targetTransform = null; // assignment to null forces new target selection.
        }
        Destroy(gameObject, 0.1f);
    }
}
