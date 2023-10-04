using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerProjectile : Projectile
{
    public override float Speed => 3;

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
        if (collision.gameObject.tag == "Enemy") {
            HashSet<Animal> animals = sourcePlayer.followers;
            foreach (Animal animal in animals)
            {
                animal.currEmotion = Emotion.ANGER;
            }
            animals.Clear();
        }
        Destroy(gameObject);
    }
}
